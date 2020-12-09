using System;

namespace WebClient.Shared.Models
{
    public class MenuItem
    {
        public bool isActive { get; set; }
        public string iconColor { get; set; }
        public string label { get; set; }
        public Guid referenceId { get; set; }

        protected virtual void OnClickCallback(object e)
        {
            EventHandler<object> handler = ClickCallback;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler<object> ClickCallback;
        public void InvokClickCallback(object e) { OnClickCallback(e); }
    }
}
