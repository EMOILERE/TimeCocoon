namespace Yckj.Admin.Application;

/// <summary>
/// BusFocusLog输出参数
/// </summary>
public class BusFocusLogOutput
{
    /// <summary>
    /// 唯一编号
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 用户编码
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// 姓名
    /// </summary>
    public string NickName { get; set; }
    
    /// <summary>
    /// 日期
    /// </summary>
    public string Time { get; set; }
    
    /// <summary>
    /// 0专注成功 1专注失败
    /// </summary>
    public bool Status { get; set; }
    
    /// <summary>
    /// 专注时长
    /// </summary>
    public int Duration { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    
    }
 

