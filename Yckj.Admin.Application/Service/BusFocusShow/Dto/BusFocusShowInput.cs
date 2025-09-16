using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusFocusShow基础输入参数
    /// </summary>
    public class BusFocusShowBaseInput
    {
        /// <summary>
        /// user_name
        /// </summary>
        public virtual string UserName { get; set; }
        
        /// <summary>
        /// focus_progress
        /// </summary>
        public virtual int FocusProgress { get; set; }
        
        /// <summary>
        /// focus_time
        /// </summary>
        public virtual int FocusTime { get; set; }
        
        /// <summary>
        /// quotes
        /// </summary>
        public virtual string Quotes { get; set; }
        
        /// <summary>
        /// desc
        /// </summary>
        public virtual string Desc { get; set; }
        
        /// <summary>
        /// 0专注中1暂停中2休息中3专注成功4专注失败
        /// </summary>
        public virtual int Status { get; set; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public virtual string? Reason { get; set; }
        
        /// <summary>
        /// 专注数据
        /// </summary>
        public virtual string TextData { get; set; }
        
        /// <summary>
        /// create_time
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    public virtual string Progress { get; set; }


    }

    /// <summary>
    /// BusFocusShow分页查询输入参数
    /// </summary>
    public class BusFocusShowInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// user_name
        /// </summary>
        public string? UserName { get; set; }
        
        /// <summary>
        /// focus_progress
        /// </summary>
        public int? FocusProgress { get; set; }
        
        /// <summary>
        /// focus_time
        /// </summary>
        public int? FocusTime { get; set; }
        
        /// <summary>
        /// quotes
        /// </summary>
        public string? Quotes { get; set; }
        
        /// <summary>
        /// desc
        /// </summary>
        public string? Desc { get; set; }
        
        /// <summary>
        /// 0专注中1暂停中2休息中3专注成功4专注失败
        /// </summary>
        public int? Status { get; set; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public string? Reason { get; set; }
        
        /// <summary>
        /// 专注数据
        /// </summary>
        public string? TextData { get; set; }
        
        /// <summary>
        /// create_time
        /// </summary>
        public DateTime? CreateTime { get; set; }
        
        /// <summary>
         /// create_time范围
         /// </summary>
         public List<DateTime?> CreateTimeRange { get; set; } 
    }

    /// <summary>
    /// BusFocusShow增加输入参数
    /// </summary>
    public class AddBusFocusShowInput : BusFocusShowBaseInput
    {

        
        /// <summary>
        /// focus_progress
        /// </summary>
        [Required(ErrorMessage = "focus_progress不能为空")]
        public override int FocusProgress { get; set; }
        
        /// <summary>
        /// focus_time
        /// </summary>
        [Required(ErrorMessage = "focus_time不能为空")]
        public override int FocusTime { get; set; }
        

        
        /// <summary>
        /// 0专注中1暂停中2休息中3专注成功4专注失败
        /// </summary>
        [Required(ErrorMessage = "0专注中1暂停中2休息中3专注成功4专注失败不能为空")]
        public override int Status { get; set; }
        
        /// <summary>
        /// 专注数据
        /// </summary>
        [Required(ErrorMessage = "专注数据不能为空")]
        public override string TextData { get; set; }
        

        
    }

    /// <summary>
    /// BusFocusShow删除输入参数
    /// </summary>
    public class DeleteBusFocusShowInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusFocusShow更新输入参数
    /// </summary>
    public class UpdateBusFocusShowInput : BusFocusShowBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusFocusShow主键查询输入参数
    /// </summary>
    public class QueryByIdBusFocusShowInput : DeleteBusFocusShowInput
    {

    }
