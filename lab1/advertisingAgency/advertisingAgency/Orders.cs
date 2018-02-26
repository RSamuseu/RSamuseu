using System;
using System.Collections.Generic;
using System.Text;

namespace advertisingAgency
{
    public class Orders
    {
        public int OrdersId { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public int OrderCost { get; set; }
        public bool PaymentStatus { get; set; }
        public int EmployeeId { get; set; }
        public int? CustomersId { get; set; }
        public int? AdvedirsmentsId { get; set; }
        public virtual Advedirsments Advedirsments { get; set; }
        public virtual Customers Customers { get; set; }

        public override string ToString()
        {
            return " Id Рекламы: " + AdvedirsmentsId + " Id Заказчика: " + CustomersId + " Цена Услуги: " + 
                OrderCost + " Id Сотрудника: " + EmployeeId + " Локация рекламы: " + Location
                + " Дата договора: " + DateOrder + " Дата начала: " + DateBegin + " Дата окончания: " + DateEnd +
                " Статус оплаты: " + PaymentStatus;
        }

    }
}
