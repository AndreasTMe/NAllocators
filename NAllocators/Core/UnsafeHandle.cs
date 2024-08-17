using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NAllocators.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct UnsafeHandle<T> : IEquatable<UnsafeHandle<T>>
    where T : unmanaged
{
    public static readonly UnsafeHandle<T> None = new();

    internal unsafe T*  Ptr  { get; }
    internal        int Size { get; }

    public RWRef<T> RWRef
    {
        get
        {
            unsafe
            {
                return new RWRef<T>(Ptr);
            }
        }
    }

    public RORef<T> RORef
    {
        get
        {
            unsafe
            {
                return new RORef<T>(Ptr);
            }
        }
    }

    public UnsafeHandle()
    {
        unsafe
        {
            Ptr = (T*)Unsafe.AsPointer(ref Unsafe.NullRef<T>());
        }

        Size = 0;
    }

    internal unsafe UnsafeHandle(T* ptr, int size)
    {
        Ptr  = ptr;
        Size = size;
    }

    public bool Equals(UnsafeHandle<T> other)
    {
        unsafe
        {
            return Ptr == other.Ptr;
        }
    }

    public override bool Equals(object? obj) => obj is UnsafeHandle<T> @ref && Equals(@ref);

    public override int GetHashCode()
    {
        unsafe
        {
            return unchecked((int)(long)Ptr);
        }
    }

    public static bool operator ==(UnsafeHandle<T> left, UnsafeHandle<T> right) => left.Equals(right);

    public static bool operator !=(UnsafeHandle<T> left, UnsafeHandle<T> right) => !(left == right);
}