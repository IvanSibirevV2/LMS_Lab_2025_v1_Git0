using System;
using System.IO;
using PlantUml.Net;

class DiagramGenerator
{
    static void Main()
    {
        var renderer = new Renderer();

        // 1. Диаграмма компонентов
        GenerateComponentDiagram(renderer);

        // 2. Диаграмма развертывания
        GenerateDeploymentDiagram(renderer);

        Console.WriteLine("Диаграммы успешно сгенерированы!");
    }

    static void GenerateComponentDiagram(Renderer renderer)
    {
        string componentDiagram = @"
@startuml ComponentDiagram
title Диаграмма компонентов системы бронирования

package ""Гостиничная система"" {
    [Веб-интерфейс] as WebUI <<Angular>>
    [API Gateway] as API <<ASP.NET Core>>
    [Сервис бронирования] as Booking <<Microservice>>
    [Платежный сервис] as Payment <<Microservice>>
    [База данных] as DB <<PostgreSQL>>
}

WebUI --> API : REST API
API --> Booking : gRPC
Booking --> DB : Entity Framework
Booking --> Payment : RabbitMQ
Payment --> DB : Транзакции

note right of WebUI: Клиентское SPA-приложение
note left of DB: Основное хранилище данных
@enduml
";

        SaveDiagram(renderer, componentDiagram, "ComponentDiagram.png");
    }

    static void GenerateDeploymentDiagram(Renderer renderer)
    {
        string deploymentDiagram = @"
@startuml DeploymentDiagram
title Диаграмма развертывания системы

cloud ""Интернет"" {
    node ""Клиентские устройства"" {
        artifact ""Браузер"" as Browser
    }
}

node ""DMZ"" {
    artifact ""Балансировщик нагрузки"" as LB <<Nginx>>
    artifact ""Веб-сервер"" as Web <<Kestrel>>
}

node ""Серверный кластер"" {
    node ""Сервер приложений 1"" as App1 {
        artifact ""API Gateway"" as API1
    }
    node ""Сервер приложений 2"" as App2 {
        artifact ""API Gateway"" as API2
    }
}

node ""Сервисный кластер"" {
    artifact ""Сервис бронирования"" as Booking
    artifact ""Платежный сервис"" as Payment
}

node ""Хранилище данных"" {
    artifact ""PostgreSQL"" as DB <<Primary>>
    artifact ""PostgreSQL"" as Replica <<Replica>>
}

Browser --> LB : HTTPS (443)
LB --> Web
Web --> API1
Web --> API2
API1 --> Booking
API2 --> Booking
Booking --> DB
Booking --> Payment
Payment --> DB
DB ..> Replica : Репликация
@enduml
";

        SaveDiagram(renderer, deploymentDiagram, "DeploymentDiagram.png");
    }

    static void SaveDiagram(Renderer renderer, string diagramCode, string fileName)
    {
        try
        {
            byte[] imageData = renderer.Render(diagramCode, OutputFormat.Png);
            File.WriteAllBytes(fileName, imageData);
            Console.WriteLine($"Создан файл: {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании {fileName}: {ex.Message}");
        }
    }
}