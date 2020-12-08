﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Models.CursideOrders
{
    public class PostSyncCurbsideOrdersRequest
    {
        [Required]
        public string For { get; set; }
        [Required]
        public string Items { get; set; }
    }
}
