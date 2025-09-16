namespace Yckj.Admin.Application;

/// <summary>
/// BusAchievement输出参数
/// </summary>
public class BusAchievementOutput
{
    /// <summary>
    /// id
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 成就名称
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// 成就描述
    /// </summary>
    public string Desc { get; set; }
    
    /// <summary>
    /// emoji
    /// </summary>
    public string Emoji { get; set; }
    
    /// <summary>
    /// 奖励
    /// </summary>
    public int Reward { get; set; }
    
    /// <summary>
    /// 1.初来乍到
//2.专注新手
//3.专注达人
//4.任务起步
//5.任务管理者
//6.高效执行
//7.笔记大师
//8.情绪记录者
//9.持之以恒
//10.晶石收藏家
//11.深夜守护者
//12.专注大师
    /// </summary>
    public int Type { get; set; }
    public int Store { get; set; }
    
    }
 

