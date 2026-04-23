using e_commerce.app.Dto.DashBoardDTO;
using e_commerce.app.Dto.UserDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepo _orderRepo;

        public AdminService(UserManager<User> userManager, IOrderRepo orderRepo)
        {
            _userManager = userManager;
            _orderRepo = orderRepo;
        }


        public async Task<List<UserDto>> GetAllPandingSeller()
        {
            var allSellers = await _userManager.GetUsersInRoleAsync("Seller");

            var pendingSellers =  allSellers
                .Where(u => !u.IsApproved)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.UserName,  
                    Email = u.Email,
                    IsApproved = u.IsApproved
                })
                .ToList();

            return  pendingSellers;
        }

        public async Task  ApproveSeller(int userId)
        {
            var user = await _userManager
                .FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception ("Seller Not Found");

            user.IsApproved = true;

            await _userManager.UpdateAsync(user);
        }
        
        

        public async Task<DashboardStatsDto> GetSats()
        {
           int totalorder=await _orderRepo.GetCountOrder();
            decimal totalamout =await _orderRepo.GetTotalCount();
            int totaluser =await _userManager.Users.CountAsync();
            return new DashboardStatsDto
            {
                TotalOrders = totalorder,
                TotalRevenue = totalamout,
                TotalUsers = totaluser,
            };
                
        }
    }
}
