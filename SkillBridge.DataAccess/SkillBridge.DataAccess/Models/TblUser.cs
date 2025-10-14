using System;
using System.Collections.Generic;

namespace SkillBridge.DataAccess.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PhoneNo { get; set; }

    public string? RoleCode { get; set; }

    public bool? DeleteFlag { get; set; }
}
