using Events.Models.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }

    public int UserId { get; set; }
    public int EventId { get; set; }

    // Navigazioni
    public User User { get; set; }
    public Event Event { get; set; }
}
