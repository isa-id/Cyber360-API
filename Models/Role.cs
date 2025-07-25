﻿using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Permisoxrol> Permisoxrols { get; set; } = new List<Permisoxrol>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
