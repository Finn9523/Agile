using System;
using System.Collections.Generic;

namespace AgileEcommerce.Data;

public partial class Role
{
    public string Id { get; set; }

    public string NameRole { get; set; }

    public string NormalizedName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
