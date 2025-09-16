using Yckj.Admin.Core;

namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 用户表
/// </summary>
[SugarTable("bus_user", "用户表")]
public class BusUser : EntityBaseId
{
    /// <summary>
    /// 昵称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "nick_name", ColumnDescription = "昵称", Length = 500)]
    public string NickName { get; set; }

    /// <summary>
    /// 头像地址
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "avatar", ColumnDescription = "头像地址", Length = 500)]
    public string Avatar { get; set; }

    /// <summary>
    /// 微信openid
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "open_id", ColumnDescription = "微信openid", Length = 200)]
    public string OpenId { get; set; }

    /// <summary>
    /// 我的道具
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "focus_prop_list", IsJson = true, ColumnDescription = "我的道具")]
    public List<FocusProp> FocusPropList { get; set; }

    /// <summary>
    /// 我的成就
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "achieve_list", IsJson = true, ColumnDescription = "我的成就")]
    public List<AchieveProp> AchieveList { get; set; }

    /// <summary>
    /// 我的主题
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "theme_list", IsJson = true, ColumnDescription = "我的主题")]
    public List<long> ThemeList { get; set; }

    /// <summary>
    /// 专注时间秒
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "focus_time", ColumnDescription = "专注时间秒")]
    public int FocusTime { get; set; }

    [Required]
    [SugarColumn(ColumnName = "focus_time_list", IsJson = true, ColumnDescription = "打卡日期")]
    public List<string> FocusTimeList { get; set; }

    /// <summary>
    /// 专注进度上线150
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "focus_progress", ColumnDescription = "专注进度上线150")]
    public int FocusProgress { get; set; }

    /// <summary>
    /// 钻石数量
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "diamond_number", ColumnDescription = "钻石数量")]
    public int DiamondNumber { get; set; }

    /// <summary>
    /// 连续专注多少天
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "continuous_number", ColumnDescription = "连续专注多少天")]
    public int ContinuousNumber { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "level", ColumnDescription = "级别")]
    public int Level { get; set; }

    /// <summary>
    /// 进度
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "level_progress", ColumnDescription = "进度", IsIgnore = true)]
    public int LevelProgress { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "last_time", ColumnDescription = "最后登录时间")]
    public DateTime LastTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
    public DateTime CreateTime { get; set; }
}

public class FocusProp
{
    public long Id { get; set; }
    public string Title { get; set; }
    public int Number { get; set; }
    public string Desc { get; set; }
    public string Image { get; set; }
    public int Type { get; set; }
}

public class AchieveProp
{
    public long Id { get; set; }
    public int Type { get; set; }
    public double Progress { get; set; }
    public bool Complete { get; set; }
}