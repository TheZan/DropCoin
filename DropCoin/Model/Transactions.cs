using System;
using System.Collections.Generic;

namespace DropCoin.Model
{
    public partial class Transactions
    {
        public int TransactionId { get; set; }
        public string TransactionHash { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Count { get; set; }
        public DateTime TransactionDate { get; set; }

        public virtual Users FromNavigation { get; set; }
        public virtual Users ToNavigation { get; set; }
    }
}
