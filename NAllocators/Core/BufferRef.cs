namespace NAllocators.Core;

public readonly struct BufferRef<T> : IEquatable<BufferRef<T>>
    where T : unmanaged
{
    private readonly unsafe T*  _ptr;
    private readonly        int _index;

    public unsafe RWRef<T> RWRef => new(&_ptr[_index]);

    public unsafe RORef<T> RORef => new(&_ptr[_index]);

    public unsafe BufferRef() => ArgumentNullException.ThrowIfNull(_ptr);

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    internal unsafe BufferRef(void* ptr, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(ptr);

        if ((uint)index >= (uint)count)
        {
            throw new IndexOutOfRangeException();
        }

        _index = index;
        _ptr   = (T*)ptr;
    }

    public unsafe bool Equals(BufferRef<T> other) => _ptr == other._ptr;

    public override bool Equals(object? obj) => obj is BufferRef<T> @ref && Equals(@ref);

    public override unsafe int GetHashCode() => unchecked((int)(long)_ptr);

    public static bool operator ==(BufferRef<T> left, BufferRef<T> right) => left.Equals(right);

    public static bool operator !=(BufferRef<T> left, BufferRef<T> right) => !(left == right);
}