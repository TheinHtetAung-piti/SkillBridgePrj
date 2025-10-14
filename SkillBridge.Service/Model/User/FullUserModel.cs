using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBridge.Service.Model.User
{
    public class FullUserModel
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? IndividualName { get; set; }

        public string? IsStudent { get; set; }

        public string? ProField { get; set; }

        public string? InterestArea { get; set; }

        public string? Exp { get; set; }

        public string? IndividualImg { get; set; }

        public string? Address { get; set; }

        public int? WorkingHour { get; set; }
    }
}
