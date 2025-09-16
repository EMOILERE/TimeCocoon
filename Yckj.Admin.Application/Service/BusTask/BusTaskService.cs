using System;
using Yckj.Admin.Application.Const;
using Yckj.Admin.Application.Entity;
namespace Yckj.Admin.Application;
/// <summary>
/// BusTask服务
/// </summary>
public class BusTaskService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusTask> _rep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<BusUser> _busUserRep;
    private readonly SqlSugarRepository<BusAchievement> _repBusAchievement;
    public BusTaskService(SqlSugarRepository<BusTask> rep, UserManager userManager, SqlSugarRepository<BusUser> busUserRep, SqlSugarRepository<BusAchievement> repBusAchievement)
    {
        _rep = rep;
        _userManager = userManager;
        _busUserRep = busUserRep;
        _repBusAchievement = repBusAchievement;
    }

    /// <summary>
    /// 分页查询BusTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<BusTaskOutput>> Page(BusTaskInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.Title.Contains(input.SearchKey.Trim())
                || u.Content.Contains(input.SearchKey.Trim())
                || u.Picture.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Content), u => u.Content.Contains(input.Content.Trim()))
            .WhereIF(input.Priority > 0, u => u.Priority == input.Priority)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Picture), u => u.Picture.Contains(input.Picture.Trim()))
            .WhereIF(input.Type > 0, u => u.Type == input.Type)
            .Select<BusTaskOutput>()
;
        if (input.StartTimeRange != null && input.StartTimeRange.Count > 0)
        {
            DateTime? start = input.StartTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.StartTime > start);
            if (input.StartTimeRange.Count > 1 && input.StartTimeRange[1].HasValue)
            {
                var end = input.StartTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.StartTime < end);
            }
        }
        if (input.EndTimeRange != null && input.EndTimeRange.Count > 0)
        {
            DateTime? start = input.EndTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.EndTime > start);
            if (input.EndTimeRange.Count > 1 && input.EndTimeRange[1].HasValue)
            {
                var end = input.EndTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.EndTime < end);
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
    /// 增加BusTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    [ExtAuth]
    [UnitOfWork]
    public async Task<bool> Add(AddBusTaskInput input)
    {
        var entity = input.Adapt<BusTask>();
        entity.UserId = _userManager.UserId;
        entity.CreateTime = input.CreateTime;
        entity.Completed = false;
        var achievementList = await _repBusAchievement.GetListAsync();
        var userEntity = await _busUserRep.GetByIdAsync(_userManager.UserId);
        var total = 0;
        bool unlock = false;
        if (input.Type == 0)
        {
            //任务起步
            var qibu = achievementList.Where(it => it.Type == 4).FirstOrDefault();
            var findQibu = userEntity.AchieveList.Where(it => it.Type == 4).FirstOrDefault();
            if (findQibu == null)
            {
                userEntity.AchieveList.Add(new AchieveProp
                {
                    Id = qibu.Id,
                    Complete = true,
                    Progress = 100,
                    Type = 4
                });
                userEntity.DiamondNumber += qibu.Reward;
                total += qibu.Reward;
                unlock = true;
            }

            //任务管理者
            var count = await _rep.AsQueryable().Where(it => it.UserId == _userManager.UserId && it.Type==0).CountAsync();
            var guanlizhe = achievementList.Where(it => it.Type == 5).FirstOrDefault();
            var findGuanlizhe = userEntity.AchieveList.Where(it => it.Type == 5).FirstOrDefault();
            var progress = (Convert.ToDouble((count >=10 ? 10 : Convert.ToDouble(count + 1))) / 10) * 100;
            if (findGuanlizhe == null)
            {
                userEntity.AchieveList.Add(new AchieveProp
                {
                    Id = guanlizhe.Id,
                    Complete = progress >= 100,
                    Progress = progress,
                    Type = 5
                });
            }
            else if (findGuanlizhe.Complete == false)
            {
                findGuanlizhe.Complete = progress >= 100;
                findGuanlizhe.Progress = progress;
                if (findGuanlizhe.Complete)
                {
                    userEntity.DiamondNumber += guanlizhe.Reward;
                    total += guanlizhe.Reward;
                    unlock = true;
                }
            }
        }
        else if (input.Type==1)
        {
            var count = await _rep.AsQueryable().Where(it => it.UserId == _userManager.UserId && it.Type == 1).CountAsync();
            var bijidashi = achievementList.Where(it => it.Type == 7).FirstOrDefault();
            var findBijidashi = userEntity.AchieveList.Where(it => it.Type == 7).FirstOrDefault();
            var progress = (Convert.ToDouble((count >= 5 ? 5 : Convert.ToDouble(count + 1))) / 5) * 100;
            if (findBijidashi == null)
            {
                userEntity.AchieveList.Add(new AchieveProp
                {
                    Id = bijidashi.Id,
                    Complete = progress >= 100,
                    Progress = progress,
                    Type = 7
                });
            }
            else if (findBijidashi.Complete == false)
            {
                findBijidashi.Complete = progress >= 100;
                findBijidashi.Progress = progress;
                if (findBijidashi.Complete)
                {
                    userEntity.DiamondNumber += bijidashi.Reward;
                    total += bijidashi.Reward;
                    unlock = true;
                }
            }
        }
        else if (input.Type == 2)
        {
            var list = await _rep.AsQueryable().Where(it => it.UserId == _userManager.UserId && it.Type == 2)
                .GroupBy(it => SqlFunc.ToDate(it.CreateTime.ToString("yyyy-MM-dd")))
                .ToListAsync();
            var time=DateTime.Now.ToString("yyyy-MM-dd");
            var current = list.Where(it => it.CreateTime.ToString("yyyy-MM-dd HH:mm:ss").Contains(time)).ToList();
            var count = list.Count() >= 7 ? 7 : current.Count > 0 ? list.Count() : list.Count() + 1;
            var xinqing = achievementList.Where(it => it.Type == 8).FirstOrDefault();
            var findXinqing = userEntity.AchieveList.Where(it => it.Type == 8).FirstOrDefault();
            var progress = Convert.ToDouble(Convert.ToDouble(count) / 7) * 100;

            if (findXinqing == null)
            {
                userEntity.AchieveList.Add(new AchieveProp
                {
                    Id = xinqing.Id,
                    Complete = progress >= 100,
                    Progress = progress,
                    Type = 8
                });
            }
            else if (findXinqing.Complete == false)
            {
                findXinqing.Complete = progress >= 100;
                findXinqing.Progress = progress;
                if (findXinqing.Complete)
                {
                    userEntity.DiamondNumber += xinqing.Reward;
                    total += xinqing.Reward;
                    unlock = true;
                }
            }
        }
        //晶石收藏家
        var shouCangJia = achievementList.Where(it => it.Type == 10).FirstOrDefault();
        var findShouCangJia = userEntity.AchieveList.Where(it => it.Type == 10).FirstOrDefault();
        if (findShouCangJia.Complete == false)
        {
            findShouCangJia.Progress = (findShouCangJia.Progress + total) >= 100 ? 100 : findShouCangJia.Progress + total;
            findShouCangJia.Complete = findShouCangJia.Progress >= 100;
            if (findShouCangJia.Complete)
            {
                userEntity.DiamondNumber += shouCangJia.Reward;
                unlock = true;
            }
        }

        await _busUserRep.UpdateAsync(userEntity);
        await _rep.InsertAsync(entity);
        return unlock;
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "SetCompleted")]
    [ExtAuth]
    [UnitOfWork]
    public async Task<bool> SetCompleted(QueryByIdBusTaskInput input)
    {
        var total = 0;
        var unlock = false;
        var entity = await _rep.GetByIdAsync(input.Id);
        entity.Completed = !entity.Completed;
        var achievementList = await _repBusAchievement.GetListAsync();
        var userEntity = await _busUserRep.GetByIdAsync(_userManager.UserId);
        await _rep.AsUpdateable(entity).UpdateColumns(it => new { it.Completed }).ExecuteCommandAsync();
        var completedList = await _rep.GetListAsync(it => it.Type == 0 && it.Completed);

        //高效执行
        var zhixing = achievementList.Where(it => it.Type == 6).FirstOrDefault();
        var findZhixing = userEntity.AchieveList.Where(it => it.Type == 6).FirstOrDefault();
        var progress = (Convert.ToDouble((completedList.Count >= 5 ? 5 : Convert.ToDouble(completedList.Count + 1))) / 5) * 100;
        if (findZhixing == null)
        {
            userEntity.AchieveList.Add(new AchieveProp
            {
                Id = zhixing.Id,
                Complete = progress >= 100,
                Progress = progress,
                Type = 6
            });
        }
        else if (findZhixing.Complete == false)
        {
            findZhixing.Complete = progress >= 100;
            findZhixing.Progress = progress;
            if (findZhixing.Complete)
            {
                userEntity.DiamondNumber += zhixing.Reward;
                total += zhixing.Reward;
                unlock = true;
                
            }
        }
         //晶石收藏家
        var shouCangJia = achievementList.Where(it => it.Type == 10).FirstOrDefault();
        var findShouCangJia = userEntity.AchieveList.Where(it => it.Type == 10).FirstOrDefault();
        if (findShouCangJia.Complete == false)
        {
            findShouCangJia.Progress = (findShouCangJia.Progress + total) >= 100 ? 100 : findShouCangJia.Progress + total;
            findShouCangJia.Complete = findShouCangJia.Progress >= 100;
            if (findShouCangJia.Complete)
            {
                userEntity.DiamondNumber += shouCangJia.Reward;
                unlock = true;
            }
        }
        await _busUserRep.UpdateAsync(userEntity);
        return unlock;
    }

    /// <summary>
    /// 获取BusTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    [ExtAuth]
    public async Task<BusTask> Get([FromQuery] QueryByIdBusTaskInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id && u.UserId == _userManager.UserId);
    }
    /// <summary>
    /// 更新BusTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    [ExtAuth]
    public async Task Update(UpdateBusTaskInput input)
    {
        var entity = input.Adapt<BusTask>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除BusTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    [ExtAuth]
    public async Task Delete(DeleteBusTaskInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id && u.UserId == _userManager.UserId) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);
    }

    /// <summary>
    /// 获取BusTask列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    [ExtAuth]
    public async Task<List<BusTaskOutput>> List([FromQuery] BusTaskInput input)
    {
        return await _rep.AsQueryable()
            .Where(it => it.UserId == _userManager.UserId)
            .OrderBy(it => it.CreateTime, OrderByType.Desc)
            .Select<BusTaskOutput>()
            .ToListAsync();
    }

    /// 获取BusTask列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Statistics")]
    [ExtAuth]
    public async Task<List<BusTaskStatisticsOutput>> Statistics(BusStatisticsInput input)
    {
        var day = input.Type == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var tasks = await _rep.AsQueryable()
                    .Where(it => it.UserId == _userManager.UserId)
                    .Where(it => it.Type == 0)
                    .Where(it=>it.CreateTime<= time)
                    .Select<BusTaskOutput>()
                    .ToListAsync();
        var list = new List<BusTaskStatisticsOutput>();
        for (var i = 0; i < 3; i++)
        {
            list.Add(new BusTaskStatisticsOutput
            {
                Type = i,
                WCount = tasks.Where(it => it.Completed && it.Priority == i).Count(),
                YCount = tasks.Where(it => !it.Completed && it.Priority == i && DateTime.Now > it.EndTime).Count(),
            });
        }
        return list;
    }
}

