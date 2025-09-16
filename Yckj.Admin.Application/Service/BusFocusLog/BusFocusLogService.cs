using Yckj.Admin.Application.Const;
namespace Yckj.Admin.Application;
/// <summary>
/// BusFocusLog服务
/// </summary>
public class BusFocusLogService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusFocusLog> _rep;
    public BusFocusLogService(SqlSugarRepository<BusFocusLog> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询BusFocusLog
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<BusFocusLogOutput>> Page(BusFocusLogInput input)
    {
        var query= _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.NickName.Contains(input.SearchKey.Trim())
                || u.Time.Contains(input.SearchKey.Trim())
            )
            .WhereIF(input.UserId>0, u => u.UserId == input.UserId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.NickName), u => u.NickName.Contains(input.NickName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Time), u => u.Time.Contains(input.Time.Trim()))
            .WhereIF(input.Duration>0, u => u.Duration == input.Duration)
            .Select<BusFocusLogOutput>()
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
    /// 增加BusFocusLog
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBusFocusLogInput input)
    {
        var entity = input.Adapt<BusFocusLog>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除BusFocusLog
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBusFocusLogInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新BusFocusLog
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBusFocusLogInput input)
    {
        var entity = input.Adapt<BusFocusLog>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取BusFocusLog
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<BusFocusLog> Get([FromQuery] QueryByIdBusFocusLogInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取BusFocusLog列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<BusFocusLogOutput>> List([FromQuery] BusFocusLogInput input)
    {
        return await _rep.AsQueryable().Select<BusFocusLogOutput>().ToListAsync();
    }





}

