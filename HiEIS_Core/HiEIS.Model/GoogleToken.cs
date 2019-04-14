using System;
using System.Collections.Generic;
using System.Text;

namespace HiEIS.Model
{
    public class GoogleToken
    {
        public string Id { get; set; }
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }

        public virtual MyUser MyUser { get; set; }
    }
}
