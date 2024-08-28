using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AgileCommercee.Data;

public partial class Loai
{
    [DisplayName("Category ID")]
    public int MaLoai { get; set; }
    [DisplayName("Category Name")]
    public string TenLoai { get; set; } = null!;
    [DisplayName("Description")]
    public string? MoTa { get; set; } = null!;
    [DisplayName("Category Image")]
    public string? Hinh { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
