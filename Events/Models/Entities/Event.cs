using Events.Models.Entities;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CreatedByUserId { get; set; }

    // Navigazioni
    public User CreatedBy { get; set; }
    public ICollection<EventParticipant> EventParticipants { get; set; }
    public ICollection<Message> Messages { get; set; }
}
