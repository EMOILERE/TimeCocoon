using Yckj.Admin.Application.Const;
namespace Yckj.Admin.Application;
/// <summary>
/// BusFocusProp服务
/// </summary>
public class BusFocusPropService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusFocusProp> _rep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<BusUser> _busUserRep;
    public BusFocusPropService(SqlSugarRepository<BusFocusProp> rep, UserManager userManager, SqlSugarRepository<BusUser> busUserRep)
    {
        _rep = rep;
        _userManager = userManager;
        _busUserRep = busUserRep;
    }

    /// <summary>
    /// 分页查询BusFocusProp
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<BusFocusPropOutput>> Page(BusFocusPropInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.Title.Contains(input.SearchKey.Trim())
                || u.Desc.Contains(input.SearchKey.Trim())
                || u.Image.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Desc), u => u.Desc.Contains(input.Desc.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Image), u => u.Image.Contains(input.Image.Trim()))
            .WhereIF(input.Diamond > 0, u => u.Diamond == input.Diamond)
            .WhereIF(input.Sort > 0, u => u.Sort == input.Sort)
            .WhereIF(input.Type > 0, u => u.Type == input.Type)
            .Select<BusFocusPropOutput>()
;
        query = query.OrderBuilder(input, "", "CreateTime");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加BusFocusProp
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddBusFocusPropInput input)
    {
        var entity = input.Adapt<BusFocusProp>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除BusFocusProp
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteBusFocusPropInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 更新BusFocusProp
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBusFocusPropInput input)
    {
        var entity = input.Adapt<BusFocusProp>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取BusFocusProp
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<BusFocusProp> Get([FromQuery] QueryByIdBusFocusPropInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取BusFocusProp列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    [AllowAnonymous]
    public async Task<List<BusFocusPropOutput>> List()
    {
        return await _rep.AsQueryable()
           .OrderBy(it => it.Sort, OrderByType.Asc)
           .Select<BusFocusPropOutput>()
           .ToListAsync();
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "Buy")]
    [ExtAuth]
    [UnitOfWork]
    public async Task Buy(QueryByIdBusFocusPropInput input)
    {
        var user = await _busUserRep.GetByIdAsync(_userManager.UserId);
        var busFocus = await _rep.GetByIdAsync(input.Id);
        if (busFocus == null || user == null) throw Oops.Oh("错误请求");
        if (user.DiamondNumber < busFocus.Diamond) throw Oops.Oh("钻石不足");
        user.DiamondNumber-=busFocus.Diamond;
        var myFocusProp = user.FocusPropList.Find(it => it.Id == input.Id);
        if (myFocusProp != null) myFocusProp.Number += 1;
        else
        {
            user.FocusPropList.Add(new FocusProp {
                Id = busFocus.Id,
                Title = busFocus.Title,
                Number = 1,
                Image = busFocus.Image,
                Desc = busFocus.Desc,
                Type = busFocus.Type,
            });
        }
        await _busUserRep.UpdateAsync(user); 
    }

    [HttpPost]
    [ApiDescriptionSettings(Name = "UseProp")]
    [ExtAuth]
    [UnitOfWork]
    public async Task UseProp(UsePropInput input)
    {
        var user = await _busUserRep.GetByIdAsync(_userManager.UserId);
        foreach (var item in input.TypeList)
        {
            var focusProp = user.FocusPropList.Where(it => it.Type == item).FirstOrDefault();
            if (focusProp == null) throw Oops.Oh("您暂未拥有该道具，无法使用！");
            if (focusProp.Number < 1) throw Oops.Oh("道具不足无法使用");
            focusProp.Number -= 1;
        }
        await _busUserRep.UpdateAsync(user);
    }



}

