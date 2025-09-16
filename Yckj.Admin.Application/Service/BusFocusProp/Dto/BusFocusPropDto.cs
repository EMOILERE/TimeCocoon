namespace Yckj.Admin.Application;

    /// <summary>
    /// BusFocusProp输出参数
    /// </summary>
    public class BusFocusPropDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 奖励名称
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 奖励描述
        /// </summary>
        public string Desc { get; set; }
        
        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
        
        /// <summary>
        /// 奖励钻石
        /// </summary>
        public int Diamond { get; set; }
        
        /// <summary>
        /// 状态是否启用
        /// </summary>
        public bool Status { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 0时间加速，1专注护盾，2晶石翻倍，3连续加成，4时间暂停，5专注回溯
        /// </summary>
        public int Type { get; set; }
        
    }
