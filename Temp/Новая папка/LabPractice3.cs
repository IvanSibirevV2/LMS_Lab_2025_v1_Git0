using System;
using System.IO;
using PlantUml.Net;

class Program
{
    static void Main()
    {
        // Текст диаграммы в формате PlantUML
        string plantUmlCode = @"
@startuml HotelUseCaseDiagram

left to right direction

actor ""Администратор"" as Admin
actor ""Клиент"" as Client
actor ""Система учета"" as System

rectangle Гостиница {
    Admin --> (Управление номерами)
    Admin --> (Управление клиентами)
    Admin --> (Оформление бронирования)
    Admin --> (Оформление выселения)
    Admin --> (Просмотр отчетности)

    Client --> (Поиск номеров)
    Client --> (Бронирование)
    Client --> (Оплата)

    System --> (Авторасчет стоимости)
    System --> (Проверка доступности)

    (Оформление бронирования) .> (Проверка доступности) : включает
    (Оформление выселения) .> (Авторасчет стоимости) : включает
}

@enduml
";

        // Создаём рендерер
        var renderer = new Renderer();

        // Генерируем PNG-изображение
        byte[] diagramBytes = renderer.Render(plantUmlCode, OutputFormat.Png);

        // Сохраняем в файл
        File.WriteAllBytes("HotelUseCaseDiagram.png", diagramBytes);

        Console.WriteLine("Диаграмма сохранена в HotelUseCaseDiagram.png");
    }
}