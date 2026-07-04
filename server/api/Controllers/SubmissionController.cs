using core;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubmissionController : ControllerBase
    {
        [HttpGet(Name = "GetSubmissions")]
        public ActionResult<IEnumerable<Submission>> Get()
        {
            return Ok();
        }
    }
}
