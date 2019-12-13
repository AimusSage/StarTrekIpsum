using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StarTrekIpsum.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpsumGeneratorController : ControllerBase
    {
        private readonly ILogger<IpsumGeneratorController> _logger;

        public IpsumGeneratorController(ILogger<IpsumGeneratorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get([FromQuery(Name = "Paragraphs")] int paragraphCount = 5, [FromQuery(Name = "Captain")] StarTrekCaptain captain = StarTrekCaptain.Picard)
        {
            //return new string[] { "hello", "mister" };
            return new StarTrekIpsumGenerator(captain).MultiParagraphGenerator(paragraphCount);
        }
    }
}
