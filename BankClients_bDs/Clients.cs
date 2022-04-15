using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankClients_bDs.Deposits;

namespace BankClients_bDs
{
    internal class Clients
    {
        public int ID { get; set; }
        public string surName { get; set; }
        public string passNumber { get; set; }
        public int amountDeposit { get; set; }
        public List<Deposits> infoDeposit { get; set; }
    }
}
