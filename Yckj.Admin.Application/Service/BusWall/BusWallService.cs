using AspectCore.Extensions.Reflection;
using Furion.ClayObject;
using Furion.RemoteRequest.Extensions;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using TencentCloud.Bsca.V20210811.Models;
using TencentCloud.Iot.V20180123;
using Yckj.Admin.Application.Const;
using System.Text;
using COSXML.Network;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Yckj.Admin.Core.Util;
using Yckj.Admin.Core.Service;

namespace Yckj.Admin.Application;

/// <summary>
/// BusWall服务
/// </summary>
public class BusWallService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<BusWall> _rep;
    private readonly SqlSugarRepository<BusTask> _repBusTask;
    private readonly UserManager _userManager;
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SysCacheService _sysCacheService;

    public BusWallService(SqlSugarRepository<BusWall> rep, UserManager userManager, IHttpContextAccessor httpContextAccessor, SysCacheService sysCacheService, SqlSugarRepository<BusTask> repBusTask)
    {
        _rep = rep;
        _userManager = userManager;
        _httpClient = new System.Net.Http.HttpClient();
        _httpContextAccessor = httpContextAccessor;
        _sysCacheService = sysCacheService;
        _repBusTask = repBusTask;
    }

    /// <summary>
    /// 分页查询BusWall
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<BusWallOutput>> Page(BusWallInput input)
    {
        var query = _rep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.SearchKey), u =>
                u.Title.Contains(input.SearchKey.Trim())
            )
            .WhereIF(!string.IsNullOrWhiteSpace(input.Title), u => u.Title.Contains(input.Title.Trim()))
            .WhereIF(input.UserId > 0, u => u.UserId == input.UserId)
            .WhereIF(input.Type > 0, u => u.Type == input.Type)
            .Select<BusWallOutput>()
;
        if (input.CreateTimeRange != null && input.CreateTimeRange.Count > 0)
        {
            DateTime? start = input.CreateTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreateTime > start);
            if (input.CreateTimeRange.Count > 1 && input.CreateTimeRange[1].HasValue)
            {
                var end = input.CreateTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreateTime < end);
            }
        }
        query = query.OrderBuilder(input, "", "CreateTime");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 更新BusWall
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateBusWallInput input)
    {
        var entity = input.Adapt<BusWall>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取BusWall
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    [ExtAuth]
    public async Task<BusWall> Get([FromQuery] QueryByIdBusWallInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 删除BusWall
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    [ExtAuth]
    public async Task Delete(DeleteBusWallInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id && u.UserId == _userManager.UserId) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //真删除
    }

    /// <summary>
    /// 增加BusWall
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    [ExtAuth]
    public async Task Add(AddBusWallInput input)
    {
        var entity = input.Adapt<BusWall>();
        entity.CreateTime = DateTime.Now;
        entity.UserId = _userManager.UserId;
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 获取BusWall列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    [ExtAuth]
    public async Task<List<BusWallOutput>> List([FromQuery] BusWallListInput input)
    {
        var day = input.Day == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        return await _rep.AsQueryable()
            .Where(it => it.Type == input.Type)
            .Where(it => it.UserId == _userManager.UserId)
            .Where(it => it.CreateTime <= time)
            .Select<BusWallOutput>()
            .OrderBy(it => it.CreateTime, OrderByType.Desc)
            .ToListAsync();
    }

    /// <summary>
    /// 获取BusWall列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "GetCount")]
    [ExtAuth]
    public async Task<BusWallCountOutput> GetCount([FromQuery] BusWallListInput input)
    {
        var day = input.Day == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var list = await _rep.AsQueryable()
            .Where(it => it.UserId == _userManager.UserId)
            .Where(it => it.CreateTime <= time)
            .Select<BusWallOutput>()
            .ToListAsync();
        return new BusWallCountOutput
        {
            CCount = list.Where(it => it.Type == 0).Count(),
            LCount = list.Where(it => it.Type == 1).Count(),
        };
    }

    private string ApiUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions";
    private string ApiKey = "sk-8b654ec58c4c49f6a30cfb3d555a95d0";

    private string GetFocusPrompt()
    {
        return @"你是一个专注力分析专家，请根据用户提供的专注失败原因，生成一份结构化的分析报告，格式如下：
```json
{
  ""summary"": ""📊 [总结性标题，带emoji]"",
  ""patterns"": [
    {
      ""emoji"": ""⏰"",
      ""title"": ""[问题分类名称]"",
      ""percent"": ""[占比，如'35%']"",
      ""detail"": ""[详细分析，说明具体表现和改进方向]""
    },
    // 其他 patterns...
  ],
  ""suggestions"": [
    {
      ""emoji"": ""📝"",
      ""title"": ""[建议标题]"",
      ""content"": ""[具体建议内容]""
    },
    // 其他 suggestions...
  ],
  ""actionPlan"": ""📋 [分阶段行动方案，用换行符分隔步骤]""
}

**分析要求：**
1. **归类问题**：将用户的失败原因归类为 3 种主要模式（如时间管理、设备干扰、生理状态等）。
2. **分配权重**：根据输入原因的频率和严重性，合理分配百分比（总和 100%）。
3. **详细分析**：每个问题模式需包含具体表现和改进方向。
4. **实用建议**：提供 3 条可落地的改进策略，每条带 emoji 和标题。
5. **行动方案**：分阶段（如第一周、第二周、长期）给出具体执行步骤。

**用户输入示例：**
- ""经常刷短视频""
- ""计划总是完不成""
- ""下午容易犯困""
- ""工作时总被消息打扰""

**返回示例：**
{
  ""summary"": ""📊 深度专注失败模式分析"",
  ""patterns"": [
    {
      ""emoji"": ""📱"",
      ""title"": ""数字设备依赖"",
      ""percent"": ""40%"",
      ""detail"": ""频繁查看手机导致专注中断，平均每20分钟会分心一次。建议设置勿扰模式，并采用专注工具限制使用。""
    },
    // 其他 patterns...
  ],
  ""suggestions"": [
    {
      ""emoji"": ""🔕"",
      ""title"": ""设备管控策略"",
      ""content"": ""使用Forest等专注App锁定手机，设定每天2-3个无干扰专注时段。""
    },
    // 其他 suggestions...
  ],
  ""actionPlan"": ""📋 行动方案：\n1. 第一周：记录分心次数\n2. 第二周：试行专注时段\n3. 长期：培养无手机工作习惯""
}

**请根据用户实际输入生成分析报告。**";
    }

    private string GetInspPrompt()
    {
        return @"```json{
  ""instruction"": ""你是一个专业的创新项目分析师，请根据用户提交的灵感文本，生成结构化可行性分析报告。要求："",
  ""requirements"": {
    ""format"": {
      ""response"": ""必须返回严格符合以下JSON结构的数组，每条分析包含emoji前缀字段"",
      ""template"": [
        {
          ""summary"": ""💡/🚀/🌟/🔮 开头 + 简短总结"",
          ""categories"": [
            {""emoji"": ""📚/⚡/🎯等"", ""name"": ""分类名称"", ""count"": ""该分类下的灵感数量""}
          ],
          ""feasibilityAnalysis"": [
            {
              ""emoji"": ""📱/🤖/🎯等"",
              ""content"": ""用户输入的灵感原文"",
              ""feasibility"": ""0-100的可行性评分"",
              ""requirements"": [""技术能力"", ""市场调研"", ""用户反馈等""],
              ""timeEstimate"": ""X-X个月""
            }
          ],
          ""priorityList"": ""1. 第一步\n2. 第二步\n3. 第三步""
        }
      ]
    },
    ""content_rules"": {
      ""feasibility"": ""评分需结合技术难度、市场需求、资源投入综合判断"",
      ""timeEstimate"": ""根据项目复杂度给出合理范围"",
      ""priorityList"": ""分3步，按‘开发→测试→迭代’逻辑排列""
    }
  },
  ""examples"": [
    {
      ""input"": ""开发一个AI健身教练"",
      ""output"": {
        ""summary"": ""🤖 AI健身助手项目"",
        ""categories"": [
          {""emoji"": ""💻"", ""name"": ""技术创新"", ""count"": 1},
          {""emoji"": ""🏋️"", ""name"": ""健康管理"", ""count"": 1}
        ],
        ""feasibilityAnalysis"": [
          {
            ""emoji"": ""🤖"",
            ""content"": ""开发一个AI健身教练"",
            ""feasibility"": 78,
            ""requirements"": [""运动识别算法"", ""健身知识库"", ""用户数据安全""],
            ""timeEstimate"": ""5-7个月""
          }
        ],
        ""priorityList"": ""1. 搭建基础运动模型\n2. 设计健身计划模块\n3. 进行Beta测试""
      }
    }
  ],
  ""user_input"": ""请分析以下灵感：{{用户输入的灵感文本}}""
}";
    }

    private async Task<string> GetStrictAnalysis(List<string> records, string content, int retry = 3)
    {
        string lastError = "";
        for (int i = 0; i < retry; i++)
        {
            try
            {
                var response = await ApiUrl
                    .SetHeaders(new
                    {
                        Authorization = $"Bearer {ApiKey}",
                        Content_Type = "application/json"
                    })
                    .SetBody(new
                    {
                        model = "qwen-turbo",
                        messages = new[] {
                        new { role = "system", content = content },
                        new { role = "user", content = $"分析以下忏悔记录：\n{string.Join("\n", records.Select((r, i) => $"{i+1}. {r}"))}" }
                        },
                        response_format = new { type = "json_object" },
                        temperature = 1,  // 适当提高创造性
                        top_k = 100,         // 扩大候选范围
                        seed = 100           // 固定随机种子
                    })
                    .PostAsStringAsync();  // 直接解析为JObject

                // 验证响应完整性

                return response.ToString();
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                await Task.Delay(500 * (i + 1)); // 指数退避
            }
        }
        throw Oops.Oh($"分析失败，最后错误：{lastError}");
    }

    /// <summary>
    /// 获取BusWall列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Analysis")]
    [ExtAuth]
    public async Task<AiData> Analysis([FromQuery] BusWallListInput input)
    {
        var day = input.Day == 0 ? 7 : 31;
        var time = DateTime.Now.AddDays(day);
        var list = await _rep.AsQueryable()
            .Where(it => it.UserId == _userManager.UserId)
            .Where(it => it.Type == input.Type)
            .Where(it => it.CreateTime <= time)
            .Select<BusWallOutput>()
            .ToListAsync();
        if (list.Count == 0)
        {
            throw Oops.Oh($"暂无忏悔内容，请先创建忏悔内容");
        }
        var records = list.Select(it => it.Title).ToList();
        try
        {
            var content = "";
            if (input.Type == 0) content = GetFocusPrompt();
            else content = GetInspPrompt();
            var str = await GetStrictAnalysis(records, content);
            return JsonConvert.DeserializeObject<AiData>(str);
        }
        catch (Exception)
        {
            throw Oops.Oh($"分析失败，请稍后再试");
        }
    }

    private static string HistoryFormatter(List<Message> history, int maxRounds = 5)
    {
        var sb = new StringBuilder();
        sb.AppendLine("## 对话历史");

        foreach (var msg in history.TakeLast(maxRounds * 2)) // 保留最近5轮
        {
            sb.AppendLine($"{msg.Timestamp:HH:mm} {msg.Role}: {msg.Content}");
        }

        return sb.ToString();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task StreamChatAsync([FromBody] BusSendText busSendText)
    {
        _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
        _httpContextAccessor.HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
        _httpContextAccessor.HttpContext.Response.Headers.Add("Connection", "keep-alive");

        var historyList = new List<Message>();
        foreach (var msg in busSendText.historyMessage)
        {
            historyList.Add(new Message
            {
                Role = msg.Type == 0 ? "assistant" : "user",
                Content = msg.Content,
                Timestamp = DateTime.Now,
            });
        }

        var busTasks = await _repBusTask.GetListAsync(it => it.UserId == _userManager.UserId);
        var taskList = busTasks.Where(it => it.Type == 0).OrderByDescending(it => it.CreateTime).Take(5).ToList();
        var noteList = busTasks.Where(it => it.Type == 1).OrderByDescending(it => it.CreateTime).Take(5).ToList();
        var moodList = busTasks.Where(it => it.Type == 2).OrderByDescending(it => it.CreateTime).Take(5).ToList();

        string content = "";
        if (busSendText.Type == 0 || busSendText.Type == 2 || (busSendText.Type == 3 && busSendText.Allow))
        {
            content += "## 回答的问题必须结合我的心情和笔记数据\n";
            content += "## 我的心情数据";
            foreach (var item in moodList)
            {
                content += "\n" + item.Emoji + item.Content;
            }
            content += "## 我的笔记数据据";
            foreach (var item in noteList)
            {
                content += "\n" + $"笔记标题：{item.Title},笔记内容：{item.Content}";
            }
        }
        if (busSendText.Type == 1 || (busSendText.Type == 3 && busSendText.Allow))
        {
            content += "## 任务数据\n";
            foreach (var item in taskList)
            {
                var completedText = item.Completed ? "已完成" : "未完成";
                content += "\n" + @$"记录一个新任务 标题：{item.Title},任务内容：{item.Content},是否完成：{completedText}";
            }

        }

        var conversationPrompt = new ConversationPrompt
        {
            SystemInstruction = SystemInstructionFactory.Create(busSendText.Type, content, busSendText.RoleText),
            CurrentQuestion = busSendText.text,
        };
        string prompt = @$"{conversationPrompt.SystemInstruction}
                       {(historyList.Count > 0 ? HistoryFormatter(historyList) : "")}
                        ## 当前问题
                        {conversationPrompt.CurrentQuestion}
                      ";

        // 构建请求体 - 移除了音频相关参数
        var requestBody = new
        {
            model = "qwen-omni-turbo",
            messages = new[]
            {
            new
            {
                role = "user",
                content = new object[]
                {
                    busSendText.ImageUrl != null ? new
                    {
                        type = "image_url",
                        image_url = new
                        {
                            url = busSendText.ImageUrl
                        }
                    } : null,
                    new
                    {
                        type = "text",
                        text = prompt
                    }
                }.Where(x => x != null).ToArray()
            }
        },
            stream = true,
            stream_options = new
            {
                include_usage = true
            }
            // 移除了modalities和audio参数
        };

        historyList.Add(new Message()
        {
            Role = "user",
            Content = busSendText.text,
            Timestamp = DateTime.Now,
        });

        var request = new HttpRequestMessage(HttpMethod.Post, "https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        request.Headers.Add("X-DashScope-SSE", "enable");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

        request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
        {
            response.EnsureSuccessStatusCode();

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith(":")) continue;
                    if (line.StartsWith("id:"))
                    {
                        var eventId = line.Substring(3).Trim();
                        continue;
                    }

                    if (line.StartsWith("data:"))
                    {
                        var json = line.Substring(5).Trim();
                        if (json == "[DONE]")
                        {
                            break;
                        }

                        try
                        {
                            var result = JsonConvert.DeserializeObject<dynamic>(json);

                            // 简化后的响应处理 - 只处理文本内容
                            if (result?.choices != null)
                            {
                                foreach (var choice in result.choices)
                                {
                                    var delta = choice?.delta;
                                    if (delta?.content != null)
                                    {
                                        await _httpContextAccessor.HttpContext.Response.WriteAsync(delta.content.ToString() as string);
                                        await _httpContextAccessor.HttpContext.Response.Body.FlushAsync();
                                    }
                                }
                            }
                            else if (result?.usage != null)
                            {
                                // 处理使用量统计
                                var usage = result.usage;
                                // 可以记录token使用情况等
                            }

                            // 处理结束标志
                            if (result?.choices?[0]?.finish_reason != null)
                            {
                                var finishReason = result.choices[0].finish_reason.ToString();
                                if (finishReason == "stop")
                                {
                                    historyList.Add(new Message()
                                    {
                                        Role = "assistant",
                                        Content = result.choices[0]?.delta?.content?.ToString(),
                                        Timestamp = DateTime.Now,
                                    });
                                    break;
                                }
                                else if (finishReason != "null")
                                {
                                    throw Oops.Oh($"流结束原因: {finishReason}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw Oops.Oh($"处理响应时出错: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}