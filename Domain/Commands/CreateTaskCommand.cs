using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        private string _subject = string.Empty;
        private bool _isComplete = false;
        private Guid _assignedToId = Guid.Empty;

        public string Subject
        {
            get
            {
                return _subject;
            }

            set
            {
                _subject = value;
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
        public Guid AssignedToId
        {
            get
            {
                return _assignedToId;
            }

            set
            {
                _assignedToId = value;
            }
        }
    }
}
