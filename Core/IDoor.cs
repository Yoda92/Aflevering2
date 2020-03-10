using System;

namespace Core
{
    public class DoorStateEventArgs
    {
        public bool Open { get; set; }
    }
    
    public interface IDoor
    {
        public event EventHandler<DoorStateEventArgs> DoorStateChangedEvent;
        public void LockDoor();
        public void UnlockDoor();
    }
}