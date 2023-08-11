﻿using BillWare.App.Common;

namespace BillWare.App.Models
{
    public class Costumer : BaseModel
    {
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? NumberId { get; set; }
    }
}
