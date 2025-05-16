using System;
using System.IO;
using Idef3Net;
using Idef3Net.Models;
using Idef3Net.Generators;

class Idef3Modeler
{
    static void Main()
    {
        // Создаем модель процесса бронирования
        var idef3Model = new Idef3Diagram("Процесс бронирования гостиничного номера");

        // Добавляем элементы
        AddUnits(idef3Model);
        AddJunctions(idef3Model);
        AddLinks(idef3Model);
        AddObjects(idef3Model);

        // Генерируем диаграмму
        GenerateDiagram(idef3Model);
    }

    static void AddUnits(Idef3Diagram model)
    {
        model.AddUnit("U1", "Прием заявки", new Idef3Point(2, 3));
        model.AddUnit("U2", "Проверка доступности", new Idef3Point(4, 3));
        model.AddUnit("U3", "Оформление платежа", new Idef3Point(6, 3));
        model.AddUnit("U4", "Подтверждение брони", new Idef3Point(8, 3));
        model.AddUnit("U5", "Уведомление клиента", new Idef3Point(10, 3));
    }

    static void AddJunctions(Idef3Diagram model)
    {
        model.AddJunction("J1", JunctionType.And, new Idef3Point(3, 4));
        model.AddJunction("J2", JunctionType.Or, new Idef3Point(5, 2));
        model.AddJunction("J3", JunctionType.Xor, new Idef3Point(7, 4));
    }

    static void AddLinks(Idef3Diagram model)
    {
        // Основной поток
        model.AddLink("U1", "U2", LinkType.Sequential);
        model.AddLink("U2", "U3", LinkType.Sequential);
        model.AddLink("U3", "U4", LinkType.Sequential);
        model.AddLink("U4", "U5", LinkType.Sequential);

        // Альтернативные потоки
        model.AddLink("J1", "U2", LinkType.Relational);
        model.AddLink("U2", "J2", LinkType.Relational);
        model.AddLink("J2", "U3", LinkType.Relational);
        model.AddLink("U3", "J3", LinkType.Relational);
    }

    static void AddObjects(Idef3Diagram model)
    {
        model.AddObject("O1", "Заявка клиента", new Idef3Point(1, 3));
        model.AddObject("O2", "Данные о номерах", new Idef3Point(4, 5));
        model.AddObject("O3", "Платежные реквизиты", new Idef3Point(6, 5));
        model.AddObject("O4", "Подтверждение", new Idef3Point(10, 4));

        // Связи объектов с процессами
        model.AddObjectLink("O1", "U1");
        model.AddObjectLink("O2", "U2");
        model.AddObjectLink("O3", "U3");
        model.AddObjectLink("U5", "O4");
    }

    static void GenerateDiagram(Idef3Diagram model)
    {
        try
        {
            var generator = new Idef3ImageGenerator();

            // Настройка стиля
            var style = new Idef3Style
            {
                UnitColor = "#3498db",
                JunctionColor = "#e74c3c",
                ObjectColor = "#2ecc71",
                LinkColor = "#34495e",
                FontFamily = "Arial",
                FontSize = 10
            };
            generator.SetStyle(style);

            // Генерация PNG
            var pngBytes = generator.GeneratePng(model);
            File.WriteAllBytes("BookingIdef3.png", pngBytes);

            // Генерация SVG
            var svgContent = generator.GenerateSvg(model);
            File.WriteAllText("BookingIdef3.svg", svgContent);

            Console.WriteLine("IDEF3-диаграмма успешно сгенерирована:");
            Console.WriteLine("- BookingIdef3.png");
            Console.WriteLine("- BookingIdef3.svg");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка генерации: {ex.Message}");
        }
    }
}