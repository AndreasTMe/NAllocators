using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NAllocators.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct UnsafeBuffer<T> : IEquatable<UnsafeBuffer<T>>
    where T : unmanaged
{
    public static readonly UnsafeBuffer<T> None = new();

    internal unsafe T*  Ptr  { get; }
    internal        int Size { get; }

    public BufferRef<T> this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            unsafe
            {
                return new BufferRef<T>(Ptr, index * sizeof(T), Size);
            }
        }
    }

    public UnsafeBuffer()
    {
        unsafe
        {
            Ptr = (T*)Unsafe.AsPointer(ref Unsafe.NullRef<T>());
        }

        Size = 0;
    }

    internal unsafe UnsafeBuffer(T* ptr, int size)
    {
        Ptr  = ptr;
        Size = size;
    }

    public bool Equals(UnsafeBuffer<T> other)
    {
        unsafe
        {
            return Ptr == other.Ptr;
        }
    }

    public override bool Equals(object? obj) => obj is UnsafeBuffer<T> buffer && Equals(buffer);

    public override int GetHashCode()
    {
        unsafe
        {
            return unchecked((int)(long)Ptr);
        }
    }

    public static bool operator ==(UnsafeBuffer<T> left, UnsafeBuffer<T> right) => left.Equals(right);

    public static bool operator !=(UnsafeBuffer<T> left, UnsafeBuffer<T> right) => !(left == right);
}