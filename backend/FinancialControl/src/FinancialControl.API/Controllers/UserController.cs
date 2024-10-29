using FinancialControl.Application.Commands.Users.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());
        
        if (result.Success)
            return Ok(result);
        else
            return BadRequest(result);
    }

}
