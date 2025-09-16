using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 
/// </summary>
[SugarTable("bus_focus_show","")]
public class BusFocusShow  : EntityBaseId
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "", Length = 150)]
    public string UserName { get; set; }

    [Required]
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "")]
    public long UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "focus_progress", ColumnDescription = "")]
    public int FocusProgress { get; set; }

    [Required]
    [SugarColumn(ColumnName = "progress", ColumnDescription = "")]
    public string Progress { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "focus_time", ColumnDescription = "")]
    public int FocusTime { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "quotes", ColumnDescription = "", Length = 500)]
    public string Quotes { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "desc", ColumnDescription = "", Length = 500)]
    public string Desc { get; set; }
    
    /// <summary>
    /// 0专注中1暂停中2休息中3专注成功4专注失败
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "status", ColumnDescription = "0专注中1暂停中2休息中3专注成功4专注失败")]
    public int Status { get; set; }
    
    /// <summary>
    /// 失败原因
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "失败原因", Length = 255)]
    public string? Reason { get; set; }
    
    /// <summary>
    /// 专注数据
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "text_data", ColumnDescription = "专注数据", Length = 0)]
    public string TextData { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "")]
    public DateTime CreateTime { get; set; }
    
}
