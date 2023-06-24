using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace p10941_linebot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly LineBotConfig _lineBotConfig;

        public LineBotController(IServiceProvider serviceProvider)
        {
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = _httpContextAccessor.HttpContext;
            _lineBotConfig = new LineBotConfig();
            _lineBotConfig.channelSecret = "ec25b087ffe902bd37eb8c2341a9ad50";
            _lineBotConfig.accessToken = "29zfyjfVreEFI+6j5P9TMlNzFt3RjXTGtlzv8qIgD9ICV1EU7OD3YnjeqRGIIVxnWHgmLpSsuRd+TlK04liOcZSPXYlemmSnG6MEoHWV6P3J3xdfRii+9lby/cQFvgjVl6d/GXzDftdgJQrkHerz7QdB04t89/1O/w1cDnyilFU=";
        }
        
        //完整的路由網址就是 https://xxx/api/linebot/run
        [HttpPost("run")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var events = await _httpContext.Request.GetWebhookEventsAsync(_lineBotConfig.channelSecret);
                var lineMessagingClient = new LineMessagingClient(_lineBotConfig.accessToken);
                var lineBotApp = new LineBotApp(lineMessagingClient);
                await lineBotApp.RunAsync(events);
            }
            catch (Exception ex)
            {
                //需要 Log 可自行加入
                //_logger.LogError(JsonConvert.SerializeObject(ex));
            }
            return Ok();
        }
    }
}
