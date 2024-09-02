using System;
using System.Collections.Generic;

namespace AgileCommercee.Data;

public partial class GioHang
{
    public int MaGh { get; set; }

    public int Id { get; set; }

    public int MaHh { get; set; }

    public int SoLuong { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual HangHoa MaHhNavigation { get; set; } = null!;
}
