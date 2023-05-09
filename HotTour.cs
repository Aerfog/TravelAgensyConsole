using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgensyConsole
{
    internal class HotTour : Tour
    {
        private double _discount;
        public double Discount 
        { 
            get
            {
                return _discount;
            }
            set
            {
                if(value >= 0 && value <= 1) //набуває значення тільки від 0 до 1 включно(тобто від 0% до 100%)
                {
                    _discount = value;
                }
                else throw new Exception("Неприпустиме значення знижки!");
            }
        }

        public HotTour(uint OrderCode, string CustomerName, DateTime OrderDate, 
                       string TourName, string Country, DateTime DepartureDate, 
                       DateTime ArrivalDate, uint VouchersNumbers, double OneTicketCost, double Discount) : 
                                           base(OrderCode, CustomerName, OrderDate, 
                                                TourName, Country, DepartureDate, 
                                                ArrivalDate, VouchersNumbers, OneTicketCost)
        {
            this.Discount = Discount;
        }
        public override double CalculateCost() // шукає усю вартість замовлення з урахуванням знижки
        {
            return  OneTicketCost * VouchersNumbers - (Discount * OneTicketCost * VouchersNumbers);
        }

        public override string FormatTextForConsole()
        {
            return string.Format("{0} {1,-3:P2}", base.FormatTextForConsole(), Discount);
        }

        public override string ToString()
        {
            return base.ToString() + $" {Discount * 100}\n";
        }
    }
}
