using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class UpdateTaskCommand
    {
        Guid _id;
        Guid _assignTo;
        string _subject;
        bool _isComplete;

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
                return _assignTo;
            }

            set
            {
                _assignTo = value;
            }
        }
    }
}
