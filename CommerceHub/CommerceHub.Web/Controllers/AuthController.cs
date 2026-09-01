using CommerceHub.Web.Models.Identity;
using CommerceHub.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterRequest = CommerceHub.Web.Models.Identity.RegisterRequest;

namespace CommerceHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private UserManager<CustomUser> _userManager;
        private TokenService _tokenService;

        public AuthController(UserManager<CustomUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new CustomUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, "Customer");

            return Ok(new { message = "Kayıt başarılı" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user,request.Password))
            {
                return Unauthorized(new { message = "Girdiğiniz bilgiler hatalı" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = DateTime.Now.AddDays(20);

            return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.RefreshToken == request.RefreshToken);

            if (user is null || user.RefreshTokenExpiryDate < DateTime.Now)
            {
                return Unauthorized(new { message = "Refresh Token geçersiz ya da süresi dolmuş..." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryDate = DateTime.Now.AddDays(20);
            await _userManager.UpdateAsync(user);

            return Ok(new { accesToken = newAccessToken, refreshToken = newRefreshToken });
        }
            

        
    }

  public  record RefreshRequest(string RefreshToken);
}
