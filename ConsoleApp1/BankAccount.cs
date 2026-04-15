using System;

namespace OtherProject
{
    public class BankAccount
    {
        public decimal Balance { get; private set; }
        public bool IsLocked { get; private set; }
        public string AccountNumber { get; private set; }

        public BankAccount(string accountNumber, decimal initialBalance = 0)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            IsLocked = false;
        }

        public bool Deposit(decimal amount)
        {
            Balance += amount;
            return true;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount > Balance)
                return false;

            Balance -= amount;

            if (Balance <= 0)
                IsLocked = true;

            return true;
        }

        public bool Transfer(BankAccount target, decimal amount)
        {
            if (Withdraw(amount))
            {
                target.Deposit(amount);
                return true;
            }
            return false;
        }

        public void ApplyInterest(decimal yearlyRate)
        {
            if (yearlyRate < 0)
                return;

            Balance = Balance + yearlyRate;
        }

        public bool Unlock(decimal depositAmount)
        {
            if (depositAmount > 0)
            {
                Deposit(depositAmount);
                if (Balance > 0)
                    IsLocked = false;
                return true;
            }
            return false;
        }
    }
}