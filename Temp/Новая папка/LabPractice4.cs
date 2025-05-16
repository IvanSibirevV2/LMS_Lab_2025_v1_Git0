using System;
using System.IO;
using PlantUml.Net;

class Program
{
    static void Main()
    {
        var renderer = new Renderer();

        // Генерация диаграммы кооперации
        string collaborationDiagram = @"
@startuml CollaborationDiagram
title Диаграмма кооперации: Бронирование номера

actor Клиент
participant ""Администратор"" as Admin
participant ""Система бронирования"" as System
participant ""База данных"" as DB

Клиент -> Admin: Запрос на бронирование
Admin -> System: Проверить доступность
System -> DB: Запрос свободных номеров
DB --> System: Список номеров
System --> Admin: Результат проверки
Admin -> System: Подтвердить бронирование
System -> DB: Зафиксировать бронь
DB --> System: Подтверждение
System --> Admin: Успешное бронирование
Admin --> Клиент: Подтверждение
@enduml
";

        byte[] collaborationImage = renderer.Render(collaborationDiagram, OutputFormat.Png);
        File.WriteAllBytes("CollaborationDiagram.png", collaborationImage);

        // Генерация диаграммы последовательности
        string sequenceDiagram = @"
@startuml SequenceDiagram
title Диаграмма последовательности: Бронирование номера

actor Клиент
participant ""Администратор"" as Admin
participant ""Система бронирования"" as System
participant ""База данных"" as DB

Клиент -> Admin: Запрос на бронирование\n(Дата, тип номера)
Admin -> System: Проверить доступность
System -> DB: SELECT * FROM Номера\nWHERE тип = 'Люкс' AND дата свободна
DB --> System: Результат запроса
System --> Admin: Доступные номера
Admin -> Клиент: Показать варианты
Клиент -> Admin: Выбор номера
Admin -> System: Подтвердить бронь
System -> DB: INSERT INTO Бронирования\n(номер, клиент, дата)
DB --> System: OK
System --> Admin: Успешно
Admin --> Клиент: Ваш номер 301
@enduml
";

        byte[] sequenceImage = renderer.Render(sequenceDiagram, OutputFormat.Png);
        File.WriteAllBytes("SequenceDiagram.png", sequenceImage);

        Console.WriteLine("Диаграммы сохранены в CollaborationDiagram.png и SequenceDiagram.png");
    }
}