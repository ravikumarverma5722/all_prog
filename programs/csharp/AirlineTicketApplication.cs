using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
 
public class AirlineTicketApplication
{
    public static async Task Main(string[] args)
    {
        var app = new AirlineTicketApplication();
        await app.ProcessTicketBooking("John Doe", "Economy", 2);
    }
 
    public async Task ProcessTicketBooking(string passengerName, string ticketClass, int numberOfTickets)
    {
        double totalCost = CalculateTotalCost(ticketClass, numberOfTickets);
        bool isEligibleForDiscount = CheckDiscountEligibility(totalCost);
       
        if (isEligibleForDiscount)
        {
            totalCost = ApplyDiscount(totalCost);
        }
       
        var booking = new Booking(passengerName, ticketClass, numberOfTickets, totalCost);
        bool isBookingSuccessful = SaveBookingToDatabase(booking);
       
        if (isBookingSuccessful)
        {
            await CallConfirmationAPI(booking);
        }
        else
        {
            Console.WriteLine("Booking failed. Please try again.");
        }
    }
 
    public double CalculateTotalCost(string ticketClass, int numberOfTickets)
    {
        double pricePerTicket;
       
        switch (ticketClass.ToLower())
        {
            case "economy":
                pricePerTicket = 100.00;
                break;
            case "business":
                pricePerTicket = 200.00;
                break;
            case "firstclass":
                pricePerTicket = 300.00;
                break;
            default:
                throw new ArgumentException("Invalid ticket class: " + ticketClass);
        }
       
        return pricePerTicket * numberOfTickets;
    }
 
    public bool CheckDiscountEligibility(double totalCost)
    {
        return totalCost > 500.00;
    }
 
    public double ApplyDiscount(double totalCost)
    {
        return totalCost * 0.9; // 10% discount
    }
 
    public bool SaveBookingToDatabase(Booking booking)
    {
        string connectionString = "your_connection_string_here";
        string sql = "INSERT INTO bookings (passenger_name, ticket_class, number_of_tickets, total_cost) VALUES (@PassengerName, @TicketClass, @NumberOfTickets, @TotalCost)";
 
        using (var conn = new SqlConnection(connectionString))
        {
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PassengerName", booking.PassengerName);
            cmd.Parameters.AddWithValue("@TicketClass", booking.TicketClass);
            cmd.Parameters.AddWithValue("@NumberOfTickets", booking.NumberOfTickets);
            cmd.Parameters.AddWithValue("@TotalCost", booking.TotalCost);
 
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
 
    public async Task CallConfirmationAPI(Booking booking)
    {
        string apiUrl = "https://api.example.com/confirmBooking";
        string requestPayload = $"{{\"name\":\"{booking.PassengerName}\",\"totalCost\":{booking.TotalCost}}}";
 
 
        using (var client = new HttpClient())
        {
            var content = new StringContent(requestPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
 
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Booking confirmed.");
            }
            else
            {
                Console.WriteLine("Failed to confirm booking.");
            }
        }
    }
}
