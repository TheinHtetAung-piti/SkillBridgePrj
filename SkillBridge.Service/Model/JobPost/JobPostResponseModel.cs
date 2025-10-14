using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBridge.Service.Model.JobPost
{
    public class JobPostResponseModel
    {
        public string PostId { get; set; } = null!;
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }  // NEW
        public string? PostName { get; set; }     // NEW
        public string? PostDes { get; set; }
        public string? JpImg { get; set; }
        public bool? IsComplete { get; set; }
        public bool? DeleteFlag { get; set; }
    }

}

