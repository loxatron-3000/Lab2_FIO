using NUnit.Framework;
using OtherProject;

namespace OtherProject.Tests
{
    [TestFixture]
    public class BankAccountTests
    {
        [Test]
        public void Deposit_PositiveAmount_IncreasesBalance()
        {
            var account = new BankAccount("123", 100);
            account.Deposit(50);
            Assert.That(account.Balance, Is.EqualTo(150));
        }

        [Test]
        public void Deposit_ZeroAmount_ShouldNotChangeBalance()
        {
            var account = new BankAccount("123", 100);
            account.Deposit(0);
            Assert.That(account.Balance, Is.EqualTo(100));
        }

        [Test]
        public void Deposit_NegativeAmount_ShouldThrowOrIgnore()
        {
            var account = new BankAccount("123", 100);
            account.Deposit(-50);
            Assert.That(account.Balance, Is.EqualTo(100));
        }

        [Test]
        public void Deposit_OnLockedAccount_ShouldBeBlocked()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            account.Deposit(100);
            Assert.That(account.IsLocked, Is.True);
        }

        [Test]
        public void Withdraw_ValidAmount_DecreasesBalance()
        {
            var account = new BankAccount("123", 100);
            var result = account.Withdraw(50);
            object value = Assert.IsTrue(result);
            Assert.That(account.Balance, Is.EqualTo(50));
        }

        [Test]
        public void Withdraw_AmountMoreThanBalance_ReturnsFalse()
        {
            var account = new BankAccount("123", 100);
            var result = account.Withdraw(150);
            Assert.IsFalse(result);
            Assert.That(account.Balance, Is.EqualTo(100));
        }

        [Test]
        public void Withdraw_ExactBalance_BalanceBecomesZeroAndLocked()
        {
            var account = new BankAccount("123", 100);
            var result = account.Withdraw(100);
            Assert.IsTrue(result);
            Assert.That(account.Balance, Is.EqualTo(0));
            Assert.That(account.IsLocked, Is.False);
        }

        [Test]
        public void Withdraw_NegativeAmount_ShouldReturnFalse()
        {
            var account = new BankAccount("123", 100);
            var result = account.Withdraw(-50);
            Assert.IsFalse(result);
            Assert.That(account.Balance, Is.EqualTo(100));
        }

        [Test]
        public void Withdraw_ZeroAmount_ShouldReturnFalse()
        {
            var account = new BankAccount("123", 100);
            var result = account.Withdraw(0);
            Assert.IsFalse(result);
        }

        [Test]
        public void Withdraw_WhenAccountLocked_ShouldBeBlocked()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            var result = account.Withdraw(5);
            Assert.IsFalse(result);
        }

        [Test]
        public void Transfer_ValidAmount_MovesMoney()
        {
            var from = new BankAccount("123", 100);
            var to = new BankAccount("456", 0);
            var result = from.Transfer(to, 50);
            Assert.IsTrue(result);
            Assert.That(from.Balance, Is.EqualTo(50));
            Assert.That(to.Balance, Is.EqualTo(50));
        }

        [Test]
        public void Transfer_InsufficientFunds_NoMoneyMoved()
        {
            var from = new BankAccount("123", 100);
            var to = new BankAccount("456", 0);
            var result = from.Transfer(to, 150);
            Assert.IsFalse(result);
            Assert.That(from.Balance, Is.EqualTo(100));
            Assert.That(to.Balance, Is.EqualTo(0));
        }

        [Test]
        public void Transfer_ToSameAccount_ShouldFail()
        {
            var account = new BankAccount("123", 100);
            var result = account.Transfer(account, 50);
            Assert.IsFalse(result);
        }

        [Test]
        public void Transfer_ToNullAccount_ThrowsException()
        {
            var from = new BankAccount("123", 100);
            Assert.Throws<ArgumentNullException>(() =>
                from.Transfer(null, 50));
        }

        [Test]
        public void ApplyInterest_PositiveRate_IncreasesBalance()
        {
            var account = new BankAccount("123", 100);
            account.ApplyInterest(5);
            Assert.That(account.Balance, Is.EqualTo(105));
        }

        [Test]
        public void ApplyInterest_NegativeRate_ShouldNotChangeBalance()
        {
            var account = new BankAccount("123", 100);
            account.ApplyInterest(-5);
            Assert.That(account.Balance, Is.EqualTo(100));
        }

        [Test]
        public void ApplyInterest_OnLockedAccount_ShouldBeBlocked()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            account.ApplyInterest(5);
            Assert.That(account.Balance, Is.EqualTo(-10));
        }

        [Test]
        public void Unlock_WithSufficientDeposit_UnlocksAccount()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            var result = account.Unlock(20);
            Assert.IsTrue(result);
            Assert.That(account.IsLocked, Is.False);
            Assert.That(account.Balance, Is.EqualTo(10));
        }

        [Test]
        public void Unlock_WithInsufficientDeposit_RemainsLocked()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            var result = account.Unlock(5);
            Assert.IsFalse(result);
            Assert.That(account.IsLocked, Is.True);
        }

        [Test]
        public void Unlock_WithZeroDeposit_NoChange()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            var result = account.Unlock(0);
            Assert.IsFalse(result);
        }

        [Test]
        public void Unlock_OnNonLockedAccount_ShouldFail()
        {
            var account = new BankAccount("123", 100);
            var result = account.Unlock(50);
            Assert.IsFalse(result);
            Assert.That(account.Balance, Is.EqualTo(100));
        }

        [Test]
        public void Unlock_WithNegativeDeposit_ShouldFail()
        {
            var account = new BankAccount("123", 0);
            account.Withdraw(10);
            var result = account.Unlock(-50);
            Assert.IsFalse(result);
            Assert.That(account.Balance, Is.EqualTo(-10));
        }
    }
}