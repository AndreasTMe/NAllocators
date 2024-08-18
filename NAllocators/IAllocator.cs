using NAllocators.Core;

namespace NAllocators;

public interface IAllocator
{
    UnsafeHandle<T> New<T>() where T : unmanaged;

    UnsafeBuffer<T> New<T>(int count) where T : unmanaged;

    void Delete<T>(ref UnsafeHandle<T> handle) where T : unmanaged;
    
    void Delete<T>(ref UnsafeBuffer<T> handle) where T : unmanaged;

    void Clear();
}