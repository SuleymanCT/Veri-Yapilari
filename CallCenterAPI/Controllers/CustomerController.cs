using CallCenterAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    private readonly CallCenterSimulator _sim;

    public CustomerController(CallCenterSimulator sim)
    {
        _sim = sim;
    }

    [HttpPost("add")]
    public IActionResult AddCustomer([FromBody] string name)
    {
        _sim.AddCustomer(name);
        return Ok();
    }

    [HttpGet("queue")]
    public IActionResult GetQueue()
    {
        var list = _sim.GetCustomerNames();
        return Ok(list);
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(_sim.GetRepresentativeStatuses());
    }

    [HttpGet("trees")]
    public IActionResult GetCustomerTrees()
    {
        var trees = _sim.GetRepresentativeTrees();
        return Ok(trees);
    }

}

