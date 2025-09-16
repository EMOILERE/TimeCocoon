using Furion.DataEncryption;
using Nest;
using Yckj.Admin.Application.Const;
using Yckj.Admin.Application.Entity;
using Yckj.Admin.Core.Service;
using Yckj.Admin.Core.Util;

namespace Yckj.Admin.Application;

/// <summary>
/// BusUser服务
/// </summary>
public class BusUserService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusUser> _rep;
    private readonly SysWxOpenService _sysWxOpenService;
    private readonly SysConfigService _sysConfigService;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<BusAchievement> _repBusAchievement;
    private readonly SqlSugarRepository<BusFocusLog> _repBusFocusLog;
    private readonly SqlSugarRepository<BusTask> _resBusTask;
    public BusUserService(
        SqlSugarRepository<BusUser> rep,
        SysWxOpenService sysWxOpenService,
        SysConfigService sysConfigService,
        UserManager userManager,
        SqlSugarRepository<BusAchievement> repBusAchievement,
        SqlSugarRepository<BusFocusLog> repBusFocusLog,
        SqlSugarRepository<BusTask> resBusTask)
    {
        _rep = rep;
        _sysWxOpenService = sysWxOpenService;
        _sysConfigService = sysConfigService;
        _userManager = userManager;
        _repBusAchievement = repBusAchievement;
        _repBusFocusLog = repBusFocusLog;
        _resBusTask = resBusTask;
    }

    /// <summary>
    /// 分页查询BusUser
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<BusUserOutput>> Page(BusUserInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.NickName.Contains(input.SearchKey.Trim())
                || u.Avatar.Contains(input.SearchKey.Trim())
                || u.OpenId.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Avatar), u => u.Avatar.Contains(input.Avatar.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OpenId), u => u.OpenId.Contains(input.OpenId.Trim()))
            .WhereIF(input.FocusTime > 0, u => u.FocusTime == input.FocusTime)
            .WhereIF(input.FocusProgress > 0, u => u.FocusProgress == input.FocusProgress)
            .WhereIF(input.DiamondNumber > 0, u => u.DiamondNumber == input.DiamondNumber)
            .WhereIF(input.ContinuousNumber > 0, u => u.ContinuousNumber == input.ContinuousNumber)
            .WhereIF(input.Level > 0, u => u.Level == input.Level)
            .Select<BusUserOutput>();
        if (input.LastTimeRange != null && input.LastTimeRange.Count > 0)
        {
            DateTime? start = input.LastTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.LastTime > start);
            if (input.LastTimeRange.Count > 1 && input.LastTimeRange[1].HasValue)
            {
                var end = input.LastTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.LastTime < end);
            }
        }
        if (input.CreateTimeRange != null && input.CreateTimeRange.Count > 0)
        {
            DateTime? start = input.CreateTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreateTime > start);
            if (input.CreateTimeRange.Count > 1 && input.CreateTimeRange[1].HasValue)
            {
                var end = input.CreateTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreateTime < end);
            }
        }
        query = query.OrderBuilder(input, "", "CreateTime");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加BusUser
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBusUserInput input)
    {
        var entity = input.Adapt<BusUser>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除BusUser
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBusUserInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新BusUser
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBusUserInput input)
    {
        var entity = input.Adapt<BusUser>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取BusUser
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<BusUser> Get([FromQuery] QueryByIdBusUserInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取BusUser列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<BusUserOutput>> List([FromQuery] BusUserInput input)
    {
        return await _rep.AsQueryable().Select<BusUserOutput>().ToListAsync();
    }




    [HttpPost]
    [ApiDescriptionSettings(Name = "Login")]
    [AllowAnonymous]
    public async Task<LoginOutput> Login(BusUserLoginInput input)
    {
        var openId = await _sysWxOpenService.GetWxOpenId(input.Code);
        var userEntity = await _rep.GetFirstAsync(it => it.OpenId == openId);
        if (userEntity == null)
        {
            var busUser = new BusUser();
            busUser.Id = SnowFlakeSingle.Instance.NextId();
            busUser.OpenId = openId;
            busUser.NickName = $"时茧_{Utils.Number(4)}";
            busUser.Avatar = "Upload/img/avatar.png";
            busUser.FocusTime = 0;
            busUser.FocusProgress = 100;
            busUser.DiamondNumber = 0;
            busUser.ContinuousNumber = 0;
            busUser.Level = 0;
            busUser.CreateTime = DateTime.Now;
            var achievement = await _repBusAchievement.GetFirstAsync(it => it.Type == 1);
            var leiji = await _repBusAchievement.GetFirstAsync(it => it.Type == 10);
            busUser.FocusPropList = new List<FocusProp>();
            busUser.AchieveList = new List<AchieveProp>() {
                new AchieveProp ()
                {
                    Id = achievement.Id,
                    Complete = true,
                    Progress = 100,
                    Type = 1
                },
                new AchieveProp ()
                {
                    Id = leiji.Id,
                    Complete = false,
                    Progress =achievement.Reward,
                    Type = 10
                }
            };
            busUser.DiamondNumber = achievement.Reward;
            busUser.ThemeList = new List<long>() { 42433363633411 };
            busUser.FocusTimeList = new List<string>();
            var result = await _rep.InsertAsync(busUser);
            if (!result) throw Oops.Oh("授权登录失败");
            userEntity = busUser;
        }
        userEntity.LastTime = DateTime.Now;
        await _rep.AsUpdateable(userEntity).UpdateColumns(it => new { it.LastTime }).ExecuteCommandAsync();
        return await CreateAppToken(userEntity);
    }

    /// <summary>
    /// 获取会员信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "GetUserInfo")]
    [ExtAuth]
    public async Task<BusUser> GetUserInfo()
    {
        var user = await _rep.GetByIdAsync(_userManager.UserId);
        int level = 1;
        int requiredHours = 1; // 升级所需小时数
        int prevLevelHours = 0; // 上一级所需总小时数
        int hours = (user.FocusTime / 3600);
        while (hours >= requiredHours)
        {
            level++;
            prevLevelHours = requiredHours;
            requiredHours *= 2; // 下一级所需时间翻倍
        }
        // 计算当前等级的经验百分比
        int currentLevelRequiredHours = requiredHours - prevLevelHours;
        int currentLevelHours = hours - prevLevelHours;
        int levelProgress = Math.Min(100, (int)Math.Round((double)currentLevelHours / currentLevelRequiredHours * 100));
        user.Level = level;
        user.LevelProgress = levelProgress;
        user.ContinuousNumber = Utils.CalculateConsecutiveDays(user.FocusTimeList, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
        await _rep.UpdateAsync(user);
        return user;
    }


    [HttpGet]
    [ApiDescriptionSettings(Name = "Ranking")]
    [ExtAuth]
    public async Task<List<BusRankingOutput>> Ranking([FromQuery] RankingInput input)
    {
        var userList = await _rep.AsQueryable()
            .OrderByIF(input.Type == 0, it => it.DiamondNumber, OrderByType.Desc)
            .OrderByIF(input.Type == 1, it => it.Level, OrderByType.Desc)
            .OrderByIF(input.Type == 2, it => it.FocusTime, OrderByType.Desc)
            .ToListAsync();
        var rankingList = new List<BusRankingOutput>();
        foreach (var user in userList)
        {
            int number = 0; 
            if (input.Type == 0)
            {
                number = user.DiamondNumber;
            }
            else if (input.Type==1)
            {
                number = user.Level;
            }
            else if (input.Type == 2)
            {
                number = user.FocusTime;
            }
            rankingList.Add(new BusRankingOutput
            {
                NickName = user.NickName,
                Avatar = user.Avatar,
                Number = number,
                IsMy = user.Id == _userManager.UserId
            });
        }
        return rankingList;
    }


    /// <summary>
    /// 修改会员信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "UpdateUserInfo")]
    [ExtAuth]
    public async Task UpdateMemberInfo([FromBody] BusUserUpdateInput input)
    {
        var busUser = await _rep.GetFirstAsync(m => m.Id == _userManager.UserId);
        if (string.IsNullOrEmpty(input.NickName) && string.IsNullOrEmpty(input.Avatar)) throw Oops.Oh("请填写修改信息");
        if (!string.IsNullOrEmpty(input.NickName)) busUser.NickName = input.NickName;
        if (!string.IsNullOrEmpty(input.Avatar)) busUser.Avatar = input.Avatar;
        await _rep.AsUpdateable(busUser).UpdateColumns(m => new { m.NickName, m.Avatar }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 专注失败
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "focusFail")]
    [ExtAuth]
    [UnitOfWork]
    public async Task<int> focusFail([FromBody] BusFocus input)
    {

        var busUser = await _rep.GetFirstAsync(m => m.Id == _userManager.UserId);
        var zanting = busUser.FocusPropList.Where(it => it.Type == 5).FirstOrDefault();
        var hudun = busUser.FocusPropList.Where(it => it.Type == 2).FirstOrDefault();
        var type = 0;
        if (zanting != null && zanting.Number > 0)
        {
            type = 5;
            zanting.Number -= 1;
        }
        if (hudun != null && hudun.Number >0)
        {
            type = 2;
            hudun.Number -= 1;
        }
        if (type != 5) {
            if (type != 2) {
                busUser.FocusProgress -= 5;
                if (busUser.FocusProgress < 0) busUser.FocusProgress = 0;
            }
            await _repBusFocusLog.InsertAsync(new BusFocusLog
            {
                UserId = busUser.Id,
                NickName = busUser.NickName,
                CreateTime = DateTime.Now,
                Duration = input.Minutes,
                Status = false,
                Time = DateTime.Now.ToString("yyyy-MM-dd")
            });
        }
        await _rep.AsUpdateable(busUser).ExecuteCommandAsync();
        return type;
    }

    /// <summary>
    /// 专注
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "focus")]
    [ExtAuth]
    [UnitOfWork]
    public async Task<Boolean> focus([FromBody] BusFocus input)
    {
        var total = 0;
        var busUser = await _rep.GetFirstAsync(m => m.Id == _userManager.UserId);
        busUser.FocusTime += input.Minutes * 60;
        if (busUser.FocusProgress < 150) busUser.FocusProgress += 5;
        if (busUser.FocusProgress > 150) busUser.FocusProgress = 150;
        busUser.DiamondNumber += input.CompletionReward;
        total += input.CompletionReward;
        var focusTime = DateTime.Now.ToString("yyyy-MM-dd");
        if (!busUser.FocusTimeList.Contains(focusTime))
        {
            busUser.FocusTimeList.Add(focusTime);
        }
        var busAchievementList = await _repBusAchievement.GetListAsync();
        bool unlock = false;
        //专注新人
        var zhuanZhuXinRen = busAchievementList.Where(it => it.Type == 2).FirstOrDefault();
        var findXinren = busUser.AchieveList.Where(it => it.Type == 2).FirstOrDefault();
        if (findXinren == null)
        {
            busUser.AchieveList.Add(new AchieveProp
            {
                Id = zhuanZhuXinRen.Id,
                Progress = 100,
                Complete = true,
                Type = 2
            });
            busUser.DiamondNumber += zhuanZhuXinRen.Reward;
            total += zhuanZhuXinRen.Reward;
            unlock = true;
        }

        //专注达人lian
        var zhuanZhuDaRen = busAchievementList.Where(it => it.Type == 3).FirstOrDefault();
        var findDaRen = busUser.AchieveList.Where(it => it.Type == 3).FirstOrDefault();
        double daRenProgress = Math.Round((double)(Convert.ToDouble(busUser.FocusTime) / (10 * 3600)) * 100, 2);
        if (findDaRen == null)
        {
            busUser.AchieveList.Add(new AchieveProp
            {
                Id = zhuanZhuDaRen.Id,
                Progress = daRenProgress >= 100 ? 100 : daRenProgress,
                Complete = daRenProgress >= 100,
                Type = 3
            });
        }
        else if (findDaRen.Complete == false)
        {
            findDaRen.Progress = daRenProgress >= 100 ? 100 : daRenProgress;
            findDaRen.Complete = daRenProgress >= 100;
            if (findDaRen.Complete)
            {
                busUser.DiamondNumber += zhuanZhuDaRen.Reward;
                total += zhuanZhuDaRen.Reward;
                unlock = true;
            }
        }

        //持之以恒
        var chiZhiYiHeng = busAchievementList.Where(it => it.Type == 9).FirstOrDefault();
        var findChiZhi = busUser.AchieveList.Where(it => it.Type == 9).FirstOrDefault();
        var diamondNumber = Utils.CalculateConsecutiveDays(busUser.FocusTimeList, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
        double chiZhiYiHengProgress = Math.Round((double)(Convert.ToDouble(diamondNumber) / 7) * 100, 2);
        if (findChiZhi == null)
        {
            busUser.AchieveList.Add(new AchieveProp
            {
                Id = chiZhiYiHeng.Id,
                Progress = chiZhiYiHengProgress >= 100 ? 100 : chiZhiYiHengProgress,
                Complete = chiZhiYiHengProgress >= 100,
                Type = 9
            });
        }
        else if (findChiZhi.Complete == false)
        {
            findChiZhi.Progress = chiZhiYiHengProgress >= 100 ? 100 : chiZhiYiHengProgress;
            findChiZhi.Complete = chiZhiYiHengProgress >= 100;
            if (findChiZhi.Complete)
            {
                busUser.DiamondNumber += chiZhiYiHeng.Reward;
                total += chiZhiYiHeng.Reward;
                unlock = true;
            }
        }

        //深夜守护者
        var shenYe = busAchievementList.Where(it => it.Type == 11).FirstOrDefault();
        var findShenYe = busUser.AchieveList.Where(it => it.Type == 11).FirstOrDefault();
        TimeSpan now = DateTime.Now.TimeOfDay;
        TimeSpan nightStart = new TimeSpan(22, 0, 0);  // 22:00
        TimeSpan nightEnd = new TimeSpan(23, 59, 59);  // 23:59:59
        if (findShenYe == null && now >= nightStart && now <= nightEnd)
        {
            busUser.AchieveList.Add(new AchieveProp
            {
                Id = shenYe.Id,
                Progress = 100,
                Complete = true,
                Type = 11
            });
            busUser.DiamondNumber += shenYe.Reward;
            total += shenYe.Reward;
            unlock = true;
        }

        //专注大师
        var zhuanZhuDaShi = busAchievementList.Where(it => it.Type == 12).FirstOrDefault();
        var findDaShi = busUser.AchieveList.Where(it => it.Type == 12).FirstOrDefault();
        double daShiProgress = Math.Round((double)(Convert.ToDouble(busUser.FocusTime) / (50 * 3600)) * 100, 2);
        if (findDaShi == null)
        {
            busUser.AchieveList.Add(new AchieveProp
            {
                Id = zhuanZhuDaShi.Id,
                Progress = daShiProgress >= 100 ? 100 : daShiProgress,
                Complete = daShiProgress >= 100,
                Type = 12
            });
        }
        else if (findDaShi.Complete == false)
        {
            findDaShi.Progress = daShiProgress >= 100 ? 100 : daShiProgress;
            findDaShi.Complete = daShiProgress >= 100;
            if (findDaShi.Complete)
            {
                busUser.DiamondNumber += zhuanZhuDaShi.Reward;
                total += zhuanZhuDaShi.Reward;
                unlock = true;
            }
        }

        //晶石收藏家
        var shouCangJia = busAchievementList.Where(it => it.Type == 10).FirstOrDefault();
        var findShouCangJia = busUser.AchieveList.Where(it => it.Type == 10).FirstOrDefault();
        if (findShouCangJia.Complete == false)
        {
            findShouCangJia.Progress = (findShouCangJia.Progress + total) >= 100 ? 100 : findShouCangJia.Progress + total;
            findShouCangJia.Complete = findShouCangJia.Progress >= 100;
            if (findShouCangJia.Complete)
            {
                busUser.DiamondNumber += shouCangJia.Reward;
            }
        }

        await _repBusFocusLog.InsertAsync(new BusFocusLog
        {
            UserId = busUser.Id,
            NickName = busUser.NickName,
            CreateTime = DateTime.Now,
            Duration = input.Minutes * 60,
            Status = true,
            Time = DateTime.Now.ToString("yyyy-MM-dd")
        });

        await _rep.AsUpdateable(busUser).ExecuteCommandAsync();
        return unlock;
    }

    /// <summary>
    /// 购买主题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "BuyTheme")]
    [ExtAuth]
    public async Task BuyTheme([FromBody] BaseIdInput input)
    {
        var busUser = await _rep.GetFirstAsync(m => m.Id == _userManager.UserId);
        var dict = new Dictionary<long, int>();
        dict.Add(42433363633411, 0);
        dict.Add(42433363633412, 500);
        dict.Add(42433363633413, 800);
        dict.Add(42433363633414, 1000);
        dict.Add(42433363633415, 1200);
        dict.Add(42433363633416, 1500);
        dict.Add(42433363633417, 2000);
        var keyValues = dict.Where(it => it.Key == input.Id).FirstOrDefault();
        if (busUser.DiamondNumber < keyValues.Value)
        {
            throw Oops.Oh("钻石不够，请努力赚钻石吧！");
        }
        busUser.DiamondNumber -= keyValues.Value;
        busUser.ThemeList.Add(input.Id);
        await _rep.AsUpdateable(busUser).UpdateColumns(m => new { m.DiamondNumber, m.ThemeList }).ExecuteCommandAsync();
    }

    [NonAction]
    private async Task<LoginOutput> CreateAppToken(BusUser user)
    {
        // 生成Token令牌
        var tokenExpire = await _sysConfigService.GetTokenExpire();
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            { ClaimConst.UserId, user.Id },
            { ClaimConst.RealName, user.NickName },
            { ClaimConst.LoginMode, (int)LoginModeEnum.APP },
        }, tokenExpire);

        // 生成刷新Token令牌
        var refreshTokenExpire = await _sysConfigService.GetRefreshTokenExpire();
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

        return new LoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "TaskColumn")]
    [ExtAuth]
    public async Task<dynamic> TaskColumn()
    {
        var tasks = await _resBusTask.AsQueryable()
                   .Where(it => it.UserId == _userManager.UserId)
                   .Where(it => it.Type == 0)
                   .Select<BusTaskOutput>()
                   .ToListAsync();
        var categories = new List<string>() { "高优先级", "中优先级", "低优先级" };
        var series = new List<dynamic>();
        var wCount = new List<int>();
        var yCount = new List<int>();
        for (var i = 0; i < 3; i++)
        {
            wCount.Add(tasks.Where(it => it.Completed && it.Priority == i).Count());
            yCount.Add(tasks.Where(it => !it.Completed && it.Priority == i && DateTime.Now > it.EndTime).Count());
        }

        series.Add(new
        {
            name = "已完成",
            data = wCount
        });
        series.Add(new
        {
            name = "已逾期",
            data = yCount
        });
        return new
        {
            categories,
            series
        };
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "TaskPie")]
    [ExtAuth]
    public async Task<dynamic> TaskPie()
    {
        var tasks = await _resBusTask.AsQueryable()
                  .Where(it => it.UserId == _userManager.UserId)
                  .Where(it => it.Type == 0)
                  .Select<BusTaskOutput>()
                  .ToListAsync();
        var series = new List<dynamic>();


        series.Add(new {
            name = "已完成",
            value = tasks.Where(it => it.Completed).Count()
        });
        series.Add(new
        {
            name = "已逾期",
            value = tasks.Where(it => !it.Completed && DateTime.Now > it.EndTime).Count()
        });
        return new
        {
            series
        };
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "TaskLine")]
    [ExtAuth]
    public async Task<dynamic> TaskLine(BusStatisticsInput input)
    {
        var day = input.Type == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var tasks = await _resBusTask.AsQueryable()
                  .Where(it => it.UserId == _userManager.UserId)
                  .Where(it => it.Type == 0)
                  .Where(it => it.CreateTime <= time)
                  .Select<BusTaskOutput>()
                  .ToListAsync();

        var categories = new List<string>();
        var series = new List<dynamic>();
        var gCount = new List<int>();
        var zCount = new List<int>();
        var dCount = new List<int>();
        for (int i = 0; i < day; i++)
        {
            var wTime= DateTime.Now.AddDays(-i);
            categories.Add(wTime.Month+"/"+wTime.Day);
            var sTime= Convert.ToDateTime(wTime.ToString("yyyy-MM-dd"));
            var eTime = sTime.AddDays(1).AddMinutes(-1);
            gCount.Add(tasks.Where(it => it.Priority == 0 && it.CreateTime >= sTime && it.CreateTime <= eTime).Count());
            zCount.Add(tasks.Where(it => it.Priority == 1 && it.CreateTime >= sTime && it.CreateTime <= eTime).Count());
            dCount.Add(tasks.Where(it => it.Priority == 2 && it.CreateTime >= sTime && it.CreateTime <= eTime).Count());
        }
       
        series.Add(new
        {
            name = "高优先级",
            data = gCount
        });
        series.Add(new
        {
            name = "中优先级",
            data = zCount
        });
        series.Add(new
        {
            name = "低优先级",
            data = dCount
        });
        return new
        {
            categories,
            series
        };
    }
    [HttpPost]
    [ApiDescriptionSettings(Name = "MoodRadar")]
    [ExtAuth]
    public async Task<dynamic> MoodRadar(BusStatisticsInput input)
    {
        var day = input.Type == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var tasks = await _resBusTask.AsQueryable()
                  .Where(it => it.UserId == _userManager.UserId)
                  .Where(it => it.Type == 2)
                  .Where(it => it.CreateTime <= time)
                  .Select<BusTaskOutput>()
                  .ToListAsync();

        var categories = new List<string>();
        var series = new List<dynamic>();
        var data = new List<int>();

        for (int i = 0; i < day; i++)
        {
            var wTime = DateTime.Now.AddDays(-i);
            categories.Add(wTime.Month + "/" + wTime.Day);
            var sTime = Convert.ToDateTime(wTime.ToString("yyyy-MM-dd"));
            var eTime = sTime.AddDays(1).AddMinutes(-1);
            data.Add(tasks.Where(it => it.CreateTime >= sTime && it.CreateTime <= eTime).Count());
        }

        series.Add(new
        {
            name = "心情发布数量",
            data
        });
        return new
        {
            categories,
            series,
            max=data.Max()
        };
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "FocusArea")]
    [ExtAuth]
    public async Task<dynamic> FocusArea(BusStatisticsInput input)
    {
        var day = input.Type == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var focusLog = await _repBusFocusLog.AsQueryable()
                  .Where(it => it.UserId == _userManager.UserId)
                  .Where(it => it.CreateTime <= time)
                  .ToListAsync();

        var categories = new List<string>();
        var series = new List<dynamic>();
        var data = new List<int>();

        for (int i = 0; i < day; i++)
        {
            var wTime = DateTime.Now.AddDays(-i);
            categories.Add(wTime.Month + "/" + wTime.Day);
            var sTime = Convert.ToDateTime(wTime.ToString("yyyy-MM-dd"));
            var eTime = sTime.AddDays(1).AddMinutes(-1);
            data.Add(focusLog.Where(it => it.CreateTime >= sTime && it.CreateTime <= eTime).Sum(it=>it.Duration));
        }
        series.Add(new
        {
            name = "专注时长（秒）",
            data
        });
        return new
        {
            categories,
            series,
        };
    }
    [HttpPost]
    [ApiDescriptionSettings(Name = "FocusLine")]
    [ExtAuth]
    public async Task<dynamic> FocusLine(BusStatisticsInput input)
    {
        var day = input.Type == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var focusLog = await _repBusFocusLog.AsQueryable()
                  .Where(it => it.UserId == _userManager.UserId)
                  .Where(it => it.CreateTime <= time)
                  .ToListAsync();

        var categories = new List<string>();
        var series = new List<dynamic>();
        var cCount = new List<int>();
        var sCount = new List<int>();
        for (int i = 0; i < day; i++)
        {
            var wTime = DateTime.Now.AddDays(-i);
            categories.Add(wTime.Month + "/" + wTime.Day);
            var sTime = Convert.ToDateTime(wTime.ToString("yyyy-MM-dd"));
            var eTime = sTime.AddDays(1).AddMinutes(-1);
            cCount.Add(focusLog.Where(it => it.CreateTime >= sTime && it.CreateTime <= eTime && it.Status).Count());
            sCount.Add(focusLog.Where(it => it.CreateTime >= sTime && it.CreateTime <= eTime && !it.Status).Count());
        }
        series.Add(new
        {
            name = "成功次数",
            data= cCount,
        });
        series.Add(new
        {
            name = "失败次数",
            data = sCount,
        });
        return new
        {
            categories,
            series,
        };
    }
}