using Yckj.Admin.Core;
using System.ComponentModel.DataAnnotations;

namespace Yckj.Admin.Application;

    /// <summary>
    /// BusWall基础输入参数
    /// </summary>
    public class BusWallBaseInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        
        
        /// <summary>
        /// 0忏悔墙，1灵感墙
        /// </summary>
        public virtual int Type { get; set; }
        
    }

    /// <summary>
    /// BusWall分页查询输入参数
    /// </summary>
    public class BusWallInput : BasePageInput
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
        /// 用户id
        /// </summary>
        public long? UserId { get; set; }
        
        /// <summary>
        /// 0忏悔墙，1灵感墙
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
    /// BusWall增加输入参数
    /// </summary>
    public class AddBusWallInput : BusWallBaseInput
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        public override string Title { get; set; }
        /// <summary>
        /// 0忏悔墙，1灵感墙
        /// </summary>
        [Required(ErrorMessage = "0忏悔墙，1灵感墙不能为空")]
        public override int Type { get; set; }
        
        
    }

    /// <summary>
    /// BusWall删除输入参数
    /// </summary>
    public class DeleteBusWallInput : BaseIdInput
    {
    }

    /// <summary>
    /// BusWall更新输入参数
    /// </summary>
    public class UpdateBusWallInput : BusWallBaseInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required(ErrorMessage = "id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// BusWall主键查询输入参数
    /// </summary>
    public class QueryByIdBusWallInput : DeleteBusWallInput
    {

    }


public class BusWallListInput
{
    public int Type { get; set; }
    public int Day { get; set; }
}

public class BusSendText
{ 
    public string text { get; set; }
    public int Type { get; set; } = 0;
    public string RoleText { get; set; }
    public bool Allow { get; set; } = false;
    public List<HistoryMessage> historyMessage { get; set; }
    public string ImageUrl { get; set; }

}