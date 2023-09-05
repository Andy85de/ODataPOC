using FluentAssertions;
using ODataQueryTests.ValidatorTests.Models;
using ODataWithSprache.Validators;

namespace ODataQueryTests.ValidatorTests;

public class ValidatorTests
{
    public static List<object[]> _objectsList =
        new List<object[]>
        {
            new object[]
            {
                nameof(UserProfileSettingObject.CreatedAt),
                new List<Type>
                {
                    typeof(DateTime)
                },
                true
            },
            new object[] { nameof(UserProfileSettingObject.Id), null, true },
            new object[] { "NotValid", null, false },
            new object[]
            {
                nameof(UserProfileSettingObject.Id),
                new List<Type>
                {
                    typeof(DateTime)
                },
                false
            },
            new object[]
            {
                nameof(UserProfileSettingObject.CounterLong),
                new List<Type>
                {
                    typeof(int),
                    typeof(DateTime),
                    typeof(string),
                    typeof(DateTimeOffset)
                },
                false
            },
            new object[]
            {
                nameof(UserProfileSettingObject.CounterLong),
                new List<Type>
                {
                    typeof(long)
                },
                true
            }
        };

    [Theory]
    [MemberData(nameof(_objectsList))]
    public void attribute_in_object_should_work(string attributeName, List<Type>? allowedTypes, bool result)
    {
        bool resultValidation =
            ValidatorHelper.ValidatePropertyInResult<UserProfileSettingObject>(attributeName, allowedTypes);

        result.Should().Be(resultValidation);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void attribute_not_valid_should_fail(string attribute)
    {
        Assert.Throws<ArgumentException>(
            () => ValidatorHelper.ValidatePropertyInResult<UserProfileSettingObject>(attribute));
    }
}
