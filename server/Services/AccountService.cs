using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using WebApi.Controllers.ViewModels;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IAccountService
    {
        void UploadAvatar(string userId, byte[] avatar);
        Boolean ForgotPassword(ForgotPassword model, string origin);
        void ResetPassword(ResetPasswordRequest model);
        void SendVerificationEmail(RegisterModel register, string origin);
        string RandomTokenString();
    }

    public class AccountService : IAccountService
    {
        private readonly IEmailService _emailService;
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountService(IEmailService emailService, DataContext context, IUserService userService, IMapper mapper)
        {
            _emailService = emailService;
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }
        
        public Boolean ForgotPassword(ForgotPassword model, string origin)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Email).WithoutPassword();

            if (user == null) return false;

            // create reset token that expires after 1 day
            user.ResetToken = RandomTokenString();
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
            
            SendPasswordResetEmail(user, origin);
            return true;
        }

        public void UploadAvatar(string userId, byte[] avatar)
        {
            var user = _context.Clients.Find(userId);

            user.Avatar = avatar;
            _context.Clients.Update(user);
            _context.SaveChanges();
        }
        
        private void SendPasswordResetEmail(User account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            var messageData = new EmailMessage
            {
                ToAddresses = new List<EmailMessage.EmailAddress>()
                {
                    new EmailMessage.EmailAddress()
                    {
                        Name = account.Email,
                        Address = account.Email
                    }
                },
                FromAddresses = new List<EmailMessage.EmailAddress>()
                {
                    new EmailMessage.EmailAddress()
                    {
                        Name = "Planfi",
                        Address = "planfi.contact@gmail.com",
                    }
                },
                Subject = "Reset password E-mail",
                Content = message,
            };
            
            _emailService.Send(messageData);
        }

        public string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        
        public void ResetPassword(ResetPasswordRequest model)
        {
            var user = _context.Users.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
                throw new AppException("Invalid token");

            // update password and remove reset token
            byte[] passwordHash, passwordSalt;
            _userService.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            user.PasswordReset = DateTime.UtcNow;
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
        
        public void SendVerificationEmail(RegisterModel model, string origin)
        {

            if (_context.Clients.Any(x => x.Email == model.Email))
                throw new AppException("Email \"" + model + "\" is already taken"); 
            
            var user = _mapper.Map<Client>(model);
            user.VerificationToken = RandomTokenString();
            
            _context.Users.Add(user);
            _context.SaveChanges();
            
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={user.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{user.VerificationToken}</code></p>";
            }

            var messageData = new EmailMessage
            {
                ToAddresses = new List<EmailMessage.EmailAddress>()
                {
                    new EmailMessage.EmailAddress()
                    {
                        Name = model.Email,
                        Address = model.Email,
                    }
                },
                FromAddresses = new List<EmailMessage.EmailAddress>()
                {
                    new EmailMessage.EmailAddress()
                    {
                        Name = "Planfi",
                        Address = "planfi.contact@gmail.com",
                    }
                },
                Subject = "Activate Your Account",
                Content = $@"<h4>Activation</h4>
                         <p>Thanks for registering!</p>
                         {message}",
            };
            
            _emailService.Send(messageData);
        }
    }
}


