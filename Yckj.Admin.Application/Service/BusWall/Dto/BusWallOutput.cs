namespace Yckj.Admin.Application;

/// <summary>
/// BusWall输出参数
/// </summary>
public class BusWallOutput
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


public class BusWallCountOutput
{
    public int CCount { get; set; }
    public int LCount { get; set; }
    
}
// 配套的DTO类
public class FocusRecordRequest
{
    public string UserId { get; set; }
    public string DeviceType { get; set; }
    public List<FocusRecord> Records { get; set; }
}

public class FocusRecord
{
    public DateTime Time { get; set; }
    public string Scene { get; set; } // 场景描述
    public string Content { get; set; } // 忏悔内容
}

public class AnalysisResult
{
    public FocusAnalysis Analysis { get; set; }
    public ImprovementPlan Solutions { get; set; }
}

public class FocusAnalysis
{
    public Dictionary<string, double> InterruptionSources { get; set; }
    public List<string> HighRiskTimeSlots { get; set; }
    public List<string> BehaviorChains { get; set; }
    public string CoreIssue { get; set; }
}

public class ImprovementPlan
{
    public List<string> ImmediateActions { get; set; }
    public List<string> LongTermHabits { get; set; }
}

public class Choices
{
    public ChoicesMessage message { get; set; }
}

public class ChoicesMessage
{
    public string Content { get; set; }
}

public class AiData
{
    public List<Choices> choices { get; set; }
    public string created { get; set; }
    public string id { get; set; }
    public string model { get; set; }
}

public class ConversationPrompt
{
    public string SystemInstruction { get; set; } = "";
    public List<Message> History { get; set; } = new();
    public string CurrentQuestion { get; set; } = "";
}

public class Message
{
    public string Role { get; set; } = ""; // "user"或"assistant"
    public string Content { get; set; } = "";
    public DateTime Timestamp { get; set; }
}

public class HistoryMessage
{
    public int Type { get; set; } 
    public string Content { get; set; }
}