using GroceryList_Api.Data;
using GroceryList_Api.Models;
using GroceryList_Api.Services;
using GroceryList_Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace GroceryList_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;
        public AccountsController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("getinfo")]
        public async Task<IActionResult> GetInfos()
        {
            
            try
            {
                var user = await _context.Users.AsNoTracking().Include(u => u.GroceryLists).FirstOrDefaultAsync(x => x.Id == Guid.Parse(User.Identity.Name));
                if (user == null)
                    return NotFound();
                user.Password = "";
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getcompleteinfo")]
        public async Task<IActionResult> GetCompleteInfoList()
        {

            try
            {
                var user = await _context.Users.AsNoTracking().Include(u => u.GroceryLists).ThenInclude(x => x.Items).FirstOrDefaultAsync(x => x.Id == Guid.Parse(User.Identity.Name));
                if (user == null)
                    return NotFound();
                user.Password = "";
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromServices]TokenService tokenService,UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user == null)
                    return NotFound("Invalid Credentials");
                //TODO : VALIDAR PASSWORD USANDO CRIPTOGRAFIA
                var hashPassword = PasswordHasher.Hash(model.Password);
                if (!PasswordHasher.Verify(user.Password,model.Password))
                    return NotFound("Invalid Credentials");
                var token = tokenService.GenerateToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromServices] TokenService tokenService, UserRegistrationViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.Password != model.ConfirmPassword)
                return BadRequest("Passwords must be identical");
            
            
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user != null)
                {
                    return BadRequest("This email cannot be registered, try another email");
                }
                var hashPassword = PasswordHasher.Hash(model.Password);
                await _context.Users.AddAsync(new User
                {
                    Email = model.Email,
                    Password = hashPassword,
                    Name = model.Name
                });
                await _context.SaveChangesAsync();
                var newUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);
                string token = tokenService.GenerateToken(newUser);
                var message = $"Account created Successfully! Welcome, {newUser.Name}. You are allready logged in, use the JWT token to Authenticate!";

                UserRegistrationResponseViewModel newUserResponse = new UserRegistrationResponseViewModel
                {
                    Id = newUser.Id,
                    Message = message,
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Token = token
                };
                return Ok(newUserResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword(UserChangePasswordViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("Must be authenticated");
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(model.NewPassword != model.NewPasswordConfirmation)
                return BadRequest("The password must be equals");
            var user_id_str = User.Identity.Name;
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(user_id_str));
                if (user == null)
                    return BadRequest();

                if (!PasswordHasher.Verify(user.Password, model.Password))
                    return NotFound("Invalid Credentials");
                var hashPass = PasswordHasher.Hash(model.NewPassword);
                user.Password = hashPass;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return Ok("Password updated!");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            
        }

        [Authorize]
        [HttpDelete("deleteaccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var user_id_str = User.Identity.Name;
            try
            {
                var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(user_id_str));
                if (user == null)
                    return BadRequest();
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("Password updated!");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("passwordrecovery")]
        public async Task<IActionResult> PasswordRecovery([FromBody] UserPasswordRecoveryViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userExists = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == model.Email);
                if (userExists != null)
                {
                    userExists.Password = "";
                }
                
                //TODO = INSERIR SERVICO DE EMAIL PARA INCLUIR RECUPERAÇÃO DE CONTA
                return Ok(new
                    {
                        message = "If an account with this email exists, you will receive a message to update your password soon! Check your Email inbox!"
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }


    }
}
