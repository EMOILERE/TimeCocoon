using Furion.JsonSerialization;
using Nest;
using System.Composition;
using TencentCloud.Ie.V20200304.Models;
using Yckj.Admin.Application.Const;
using Yckj.Admin.Core.Service;
namespace Yckj.Admin.Application;
/// <summary>
/// BusFocusShow服务
/// </summary>
public class BusFocusShowService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<BusFocusShow> _rep;
    private readonly SysCacheService _sysCacheService;
    public BusFocusShowService(SqlSugarRepository<BusFocusShow> rep, UserManager userManager , SysCacheService sysCacheService)
    {
        _rep = rep;
        _userManager = userManager;
        _sysCacheService = sysCacheService;
    }


    /// <summary>
    /// 增加BusFocusShow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    [ExtAuth]
    public async Task<long> Add(AddBusFocusShowInput input)
    {
        var entity = input.Adapt<BusFocusShow>();
        entity.CreateTime = DateTime.Now;
        entity.UserId = _userManager.UserId;
        entity.UserName = _userManager.RealName;
        var id = await _rep.InsertReturnSnowflakeIdAsync(entity);
        var cacheExpireTime = TimeSpan.FromSeconds(input.FocusTime + 5000);
        _sysCacheService.Set($"ZHUANZHU-{id}", JsonConvert.SerializeObject(entity), cacheExpireTime);
        return id;
    }

    /// <summary>
    /// 更新BusFocusShow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    [ExtAuth]
    public async Task Update(UpdateBusFocusShowInput input)
    {
        var str = _sysCacheService.Get<string>($"ZHUANZHU-{input.Id}");
        var entity = JsonConvert.DeserializeObject<BusFocusShow>(str);
        entity.Reason = input.Reason;
        entity.FocusTime=input.FocusTime;
        entity.Status= input.Status;
        entity.Progress = input.Progress;
        var cacheExpireTime = TimeSpan.FromSeconds(input.FocusTime + 5000);
        _sysCacheService.Set($"ZHUANZHU-{input.Id}",JsonConvert.SerializeObject(entity), cacheExpireTime);
    }

    /// <summary>
    /// 获取BusFocusShow
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    [AllowAnonymous]
    public async Task<BusFocusShow> Get([FromQuery] QueryByIdBusFocusShowInput input)
    {
        var str = _sysCacheService.Get<string>($"ZHUANZHU-{input.Id}");
        var entity = JsonConvert.DeserializeObject<BusFocusShow>(str);
        return entity;
    }

    /// <summary>
    /// 获取BusFocusShow列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<BusFocusShowOutput>> List([FromQuery] BusFocusShowInput input)
    {
        return await _rep.AsQueryable().Select<BusFocusShowOutput>().ToListAsync();
    }





}

