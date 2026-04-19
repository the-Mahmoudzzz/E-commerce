using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.WirhDrawlsDTO
{
    public class WithdrawalResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public WithdrawlsStatus Status { get; set; }
    }
}
