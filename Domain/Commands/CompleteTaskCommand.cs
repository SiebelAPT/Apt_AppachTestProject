using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CompleteTaskCommand
    {
        private bool _isComplete = false;
        private Guid _id = Guid.Empty;

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

        public bool IsComplete
        {
            get
            {
                return _isComplete;
            }

            set
            {
                _isComplete = value;
            }
        }        
    }
}
