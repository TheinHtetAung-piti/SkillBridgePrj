using System;
using System.Collections.Generic;

namespace SkillBridge.DataAccess.Models;

public partial class TblProject
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? ProjectTitle { get; set; }

    public string? ProjectDes { get; set; }

    public string? ProjectSub { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? CreatedBy { get; set; }

    public string? ProjImg { get; set; }

    public int? AvaPos { get; set; }

    public bool? IsComplete { get; set; }

    public bool? DeleteFlag { get; set; }
}
