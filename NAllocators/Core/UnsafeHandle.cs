using System.Runtime.InteropServices;

namespace NAllocators.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct UnsafeHandle<T> : IEquatable<UnsafeHandle<T>>
    where T : unmanaged
{
    public static readonly UnsafeHandle<T> None = new();

    internal unsafe T*  Ptr  { get; }
    internal        int Size { get; }

    public unsafe RWRef<T> RWRef => new(Ptr);
    public unsafe RORef<T> RORef => new(Ptr);

    public unsafe UnsafeHandle() => ArgumentNullException.ThrowIfNull(Ptr);

    internal unsafe UnsafeHandle(void* ptr, int size)
    {
        ArgumentNullException.ThrowIfNull(ptr);

        Size = size;
        Ptr  = (T*)ptr;
    }

    public unsafe bool Equals(UnsafeHandle<T> other) => Ptr == other.Ptr;

    public override bool Equals(object? obj) => obj is UnsafeHandle<T> handle && Equals(handle);

    public override unsafe int GetHashCode() => unchecked((int)(long)Ptr);

    public static bool operator ==(UnsafeHandle<T> left, UnsafeHandle<T> right) => left.Equals(right);

    public static bool operator !=(UnsafeHandle<T> left, UnsafeHandle<T> right) => !(left == right);
}