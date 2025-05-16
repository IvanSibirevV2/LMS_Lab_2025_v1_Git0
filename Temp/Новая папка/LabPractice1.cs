using System;
using System.Collections.Generic;
using System.Linq;

// Класс "Клиент"
public class Client
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string PassportData { get; set; }
    public string Comment { get; set; }
}

// Класс "Номер"
public class Room
{
    public int Id { get; set; }
    public string Number { get; set; }
    public int Capacity { get; set; }
    public string ComfortType { get; set; } // "Люкс", "Полулюкс", "Обычный"
    public decimal PricePerDay { get; set; }
}

// Класс "Бронирование"
public class Booking
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; } // Null, если клиент еще не выехал
    public string Note { get; set; }
}

// Класс "Гостиница" (управление данными)
public class Hotel
{
    public List<Client> Clients { get; set; } = new List<Client>();
    public List<Room> Rooms { get; set; } = new List<Room>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    // Добавление клиента
    public void AddClient(Client client)
    {
        Clients.Add(client);
    }

    // Добавление номера
    public void AddRoom(Room room)
    {
        Rooms.Add(room);
    }

    // Бронирование номера
    public bool BookRoom(int clientId, int roomId, DateTime checkInDate, string note)
    {
        var room = Rooms.Find(r => r.Id == roomId);
        if (room == null)
            return false;

        var booking = new Booking
        {
            Id = Bookings.Count + 1,
            ClientId = clientId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = null,
            Note = note
        };

        Bookings.Add(booking);
        return true;
    }

    // Выселение (фиксация даты выезда)
    public bool CheckOut(int bookingId, DateTime checkOutDate)
    {
        var booking = Bookings.Find(b => b.Id == bookingId);
        if (booking == null || booking.CheckOutDate != null)
            return false;

        booking.CheckOutDate = checkOutDate;
        return true;
    }

    // Расчет стоимости проживания
    public decimal CalculateBookingCost(int bookingId)
    {
        var booking = Bookings.Find(b => b.Id == bookingId);
        if (booking == null || booking.CheckOutDate == null)
            return 0;

        var room = Rooms.Find(r => r.Id == booking.RoomId);
        if (room == null)
            return 0;

        TimeSpan duration = booking.CheckOutDate.Value - booking.CheckInDate;
        return (decimal)duration.TotalDays * room.PricePerDay;
    }

    // Вывод списка свободных номеров
    public List<Room> GetAvailableRooms(DateTime date)
    {
        var bookedRoomIds = Bookings
            .Where(b => b.CheckOutDate == null || b.CheckOutDate > date)
            .Select(b => b.RoomId)
            .ToList();

        return Rooms.Where(r => !bookedRoomIds.Contains(r.Id)).ToList();
    }
}

class Program
{
    static void Main()
    {
        Hotel hotel = new Hotel();

        // Добавляем клиентов
        hotel.AddClient(new Client { Id = 1, LastName = "Иванов", FirstName = "Иван", PassportData = "1234 567890" });
        hotel.AddClient(new Client { Id = 2, LastName = "Петрова", FirstName = "Мария", PassportData = "0987 654321" });

        // Добавляем номера
        hotel.AddRoom(new Room { Id = 1, Number = "101", Capacity = 2, ComfortType = "Обычный", PricePerDay = 1500 });
        hotel.AddRoom(new Room { Id = 2, Number = "201", Capacity = 3, ComfortType = "Полулюкс", PricePerDay = 3000 });
        hotel.AddRoom(new Room { Id = 3, Number = "301", Capacity = 1, ComfortType = "Люкс", PricePerDay = 5000 });

        // Бронируем номера
        hotel.BookRoom(1, 1, new DateTime(2024, 1, 10), "Без питания");
        hotel.BookRoom(2, 3, new DateTime(2024, 1, 12), "VIP-обслуживание");

        // Выселяем клиента
        hotel.CheckOut(1, new DateTime(2024, 1, 15));

        // Выводим информацию
        Console.WriteLine("Свободные номера на 2024-01-20:");
        foreach (var room in hotel.GetAvailableRooms(new DateTime(2024, 1, 20)))
        {
            Console.WriteLine($"{room.Number} ({room.ComfortType}), {room.PricePerDay} руб./сут.");
        }

        Console.WriteLine("\nСтоимость проживания для бронирования #1:");
        Console.WriteLine(hotel.CalculateBookingCost(1) + " руб.");
    }
}