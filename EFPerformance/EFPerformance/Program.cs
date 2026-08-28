using EFPerformance.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var connectionStrings = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Northwind;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

var stopWatch1 = Stopwatch.StartNew();
for (int i = 0; i <= 100; i++)
{
    using var ctx = new NorthwindContext();
    var product = ctx.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == i);
}
stopWatch1.Stop();
Console.WriteLine($"Normal sorgu 100 kez tekrar ediyor. Toplam süre: {stopWatch1.ElapsedMilliseconds} ms");


//Derlenmiş sorgu, EF Functions gibi çalışır. 
var getById = EF.CompileQuery((NorthwindContext ctx, int id) => ctx.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id));

var sw2 = Stopwatch.StartNew();
for (int i = 0; i <= 100; i++)
{
    using var ctx = new NorthwindContext();
    var product = getById(ctx, i);
}
sw2.Stop();

Console.WriteLine($"Derlenmiş sorgu 100 kez tekrar ediyor. Toplam süre: {sw2.ElapsedMilliseconds} ms");

static decimal ExpensiveCalculation(decimal price)
{
    decimal result = price;
    for (int i = 0; i < 1_000_000; i++)
    {
        result = (result * 1.000001m) % 100_000m;
    }
    return result;
}

var prices = Enumerable.Range(1, 500).Select(i => (decimal)(i * 10)).ToList();
var sw3 = Stopwatch.StartNew();
var sirali = prices.Select(ExpensiveCalculation).Sum();
sw3.Stop();
Console.WriteLine($"Sıralı işlem, {sw3.ElapsedMilliseconds} ms sürdü ");

var sw4 = Stopwatch.StartNew();
var paralel = prices.AsParallel().Select(ExpensiveCalculation).Sum();
sw4.Stop();
Console.WriteLine($"Paralel işlem, {sw4.ElapsedMilliseconds} ms sürdü ");


