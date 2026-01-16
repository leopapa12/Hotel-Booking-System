namespace Final.Models;

public class Reservation
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } 
    public DateTime BookingDate { get; set; }
    public bool IsPaid { get; set; }
}

public class ReservationData
{
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
}