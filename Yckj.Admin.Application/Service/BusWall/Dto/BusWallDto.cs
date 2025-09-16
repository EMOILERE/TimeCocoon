namespace Yckj.Admin.Application;

    /// <summary>
    /// BusWall输出参数
    /// </summary>
    public class BusWallDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// 0忏悔墙，1灵感墙
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
    }
