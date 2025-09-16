using Yckj.Admin.Application.Const;
namespace Yckj.Admin.Application;
/// <summary>
/// BusAchievement服务
/// </summary>
public class BusAchievementService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusAchievement> _rep;
    public BusAchievementService(SqlSugarRepository<BusAchievement> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询BusAchievement
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]

    public async Task<SqlSugarPagedList<BusAchievementOutput>> Page(BusAchievementInput input)
    {
        var query= _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.Title.Contains(input.SearchKey.Trim())
                || u.Desc.Contains(input.SearchKey.Trim())
                || u.Emoji.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Desc), u => u.Desc.Contains(input.Desc.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Emoji), u => u.Emoji.Contains(input.Emoji.Trim()))
            .WhereIF(input.Reward>0, u => u.Reward == input.Reward)
            .WhereIF(input.Type>0, u => u.Type == input.Type)
            .Select<BusAchievementOutput>()
;
        query = query.OrderBuilder(input, "", "CreateTime");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加BusAchievement
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBusAchievementInput input)
    {
        var entity = input.Adapt<BusAchievement>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除BusAchievement
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBusAchievementInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新BusAchievement
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBusAchievementInput input)
    {
        var entity = input.Adapt<BusAchievement>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取BusAchievement
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<BusAchievement> Get([FromQuery] QueryByIdBusAchievementInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取BusAchievement列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    [AllowAnonymous]
    public async Task<List<BusAchievementOutput>> List([FromQuery] BusAchievementInput input)
    {
        return await _rep.AsQueryable().Select<BusAchievementOutput>().OrderBy(it=>it.Store).ToListAsync();
    }

}

