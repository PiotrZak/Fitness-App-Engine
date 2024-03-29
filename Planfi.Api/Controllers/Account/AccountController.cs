﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanfiApi.Helpers;
using PlanfiApi.Interfaces;
using PlanfiApi.Models;
using PlanfiApi.Models.ViewModels;
using PlanfiApi.Services.Files;
using WebApi.Common;
using WebApi.Controllers.ViewModels;
using WebApi.Data.ViewModels;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services.Account;

namespace PlanfiApi.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;


        public AccountController(
            IEmailService emailService,
            IAccountService accountService,
            IUserService userService,
            IFileService fileService)
        {
            _accountService = accountService;
            _userService = userService;
            _fileService = fileService;
            _emailService = emailService;
            }

        [AllowAnonymous]
        [HttpPost("sendMail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessage message)
        {
            await _emailService.SendEmail(message);
            return Ok(message);
        }

        public class RegisterGmailModel
        {
            [Required]
            public string OrganizationId { get; set; }
            [Required]
            public string Email { get; set; }
            public string Role { get; set; }
            public string? ImageUrl { get; set; }
            
        }
        
        [AllowAnonymous]
        [HttpPost("gmailSignUp")]
        public async Task<IActionResult> GmailSignUp([FromBody] RegisterGmailModel model)
        {
            try
            {
                var user = await _userService.GetUserWithoutPassword(model.Email);
                var token = ExtensionMethods.GenerateJwt(user);
                
                byte[] avatarFromGoogle = { };
                
                if (user.Avatar == null && model.ImageUrl != null)
                {
                  avatarFromGoogle = Encoding.ASCII.GetBytes(model.ImageUrl);
                  await _accountService.UploadAvatar(avatarFromGoogle, user.UserId);
                }
                
                var result = new ObjectResult(new UserViewModel
                {
                    UserId = user.UserId,
                    OrganizationId = user.OrganizationId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Avatar = user.Avatar ?? avatarFromGoogle,
                    Role = new Role
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name,
                    },
                    Token = token
                })
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
                
                Response.Headers.Add("JWT", token);
                return result;

            }
            catch(Exception e)
            {
                var failure = ApiCommonResponse.Create()
                    .WithFailure()
                    .WithData(e)
                    .Build();
                
                return CommonResponse(failure);
            }
        }
        
        

        [AllowAnonymous]
        [HttpPost("uploadAvatar")]
        public async Task<IActionResult> UploadAvatar([FromForm]IFormFile avatar)
        {
          try
          {
            await _accountService.ProcessAvatar(avatar, null);
            return Ok();
          }
          catch (AppException ex)
          {
            return BadRequest(new {message = ex.Message});
          }
        }
        
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            try
            {
                var result = await _accountService.ForgotPassword(model, Request.Headers["origin"]);
                if (result)
                {
                    return Ok(new { message = "Please check your email for password reset instructions" });     
                }

                return BadRequest(new {message = "This User dont exist"});
            }
            catch
            {
                return Ok(new { message = "This User dont exist" });
            }
        }
        
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var resetPasswordResponse = await _accountService.ResetPassword(model);
                
                var success = ApiCommonResponse.Create()
                    .WithSuccess()
                    .WithData(resetPasswordResponse)
                    .Build();
                
                return CommonResponse(success);
            }

            catch(Exception e)
            {
                var failure = ApiCommonResponse.Create()
                    .WithFailure()
                    .Build();
                
                return CommonResponse(failure);
            }
        }
        

        [AllowAnonymous]
        [HttpPost("invite")]
        public async Task<IActionResult> RegisterAccount(RegisterModel model)
        {

            try
            {
                var sendVerificationResponse = await _accountService.SendVerificationEmail(model, Request.Headers["origin"]);
                
                var success = ApiCommonResponse.Create()
                    .WithSuccess()
                    .WithData(sendVerificationResponse)
                    .Build();
                
                return CommonResponse(success);
            }

            catch(Exception e)
            {
                var failure = ApiCommonResponse.Create()
                    .WithFailure()
                    .WithData(e)
                    .Build();
                
                return CommonResponse(failure);
            }
        }
        
        [AllowAnonymous]
        [HttpPost("activate")]
        public async Task<IActionResult> ActivateAccount(ActivateAccount model)
        {
            var user = await _accountService.Activate(model);

            return Ok(user);
        }

    }
}
