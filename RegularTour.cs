using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgensyConsole
{
    public class RegularTour : Tour
    {
        public RegularTour(uint OrderCode, string CustomerName, DateTime OrderDate, 
                           string TourName, string Country, DateTime DepartureDate, 
                           DateTime ArrivalDate, uint VouchersNumbers, double OneTicketCost) : 
                                               base(OrderCode, CustomerName, OrderDate,
                                                    TourName, Country, DepartureDate,
                                                    ArrivalDate, VouchersNumbers, OneTicketCost){}
        public override double CalculateCost() // шукає усю вартість замовлення
        {
            return OneTicketCost * VouchersNumbers;
        }

        public override string FormatTextForConsole()
        {
            return base.FormatTextForConsole();
        }

        public override string ToString()
        {
            return base.ToString()+"\n";
        }
    }
}
