using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class AssignTaskCommand
    {
        public Guid Id { get; set; }
        public Guid AssignedToId { get; set; }
    }
}
