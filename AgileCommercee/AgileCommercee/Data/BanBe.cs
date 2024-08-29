using System;
using System.Collections.Generic;

namespace AgileCommercee.Data;

public partial class BanBe
{
    public int MaBb { get; set; }

    public int Id { get; set; }

    public int MaHh { get; set; }

    public string? HoTen { get; set; }

    public string Email { get; set; } = null!;

    public DateTime NgayGui { get; set; }

    public string? GhiChu { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual HangHoa MaHhNavigation { get; set; } = null!;
}
