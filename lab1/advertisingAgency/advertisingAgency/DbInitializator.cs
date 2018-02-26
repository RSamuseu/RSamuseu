using System;
using System.Linq;

namespace advertisingAgency
{
    public static class DbInitializator
    {
        public static void Initialize(adAgencyContext db)
        {
            db.Database.EnsureCreated();
 
            if (db.Advedirsments.Any())
            {
                return;   // База данных инициализирована
            }

            int advedirsmentsNumber = 5;
            int customersNumber = 100;
            int ordersNumber = 100;
            string adType;
            string adDescription;
            string customerName;
            string customerAddress;
            string customerTelephone;

            Random randObj = new Random(1);
            //словарь типов рекламы
            string[] adTypes = { "Телевизионная", "Интернет-реклама", "Печатная", "Радио", "Уличная" };
            //словарь описание типов рекламы
            string[] adDescrip = { "Видеоролик, бегущая строка", "реклама в Интернете, блогах, SMM",
                                "реклама в газетах, наклейки, визтки", "реклама в радиовещании", "Баннеры" };
            //заполнение таблицы рекламы
            for (int advedirsmentId = 1; advedirsmentId <= advedirsmentsNumber; advedirsmentId++)
            {
                adType = adTypes[advedirsmentId - 1];
                adDescription = adDescrip[advedirsmentId - 1];
                db.Advedirsments.Add(new Advedirsments { AdType = adType, AdDescription = adDescription, AdvedirsmentsId = advedirsmentId });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

            //словарь название заказчиков
            string[] custNames = { "LowAgency", "FedEx", "BSBank", "Samsung", "Unicredit", "MTS", "Bordeux" };
            //словарь адресов заказчиков
            string[] custAddress = { "ул. Пушкина 2", "пр.Речицкий 80", "ул. Гагарина 38",
                                     "ул. Центральная 7", "ул. Киевская 7", "ул. Давыдовская 14",
                                     "ул. Б.Хмельницкого 80" };
            //словарь телефонов заказчиков
            string[] custTelehone = { "577-19-05", "303-23-68", "158-38-64", "190-65-87", "715-71-77" };
            //заполнение таблицы заказчики
            for (int customerId = 1; customerId <= customersNumber; customerId++)
            {
                customerName = custNames[randObj.Next(custNames.Length)];
                customerAddress = custAddress[randObj.Next(custAddress.Length)];
                customerTelephone = custTelehone[randObj.Next(custTelehone.Length)];
                db.Customers.Add(new Customers { CustomerName = customerName, CustomerAddress = customerAddress, CustomerTelephone = customerTelephone });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

            //массив id сотрудника
            int[] employesId = { 1, 2, 3, 4, 5 };
            //словарь рапсоложений реклам заказчика
            string[] locations = { "ул. Богданова", "пр. Речицкий", "ул. Барыкина", "ул. Машерова",
                                  "ул. Спартака", "пр. Космонавтов", "ул. Ветеранов", "пл. Соборная" };

            for(int orderId = 1; orderId <= ordersNumber; orderId++)
            {
                int advedirsmentId = randObj.Next(1, advedirsmentsNumber - 1);
                int customerId = randObj.Next(1, customersNumber - 1);
                int orderCost = randObj.Next(1000, 10000);
                int employeerId = employesId[randObj.Next(employesId.Length)];
                string location = locations[randObj.Next(locations.Length)];
                DateTime today = DateTime.Now.Date;
                DateTime dateOrder = today.AddDays(-orderId);
                DateTime dateBegin = dateOrder.AddDays(randObj.Next(7));
                DateTime dateEnd = dateBegin.AddDays(randObj.Next(7, 30));
                bool paymentStatus;
                int isPay = randObj.Next(0, 1);
                if (isPay == 1)
                    paymentStatus = true;
                else
                    paymentStatus = false;
                db.Orders.Add(new Orders {DateOrder = dateOrder, DateBegin = dateBegin, DateEnd = dateEnd,
                    Location = location, OrderCost = orderCost, PaymentStatus = paymentStatus, EmployeeId = employeerId,
                    CustomersId = customerId, AdvedirsmentsId = advedirsmentId });
            }
            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();
        }
    }
}
