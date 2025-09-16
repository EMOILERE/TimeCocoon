namespace Yckj.Admin.Application;

    /// <summary>
    /// BusAchievement输出参数
    /// </summary>
    public class BusAchievementDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 成就名称
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 成就描述
        /// </summary>
        public string Desc { get; set; }
        
        /// <summary>
        /// emoji
        /// </summary>
        public string Emoji { get; set; }
        
        /// <summary>
        /// 奖励
        /// </summary>
        public int Reward { get; set; }
        
        /// <summary>
        /// 1.初来乍到
        /// </summary>
        public int Type { get; set; }
        
    }
