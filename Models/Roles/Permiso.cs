﻿using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public string NombrePermiso { get; set; } = null!;

    public virtual ICollection<Permisoxrol> Permisoxrols { get; set; } = new List<Permisoxrol>();
}
