using System;

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
            _charger.CurrentValueEvent += HandleCurrentValueChanged;
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
            double current = args.Current;
            if (current < 0)
            {
                //In a real system this would be handled otherwise, but we wanted to experiment with testing errors
                throw new Exception("Subzero current read");
            } else if (current <= 5)
            {
                _disp.DisplayChargingMessage("Charging done...");
            } else if (current <= 500)
            {
                _disp.DisplayChargingMessage("Charging...");
            }
            else // Above 500mA
            {
                _charger.StopCharge();
                _disp.DisplayChargingMessage("Error - Stopped Charging!");
            }
        }
    }
}