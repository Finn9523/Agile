using System;
using System.Collections.Generic;

namespace AgileCommercee.Data;

public partial class YeuThich
{
    public int MaYt { get; set; }

    public int? MaHh { get; set; }

    public int Id { get; set; }

    public DateTime? NgayChon { get; set; }

    public string? MoTa { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual HangHoa? MaHhNavigation { get; set; }
}
