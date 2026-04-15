using System;

namespace MyProject
{
    public class OrderCalculator
    {
        public decimal CalculateTotal(decimal subtotal, bool isIndividual, bool needsDelivery)
        {
            if (subtotal < 0)
                throw new ArgumentException("Сумма заказа не может быть отрицательной");
            
            if (subtotal == 0 && needsDelivery)
                return 300;
            
            decimal discount = GetBaseDiscount(subtotal);
            decimal afterDiscount = subtotal * (1 - discount);
            
            if (subtotal >= 10000)
            {
                afterDiscount *= 0.95m;
            }
            
            decimal tax = isIndividual ? 0 : afterDiscount * 0.2m;
            decimal total = afterDiscount + tax;
            
            if (needsDelivery && total < 2000)
                total += 300;
            
            return Math.Round(total, 2);
        }
        
        private decimal GetBaseDiscount(decimal subtotal)
        {
            if (subtotal > 5000) return 0.20m;
            if (subtotal > 3000) return 0.15m;
            if (subtotal > 1000) return 0.10m;
            if (subtotal >= 500) return 0.05m;
            return 0;
        }
    }
}