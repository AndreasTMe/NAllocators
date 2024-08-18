using NAllocators.Allocation.Abstractions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NAllocators.Allocation;

public sealed class ArenaAllocator : AbstractAllocator, IEquatable<ArenaAllocator>
{
    public ArenaAllocator(int size)
        : base(size)
    {
    }

    ~ArenaAllocator() => Dispose(false);

    public override unsafe void Clear()
    {
        NativeMemory.Clear(buffer, (nuint)bufferSize);
        bufferOffset = 0;
    }

    protected override unsafe void* Allocate(int size)
    {
        var currentPosition = (nuint)buffer + (nuint)bufferOffset;
        var positionOffset  = GetAlignedAddress(currentPosition);
        positionOffset -= (nuint)buffer;

        if ((int)positionOffset + size > bufferSize)
        {
            return Unsafe.AsPointer(ref Unsafe.NullRef<byte>());
        }

        bufferOffset += size;
        return (byte*)buffer + positionOffset;
    }

    protected override unsafe void Free(void* ptr, int size)
    {
        NativeMemory.Clear(ptr, (nuint)size);
    }

    public bool Equals(ArenaAllocator? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        unsafe
        {
            return buffer == other.buffer
                   && bufferSize == other.bufferSize
                   && bufferOffset == other.bufferOffset
                   && GetHashCode() == other.GetHashCode();
        }
    }

    public override bool Equals(object? obj) => obj is ArenaAllocator other && Equals(other);

    public override unsafe int GetHashCode() => ((nuint)buffer).GetHashCode();

    public static bool operator ==(ArenaAllocator lhs, ArenaAllocator rhs) => lhs.Equals(rhs);

    public static bool operator !=(ArenaAllocator lhs, ArenaAllocator rhs) => !(lhs == rhs);

    public override string ToString() => $"{nameof(ArenaAllocator)}[Size: {bufferSize} | Used: {bufferOffset}]";
}