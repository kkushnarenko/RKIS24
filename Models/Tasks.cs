using System;
using System.Collections.Generic;

namespace context.Models;
public partial class Tasks
{
    public int Id { get; set; }

    public string NameTask { get; set; } = null!;

    public DateTime Deadline { get; set; }

    public string? Description { get; set; }

    public int IdUser { get; set; }

    public virtual Users IdUserNavigation { get; set; } = null!;
}
