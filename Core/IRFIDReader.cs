using System;

namespace Core
{
    public class RFIDReadEventArgs
    {
        public int ID { get; set; }
    }

    public interface IRFIDReader
    {
        public event EventHandler<RFIDReadEventArgs> RFIDReadEvent;
    }
}