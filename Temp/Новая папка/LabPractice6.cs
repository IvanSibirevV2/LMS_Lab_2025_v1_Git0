using System;
using System.IO;
using PlantUml.Net;

class Program
{
    static void Main()
    {
        var renderer = new Renderer();

        // Генерация Activity Diagram
        string activityDiagram = @"
@startuml ActivityDiagram
title Диаграмма деятельности: Процесс бронирования номера

start
:Клиент выбирает даты и тип номера;
:Система проверяет доступность;
if (Номер доступен?) then (Да)
  :Забронировать номер;
  :Создать счет;
  :Ожидание оплаты;
  if (Оплата получена?) then (Да)
    :Подтвердить бронь;
    :Отправить уведомление клиенту;
  else (Нет)
    :Отменить бронь;
  endif
else (Нет)
  :Показать альтернативы;
endif
stop
@enduml
";

        byte[] activityImage = renderer.Render(activityDiagram, OutputFormat.Png);
        File.WriteAllBytes("Activity_Diagram.png", activityImage);

        // Генерация State Diagram
        string stateDiagram = @"
@startuml StateDiagram
title Диаграмма состояний: Жизненный цикл бронирования

[*] --> Created
Created --> Reserved : Забронировать
Reserved --> Paid : Оплачено
Reserved --> Canceled : Отмена (до оплаты)
Paid --> Active : Дата заезда
Active --> Completed : Дата выезда
Completed --> [*]
Canceled --> [*]
@enduml
";

        byte[] stateImage = renderer.Render(stateDiagram, OutputFormat.Png);
        File.WriteAllBytes("State_Diagram.png", stateImage);

        Console.WriteLine("Диаграммы сохранены в Activity_Diagram.png и State_Diagram.png");
    }
}