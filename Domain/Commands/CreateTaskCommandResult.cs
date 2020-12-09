using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTaskCommandResult
    {
        public TaskVm Payload { get; set; }
    }
}
