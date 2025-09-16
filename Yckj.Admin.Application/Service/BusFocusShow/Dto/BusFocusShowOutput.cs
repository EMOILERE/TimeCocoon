namespace Yckj.Admin.Application;

/// <summary>
/// BusFocusShow输出参数
/// </summary>
public class BusFocusShowOutput
{
    /// <summary>
    /// id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// user_name
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// focus_progress
    /// </summary>
    public int FocusProgress { get; set; }
    
    /// <summary>
    /// focus_time
    /// </summary>
    public int FocusTime { get; set; }
    
    /// <summary>
    /// quotes
    /// </summary>
    public string Quotes { get; set; }
    
    /// <summary>
    /// desc
    /// </summary>
    public string Desc { get; set; }
    
    /// <summary>
    /// 0专注中1暂停中2休息中3专注成功4专注失败
    /// </summary>
    public int Status { get; set; }
    
    /// <summary>
    /// 失败原因
    /// </summary>
    public string? Reason { get; set; }
    
    /// <summary>
    /// 专注数据
    /// </summary>
    public string TextData { get; set; }
    
    /// <summary>
    /// create_time
    /// </summary>
    public DateTime CreateTime { get; set; }
    
    }
 

