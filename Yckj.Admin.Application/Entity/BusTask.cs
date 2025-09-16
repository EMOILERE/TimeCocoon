using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 任务/笔记/心情表
/// </summary>
[SugarTable("bus_task","任务/笔记/心情表")]
public class BusTask  : EntityBaseId
{
    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnName = "title", ColumnDescription = "标题", Length = 2000)]
    public string? Title { get; set; }
    
    /// <summary>
    /// 内容
    /// </summary>
    [SugarColumn(ColumnName = "content", ColumnDescription = "内容", Length = 2000)]
    public string? Content { get; set; }

    /// <summary>
    /// 任务开始时间
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户编号")]
    public long UserId { get; set; }

    /// <summary>
    /// 优先级0高，1中，2低
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级0高，1中，2低")]
    public int Priority { get; set; }
    
    /// <summary>
    /// 任务开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "任务开始时间")]
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// 任务结束时间
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "任务结束时间")]
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 图片
    /// </summary>
    [SugarColumn(ColumnName = "picture", ColumnDescription = "图片", Length = 255)]
    public string? Picture { get; set; }

    /// <summary>
    /// 是否完成.
    /// </summary>
    [SugarColumn(ColumnName = "completed", ColumnDescription = "是否完成")]
    public bool Completed { get; set; }

    /// <summary>
    /// emoji表情.
    /// </summary>
    [SugarColumn(ColumnName = "emoji", ColumnDescription = "emoji表情")]
    public string? Emoji { get; set; }

    /// <summary>
    /// 0任务，1笔记，2心情
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "type", ColumnDescription = "0任务，1笔记，2心情")]
    public int Type { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
    public DateTime CreateTime { get; set; }
    
}
