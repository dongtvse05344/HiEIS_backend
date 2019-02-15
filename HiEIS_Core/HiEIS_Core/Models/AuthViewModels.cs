using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HiEIS_Core.Models
{
    public class Token
    {
        public string[] roles { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}
