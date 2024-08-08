using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DomainLayer.DTO
{
    public class UpdatePasswordDTO
    {
        public string CurrentPassword { get; set; }
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
        public string NewPassword { get; set; }
    }
}
