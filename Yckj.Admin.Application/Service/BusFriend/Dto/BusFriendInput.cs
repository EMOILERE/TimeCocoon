using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusFriend基础输入参数
    /// </summary>
    public class BusFriendBaseInput
    {
        /// <summary>
        /// user_id
        /// </summary>
        public virtual long UserId { get; set; }
        
        /// <summary>
        /// friend_user_id
        /// </summary>
        public virtual long FriendUserId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// BusFriend分页查询输入参数
    /// </summary>
    public class BusFriendInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// user_id
        /// </summary>
        public long? UserId { get; set; }
        
        /// <summary>
        /// friend_user_id
        /// </summary>
        public long? FriendUserId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
         /// 创建时间范围
         /// </summary>
         public List<DateTime?> CreateTimeRange { get; set; } 
    }

    /// <summary>
    /// BusFriend增加输入参数
    /// </summary>
    public class AddBusFriendInput : BusFriendBaseInput
    {
        /// <summary>
        /// user_id
        /// </summary>
        [Required(ErrorMessage = "user_id不能为空")]
        public override long UserId { get; set; }
        
        /// <summary>
        /// friend_user_id
        /// </summary>
        [Required(ErrorMessage = "friend_user_id不能为空")]
        public override long FriendUserId { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public override DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// BusFriend删除输入参数
    /// </summary>
    public class DeleteBusFriendInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusFriend更新输入参数
    /// </summary>
    public class UpdateBusFriendInput : BusFriendBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusFriend主键查询输入参数
    /// </summary>
    public class QueryByIdBusFriendInput : DeleteBusFriendInput
    {

    }
