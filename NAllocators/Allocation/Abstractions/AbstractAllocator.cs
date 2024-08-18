using NAllocators.Core;
using System.Runtime.InteropServices;

namespace NAllocators.Allocation.Abstractions;

public abstract class AbstractAllocator : IAllocator, IDisposable
{
    private static readonly unsafe int Alignment = sizeof(nuint);

    protected readonly unsafe void* buffer;
    protected readonly        int   bufferSize;

    protected int bufferOffset;

    private bool _isDisposed;

    public int BufferSize   => bufferSize;
    public int BufferOffset => bufferOffset;

    protected AbstractAllocator(int size)
    {
        unsafe
        {
            var modulo = size % Alignment;
            if (modulo > 0)
            {
                size += Alignment - modulo;
            }

            buffer = NativeMemory.AllocZeroed((nuint)size);
        }

        bufferSize   = size;
        bufferOffset = 0;
    }

    ~AbstractAllocator() => Dispose(false);

    protected abstract unsafe void* Allocate(int size);

    protected abstract unsafe void Free(void* ptr, int size);

    public abstract void Clear();

    public UnsafeHandle<T> New<T>()
        where T : unmanaged
    {
        unsafe
        {
            var sizeOfT = sizeof(T);
            var ptr     = (T*)Allocate(sizeOfT);
            return new UnsafeHandle<T>(ptr, sizeOfT);
        }
    }

    public UnsafeBuffer<T> New<T>(int count)
        where T : unmanaged
    {
        unsafe
        {
            var totalSize = sizeof(T) * count;
            var ptr       = (T*)Allocate(totalSize);
            return new UnsafeBuffer<T>(ptr, totalSize);
        }
    }

    public void Delete<T>(ref UnsafeHandle<T> handle)
        where T : unmanaged
    {
        unsafe
        {
            Free(handle.Ptr, handle.Size);
        }
    }

    public void Delete<T>(ref UnsafeBuffer<T> handle)
        where T : unmanaged
    {
        unsafe
        {
            Free(handle.Ptr, handle.Size);
        }
    }

    public void Dispose()
    {
        if (bufferSize == 0)
        {
            return;
        }

        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            unsafe
            {
                NativeMemory.Free(buffer);
            }
        }

        _isDisposed = true;
    }

    protected static nuint GetAlignedAddress(nuint address)
    {
        var newAddress = address;
        var modulo     = newAddress & ((nuint)Alignment - 1);

        if (modulo != 0)
        {
            newAddress += (nuint)Alignment - modulo;
        }

        return newAddress;
    }

    private static int GetAlignedOffset(int value) => (value + (Alignment - 1)) & ~(Alignment - 1);
}