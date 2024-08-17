using NAllocators.Allocation;
using System.Runtime.CompilerServices;
using Xunit;

namespace NAllocators.Tests.Allocation;

public class ArenaAllocatorTests
{
    [Fact]
    public void GivenAllocatorRequested_WhenSize_ThenCreateAllocatorWithThatSize()
    {
        // Arrange
        using var allocator = new ArenaAllocator(16);

        // Act
        // Assert
        Assert.Equal(16, allocator.BufferSize);
        Assert.Equal(0, allocator.BufferOffset);
    }

    [Fact]
    public void GivenAllocatorRequested_WhenSizeWithBadAlignment_ThenCreateAllocatorWithAlignedSize()
    {
        // Arrange
        using var allocator = new ArenaAllocator(17);

        // Act
        // Assert
        Assert.Equal(24, allocator.BufferSize);
        Assert.Equal(0, allocator.BufferOffset);
    }

    [Fact]
    public void GivenAllocatorRequested_WhenNewInteger_ThenCreate()
    {
        // Arrange
        using var allocator = new ArenaAllocator(16);

        // Act
        var number = allocator.New<int>();
        number.RWRef.Value = 10;

        // Assert
        Assert.Equal(10, number.RORef.Value);
        Assert.Equal(16, allocator.BufferSize);
        Assert.Equal(Unsafe.SizeOf<int>(), allocator.BufferOffset);
    }

    [Fact]
    public void GivenAllocatorRequested_WhenDeleteInteger_ThenRemoveValueButUsedMemoryIsTheSame()
    {
        // Arrange
        using var allocator = new ArenaAllocator(16);

        var first = allocator.New<int>();
        first.RWRef.Value = 10;
        var second = allocator.New<int>();
        second.RWRef.Value = 5;

        // Act
        allocator.Delete(ref first);

        // Assert
        Assert.Equal(0, first.RORef.Value);
        Assert.Equal(5, second.RORef.Value);
        Assert.Equal(16, allocator.BufferSize);
        Assert.Equal(2 * Unsafe.SizeOf<int>(), allocator.BufferOffset);
    }

    [Fact]
    public void GivenAllocatorExists_WhenClear_ThenNoMemoryForAllocations()
    {
        // Arrange
        using var allocator = new ArenaAllocator(16);

        var first = allocator.New<int>();
        first.RWRef.Value = 10;

        // Act
        allocator.Clear();

        // Assert
        Assert.Equal(0, first.RORef.Value);
        Assert.Equal(16, allocator.BufferSize);
        Assert.Equal(0, allocator.BufferOffset);
    }

    [Fact]
    public void GivenAllocatorCreated_WhenNoMemoryLeft_ThenNotCreateMoreItems()
    {
        // Arrange
        using var allocator = new ArenaAllocator(2);

        // Act
        var first = allocator.New<int>();

        // Assert
        Assert.Equal(0, first.RORef.Value);
    }
}