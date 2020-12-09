using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class GetAllTasksCommandResult
    {
        public IEnumerable<TaskVm> Payload { get; set; }
    }
}
