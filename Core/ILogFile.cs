namespace Core
{
    public interface ILogFile
    {
        public void LogDoorLocked(int ID);
        public void LogDoorUnlocked(int ID);
    }
}