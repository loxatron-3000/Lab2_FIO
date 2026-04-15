using NUnit.Framework;
using MyProject;

namespace MyProject.Tests
{
    [TestFixture]
    public class OrderCalculatorTests
    {
        private OrderCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new OrderCalculator();
        }

        [Test]
        public void CalculateTotal_SubtotalLessThan500_NoDiscount()
        {
            var result = _calculator.CalculateTotal(499, true, false);
            Assert.That(result, Is.EqualTo(499));
        }

        [Test]
        public void CalculateTotal_Subtotal500Exactly_FivePercentDiscount()
        {
            var result = _calculator.CalculateTotal(500, true, false);
            Assert.That(result, Is.EqualTo(475));
        }

        [Test]
        public void CalculateTotal_Subtotal999_FivePercentDiscount()
        {
            var result = _calculator.CalculateTotal(999, true, false);
            Assert.That(result, Is.EqualTo(949.05m));
        }

        [Test]
        public void CalculateTotal_Subtotal1000Exactly_TenPercentDiscount()
        {
            var result = _calculator.CalculateTotal(1000, true, false);
            Assert.That(result, Is.EqualTo(900));
        }

        [Test]
        public void CalculateTotal_Subtotal2000_TenPercentDiscount()
        {
            var result = _calculator.CalculateTotal(2000, true, false);
            Assert.That(result, Is.EqualTo(1800));
        }

        [Test]
        public void CalculateTotal_Subtotal3000Exactly_FifteenPercentDiscount()
        {
            var result = _calculator.CalculateTotal(3000, true, false);
            Assert.That(result, Is.EqualTo(2550));
        }

        [Test]
        public void CalculateTotal_Subtotal4000_FifteenPercentDiscount()
        {
            var result = _calculator.CalculateTotal(4000, true, false);
            Assert.That(result, Is.EqualTo(3400));
        }

        [Test]
        public void CalculateTotal_Subtotal5000Exactly_TwentyPercentDiscount()
        {
            var result = _calculator.CalculateTotal(5000, true, false);
            Assert.That(result, Is.EqualTo(4000));
        }

        [Test]
        public void CalculateTotal_Subtotal10000Exactly_AdditionalFivePercentDiscount()
        {
            var result = _calculator.CalculateTotal(10000, true, false);
            Assert.That(result, Is.EqualTo(7600));
        }

        [Test]
        public void CalculateTotal_Subtotal15000_AdditionalFivePercentDiscount()
        {
            var result = _calculator.CalculateTotal(15000, true, false);
            Assert.That(result, Is.EqualTo(11400));
        }

        [Test]
        public void CalculateTotal_Subtotal9999_NoAdditionalDiscount()
        {
            var result = _calculator.CalculateTotal(9999, true, false);
            Assert.That(result, Is.EqualTo(7999.20m));
        }

        [Test]
        public void CalculateTotal_Subtotal10001_AdditionalDiscountApplied()
        {
            var result = _calculator.CalculateTotal(10001, true, false);
            Assert.That(result, Is.EqualTo(7600.76m));
        }

        [Test]
        public void CalculateTotal_Individual_NoTax()
        {
            var result = _calculator.CalculateTotal(1000, true, false);
            Assert.That(result, Is.EqualTo(900));
        }

        [Test]
        public void CalculateTotal_Business_TaxApplied()
        {
            var result = _calculator.CalculateTotal(1000, false, false);
            Assert.That(result, Is.EqualTo(1080));
        }

        [Test]
        public void CalculateTotal_BusinessWithHighDiscount_TaxOnDiscountedAmount()
        {
            var result = _calculator.CalculateTotal(5000, false, false);
            Assert.That(result, Is.EqualTo(4800));
        }

        [Test]
        public void CalculateTotal_BusinessWithAdditionalDiscount_TaxAfterAllDiscounts()
        {
            var result = _calculator.CalculateTotal(10000, false, false);
            Assert.That(result, Is.EqualTo(9120));
        }

        [Test]
        public void CalculateTotal_BusinessSmallOrder_TaxApplied()
        {
            var result = _calculator.CalculateTotal(100, false, false);
            Assert.That(result, Is.EqualTo(120));
        }

        [Test]
        public void CalculateTotal_NeedsDeliveryAndTotalLessThan2000_Adds300()
        {
            var result = _calculator.CalculateTotal(1500, true, true);
            Assert.That(result, Is.EqualTo(1650));
        }

        [Test]
        public void CalculateTotal_NeedsDeliveryAndTotal2000Exactly_FreeDelivery()
        {
            var result = _calculator.CalculateTotal(2000, true, true);
            Assert.That(result, Is.EqualTo(2100));
        }

        [Test]
        public void CalculateTotal_NeedsDeliveryAndTotalAbove2000_FreeDelivery()
        {
            var result = _calculator.CalculateTotal(2500, true, true);
            Assert.That(result, Is.EqualTo(2250));
        }

        [Test]
        public void CalculateTotal_NoDelivery_NoDeliveryFee()
        {
            var result = _calculator.CalculateTotal(1000, true, false);
            Assert.That(result, Is.EqualTo(900));
        }

        [Test]
        public void CalculateTotal_BusinessWithDelivery_CalculatesCorrectly()
        {
            var result = _calculator.CalculateTotal(1000, false, true);
            Assert.That(result, Is.EqualTo(1380));
        }

        [Test]
        public void CalculateTotal_ZeroSubtotalNoDelivery_ReturnsZero()
        {
            var result = _calculator.CalculateTotal(0, true, false);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CalculateTotal_ZeroSubtotalWithDelivery_ReturnsDeliveryCost()
        {
            var result = _calculator.CalculateTotal(0, true, true);
            Assert.That(result, Is.EqualTo(300));
        }

        [Test]
        public void CalculateTotal_NegativeSubtotal_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                _calculator.CalculateTotal(-100, true, false));
        }

        [Test]
        public void CalculateTotal_MaximumDecimalValue_NoOverflow()
        {
            var result = _calculator.CalculateTotal(decimal.MaxValue, true, false);
            Assert.That(result, Is.LessThan(decimal.MaxValue));
        }

        [Test]
        public void CalculateTotal_MinimumPositiveValue_WorksCorrectly()
        {
            var result = _calculator.CalculateTotal(0.01m, true, true);
            Assert.That(result, Is.EqualTo(300.01m));
        }

        [Test]
        public void CalculateTotal_BusinessZeroOrderWithDelivery_ReturnsTaxOnDelivery()
        {
            var result = _calculator.CalculateTotal(0, false, true);
            Assert.That(result, Is.EqualTo(300));
        }
    }
}