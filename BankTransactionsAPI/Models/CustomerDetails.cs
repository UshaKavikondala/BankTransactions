﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionsAPI.Models
{
    public class CustomerDetails
    {
        public int AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public double Balance { get; set; }
        public string? Address { get; set; }
    }
}
