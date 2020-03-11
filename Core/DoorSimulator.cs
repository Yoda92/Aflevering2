using System;

namespace Core
{
    public class DoorSimulator : IDoor
    {
        public bool IsLocked { get; private set; }
        public event EventHandler<DoorStateEventArgs> DoorStateChangedEvent;

        public DoorSimulator()
        {
            IsLocked = false;
        }

        
        public void LockDoor()
        {
            IsLocked = true;
        }

        public void UnlockDoor()
        {
            IsLocked = false;
        }

        public void SimulateDoorOpen()
        {
            OnDoorStateChanged(new DoorStateEventArgs() {Open = true});
        }
        
        public void SimulateDoorClose()
        {
            OnDoorStateChanged(new DoorStateEventArgs() {Open = false});
        }

        private void OnDoorStateChanged(DoorStateEventArgs e)
        {
           DoorStateChangedEvent?.Invoke(this, e); 
        }
    }
}