using e_commerce.app.Dto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Interfaces
{
    public interface ISellerRepository
    {
        Task<List<Order>> GetSellerOrdersAsync(int sellerId, PaginationParamsDto pagination);
        Task<List<OrderDetail>> GetSellerOrderDetailsAsync(int sellerId, PaginationParamsDto pagination);
    }
}
