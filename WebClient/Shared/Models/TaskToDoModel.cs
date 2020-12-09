using System;

namespace WebClient.Shared.Models
{
    public class TaskToDoModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Guid AssignedToId { get; set; }
        public string Avatar { get; set; }
    }
}
