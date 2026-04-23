using e_commerce.app.Dto.WirhDrawlsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.IServices
{
    public interface IWithdrawalService
    {
        Task<WithdrawalResponseDto> RequestWithdrawalAsync(int sellerid,CreateWithdrawalDto dto);
        Task ApproveWithdrawalAsync(int withdrawalId);
    }
}
