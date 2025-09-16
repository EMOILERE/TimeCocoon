using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 双面墙表
/// </summary>
[SugarTable("bus_wall","双面墙表")]
public class BusWall  : EntityBaseId
{
    /// <summary>
    /// 标题
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "title", ColumnDescription = "标题", Length = 500)]
    public string Title { get; set; }
    
    /// <summary>
    /// 用户id
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户id")]
    public long UserId { get; set; }
    
    /// <summary>
    /// 0忏悔墙，1灵感墙
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "type", ColumnDescription = "0忏悔墙，1灵感墙")]
    public int Type { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
    public DateTime CreateTime { get; set; }
    
}
