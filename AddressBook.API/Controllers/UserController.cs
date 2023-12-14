using AddressBook.Domain.Abstractions.Services;
using AddressBook.Domain.Pagination;
using AddressBook.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;       
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedUsersAsync([FromQuery] PagingInfo pagingInfo, [FromQuery] QueryParameters parameters)
        {
            try
            {
                var users = await _userService.GetPaginatedUsersAsync(pagingInfo, parameters);

                return Ok(users);
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
      
        }
    }
}
