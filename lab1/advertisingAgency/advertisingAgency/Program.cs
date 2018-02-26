using System;
using System.Collections;
using System.Linq;


namespace advertisingAgency
{
    class Program
    {
        static void Main(string[] args)
        {
            using (adAgencyContext db = new adAgencyContext())
            {
                DbInitializator.Initialize(db);

                Console.WriteLine("Первое задание");
                Print("Выборка записей таблицы Рекламы", db.Advedirsments.ToArray());
                Print("Выборка записей таблицы Заказчики", db.Customers.ToArray());

                Console.WriteLine("\nВторое задание\n");

                Print("Выборка с ограничением из таблицы Рекламы", db.Advedirsments.Where(a => a.AdvedirsmentsId == db.Advedirsments.First().AdvedirsmentsId).ToArray());
                Console.WriteLine("\nТретье задание\n");

                var query1 = from o in db.Orders
                             group o.OrderCost by o.CustomersId into gr
                             select new
                             {
                                 customersId = gr.Key,
                                 Сумма = gr.Sum()
                             };
                Print("Выборка с суммой стоимости заказов по клиентам:", query1.ToArray());
                Console.WriteLine("\nЧетвёртое задание\n");

                var query2 = from o in db.Orders
                             join c in db.Customers
                             on o.CustomersId equals c.CustomersId
                             orderby c.CustomerName descending
                             select new
                             {
                                 Заказчик = c.CustomerName,
                                 Дата_соглашения = o.DateOrder,
                                 Стоимость_заказа = o.OrderCost
                             };

                Print("Выборка по полям двух таблиц связи один-ко-многим:", query2.ToArray());
                Console.WriteLine("\nПятое задание\n");

                DateTime day = new DateTime(2018, 1, 1);
                var query3 = from o in db.Orders
                             join c in db.Customers
                             on o.CustomersId equals c.CustomersId
                             where(o.DateBegin >= day)
                             orderby c.CustomerName descending
                             select new
                             {
                                 Заказчик = c.CustomerName,
                                 Дата_соглашения = o.DateOrder,
                                 Стоимость_заказа = o.OrderCost
                             };

                Print("Выборка с ограничением по полям двух таблиц связи один-ко-многим:", query3.ToArray());
                Console.WriteLine("\nШестое задание\n");

                // Создать новую рекламу
                Advedirsments ad = new Advedirsments
                {
                    AdvedirsmentsId = 6,
                    AdType = "SMS-реклама",
                    AdDescription = "Реклама распространяется клиентам через SMS"
                };
                // Создать нового заказчика
                Customers customer = new Customers
                {
                    CustomerName = "БургерКинг",
                    CustomerAddress = "ул. Северная 7",
                    CustomerTelephone = "380-65-87"
                };
                db.Advedirsments.Add(ad);
                db.Customers.Add(customer);
                db.SaveChanges();
                Print("Таблица рекламы после вставки: ", db.Advedirsments.ToArray());
                Print("\nТаблица заказчики после вставки: ", db.Advedirsments.ToArray());

                Console.WriteLine("\nСедьмое задание\n");

                DateTime today = DateTime.Now.Date;
                //создать новый заказ
                Orders order = new Orders
                {
                    AdvedirsmentsId = 1,
                    CustomersId = 3,
                    OrderCost = 1000,
                    EmployeeId = 5,
                    Location = "ул. Набережная",
                    DateOrder = today.AddDays(-15),
                    DateBegin = today.AddDays(10),
                    DateEnd = today.AddDays(30),
                    PaymentStatus = true
                };
                db.Orders.Add(order);
                db.SaveChanges();
                Print("Таблица заказы после вставки: ", db.Orders.ToArray());

                Console.WriteLine("\nВосьмое задание\n");

                var adDel = db.Advedirsments.Where(a => a.AdvedirsmentsId == 6);
                db.Advedirsments.RemoveRange(adDel);
                db.SaveChanges();
                Print("Таблица рекламы после удаления записи: ", db.Advedirsments.ToArray());

                Console.WriteLine("\nДевятое задание\n");

                db.Orders.Remove(db.Orders.ToArray()[10]);
                db.SaveChanges();
                Print("Таблица заказы после удаления записи: ", db.Advedirsments.ToArray());

                db.Orders.SingleOrDefault(o => o.OrdersId == db.Orders.First().OrdersId).OrderCost += 3000;
                db.SaveChanges();
                Print("Обновлённая таблица заказы", db.Orders.ToArray());

                Console.ReadKey();
            }
        }

        static void Print(string sqltext, IEnumerable items)
        {
            Console.WriteLine(sqltext);
            Console.WriteLine("Записи: ");
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
