using System;

namespace Core
{
    public class RFIDReadEventArgs : EventArgs
    {
        public int ID { get; set; }
    }

    public interface IRFIDReader
    {
        public event EventHandler<RFIDReadEventArgs> RFIDReadEvent;
    }
}