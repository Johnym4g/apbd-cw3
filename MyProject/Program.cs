using System;
using System.Linq;
using MyProject.Models;
using MyProject.Services;

var calculator = new PenaltyCalculator();
var service = new RentalLogic(calculator);

var laptop = new Laptop("Acer-v1", "Intel-i9", 32);
var camera = new Camera("Sony-v2", "35mm", 120);
var projector = new Projector("Samsung-v3", "4K", 5000);

service.AddEquipment(laptop);
service.AddEquipment(camera);
service.AddEquipment(projector);

var student = new Student("Jan", "Kowalski");
var employee = new Employee("Anna", "Kruk");

service.AddUser(student);
service.AddUser(employee);

Console.WriteLine("--- lista wszystkich sprzetow w systemie ---");
foreach (var eq in service.GetAllEquipment())
{
    Console.WriteLine($"- {eq.Name} (dostepny: {eq.IsAvailable})");
}

Console.WriteLine("\n--- scenariusz 1: poprawne wypozyczenie ---");
var result1 = service.RentEquipment(student, laptop, TimeSpan.FromDays(7));
Console.WriteLine(result1.IsSuccess ? "sukces: wypozyczono laptopa" : $"blad: {result1.ErrorMessage}");

Console.WriteLine("\n--- aktualnie dostepny sprzet ---");
foreach (var eq in service.GetAvailableEquipment())
{
    Console.WriteLine($"- {eq.Name}");
}

Console.WriteLine("\n--- scenariusz 2: proba wypozyczenia niedostepnego sprzetu ---");
var result2 = service.RentEquipment(employee, laptop, TimeSpan.FromDays(3));
Console.WriteLine(result2.IsSuccess ? "sukces" : $"oczekiwany blad: {result2.ErrorMessage}");

Console.WriteLine("\n--- scenariusz 3: przekroczenie limitu (student max 2) ---");
service.RentEquipment(student, camera, TimeSpan.FromDays(2));
var result3 = service.RentEquipment(student, projector, TimeSpan.FromDays(2));
Console.WriteLine(result3.IsSuccess ? "sukces" : $"oczekiwany blad limitu: {result3.ErrorMessage}");

Console.WriteLine("\n--- scenariusz 4: sprawdzanie zaleglosci ---");
var futureDate = DateTime.Now.AddDays(8); 
var overdueItems = service.GetOverdueRentals(futureDate).ToList();
Console.WriteLine($"liczba przetrzymanych sprzetow w dniu {futureDate.ToShortDateString()}: {overdueItems.Count}");

Console.WriteLine("\n--- scenariusz 5: zwrot w terminie ---");
var studentActive = service.GetActiveRentalsForUser(student).ToList();
var result4 = service.ReturnEquipment(studentActive[0], DateTime.Now.AddDays(5));
Console.WriteLine(result4.IsSuccess ? $"sprzet zwrocony. kara: {studentActive[0].PenaltyAmount} pln" : "blad zwrotu");

Console.WriteLine("\n--- scenariusz 6: zwrot opozniony (kara) ---");
var result5 = service.ReturnEquipment(studentActive[1], DateTime.Now.AddDays(10));
Console.WriteLine($"zwrot po terminie. naliczona kara: {studentActive[1].PenaltyAmount} pln");

Console.WriteLine("\n--- raport koncowy ---");
Console.WriteLine(service.GenerateReport());