namespace NAllocators.Core;

public readonly struct RORef<T> : IEquatable<RORef<T>>
    where T : unmanaged
{
    private readonly unsafe T* _ptr;

    public unsafe T Value => *_ptr;

    public unsafe RORef() => ArgumentNullException.ThrowIfNull(_ptr);

    internal unsafe RORef(T* ptr)
    {
        ArgumentNullException.ThrowIfNull(ptr);

        _ptr = ptr;
    }

    public unsafe bool Equals(RORef<T> other) => _ptr == other._ptr;

    public override bool Equals(object? obj) => obj is RORef<T> @ref && Equals(@ref);

    public override unsafe int GetHashCode() => unchecked((int)(long)_ptr);

    public static bool operator ==(RORef<T> left, RORef<T> right) => left.Equals(right);

    public static bool operator !=(RORef<T> left, RORef<T> right) => !(left == right);
}