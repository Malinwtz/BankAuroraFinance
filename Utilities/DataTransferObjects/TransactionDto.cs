﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.DataTransferObjects
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
