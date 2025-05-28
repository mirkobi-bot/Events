using Events.Models.Entities;

public class EventParticipant
{
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime JoinedAt { get; set; }

    // Navigazioni
    public User User { get; set; }
    public Event Event { get; set; }
}
