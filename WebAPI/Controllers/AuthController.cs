using Jakub_Zajega_14987.Application.Contracts.Identity;
using Jakub_Zajega_14987.Application.Contracts.Persistence;
using Jakub_Zajega_14987.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Jakub_Zajega_14987.WebAPI.Controllers;

public class AuthController : ApiBaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var user = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return Ok(new { Token = token });
    }
}