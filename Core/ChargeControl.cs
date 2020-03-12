using System;
using UsbSimulator;

namespace Core
{
    public class ChargeControl : IChargeControl
    {
        private IDisplay _disp;
        private IUsbCharger _charger;

        public ChargeControl(IDisplay disp, IUsbCharger charger)
        {
            _disp = disp;
            _charger = charger;
        } 
        
        public bool IsConnected()
        {
            return _charger.Connected;
        }

        public void StartCharge()
        {
           _charger.StartCharge(); 
        }

        public void StopCharge()
        {
            _charger.StopCharge();
        }

        private void HandleCurrentValueChanged(Object o, CurrentEventArgs args)
        {
            
        }
    }
}