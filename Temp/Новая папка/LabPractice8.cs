using System;
using System.IO;
using NetCoreIDEF;
using NetCoreIDEF.Models;

class IDEF0Modeler
{
    static void Main()
    {
        // Создаем модель бизнес-процесса бронирования
        var bookingProcess = new IDEF0Model("Бронирование номера в гостинице");

        // Добавляем основные элементы
        AddMainProcess(bookingProcess);
        AddInputsOutputs(bookingProcess);
        AddControlsMechanisms(bookingProcess);

        // Генерируем и сохраняем диаграмму
        GenerateDiagram(bookingProcess);
    }

    static void AddMainProcess(IDEF0Model model)
    {
        model.AddFunction(
            id: "A1",
            name: "Обработка заявки на бронирование",
            position: new IDEF0Position(2, 2),
            description: "Полный цикл обработки заявки от клиента");
    }

    static void AddInputsOutputs(IDEF0Model model)
    {
        // Входы
        model.AddInput("I1", "Запрос клиента", new IDEF0Position(0, 1));
        model.AddInput("I2", "Данные о доступности", new IDEF0Position(0, 3));

        // Выходы
        model.AddOutput("O1", "Подтверждение брони", new IDEF0Position(4, 1));
        model.AddOutput("O2", "Отчет о занятости", new IDEF0Position(4, 3));

        // Связи
        model.Connect("I1", "A1");
        model.Connect("I2", "A1");
        model.Connect("A1", "O1");
        model.Connect("A1", "O2");
    }

    static void AddControlsMechanisms(IDEF0Model model)
    {
        // Управление
        model.AddControl("C1", "Правила бронирования", new IDEF0Position(2, 0));

        // Механизмы
        model.AddMechanism("M1", "Система бронирования", new IDEF0Position(2, 4));
        model.AddMechanism("M2", "Персонал гостиницы", new IDEF0Position(1, 4));

        // Связи
        model.Connect("C1", "A1", ConnectionType.Control);
        model.Connect("A1", "M1", ConnectionType.Mechanism);
        model.Connect("A1", "M2", ConnectionType.Mechanism);
    }

    static void GenerateDiagram(IDEF0Model model)
    {
        try
        {
            var generator = new IDEF0DiagramGenerator();
            var diagram = generator.Generate(model);

            File.WriteAllBytes("BookingProcess.png", diagram.ToPng());
            Console.WriteLine("Диаграмма успешно сохранена как BookingProcess.png");

            // Дополнительные форматы
            File.WriteAllText("BookingProcess.svg", diagram.ToSvg());
            File.WriteAllText("BookingProcess.html", diagram.ToHtml());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка генерации: {ex.Message}");
        }
    }
}