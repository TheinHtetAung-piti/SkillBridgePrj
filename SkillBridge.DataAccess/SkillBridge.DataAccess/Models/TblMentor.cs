using System;
using System.Collections.Generic;

namespace SkillBridge.DataAccess.Models;

public partial class TblMentor
{
    public string? RoleCode { get; set; }

    public int MentorId { get; set; }

    public string MentorName { get; set; } = null!;

    public string? MentorImg { get; set; }

    public string? SpecialField { get; set; }

    public bool? DeleteFlag { get; set; }
}
