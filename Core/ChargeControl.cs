using System;

namespace Core
{
    public class ChargeControl : IChargeControl
    {
        public bool IsCharging { get; private set; }
        private IDisplay _disp;
        private IUsbCharger _charger;

        public ChargeControl(IDisplay disp, IUsbCharger charger)
        {
            _disp = disp;
            _charger = charger;
            _charger.CurrentValueEvent += HandleCurrentValueChanged;
            IsCharging = false;
        }

        public bool IsConnected()
        {
            return _charger.Connected;
        }

        public void StartCharge()
        {
           _charger.StartCharge();
           IsCharging = true;
        }

        public void StopCharge()
        {
            _charger.StopCharge();
            IsCharging = false;
        }

        private void HandleCurrentValueChanged(Object o, CurrentEventArgs args)
        {
            double current = args.Current;
            if (current < 0)
            {
                //In a real system this would be handled otherwise, but we wanted to experiment with testing errors
                throw new Exception("Subzero current read");
            }

            if (current == 0)
            {
                _disp.DisplayChargingMessage("Finished Charging...");
            } 
            else if (current <= 5)
            {
                _charger.StopCharge();
            } 
            else if (current <= 500)
            {
                _disp.DisplayChargingMessage($"Charging: current:{current}");
            }
            else // Above 500mA
            {
                _charger.StopCharge();
                IsCharging = false;
                _disp.DisplayChargingMessage("Error - Stopped Charging!");
            }
        }
    }
}