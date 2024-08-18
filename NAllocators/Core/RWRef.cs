using System.Runtime.CompilerServices;

namespace NAllocators.Core;

public readonly struct RWRef<T> : IEquatable<RWRef<T>>
    where T : unmanaged
{
    private static readonly T Default = default;

    private readonly unsafe T* _ptr;

    public ref T Value
    {
        get
        {
            unsafe
            {
                if (_ptr == (void*)0)
                {
                    return ref Unsafe.AsRef(in Default);
                }

                return ref *_ptr;
            }
        }
    }

    internal unsafe RWRef(T* ptr) => _ptr = ptr;

    public bool Equals(RWRef<T> other)
    {
        unsafe
        {
            return _ptr == other._ptr;
        }
    }

    public override bool Equals(object? obj) => obj is RWRef<T> @ref && Equals(@ref);

    public override int GetHashCode()
    {
        unsafe
        {
            return unchecked((int)(long)_ptr);
        }
    }

    public static bool operator ==(RWRef<T> left, RWRef<T> right) => left.Equals(right);

    public static bool operator !=(RWRef<T> left, RWRef<T> right) => !(left == right);
}