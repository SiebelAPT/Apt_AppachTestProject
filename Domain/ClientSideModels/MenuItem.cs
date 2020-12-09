using System;

namespace Domain.ClientSideModels
{
    public class MenuItem
    {
        public bool isActive {get; set;}
        public string iconColor { get; set; }
        public string label { get; set; }
        public Guid referenceId { get; set; }
    }
}
