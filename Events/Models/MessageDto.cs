namespace Events.Models
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string Username { get; set; }  // per mostrare chi ha scritto
    }
}
