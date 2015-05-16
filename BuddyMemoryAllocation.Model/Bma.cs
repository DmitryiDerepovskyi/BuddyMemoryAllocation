using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuddyMemoryAllocation.Model.Contract;

namespace BuddyMemoryAllocation.Model
{
    public class Bma : IMemoryManager
    {
        public Bma(OperationMemory memory)
        {
            _memory = memory;
            _freeBlocks.Add(memory.Map.First.Value);
        }

        private readonly OperationMemory _memory;
        private readonly List<BlockMemory> _freeBlocks = new List<BlockMemory>();
        public void Allocate(Process process)
        {
            BlockMemory freeBlock = null;
            foreach (var blockMemory in _freeBlocks)
            {
                if (blockMemory.Size >= process.Size)
                {
                    freeBlock = blockMemory;
                    _freeBlocks.RemoveAll(b => b.Address == freeBlock.Address);
                    break;
                }
            }
            if(freeBlock == null) throw new OutOfMemoryException();
            for (BlockMemory tempBlock = freeBlock; tempBlock.Size >= _memory.MinimalBlockSize; )
            {
                if (tempBlock.Size/2 >= process.Size)
                {
                    var listBlock = SplitBlock(tempBlock, tempBlock.Size/2);
                    _freeBlocks.Add(listBlock.Last());
                    tempBlock = listBlock.First();
                }
                else
                {
                    LoadProcess(tempBlock, process);
                    break;
                }
            }
            SortedListBlock(_freeBlocks);
        }

        public void Deallocate(int processId)
        {
            BlockMemory newFreeBlock = null;
            for(var node = _memory.Map.First; node != null; node = node.Next)
            {
                if(!node.Value.IsFree)
                    if (node.Value.Process.Id == processId)
                    {
                        node.Value.Free();
                        newFreeBlock = node.Value;
                        break;
                    }
            }
            if(newFreeBlock == null) throw new NullReferenceException("Can't free process");
            while (true)
            {
                var addressBuddy = newFreeBlock.Address ^ newFreeBlock.Size;
                var buddyBlock = _freeBlocks.Find(b => b.Address == addressBuddy);
                if (buddyBlock != null)
                {
                    newFreeBlock = JoinBlock(newFreeBlock, buddyBlock);
                    _freeBlocks.Remove(newFreeBlock);
                    _freeBlocks.Remove(buddyBlock);
                }
                else
                {
                    _freeBlocks.Add(newFreeBlock);
                    break;
                }
            }
            SortedListBlock(_freeBlocks);
        }

        public List<BlockMemory> SplitBlock(BlockMemory blockMemory, int size)
        {
            var blockMap = _memory.Map.Find(blockMemory);
            var newBlocks = new List<BlockMemory>();
            newBlocks.Add(_memory.Map.AddBefore(blockMap, 
                new BlockMemory(blockMemory.Address, size, null)).Value);
            newBlocks.Add(_memory.Map.AddAfter(blockMap, 
                new BlockMemory(blockMemory.Address + size, blockMap.Value.Size - size, null)).Value);
            _memory.Map.Remove(blockMemory);
            return newBlocks;
        }

        public BlockMemory JoinBlock(BlockMemory firstBlock, BlockMemory secondBlock)
        {
            var firstBlockMemory = firstBlock.Address < secondBlock.Address ? firstBlock : secondBlock;

            var nodeFirstBlock = _memory.Map.Find(firstBlockMemory);
            if (nodeFirstBlock == null)
                throw new NullReferenceException("The block doesn't allocated in memory");
            var newFreeBlock = new BlockMemory(
                firstBlockMemory.Address,
                firstBlockMemory.Size*2,
                null);
            _memory.Map.AddBefore(nodeFirstBlock, newFreeBlock);
            _memory.Map.Remove(firstBlock);
            _memory.Map.Remove(secondBlock);
            return newFreeBlock;
        }

        public void LoadProcess(BlockMemory blockMemory, Process process)
        {
            if(blockMemory == null || process == null) throw new ArgumentNullException();
            _memory.Map.Find(blockMemory).Value.LoadProcess(process);
        }

        public void SortedListBlock(List<BlockMemory> listBlockMemories)
        {
            listBlockMemories.Sort((memory, blockMemory) => memory.Size.CompareTo(blockMemory.Size));
        }
    }
}
