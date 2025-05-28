namespace Events.Models
{
    public class CreateMessageDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string Content { get; set; }
    }
}
