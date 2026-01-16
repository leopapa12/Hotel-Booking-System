using Final.Models;
using Final.Utils;

namespace Final.Services;

public class ReservationService
{
    private const string FilePath = "Data/Reservations.json";
    private ReservationData reservationData;
    private readonly RoomService roomService;

    public ReservationService(RoomService roomService)
    {
        this.roomService = roomService;
        reservationData = JsonHelper.LoadData<ReservationData>(FilePath);
    }

    public bool BookRoom(int userId, int roomId, DateTime checkIn, DateTime checkOut)
    {
        var room = roomService.GetRoomById(roomId);
        if (room == null || !room.IsAvailable)
            return false;

        int nights = (checkOut - checkIn).Days;
        if (nights <= 0)
            return false;

        decimal totalPrice = room.PricePerNight * nights;

        var reservation = new Reservation
        {
            ReservationId = reservationData.Reservations.Count > 0 ? reservationData.Reservations.Max(r => r.ReservationId) + 1 : 1,
            UserId = userId,
            RoomId = roomId,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            TotalPrice = totalPrice,
            Status = "Confirmed",
            BookingDate = DateTime.Now,
            IsPaid = false
        };

        reservationData.Reservations.Add(reservation);
        roomService.SetRoomAvailability(roomId, false);
        SaveData();
        return true;
    }

    public List<Reservation> GetUserReservations(int userId)
    {
        return reservationData.Reservations.Where(r => r.UserId == userId).ToList();
    }

    public List<Reservation> GetAllReservations()
    {
        return reservationData.Reservations;
    }

    public bool CancelReservation(int reservationId, int userId)
    {
        var reservation = reservationData.Reservations.FirstOrDefault(r => r.ReservationId == reservationId && r.UserId == userId);
        if (reservation != null && reservation.Status == "Confirmed")
        {
            reservation.Status = "Cancelled";
            roomService.SetRoomAvailability(reservation.RoomId, true);
            SaveData();
            return true;
        }
        return false;
    }

    public bool MakePayment(int reservationId, int userId)
    {
        var reservation = reservationData.Reservations.FirstOrDefault(r => r.ReservationId == reservationId && r.UserId == userId);
        if (reservation != null && !reservation.IsPaid)
        {
            reservation.IsPaid = true;
            SaveData();
            return true;
        }
        return false;
    }

    private void SaveData()
    {
        JsonHelper.SaveData(FilePath, reservationData);
    }
}

