using System;

namespace WebClient.Shared.Models
{
    public class FamilyMember
    {
        public Guid id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string avtar { get; set; }
    }
}
