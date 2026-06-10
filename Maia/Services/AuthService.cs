using Maia.Data;
using Maia.Data.DTO;
using Maia.Data.Interface;
using Maia.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Maia.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IJwtService _jwt;
        private readonly IConfiguration _config;

        public AuthService(DataContext context, IJwtService jwt, IConfiguration config)
        {
            _context = context;
            _jwt = jwt;
            _config = config;
        }

        public async Task<string> Register(RegisterDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _jwt.GenerateToken(user);
        }

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return _jwt.GenerateToken(user);
        }

        public async Task ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null) return; // Silent — don't reveal if email exists

            var token = Guid.NewGuid().ToString("N");
            user.PasswordResetToken = token;
            user.PasswordResetExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            var resetLink = $"http://localhost:5000/reset?token={token}";

            _ = Task.Run(() => SendResetEmail(dto.Email, user.FirstName, resetLink));
        }

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.PasswordResetToken == dto.Token &&
                x.PasswordResetExpiry > DateTime.UtcNow);

            if (user == null)
                throw new Exception("Invalid or expired reset token.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetExpiry = null;
            await _context.SaveChangesAsync();
        }

        private void SendResetEmail(string toEmail, string firstName, string resetLink)
        {
            try
            {
                var fromEmail = _config["Email:From"] ?? "maia.shopservice@gmail.com";
                var password  = _config["Email:Password"] ?? "";

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromEmail, password)
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail, "MAIA"),
                    Subject = "Reset your MAIA password",
                    IsBodyHtml = false,
                    Body = $@"Hi {firstName},

You requested a password reset for your MAIA account.

Click the link below to set a new password (valid for 1 hour):

{resetLink}

If you did not request this, you can safely ignore this email.

— The MAIA Team"
                };

                mail.To.Add(toEmail);
                smtp.Send(mail);
            }
            catch
            {
                // Email failure is non-critical for the API response
            }
        }
    }
}
