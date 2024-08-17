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

public readonly struct RORef<T> : IEquatable<RORef<T>>
    where T : unmanaged
{
    private readonly unsafe T* _ptr;

    public T Value
    {
        get
        {
            unsafe
            {
                if (_ptr == (void*)0)
                {
                    return default;
                }

                return *_ptr;
            }
        }
    }

    internal unsafe RORef(T* ptr) => _ptr = ptr;

    public bool Equals(RORef<T> other)
    {
        unsafe
        {
            return _ptr == other._ptr;
        }
    }

    public override bool Equals(object? obj) => obj is RORef<T> @ref && Equals(@ref);

    public override int GetHashCode()
    {
        unsafe
        {
            return unchecked((int)(long)_ptr);
        }
    }

    public static bool operator ==(RORef<T> left, RORef<T> right) => left.Equals(right);

    public static bool operator !=(RORef<T> left, RORef<T> right) => !(left == right);
}