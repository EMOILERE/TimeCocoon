using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusFocusLog基础输入参数
    /// </summary>
    public class BusFocusLogBaseInput
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public virtual long UserId { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string NickName { get; set; }
        
        /// <summary>
        /// 日期
        /// </summary>
        public virtual string Time { get; set; }
        
        /// <summary>
        /// 0专注成功 1专注失败
        /// </summary>
        public virtual bool Status { get; set; }
        
        /// <summary>
        /// 专注时长
        /// </summary>
        public virtual int Duration { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// BusFocusLog分页查询输入参数
    /// </summary>
    public class BusFocusLogInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public long? UserId { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        public string? NickName { get; set; }
        
        /// <summary>
        /// 日期
        /// </summary>
        public string? Time { get; set; }
        
        /// <summary>
        /// 0专注成功 1专注失败
        /// </summary>
        public bool? Status { get; set; }
        
        /// <summary>
        /// 专注时长
        /// </summary>
        public int? Duration { get; set; }
        
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
    /// BusFocusLog增加输入参数
    /// </summary>
    public class AddBusFocusLogInput : BusFocusLogBaseInput
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        [Required(ErrorMessage = "用户编码不能为空")]
        public override long UserId { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        public override string NickName { get; set; }
        
        /// <summary>
        /// 日期
        /// </summary>
        [Required(ErrorMessage = "日期不能为空")]
        public override string Time { get; set; }
        
        /// <summary>
        /// 0专注成功 1专注失败
        /// </summary>
        [Required(ErrorMessage = "0专注成功 1专注失败不能为空")]
        public override bool Status { get; set; }
        
        /// <summary>
        /// 专注时长
        /// </summary>
        [Required(ErrorMessage = "专注时长不能为空")]
        public override int Duration { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public override DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// BusFocusLog删除输入参数
    /// </summary>
    public class DeleteBusFocusLogInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusFocusLog更新输入参数
    /// </summary>
    public class UpdateBusFocusLogInput : BusFocusLogBaseInput
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [Required(ErrorMessage = "唯一编号不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusFocusLog主键查询输入参数
    /// </summary>
    public class QueryByIdBusFocusLogInput : DeleteBusFocusLogInput
    {

    }
