using System;
using System.Collections.Generic;

namespace SkillBridge.DataAccess.Models;

public partial class TblIndividualPf
{
    public int IndividualId { get; set; }

    public string? RoleCode { get; set; }

    public string? IndividualName { get; set; }

    public string? IsStudent { get; set; }

    public string? ProField { get; set; }

    public string? InterestArea { get; set; }

    public string? Exp { get; set; }

    public string? IndividualImg { get; set; }

    public string? Address { get; set; }

    public int? WorkingHour { get; set; }

    public int? MentorToken { get; set; }

    public int? ProjToken { get; set; }

    public bool? DeleteFlag { get; set; }
}
