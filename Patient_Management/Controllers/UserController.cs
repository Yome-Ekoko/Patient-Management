using Patient_Management.Core.Contract;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Patient_Management.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService accountService)
        {
            _userService = accountService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<Response<AuthenticationResponse>>> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _userService.AuthenticateAsync(request, HttpContext.RequestAborted));
        }
        [HttpPost("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            return Ok(await _userService.LogoutAsync());
        }
        [AllowAnonymous]
        [HttpGet("getUsers")]
        public async Task<ActionResult<PagedResponse<List<UserResponse>>>> GetUsers([FromQuery] UserQueryParameters queryParameters)
        {
            return Ok(await _userService.GetUsersAsync(queryParameters, HttpContext.RequestAborted));
        }
        [AllowAnonymous]
        [HttpGet("getUser/{id}")]
        public async Task<ActionResult<Response<UserResponse>>> GetUserById(string id)
        {
            return Ok(await _userService.GetUserById(id, HttpContext.RequestAborted));
        }
        
        [AllowAnonymous]
        [HttpPost("editUser")]
        public async Task<ActionResult<Response<string>>> EditUser([FromBody] EditUserRequest request)
        {
            return Ok(await _userService.EditUserAsync(request, HttpContext.RequestAborted));
        }
        //[Authorize(Roles = "Administrator")]
        //[HttpPost("deleteUser")]
        //public async Task<ActionResult<Response<string>>> DeleteUser([FromBody] DeleteUserRequest request)
        //{
        //    return Ok(await _userService.DeleteUserAsync(request, HttpContext.RequestAborted));
        //}
        [AllowAnonymous]
        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string userId)
        {
            return Ok(await _userService.SoftDeleteUserAsync(userId));
        }
           
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<Response<string>>> Register([FromBody] RegisterRequest request)
        {
            return Ok(await _userService.RegisterUser(request));
        }
        [AllowAnonymous]
        [HttpPost("CreatePassword")]
        public async Task<ActionResult<Response<string>>> createPassword([FromBody] CreatePasswordRequest request)
        {
            return Ok(await _userService.CreatePassword(request));
        }
       
    }
}