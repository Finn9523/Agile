using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AgileCommercee.Data;

public partial class NhaCungCap
{
    [DisplayName("Supplier ID")]
    public string MaNcc { get; set; } = null!;
    [DisplayName("Supplier Name")]
    public string TenCongTy { get; set; } = null!;
    [DisplayName("Logo")]
    public string Logo { get; set; } = null!;
    [DisplayName("Contact")]
    public string? NguoiLienLac { get; set; }
    [DisplayName("Email")]
    public string Email { get; set; } = null!;
    [DisplayName("Phone Number")]
    public string? DienThoai { get; set; } = null!;
    [DisplayName("Address")]
    public string? DiaChi { get; set; } = null!;
    [DisplayName("Description")]
    public string? MoTa { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
