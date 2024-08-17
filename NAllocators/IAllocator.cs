using NAllocators.Core;

namespace NAllocators;

public interface IAllocator
{
    UnsafeHandle<T> New<T>() where T : unmanaged;

    void Delete<T>(ref UnsafeHandle<T> handle) where T : unmanaged;

    void Clear();
}