using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusTask基础输入参数
    /// </summary>
    public class BusTaskBaseInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string? Title { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string? Content { get; set; }
        
        /// <summary>
        /// 优先级0高，1中，2低
        /// </summary>
        public virtual int Priority { get; set; }
        
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public virtual DateTime? StartTime { get; set; }
        
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        public virtual string? Picture { get; set; }
        
        /// <summary>
        /// 0任务，1笔记，2心情
        /// </summary>
        public virtual int Type { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
    public virtual bool Completed { get; set; }
    public virtual string? Emoji { get; set; }

}

    /// <summary>
    /// BusTask分页查询输入参数
    /// </summary>
    public class BusTaskInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }
        
        /// <summary>
        /// 优先级0高，1中，2低
        /// </summary>
        public int? Priority { get; set; }
        
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        
        /// <summary>
         /// 任务开始时间范围
         /// </summary>
         public List<DateTime?> StartTimeRange { get; set; } 
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        
        /// <summary>
         /// 任务结束时间范围
         /// </summary>
         public List<DateTime?> EndTimeRange { get; set; } 
        /// <summary>
        /// 图片
        /// </summary>
        public string? Picture { get; set; }
        
        /// <summary>
        /// 0任务，1笔记，2心情
        /// </summary>
        public int? Type { get; set; }
        
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
    /// BusTask增加输入参数
    /// </summary>
    public class AddBusTaskInput : BusTaskBaseInput
    {
        /// <summary>
        /// 优先级0高，1中，2低
        /// </summary>
        [Required(ErrorMessage = "优先级0高，1中，2低不能为空")]
        public override int Priority { get; set; }=0;
        
        /// <summary>
        /// 0任务，1笔记，2心情
        /// </summary>
        [Required(ErrorMessage = "0任务，1笔记，2心情不能为空")]
        public override int Type { get; set; }
        
    }

    /// <summary>
    /// BusTask删除输入参数
    /// </summary>
    public class DeleteBusTaskInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusTask更新输入参数
    /// </summary>
    public class UpdateBusTaskInput : BusTaskBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusTask主键查询输入参数
    /// </summary>
    public class QueryByIdBusTaskInput : DeleteBusTaskInput
    {

    }

public class BusStatisticsInput
{
    public int Type { get; set; }
}