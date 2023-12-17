using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.ViewModel
{
    public class TokenRequestViewModel
    {
        public string JwtKey { get; set; }
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
    }
}
