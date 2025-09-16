namespace Yckj.Admin.Application;

/// <summary>
/// BusUser输出参数
/// </summary>
public class BusUserDto
{
    /// <summary>
    /// 唯一编号
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 微信openid
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// 专注时间秒
    /// </summary>
    public int FocusTime { get; set; }

    /// <summary>
    /// 专注进度上线150
    /// </summary>
    public int FocusProgress { get; set; }

    /// <summary>
    /// 钻石数量
    /// </summary>
    public int DiamondNumber { get; set; }

    /// <summary>
    /// 连续专注多少天
    /// </summary>
    public int ContinuousNumber { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime LastTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    [SugarColumn(IsJson = true)]
    public List<FocusProp> FocusPropList { get; set; }

    [SugarColumn(IsJson = true)]
    public List<AchieveProp> AchieveList { get; set; }

}
