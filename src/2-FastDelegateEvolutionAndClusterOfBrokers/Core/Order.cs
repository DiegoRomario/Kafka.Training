using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Client { get; set; }
    }
}
