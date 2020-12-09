using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CompleteTaskCommand
    {
        public Guid Id { get; set; }
        public Boolean IsComplete { get; set; }
    }
}
