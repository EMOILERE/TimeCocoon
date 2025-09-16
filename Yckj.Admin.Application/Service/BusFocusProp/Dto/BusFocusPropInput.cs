using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusFocusProp基础输入参数
    /// </summary>
    public class BusFocusPropBaseInput
    {
        /// <summary>
        /// 奖励名称
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// 奖励描述
        /// </summary>
        public virtual string Desc { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        public virtual string Image { get; set; }
        
        /// <summary>
        /// 奖励钻石
        /// </summary>
        public virtual int Diamond { get; set; }
        
        /// <summary>
        /// 状态是否启用
        /// </summary>
        public virtual bool Status { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
        
        /// <summary>
        /// 0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯
        /// </summary>
        public virtual int Type { get; set; }
        
    }

    /// <summary>
    /// BusFocusProp分页查询输入参数
    /// </summary>
    public class BusFocusPropInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 奖励名称
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// 奖励描述
        /// </summary>
        public string? Desc { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        public string? Image { get; set; }
        
        /// <summary>
        /// 奖励钻石
        /// </summary>
        public int? Diamond { get; set; }
        
        /// <summary>
        /// 状态是否启用
        /// </summary>
        public bool? Status { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
        
        /// <summary>
        /// 0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯
        /// </summary>
        public int? Type { get; set; }
        
    }

    /// <summary>
    /// BusFocusProp增加输入参数
    /// </summary>
    public class AddBusFocusPropInput : BusFocusPropBaseInput
    {
        /// <summary>
        /// 奖励名称
        /// </summary>
        [Required(ErrorMessage = "奖励名称不能为空")]
        public override string Title { get; set; }
        
        /// <summary>
        /// 奖励描述
        /// </summary>
        [Required(ErrorMessage = "奖励描述不能为空")]
        public override string Desc { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        [Required(ErrorMessage = "图片不能为空")]
        public override string Image { get; set; }
        
        /// <summary>
        /// 奖励钻石
        /// </summary>
        [Required(ErrorMessage = "奖励钻石不能为空")]
        public override int Diamond { get; set; }
        
        /// <summary>
        /// 状态是否启用
        /// </summary>
        [Required(ErrorMessage = "状态是否启用不能为空")]
        public override bool Status { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        [Required(ErrorMessage = "排序不能为空")]
        public override int Sort { get; set; }
        
        /// <summary>
        /// 0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯
        /// </summary>
        [Required(ErrorMessage = "0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯不能为空")]
        public override int Type { get; set; }
        
    }

    /// <summary>
    /// BusFocusProp删除输入参数
    /// </summary>
    public class DeleteBusFocusPropInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusFocusProp更新输入参数
    /// </summary>
    public class UpdateBusFocusPropInput : BusFocusPropBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusFocusProp主键查询输入参数
    /// </summary>
    public class QueryByIdBusFocusPropInput : DeleteBusFocusPropInput
    {

    }

public class UsePropInput
{
    public List<int> TypeList { get; set; }
}
