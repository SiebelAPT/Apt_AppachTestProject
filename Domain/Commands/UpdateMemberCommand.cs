using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class UpdateMemberCommand
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string Avatar { get; set; }
    }
}
