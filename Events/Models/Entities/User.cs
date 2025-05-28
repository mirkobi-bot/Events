namespace Events.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public String Username { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String Phone { get; set; }

        // Navigazioni
        public ICollection<EventParticipant> EventParticipants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
