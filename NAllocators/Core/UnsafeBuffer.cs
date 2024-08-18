using System.Runtime.InteropServices;

namespace NAllocators.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct UnsafeBuffer<T> : IEquatable<UnsafeBuffer<T>>
    where T : unmanaged
{
    public static readonly UnsafeBuffer<T> None = new();

    internal unsafe T*  Ptr  { get; }
    internal        int Size { get; }

    public unsafe BufferRef<T> this[int index] => new(Ptr, index * sizeof(T), Size);

    public unsafe UnsafeBuffer() => ArgumentNullException.ThrowIfNull(Ptr);

    internal unsafe UnsafeBuffer(void* ptr, int size)
    {
        ArgumentNullException.ThrowIfNull(ptr);

        Size = size;
        Ptr  = (T*)ptr;
    }

    public unsafe bool Equals(UnsafeBuffer<T> other) => Ptr == other.Ptr;

    public override bool Equals(object? obj) => obj is UnsafeBuffer<T> buffer && Equals(buffer);

    public override unsafe int GetHashCode() => unchecked((int)(long)Ptr);

    public static bool operator ==(UnsafeBuffer<T> left, UnsafeBuffer<T> right) => left.Equals(right);

    public static bool operator !=(UnsafeBuffer<T> left, UnsafeBuffer<T> right) => !(left == right);
}