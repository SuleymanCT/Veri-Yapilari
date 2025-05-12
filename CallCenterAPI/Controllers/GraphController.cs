using Microsoft.AspNetCore.Mvc;
using CallCenterAPI.Services;

namespace CallCenterAPI.Controllers
{
    [ApiController]
    [Route("api/graph")]
    public class GraphController : ControllerBase
    {
        private readonly GraphSimulator _sim;

        public GraphController(GraphSimulator sim)
        {
            _sim = sim;
        }

        [HttpGet("structure")]
        public IActionResult GetGraph()
        {
            return Ok(_sim.GetGraphStructure());
        }
        [HttpGet("recent-transfers")]
        public IActionResult GetTransfers()
        {
            return Ok(_sim.GetAndClearTransfers());
        }

    }
}
