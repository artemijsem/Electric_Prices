using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Electric_Prices.Models
{
    public partial class EpApiTable
    {
        
        public bool Success { get; set; }

        [Key]
        public string? Data { get; set; }

        public int? Timestamp { get; set; }
        public double Price { get; set; }

        
        public DateTime Date { get; set; }
    }
}
