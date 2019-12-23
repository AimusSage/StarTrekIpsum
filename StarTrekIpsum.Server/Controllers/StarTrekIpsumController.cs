using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StarTrekIpsum.Server.Ipsum;

namespace StarTrekIpsum.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpsumsController : ControllerBase
    {
        private readonly IStarTrekIpsumGenerator _starTrekIpsumGenerator;
        private readonly ILogger<IpsumsController> _logger;

        public IpsumsController(IStarTrekIpsumGenerator starTrekIpsumGenerator, ILogger<IpsumsController> logger)
        {
            _starTrekIpsumGenerator = starTrekIpsumGenerator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get([FromQuery(Name = "Paragraphs")] int paragraphCount = 5, [FromQuery(Name = "Captain")] StarTrekCaptain captain = StarTrekCaptain.Picard)
        {
            return await _starTrekIpsumGenerator.MultiParagraphGenerator(paragraphCount, captain);
        }
    }
}
