
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarenderyaWebApp.Model;
using KarenderyaWebApp.Services;

namespace KarenderyaWebApp.Controllers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDBContext _db;

        public LoginController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(
            [FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new{ success = false, message = "Username and password are required." });
                }

                string username = request.Username.Trim();

                var user = await _db.USERS.FirstOrDefaultAsync(x => x.Username == username);

                if (user == null)
                {
                    return Unauthorized(new { success = false, message = "Invalid username or password." });
                }

                if (!user.IsActive)
                {
                    return Unauthorized(new { success = false, message = "Your account is deactivated." });
                }

                string hashedPassword = PasswordHelper.HashPassword(request.Password);

                if (user.PasswordHash != hashedPassword)
                {
                    return Unauthorized(new { success = false, message = "Invalid username or password." });
                }

                // Create the user's session
                HttpContext.Session.SetInt32("UserId", user.ID);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin") {
                    return Ok(new{ success = true, message = "Login successful.",page = "/Admin/Dashboard"});
                }
                else {
                    return Ok(new{ success = true, message = "Login successful.",page = "/Cashier/Dashboard"});
                }

            }
            catch (Exception)
            {
                return StatusCode(500, new{ success = false,message = "Something went wrong while processing your request." });
            }
        }
    }
}