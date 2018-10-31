using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Context;
using Core.Email;
using Core.Token;
using Core.User;

namespace Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IEmailService emailService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<UserModel>> GetAsync()
        {
            return await _userRepository.FindAsync();
        }

        public async Task<UserModel> GetByIdAsync(ContextModel ctx, string id)
        {
			if (string.IsNullOrWhiteSpace(id))
				return null;
			
            return await _userRepository.FindByIdAsync(id);
        }

        public async Task<UserModel> GetByEmailAsync(string email) 
		{
            if (string.IsNullOrWhiteSpace(email))
                return null;
            
            return await _userRepository.FindByEmailAsync(email);
        }

        public async Task<UserModel> CreateAsync(UserModel user)
        {
			if (user == null)
				return null;
					
            var userFound = await _userRepository.FindByEmailAsync(user.Email);
            if (userFound != null)
                return null;

            user.Password = HashPassword(user.Password);

            return await _userRepository.InsertAsync(user);
        }

        public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
			if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
				return null;
			
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
                return false;
            
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
                return false;

            user.Password = HashPassword(newPassword);

            return await _userRepository.UpdateAsync(user);
        }

        public async Task ForgotPasswordAsync(string email)
        {
			if (string.IsNullOrWhiteSpace(email))
				return;
			
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
                return;

            var token = _tokenService.CreateToken(user);

            await _emailService.SendEmailAsync(email, "Forgot Password", token);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
			if (string.IsNullOrWhiteSpace(newPassword))
				return false;
			
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
                return false;

            user.Password = HashPassword(newPassword);

            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }

        private static string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
