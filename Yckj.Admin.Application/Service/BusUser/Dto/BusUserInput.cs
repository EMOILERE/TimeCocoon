using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusUser基础输入参数
    /// </summary>
    public class BusUserBaseInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }
        
        /// <summary>
        /// 头像地址
        /// </summary>
        public virtual string Avatar { get; set; }
        
        /// <summary>
        /// 微信openid
        /// </summary>
        public virtual string OpenId { get; set; }
        
        /// <summary>
        /// 专注时间秒
        /// </summary>
        public virtual int FocusTime { get; set; }
        
        /// <summary>
        /// 专注进度上线150
        /// </summary>
        public virtual int FocusProgress { get; set; }
        
        /// <summary>
        /// 钻石数量
        /// </summary>
        public virtual int DiamondNumber { get; set; }
        
        /// <summary>
        /// 连续专注多少天
        /// </summary>
        public virtual int ContinuousNumber { get; set; }
        
        /// <summary>
        /// 级别
        /// </summary>
        public virtual int Level { get; set; }
        
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public virtual DateTime LastTime { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// BusUser分页查询输入参数
    /// </summary>
    public class BusUserInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; set; }
        
        /// <summary>
        /// 头像地址
        /// </summary>
        public string? Avatar { get; set; }
        
        /// <summary>
        /// 微信openid
        /// </summary>
        public string? OpenId { get; set; }
        
        /// <summary>
        /// 专注时间秒
        /// </summary>
        public int? FocusTime { get; set; }
        
        /// <summary>
        /// 专注进度上线150
        /// </summary>
        public int? FocusProgress { get; set; }
        
        /// <summary>
        /// 钻石数量
        /// </summary>
        public int? DiamondNumber { get; set; }
        
        /// <summary>
        /// 连续专注多少天
        /// </summary>
        public int? ContinuousNumber { get; set; }
        
        /// <summary>
        /// 级别
        /// </summary>
        public int? Level { get; set; }
        
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastTime { get; set; }
        
        /// <summary>
         /// 最后登录时间范围
         /// </summary>
         public List<DateTime?> LastTimeRange { get; set; } 
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
    /// BusUser增加输入参数
    /// </summary>
    public class AddBusUserInput : BusUserBaseInput
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空")]
        public override string NickName { get; set; }
        
        /// <summary>
        /// 头像地址
        /// </summary>
        [Required(ErrorMessage = "头像地址不能为空")]
        public override string Avatar { get; set; }
        
        /// <summary>
        /// 微信openid
        /// </summary>
        [Required(ErrorMessage = "微信openid不能为空")]
        public override string OpenId { get; set; }
        
        /// <summary>
        /// 专注时间秒
        /// </summary>
        [Required(ErrorMessage = "专注时间秒不能为空")]
        public override int FocusTime { get; set; }
        
        /// <summary>
        /// 专注进度上线150
        /// </summary>
        [Required(ErrorMessage = "专注进度上线150不能为空")]
        public override int FocusProgress { get; set; }
        
        /// <summary>
        /// 钻石数量
        /// </summary>
        [Required(ErrorMessage = "钻石数量不能为空")]
        public override int DiamondNumber { get; set; }
        
        /// <summary>
        /// 连续专注多少天
        /// </summary>
        [Required(ErrorMessage = "连续专注多少天不能为空")]
        public override int ContinuousNumber { get; set; }
        
        /// <summary>
        /// 级别
        /// </summary>
        [Required(ErrorMessage = "级别不能为空")]
        public override int Level { get; set; }
        
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Required(ErrorMessage = "最后登录时间不能为空")]
        public override DateTime LastTime { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required(ErrorMessage = "创建时间不能为空")]
        public override DateTime CreateTime { get; set; }
        
    }

    /// <summary>
    /// BusUser删除输入参数
    /// </summary>
    public class DeleteBusUserInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusUser更新输入参数
    /// </summary>
    public class UpdateBusUserInput : BusUserBaseInput
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [Required(ErrorMessage = "唯一编号不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusUser主键查询输入参数
    /// </summary>
    public class QueryByIdBusUserInput : DeleteBusUserInput
    {

    }


public class BusUserLoginInput
{
    /// <summary>
    /// Code
    /// </summary>
    [Required(ErrorMessage = "Code不能为空"), MinLength(10, ErrorMessage = "Code错误")]
    public string Code { get; set; }
}

public class BusUserUpdateInput
{
    public string Avatar { get; set; }
    public string NickName { get; set; }
}
public class BusFocus
{
    public int CompletionReward { get; set; }
    public int Minutes { get; set; }
    public int Type { get; set; } = 0;
}

public class RankingInput
{ 
    public int Type { get; set; }
}