using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBridge.Service.Model.Project
{
    public class ProjectRequestModel
    {
        public string? ProjectName { get; set; }
        public string? ProjectTitle { get; set; }
        public string? ProjectDes { get; set; }
        public string? ProjectSub { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ProjImg { get; set; }
        public int? AvaPos { get; set; }
        public bool? IsComplete { get; set; }
        public bool? DeleteFlag { get; set; }
        public int? CreatedBy { get; set; }
    }
}

