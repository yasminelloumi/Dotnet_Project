using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Modeles;
using ProjetNET.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var user = await _userManager.FindByIdAsync(userId);
            return user ?? throw new Exception("User not found");
        }

        // Get user by Username
        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user ?? throw new Exception("User not found");
        }

        // Create a new user
        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors));

            return user;
        }

        // Update an existing user
        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await GetUserByIdAsync(user.Id);

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
                throw new Exception("User update failed: " + string.Join(", ", result.Errors));

            return existingUser;
        }

        // Delete a user
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception("User deletion failed: " + string.Join(", ", result.Errors));

            return true;
        }

        // Get all users
        public async Task<IList<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // Assign role to user
        public async Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("Role does not exist");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception("Role assignment failed: " + string.Join(", ", result.Errors));

            return true;
        }

        // Remove role from user
        public async Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new Exception("Role does not exist");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception("Role removal failed: " + string.Join(", ", result.Errors));

            return true;
        }

        // Authenticate user (optional, depending on implementation)
        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (!await _userManager.CheckPasswordAsync(user, password))
                throw new Exception("Invalid credentials");

            return user.Id; // Or generate a token here if JWT is being used
        }
    }
}
