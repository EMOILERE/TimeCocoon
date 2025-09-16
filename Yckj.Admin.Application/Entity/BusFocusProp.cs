using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 道具表
/// </summary>
[SugarTable("bus_focus_prop","道具表")]
public class BusFocusProp  : EntityBaseId
{
    /// <summary>
    /// 奖励名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "title", ColumnDescription = "奖励名称", Length = 200)]
    public string Title { get; set; }
    
    /// <summary>
    /// 奖励描述
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "desc", ColumnDescription = "奖励描述", Length = 200)]
    public string Desc { get; set; }
    
    /// <summary>
    /// 图片
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "image", ColumnDescription = "图片", Length = 200)]
    public string Image { get; set; }
    
    /// <summary>
    /// 钻石
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "diamond", ColumnDescription = "钻石")]
    public int Diamond { get; set; }
    
    /// <summary>
    /// 状态是否启用
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态是否启用")]
    public bool Status { get; set; }
    
    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "sort", ColumnDescription = "排序")]
    public int Sort { get; set; }
    
    /// <summary>
    /// 0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "type", ColumnDescription = "0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯")]
    public int Type { get; set; }
    
}
