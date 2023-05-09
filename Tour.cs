using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgensyConsole
{
    public abstract class Tour
    {
        private DateTime _departureTime;
        private DateTime _arrivalTime;
        private uint _vouchersNumbers;
        private double _oneTicketCost;
        private DateTime _orderDate;

        public uint OrderCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate 
        { 
            get
            {
               return _orderDate; 
            } 
            set 
            { 
                if(value <= DateTime.Now) _orderDate = value; 
                else throw new Exception("Неприпустиме значення дати замовлення!");
            }
        }
        public string TourName { get; set; }
        public string Country { get; set; }
        public DateTime DepartureDate
        {
            get { return _departureTime; }
            set
            {   //якщо дата відбуття пізніше за дату прибуття та раніше за дату заказу - напише про помилку, якщо ні - надасть значення приватній змінній
                if (value < ArrivalDate && value > OrderDate)
                    _departureTime = value;
                else throw new Exception("Неприпустима дата від'їзду!"); 
            }
        }
        public DateTime ArrivalDate
        {
            get { return _arrivalTime; }
            set
            {   //якщо дата відбуття раніше за дату прибуття - напише про помилку, якщо ні - надасть значення приватній змінній
                if (value > DepartureDate) _arrivalTime = value;
                else throw new Exception("Неприпустима дата від'їзду!"); 
            }
        }
        public uint VouchersNumbers
        {
            get { return _vouchersNumbers; }
            set
            {
                _vouchersNumbers = value;
            }
        }
        public double OneTicketCost
        {
            get { return _oneTicketCost; }
            set
            {
                if (value > 0) _oneTicketCost = value; //ціна повинна бути невід'ємною
                else throw new Exception("Неприпустима вартість однієї путівки!"); 
            }
        }

        public Tour(uint OrderCode, string CustomerName, DateTime OrderDate,
                    string TourName, string Country, DateTime DepartureDate,
                    DateTime ArrivalDate, uint VouchersNumbers, double OneTicketCost)
        {
            this.OrderCode = OrderCode;
            this.CustomerName = CustomerName;
            this.OrderDate = OrderDate;
            this.TourName = TourName;
            this.Country = Country;
            this.ArrivalDate = ArrivalDate;
            this.DepartureDate = DepartureDate;
            this.VouchersNumbers = VouchersNumbers;
            this.OneTicketCost = OneTicketCost;
        }

        public virtual double CalculateCost() { return 0; }

        public virtual string FormatTextForConsole()
        {
            return string.Format("{0,-5} {1,-35} {2:dd-MM-yy} {3,-10} {4,-10} {5:dd-MM-yy} {6:dd-MM-yy} {7,-2} {8,-8:F2}$", OrderCode, CustomerName, OrderDate, TourName, Country, DepartureDate, ArrivalDate, VouchersNumbers, OneTicketCost);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2:dd-MM-yy} {3} {4} {5:dd-MM-yy} {6:dd-MM-yy} {7} {8:F2}", OrderCode, CustomerName, OrderDate, TourName, Country, DepartureDate, ArrivalDate, VouchersNumbers, OneTicketCost);
        }
    }
}
