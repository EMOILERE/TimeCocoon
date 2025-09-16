namespace Yckj.Admin.Application;

/// <summary>
/// BusTask输出参数
/// </summary>
public class BusTaskOutput
{
    /// <summary>
    /// id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }
    
    /// <summary>
    /// 优先级0高，1中，2低
    /// </summary>
    public int Priority { get; set; }
    
    /// <summary>
    /// 任务开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// 任务结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    
    /// <summary>
    /// 图片
    /// </summary>
    public string? Picture { get; set; }
    
    /// <summary>
    /// 0任务，1笔记，2心情
    /// </summary>
    public int Type { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    public bool Completed { get; set; }
    public string? Emoji { get; set; }

}

public class BusTaskStatisticsOutput
{ 
    public int Type { get; set; }
    public int WCount { get; set; }
    public int YCount { get; set; }
}
