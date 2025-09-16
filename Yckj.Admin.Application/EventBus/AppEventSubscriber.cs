using Furion.JsonSerialization;
using Yckj.Admin.Core.Const;
using Yckj.Admin.Core.Service;

namespace Yckj.Admin.Application;

/// <summary>
/// 事件订阅
/// </summary>
public class AppEventSubscriber : IEventSubscriber, ISingleton, IDisposable
{
    private readonly SysWxOpenService _sysWxOpenService;
    private readonly IServiceScope _serviceScope;

    public AppEventSubscriber(IServiceScopeFactory scopeFactory
        , SysWxOpenService sysWxOpenService)
    {
        _sysWxOpenService = sysWxOpenService;
        _serviceScope = scopeFactory.CreateScope();
    }

    /// <summary>
    /// 增加异常日志
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [EventSubscribe(EventConst.AddExLog)]
    public async Task CreateExLog(EventHandlerExecutingContext context)
    {
        var rep = _serviceScope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
        await rep.InsertAsync((SysLogEx)context.Source.Payload);
    }

    /// <summary>
    /// 发送异常邮件
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [EventSubscribe(EventConst.SendErrorMail)]
    public async Task SendOrderErrorMail(EventHandlerExecutingContext context)
    {
        //var mailTempPath = Path.Combine(App.WebHostEnvironment.WebRootPath, "Temp\\ErrorMail.tp");
        //var mailTemp = File.ReadAllText(mailTempPath);
        //var mail = await _serviceScope.ServiceProvider.GetRequiredService<IViewEngine>().RunCompileFromCachedAsync(mailTemp, );

        var title = "Yckj.Admin 框架异常";
        await _serviceScope.ServiceProvider.GetRequiredService<SysMessageService>().SendEmail(JSON.Serialize(context.Source.Payload), title, true);
    }

    /// <summary>
    /// 微信模板消息发送
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>

    [EventSubscribe(EventConst.SendWxTemplate)]
    public async Task SendWxTemplate(EventHandlerExecutingContext context)
    {
        var payload = context.Source.Payload;
        var sendList = JsonConvert.DeserializeObject<List<SendSubscribeMessageInput>>(payload.ToString());
        foreach (var item in sendList)
        {
            await _sysWxOpenService.SendSubscribeMessage(item);
        }
    }

    /// <summary>
    /// 释放服务作用域
    /// </summary>
    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}