using System;
using System.Collections.Generic;

namespace AgileEcommerce.Data;

public partial class User
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string PhoneNumber { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

}
