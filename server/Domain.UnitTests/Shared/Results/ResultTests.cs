using Domain.Shared.Results;
using Xunit;

namespace Domain.UnitTests.Shared.Results;

public class ResultTests
{
    [Fact]
    public void SuccessResult_ShouldHaveIsSuccessTrue()
    {
        // Arrange
        var result = Result.Success();
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public void ErrorResult_ShouldHaveIsErrorTrue()
    {
        // Arrange
        var errorMessage = "An error occurred.";
        var result = Result.Failure(new Error(errorMessage, ErrorType.Internal));
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(errorMessage, result.Error?.Message);
    }

    [Fact]
    public void SuccessResultWithValue_ShouldHaveValue()
    {
        // Arrange
        var value = 42;
        var result = Result<int>.Success(value);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value);
    }
}
