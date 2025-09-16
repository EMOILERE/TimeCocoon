using Yckj.Admin.Application.Const;
namespace Yckj.Admin.Application;
/// <summary>
/// BusFriend服务
/// </summary>
public class BusFriendService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusFriend> _rep;
    public BusFriendService(SqlSugarRepository<BusFriend> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询BusFriend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<BusFriendOutput>> Page(BusFriendInput input)
    {
        var query= _rep.AsQueryable()
            .WhereIF(input.UserId>0, u => u.UserId == input.UserId)
            .WhereIF(input.FriendUserId>0, u => u.FriendUserId == input.FriendUserId)
            .Select<BusFriendOutput>()
;
        if(input.CreateTimeRange != null && input.CreateTimeRange.Count >0)
        {
            DateTime? start= input.CreateTimeRange[0]; 
            query = query.WhereIF(start.HasValue, u => u.CreateTime > start);
            if (input.CreateTimeRange.Count >1 && input.CreateTimeRange[1].HasValue)
            {
                var end = input.CreateTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreateTime < end);
            }
        } 
        query = query.OrderBuilder(input, "", "CreateTime");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加BusFriend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBusFriendInput input)
    {
        var entity = input.Adapt<BusFriend>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除BusFriend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBusFriendInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新BusFriend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBusFriendInput input)
    {
        var entity = input.Adapt<BusFriend>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取BusFriend
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<BusFriend> Get([FromQuery] QueryByIdBusFriendInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取BusFriend列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<BusFriendOutput>> List([FromQuery] BusFriendInput input)
    {
        return await _rep.AsQueryable().Select<BusFriendOutput>().ToListAsync();
    }





}

