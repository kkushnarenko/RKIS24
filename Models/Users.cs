using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace context.Models;


public partial class Users
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
}
