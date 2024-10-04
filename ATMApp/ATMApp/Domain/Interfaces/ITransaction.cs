using ATMApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Interfaces
{
    public interface ITransaction
    {
        void InsertTransaction(long UserBankID, TransactionType transactionType,
            Decimal TransactionAmount, string Description);
        void ViewTransactions();
    }
}
