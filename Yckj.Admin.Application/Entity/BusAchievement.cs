using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 
/// </summary>
[SugarTable("bus_achievement","")]
public class BusAchievement  : EntityBaseId
{
    /// <summary>
    /// 成就名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "title", ColumnDescription = "成就名称", Length = 255)]
    public string Title { get; set; }
    
    /// <summary>
    /// 成就描述
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "desc", ColumnDescription = "成就描述", Length = 500)]
    public string Desc { get; set; }
    
    /// <summary>
    /// emoji
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "emoji", ColumnDescription = "emoji", Length = 255)]
    public string Emoji { get; set; }
    
    /// <summary>
    /// 奖励
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "reward", ColumnDescription = "奖励")]
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
    [Required]
    [SugarColumn(ColumnName = "type", ColumnDescription = "")]
    public int Type { get; set; }
    public int Store { get; set; }


}
