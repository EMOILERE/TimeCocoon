using Yckj.Admin.Core;
namespace Yckj.Admin.Application.Entity;

/// <summary>
/// 
/// </summary>
[SugarTable("bus_friend","")]
public class BusFriend  : EntityBaseId
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "")]
    public long UserId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "friend_user_id", ColumnDescription = "")]
    public long FriendUserId { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间")]
    public DateTime CreateTime { get; set; }
    
}
