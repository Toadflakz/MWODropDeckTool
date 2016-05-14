using System;
using Prism.Events;

namespace MwoCWDropDeckBuilder.Model.Events
{
    public class BusyEvent : PubSubEvent<BusyEventArgs>
    {
    }

    public class BusyEventArgs : EventArgs
    {
        public bool IsBusy { get; set; }
        public string BusyMessage { get; set; }
        public TimeSpan Countdown { get; set; }
    }
}