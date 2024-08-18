using NAllocators.Core;

namespace NAllocators.Extensions;

public static class UnsafeBufferExtensions
{
    public static ReadOnlySpan<T> AsSpan<T>(this UnsafeBuffer<T> str)
        where T : unmanaged
    {
        unsafe
        {
            return new ReadOnlySpan<T>(str.Ptr, str.Size);
        }
    }

    public static ReadOnlySpan<byte> AsBytes<T>(this UnsafeBuffer<T> str)
        where T : unmanaged
    {
        unsafe
        {
            return new ReadOnlySpan<byte>(str.Ptr, str.Size);
        }
    }
}