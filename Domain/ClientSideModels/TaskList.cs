using System;

namespace Domain.ClientSideModels
{
    public class TaskList
    {
        public Guid Id { get; set; }
        public bool isComplete {get; set;}
        public string subject { get; set; }
        public Guid AssignToId { get; set; }
    }
}
