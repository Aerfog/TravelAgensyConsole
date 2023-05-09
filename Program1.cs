using System.Collections;
using System.ComponentModel.Design;
using System.Text;

namespace TravelAgensyConsole
{
    internal class Program
    {
        private const string commands =
        @"Перелік команд:
1.Зареєструвати замовлення
2.Редагувати дані замовлення
3.Видалити замовлення
4.Обчислити вартість замовлення
5.Переглянути 'гарячі' тури
6.Впорядкувати замовлення за ціною
7.Вихід";
        private const string editCommands =
        @"Перелік даних на зміну:
1.Код замовлення
2.ПІБ замовника
3.Дата замовлення
4.Назва туру
5.Країна
6.Дата від'їзду
7.Дата приїзду
8.Кількість путівок у замовленні
9.Вартість 1 путівки
10.Знижка(у %) для 'гарячого' туру
11.Вихід";

        public static Dictionary<uint, Tour> tours = new Dictionary<uint, Tour>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            ReadFile();
            StartProgram();
        }

        //зчитує інформацію з файлу та по рядку передає методу CreateTour()
        public static void ReadFile()
        {
            string[] toursData = File.ReadAllLines("ToursBase.txt");
            foreach (string item in toursData)
                CreateTour(item);
        }

        //записує у StringBuilder данні зі словника, після цього переводить результат у рядок та перезаписує файл
        public static void WriteFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in tours.Values)
            {
                if (item is RegularTour regular)
                    stringBuilder.Append(regular.ToString());
                else if (item is HotTour hot)
                    stringBuilder.Append(hot.ToString());
            }
            File.WriteAllText("ToursBase.txt", stringBuilder.ToString());
        }

        //ділить рядок на частини та залежно від кількості цих частин створює замовлення та додає його до словника 
        public static bool CreateTour(string item)
        {
            try
            {
                string[] data = item.Split(' ');
                if (data.Length == 11)
                {
                    var tour = CreateRegularTours(data);
                    tours.Add(tour.OrderCode, tour);
                }
                else if (data.Length == 12)
                {
                    var tour = CreateHotTours(data);
                    tours.Add(tour.OrderCode, tour);
                }
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //отримує данні з GetTourValues та 12ї частини наданого рядка, на їх основі створює та повертає новий "гарячий" тур
        private static Tour CreateHotTours(string[] data)
        {
            try
            {
                var values = GetTourValues(data);
                double discount = Convert.ToDouble(data[11]) / 100;
                return new HotTour(values.OrderCode, values.CustomerName, values.OrderDate,
                                       values.TourName, values.Country, values.DepartureDate,
                                       values.ArrivalDate, values.VouchersNumbers, values.OneTicketCost, discount);
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        //отримує данні з GetTourValues, на їх основі створює та повертає новий звичайний тур
        private static Tour CreateRegularTours(string[] data)
        {
            try
            {
                var values = GetTourValues(data);
                return new RegularTour(values.OrderCode, values.CustomerName, values.OrderDate,
                                       values.TourName, values.Country, values.DepartureDate,
                                       values.ArrivalDate, values.VouchersNumbers, values.OneTicketCost);
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        //перетворює частини рядка на необхідні змінні та повертає їх
        public static (uint OrderCode, string CustomerName, DateTime OrderDate,
                       string TourName, string Country, DateTime DepartureDate,
                       DateTime ArrivalDate, uint VouchersNumbers, double OneTicketCost)
                       GetTourValues(string[] data)
        {

            try
            {
                uint OrderCode = Convert.ToUInt32(data[0]);
                string CustomerName = string.Format("{0} {1} {2}", data[1], data[2], data[3]);
                DateTime OrderDate = Convert.ToDateTime(data[4]);
                string TourName = data[5];
                string Country = data[6];
                DateTime DepartureDate = Convert.ToDateTime(data[7]);
                DateTime ArrivalDate = Convert.ToDateTime(data[8]);
                uint VouchersNumbers = Convert.ToUInt32(data[9]);
                double OneTicketCost = Convert.ToDouble(data[10]);
                return (OrderCode, CustomerName, OrderDate,
                        TourName, Country, DepartureDate,
                        ArrivalDate, VouchersNumbers, OneTicketCost);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //цикл що виводить у консоль наявні команди та надає можливість обрати команду введену з консолі, якщо натиснути 7 - цикл завершиться 
        private static void StartProgram()
        {
            while (true)
            {
                Console.WriteLine(commands);
                Console.Write("Оберіть команду: ");
                try
                {
                    if (!SelectOperation(Convert.ToInt32(Console.ReadLine()))) { return; }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                Console.WriteLine("Натисніть будь-яку клавішу.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        //селектор команд, що залежно від введеного числа віконує відповідну команду
        private static bool SelectOperation(int number)
        {
            switch (number)
            {
                case 1:
                    RegisterOrder();
                    WriteFile();
                    break;
                case 2:
                    ChooseOrder(EditOrder);
                    WriteFile();
                    break;
                case 3:
                    ChooseOrder(DeleteOrder);
                    WriteFile();
                    break;
                case 4:
                    ChooseOrder(CalculateCost);
                    break;
                case 5:
                    ViewHotTours();
                    break;
                case 6:
                    SortByCost();
                    break;
                case 7:
                    return false;
                default:
                    Console.WriteLine("Такої команди не існує!");
                    break;
            }
            return true;
        }

        //отримує данні з консолі та передає їх методу CreateTour
        private static void RegisterOrder()
        {
            Console.WriteLine("Введіть дані нового замовлення");
            Console.WriteLine("|Код замовлення|ПІБ замовника|Дата замовлення|Назва туру|Країна|Дата від'їзду|Дата приїзду|Кількість путівок у замовленні|Вартість 1 путівки|Знижка(у %) для 'гарячого' туру|");
            var data = Console.ReadLine();
            if (data != null) CreateTour(data);
        }

        //отримує з консолі код замовлення та передає його методу зазначенному у аргументі, якщо замовлення з таким кодом є у базі
        private static void ChooseOrder(Action<uint> action)
        {
            Console.Write("Введіть код замовлення: ");
            try
            {
                uint code = Convert.ToUInt32(Console.ReadLine());
                if (tours.ContainsKey(code))
                {
                    action(code);
                }
                else throw new Exception("Замовлення з таким кодом не знайдено!");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        //виводить перелік доступних для редагування даних та отримує з консолі яку саме змінну треба змінити та передає її SelectEdit
        private static void EditOrder(uint code)
        {
            Console.WriteLine(editCommands);
            Console.WriteLine("Що саме ви бажаєте змінити ?");
            try
            {
                uint command = Convert.ToUInt32(Console.ReadLine());
                SelectEdit(code, command);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        //залежно від переданих аргументів змінює необхідну інформацію про замовлення на данні введені з консолі, якщо заміна виконується успішно - виводить Success
        private static void SelectEdit(uint code, uint command)
        {
            Console.WriteLine("Введіть нове значення");
            switch (command)
            {
                case 1:
                    if (EditOrderCode(code)) Console.WriteLine("Виконано!");
                    break;
                case 2:
                    if (EditCustomerName(code)) Console.WriteLine("Виконано!");
                    break;
                case 3:
                    if (EditOrderDate(code)) Console.WriteLine("Виконано!");
                    break;
                case 4:
                    if (EditTourName(code)) Console.WriteLine("Виконано!");
                    break;
                case 5:
                    if (EditCountry(code)) Console.WriteLine("Виконано!");
                    break;
                case 6:
                    if (EditDepartureData(code)) Console.WriteLine("Виконано!");
                    break;
                case 7:
                    if (EditArrivalDate(code)) Console.WriteLine("Виконано!");
                    break;
                case 8:
                    if (EditVouchersNumbers(code)) Console.WriteLine("Виконано!");
                    break;
                case 9:
                    if (EditOneTicketCost(code)) Console.WriteLine("Виконано!");
                    break;
                case 10:
                    if (EditDiscount(code)) Console.WriteLine("Виконано!");
                    break;
                case 11:
                    return;
            }
        }

        //змінює у замовленні данні про країну
        private static bool EditCountry(uint code)
        {
            try
            {
                var newCountry = Console.ReadLine();
                if (newCountry == null) throw new Exception("Рядок порожній!");
                (tours[code] as Tour).Country = newCountry; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про знижку
        private static bool EditDiscount(uint code)
        {
            try
            {
                double newDiscount = Convert.ToDouble(Console.ReadLine());
                if (tours[code] is not HotTour) throw new Exception("Цей тур не є \"гарячим\"");
                (tours[code] as HotTour).Discount = newDiscount; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про вартість одного квитка
        private static bool EditOneTicketCost(uint code)
        {
            try
            {
                double newOneTicketCost = Convert.ToDouble(Console.ReadLine());
                (tours[code] as Tour).OneTicketCost = newOneTicketCost; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про кількість квитків
        private static bool EditVouchersNumbers(uint code)
        {
            try
            {
                uint newVouchersNumbers = Convert.ToUInt32(Console.ReadLine());
                (tours[code] as Tour).VouchersNumbers = newVouchersNumbers; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про дату приїзду
        private static bool EditArrivalDate(uint code)
        {
            try
            {
                DateTime newArrivalDate = Convert.ToDateTime(Console.ReadLine());
                (tours[code] as Tour).ArrivalDate = newArrivalDate; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про дату від'їзду
        private static bool EditDepartureData(uint code)
        {
            try
            {
                DateTime newDepartureDate = Convert.ToDateTime(Console.ReadLine());
                (tours[code] as Tour).DepartureDate = newDepartureDate; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про назву туру
        private static bool EditTourName(uint code)
        {
            try
            {
                var newTourName = Console.ReadLine();
                if (newTourName == null) throw new Exception("Рядок порожній!");
                (tours[code] as Tour).TourName = newTourName; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про дату замовлення
        private static bool EditOrderDate(uint code)
        {
            try
            {
                DateTime newOrderDate = Convert.ToDateTime(Console.ReadLine());
                (tours[code] as Tour).OrderDate = newOrderDate; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про ПІБ замовника
        private static bool EditCustomerName(uint code)
        {
            try
            {
                var newCustomerName = Console.ReadLine();
                if (newCustomerName == null) throw new Exception("Рядок порожній!");
                (tours[code] as Tour).CustomerName = newCustomerName; return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }

        //змінює у замовленні данні про код замовлення
        private static bool EditOrderCode(uint code)
        {
            try
            {
                uint newCode = Convert.ToUInt32(Console.ReadLine());
                if (!tours.ContainsKey(newCode))
                {
                    if (tours[code] is RegularTour regular)
                    {
                        tours.Remove(code); //видаляє зі словника замовлення з таким кодом
                        regular.OrderCode = newCode;// змінює код на новий
                        tours.Add(regular.OrderCode, regular); //додає до словника замовлення зі зміненним кодом за зміненним ключем
                    }
                    else if (tours[code] is HotTour hot)
                    {
                        tours.Remove(code);
                        hot.OrderCode = newCode;
                        tours.Add(hot.OrderCode, hot);
                    }
                    return true;
                }
                else throw new Exception("Замовлення з таким кодом вже існує!");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }
        }


        //видаляє зі словника замовлення із заданим кодом  
        private static void DeleteOrder(uint code)
        {
            tours.Remove(code);
            Console.WriteLine("Виконано!");
        }

        //залежно від типу замовлення шукає його вартість та виводить у консоль
        private static void CalculateCost(uint code)
        {
            if (tours[code] is RegularTour regular)
                Console.WriteLine("{0:F2}", regular.CalculateCost());
            else if (tours[code] is HotTour hot)
                Console.WriteLine("{0:F2}", hot.CalculateCost());
        }

        //шукає у словнику усі гарячі замовлення, додає їх у перелік та виводить його у консоль, повертає цей перелік
        private static List<HotTour> ViewHotTours()
        {
            List<HotTour> hotTours = new List<HotTour>();
            foreach (var item in tours.Values)
            {
                if (item is HotTour hot)
                {
                    hotTours.Add(hot);
                    Console.WriteLine(hot.FormatTextForConsole());
                }
            }
            if (hotTours.Count == 0)
            {
                Console.WriteLine("\"Гарячих турів\" не знайдено");
            }
            return hotTours;
        }

        //додає усі замовлення зі словника у перелік, сортує його за допомогою класа Sorter, виводить відсортований перелік у консоль, повертає цей перелік
        private static List<Tour> SortByCost()
        {
            List<Tour> costList = new List<Tour>();
            foreach (var item in tours.Values)
            {
                if (item is HotTour hot)
                    costList.Add(hot);
                else if (item is RegularTour regular)
                    costList.Add(regular);
            }
            if (costList.Count == 0)
            {
                Console.WriteLine("Турів не знайдено");
            }
            costList.Sort(new Sorter());
            foreach (var item in costList)
            {
                if (item is HotTour hot)
                    Console.WriteLine(hot.FormatTextForConsole());
                else if (item is RegularTour regular)
                    Console.WriteLine(regular.FormatTextForConsole());
            }
            return costList;
        }
    }

    //клас що сортує замовлення за вартістю однієї путівки (від меньшої до більшої)
    public class Sorter : IComparer<Tour>
    {
        public int Compare(Tour? x, Tour? y)
        {
            if (x.OneTicketCost > y.OneTicketCost)
            {
                return 1;
            }
            else if (x.OneTicketCost == y.OneTicketCost)
            {
                return 0;
            }
            else return -1;
        }
    }
}