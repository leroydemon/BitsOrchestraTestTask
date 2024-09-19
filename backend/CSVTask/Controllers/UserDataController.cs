using Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using BussinessLevel.Dtos;
using BussinessLevel.Interfaces;

namespace CSVTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly IUserDataService _userDataService;

        public UserDataController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        [HttpPost("data")]
        public async Task<IActionResult> GetData([FromBody] UserDataFilter filter)
        {
            if (filter == null)
            {
                return BadRequest("The filter field is required.");
            }

            try
            {
                var data = await _userDataService.GetUserDataAsync(filter);
                return Ok(new { Success = true, Data = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Success = false, Message = "File is empty or missing." });
            }

            try
            {
                await _userDataService.UploadCsvDataAsync(file);
                return Ok(new { Success = true, Message = "Data uploaded successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error processing CSV file: {ex.Message}" });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDataDto userData, int id)
        {
            if (userData == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                userData.Id = id;
                await _userDataService.UpdateUserDataAsync(userData);
                return Ok("Userdata updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating user data: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            try
            {
                await _userDataService.RemoveAsync(id);
                return Ok($"User with ID {id} removed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error removing user: {ex.Message}");
            }
        }
    }
}
