using System;
using System.Collections.Generic;

namespace SkillBridge.DataAccess.Models;

public partial class TblJobPost
{
    public string PostId { get; set; } = null!;

    public int? CompanyId { get; set; }

    public string? PostDes { get; set; }
    public string? PostName { get; set; }  // NEW

    public string? JpImg { get; set; }

    public int? CreatedBy { get; set; }

    public bool? IsComplete { get; set; }

    public bool? DeleteFlag { get; set; }
}
