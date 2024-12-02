using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Modeles;
using ProjetNET.Repositories;
using System;



namespace ProjetNET.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Context _context;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, Context context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // Get user by Id
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        // Get user by Username
        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        // Create a new user
        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return user;
            }
            throw new Exception("User creation failed");
        }

        // Update an existing user
        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                return existingUser;
            }
            throw new Exception("User update failed");
        }

        // Delete a user
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        // Get all users
        public async Task<IList<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Assign role to user
        public async Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            throw new Exception("Role does not exist");
        }

        // Remove role from user
        public async Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                return result.Succeeded;
            }
            throw new Exception("Role does not exist");
        }
    }
}
