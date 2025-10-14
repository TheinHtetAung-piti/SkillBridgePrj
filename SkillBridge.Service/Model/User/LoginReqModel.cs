using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBridge.Service.Model.User
{
    public class LoginReqModel
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } 

        public string Password { get; set; }


    }
}
