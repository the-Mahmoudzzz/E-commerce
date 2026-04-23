using e_commerce.app.Dto.DashBoardDTO;
using e_commerce.app.Dto.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IAdminService
    {
        Task<List<UserDto>> GetAllPandingSeller();
        Task ApproveSeller(int sellrid);
        Task<DashboardStatsDto> GetSats();


    }
}
