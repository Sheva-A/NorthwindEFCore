using System.Text;
using NorthwindEfCore;
using NorthwindEfCore.Models;

Console.OutputEncoding = Encoding.UTF8;

// Контекст БД, згенерований через scaffolding.
// LINQ-запити до його таблиць EF Core перетворює на SQL і виконує,
// коли TablePrinter перебирає результат
using var db = new NorthwindContext();

// Запит 3. Загальна кількість замовлених одиниць для кожного продукту
var totalOrdered =
    from p in db.Products
    join od in db.OrderDetails on p.ProductId equals od.ProductId
    group od by new { p.ProductId, p.ProductName } into g
    select new
    {
        g.Key.ProductId,
        g.Key.ProductName,
        TotalOrdered = g.Sum(od => od.Quantity)
    };
TablePrinter.Print("Запит 3. Загальна кількість замовлених одиниць для кожного продукту",
    totalOrdered);

// Запит 4. Кількість клієнтів і співробітників з кожного міста, у якому є клієнти
var cityCounts =
    from c in db.Customers
    group c by c.City into g
    orderby g.Key
    select new
    {
        City = g.Key,
        CustomerCount = g.Count(),
        EmployeeCount = db.Employees.Count(e => e.City == g.Key)
    };
TablePrinter.Print("Запит 4. Кількість клієнтів і співробітників з кожного міста", cityCounts);

// Запит 5. Покупці з Великої Британії, які витратили більше 1000 у.о.
var ukCustomers =
    (from c in db.Customers
     join o in db.Orders on c.CustomerId equals o.CustomerId
     join od in db.OrderDetails on o.OrderId equals od.OrderId
     where c.Country == "UK"
     group od by new { c.CustomerId, c.CompanyName } into g
     select new
     {
         g.Key.CustomerId,
         g.Key.CompanyName,
         TotalSpent = g.Sum(od => od.UnitPrice * od.Quantity * (decimal)(1 - od.Discount))
     })
    .Where(customer => customer.TotalSpent > 1000); // умова після групування (HAVING)
TablePrinter.Print("Запит 5. Покупці з Великої Британії, які витратили більше 1000 у.о.",
    ukCustomers);

// Запит 6. Назви клієнтів, які розмістили замовлення в 1997 році (без повторів)
var customerIds1997 =
    from o in db.Orders
    where o.OrderDate!.Value.Year == 1997
    select o.CustomerId;
var customers1997 =
    (from c in db.Customers
     where customerIds1997.Contains(c.CustomerId)
     select new { c.CompanyName }).Distinct();
TablePrinter.Print("Запит 6. Клієнти, які розмістили замовлення в 1997 році", customers1997);
