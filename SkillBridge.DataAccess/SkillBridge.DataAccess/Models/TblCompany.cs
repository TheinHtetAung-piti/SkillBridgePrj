using System;
using System.Collections.Generic;

namespace SkillBridge.DataAccess.Models;

public partial class TblCompany
{
    public int CompanyId { get; set; }
    public string RoleCode { get; set; }
    public string CompanyName { get; set; } = null!;

    public string? BusWebUrl { get; set; }

    public string? ComImg { get; set; }

    public string? Address { get; set; }

    public int? PostToken { get; set; }

    public bool? DeleteFlag { get; set; }
}
