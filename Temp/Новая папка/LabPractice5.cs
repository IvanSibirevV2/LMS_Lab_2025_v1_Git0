using System;
using System.IO;
using PlantUml.Net;

class Program
{
    static void Main()
    {
        var renderer = new Renderer();

        // Генерация DFD
        string dfdDiagram = @"
@startuml DFD
title Диаграмма потока данных (DFD): Система бронирования гостиницы

actor ""Клиент"" as Client
database ""База данных"" as DB

rectangle ""Система бронирования"" {
    (Поиск номеров) as Search
    (Бронирование) as Booking
    (Оплата) as Payment
}

Client --> Search : Запрос (дата, тип номера)
Search --> DB : Получить список номеров
DB --> Search : Данные номеров
Search --> Client : Доступные варианты

Client --> Booking : Выбор номера
Booking --> DB : Проверить доступность
DB --> Booking : Подтверждение
Booking --> Payment : Создать счет
Payment --> Client : Счет на оплату
Client --> Payment : Подтверждение оплаты
Payment --> DB : Обновить статус брони
@enduml
";

        byte[] dfdImage = renderer.Render(dfdDiagram, OutputFormat.Png);
        File.WriteAllBytes("DFD_Diagram.png", dfdImage);

        // Генерация Class Diagram
        string classDiagram = @"
@startuml ClassDiagram
title Диаграмма классов: Система бронирования гостиницы

class Client {
  +Id: int
  +LastName: string
  +FirstName: string
  +PassportData: string
  +BookRoom()
}

class Room {
  +Id: int
  +Number: string
  +Capacity: int
  +ComfortType: string
  +PricePerDay: decimal
  +IsAvailable()
}

class Booking {
  +Id: int
  +ClientId: int
  +RoomId: int
  +CheckInDate: DateTime
  +CheckOutDate: DateTime?
  +CalculateTotal()
}

class Payment {
  +Id: int
  +BookingId: int
  +Amount: decimal
  +ProcessPayment()
}

Client ""1"" --> ""0..*"" Booking
Room ""1"" --> ""0..*"" Booking
Booking ""1"" --> ""1"" Payment
@enduml
";

        byte[] classImage = renderer.Render(classDiagram, OutputFormat.Png);
        File.WriteAllBytes("Class_Diagram.png", classImage);

        Console.WriteLine("Диаграммы сохранены в DFD_Diagram.png и Class_Diagram.png");
    }
}