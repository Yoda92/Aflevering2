using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


namespace Core
{

    public class RFIDReaderSimulator : IRFIDReader
    {
        public void SimulateReadRFID(int id)
        {
            OnRFIDRead(new RFIDReadEventArgs() {ID = id});
        }

        public event EventHandler<RFIDReadEventArgs> RFIDReadEvent;


        private void OnRFIDRead(RFIDReadEventArgs e)
        {
            RFIDReadEvent?.Invoke(this, e);
        }
    }
}
