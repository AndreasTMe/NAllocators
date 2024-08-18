namespace NAllocators.Core;

public readonly struct RWRef<T> : IEquatable<RWRef<T>>
    where T : unmanaged
{
    private readonly unsafe T* _ptr;

    public unsafe ref T Value => ref *_ptr;

    public unsafe RWRef() => ArgumentNullException.ThrowIfNull(_ptr);

    internal unsafe RWRef(T* ptr)
    {
        ArgumentNullException.ThrowIfNull(ptr);

        _ptr = ptr;
    }

    public unsafe bool Equals(RWRef<T> other) => _ptr == other._ptr;

    public override bool Equals(object? obj) => obj is RWRef<T> @ref && Equals(@ref);

    public override unsafe int GetHashCode() => unchecked((int)(long)_ptr);

    public static bool operator ==(RWRef<T> left, RWRef<T> right) => left.Equals(right);

    public static bool operator !=(RWRef<T> left, RWRef<T> right) => !(left == right);
}