namespace BuddyMemoryAllocation.Model.Contract
{
    public interface IMemoryManager
    {
        void Allocate(Process process);
        void Deallocate(int processId);
    }
}
