using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CompleteMemberTaskCommand
    {
        public Guid Id { get; set; }
        public Boolean IsComplete { get; set; }
        public Guid AssignedToId { get; set; }
    }
}
