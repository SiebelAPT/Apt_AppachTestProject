using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class AssignTaskCommand
    {
        Guid _id;
        Guid _assignTo;
        
        public Guid Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        public Guid AssignedToId
        {
            get
            {
                return _assignTo;
            }

            set
            {
                _assignTo = value;
            }
        }
    }
}
