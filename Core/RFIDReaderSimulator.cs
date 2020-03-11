using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UsbSimulator;

namespace Core
{

    class RFIDReaderSimulator : IRFIDReader
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
