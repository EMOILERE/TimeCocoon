using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 
/// </summary>
[SugarTable("bus_focus_log","")]
public class BusFocusLog  : EntityBaseId
{
    /// <summary>
    /// 用户编码
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户编码")]
    public long UserId { get; set; }
    
    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "nick_name", ColumnDescription = "姓名", Length = 255)]
    public string NickName { get; set; }
    
    /// <summary>
    /// 日期
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "time", ColumnDescription = "日期", Length = 50)]
    public string Time { get; set; }
    
    /// <summary>
    /// 0专注成功 1专注失败
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "status", ColumnDescription = "0专注成功 1专注失败")]
    public bool Status { get; set; }
    
    /// <summary>
    /// 专注时长
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "duration", ColumnDescription = "专注时长")]
    public int Duration { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
    public DateTime CreateTime { get; set; }
    
}
