namespace NAllocators.Core;

public readonly struct BufferRef<T> : IEquatable<BufferRef<T>>
    where T : unmanaged
{
    private readonly unsafe T*  _ptr;
    private readonly        int _index;

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    internal unsafe BufferRef(T* ptr, int index, int count)
    {
        if ((uint)index >= (uint)count)
        {
            throw new IndexOutOfRangeException();
        }

        _ptr   = ptr;
        _index = index;
    }

    public RWRef<T> RWRef
    {
        get
        {
            unsafe
            {
                return new RWRef<T>(&_ptr[_index]);
            }
        }
    }

    public RORef<T> RORef
    {
        get
        {
            unsafe
            {
                return new RORef<T>(&_ptr[_index]);
            }
        }
    }

    public bool Equals(BufferRef<T> other)
    {
        unsafe
        {
            return _ptr == other._ptr;
        }
    }

    public override bool Equals(object? obj) => obj is BufferRef<T> @ref && Equals(@ref);

    public override int GetHashCode()
    {
        unsafe
        {
            return unchecked((int)(long)_ptr);
        }
    }

    public static bool operator ==(BufferRef<T> left, BufferRef<T> right) => left.Equals(right);

    public static bool operator !=(BufferRef<T> left, BufferRef<T> right) => !(left == right);
}