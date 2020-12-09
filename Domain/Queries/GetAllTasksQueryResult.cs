using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Queries
{
    public class GetAllTasksQueryResult
    {
        public IEnumerable<TaskVm> Payload { get; set; }        
    }

}
