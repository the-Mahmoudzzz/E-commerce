using e_commerce.app.Dto;
using e_commerce.app.Dto.SellerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface ISellerService
    {
        Task<SellerDashboardDto> GetDashboardAsync();
        Task<SellerEarningsDto> GetEarningsAsync();
    }
}
