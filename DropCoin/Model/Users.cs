using System;
using System.Collections.Generic;

namespace DropCoin.Model
{
    public partial class Users
    {
        public Users()
        {
            TransactionsFromNavigation = new HashSet<Transactions>();
            TransactionsToNavigation = new HashSet<Transactions>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DrpAddress { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Transactions> TransactionsFromNavigation { get; set; }
        public virtual ICollection<Transactions> TransactionsToNavigation { get; set; }
    }
}
