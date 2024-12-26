﻿using System;
using System.Collections.Generic;

namespace ABZQueryLibrary.Models;

public partial class Customer
{
    public string CustomerID { get; set; } = null!;

    public virtual ICollection<CustomerQuery> CustomerQueries { get; set; } = new List<CustomerQuery>();
}
