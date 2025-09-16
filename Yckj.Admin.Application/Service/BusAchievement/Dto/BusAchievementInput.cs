using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusAchievement基础输入参数
    /// </summary>
    public class BusAchievementBaseInput
    {
        /// <summary>
        /// 成就名称
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// 成就描述
        /// </summary>
        public virtual string Desc { get; set; }
        
        /// <summary>
        /// emoji
        /// </summary>
        public virtual string Emoji { get; set; }
        
        /// <summary>
        /// 奖励
        /// </summary>
        public virtual int Reward { get; set; }
        
        /// <summary>
        /// 1.初来乍到

        /// </summary>
        public virtual int Type { get; set; }
        
    }

    /// <summary>
    /// BusAchievement分页查询输入参数
    /// </summary>
    public class BusAchievementInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 成就名称
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// 成就描述
        /// </summary>
        public string? Desc { get; set; }
        
        /// <summary>
        /// emoji
        /// </summary>
        public string? Emoji { get; set; }
        
        /// <summary>
        /// 奖励
        /// </summary>
        public int? Reward { get; set; }
        
        /// <summary>
        /// 1.初来乍到

        /// </summary>
        public int? Type { get; set; }
        
    }

    /// <summary>
    /// BusAchievement增加输入参数
    /// </summary>
    public class AddBusAchievementInput : BusAchievementBaseInput
    {
        /// <summary>
        /// 成就名称
        /// </summary>
        [Required(ErrorMessage = "成就名称不能为空")]
        public override string Title { get; set; }
        
        /// <summary>
        /// 成就描述
        /// </summary>
        [Required(ErrorMessage = "成就描述不能为空")]
        public override string Desc { get; set; }
        
        /// <summary>
        /// emoji
        /// </summary>
        [Required(ErrorMessage = "emoji不能为空")]
        public override string Emoji { get; set; }
        
        /// <summary>
        /// 奖励
        /// </summary>
        [Required(ErrorMessage = "奖励不能为空")]
        public override int Reward { get; set; }
        
        /// <summary>
        /// 1.初来乍到

        /// </summary>
        [Required(ErrorMessage = "")]
        public override int Type { get; set; }
        
    }

    /// <summary>
    /// BusAchievement删除输入参数
    /// </summary>
    public class DeleteBusAchievementInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusAchievement更新输入参数
    /// </summary>
    public class UpdateBusAchievementInput : BusAchievementBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusAchievement主键查询输入参数
    /// </summary>
    public class QueryByIdBusAchievementInput : DeleteBusAchievementInput
    {

    }
