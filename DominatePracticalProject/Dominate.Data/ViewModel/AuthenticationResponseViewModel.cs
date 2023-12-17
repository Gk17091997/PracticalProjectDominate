using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.ViewModel
{
    public class AuthenticationResponseViewModel
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
