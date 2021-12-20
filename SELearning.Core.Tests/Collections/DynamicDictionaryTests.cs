using System;
using System.Collections.Generic;
using SELearning.Core.Collections;

namespace SELearning.Core.Tests.Collections;

public class DynamicDictionaryTests
{
    [Fact]
    public void Get_GivenKeyAndType_ReturnsInsertedValue()
    {
        var dict = new DynamicDictionary();
        dict.Set("my-key", 1234);

        var result = dict.Get<int>("my-key");

        Assert.Equal(1234, result);
    }

    [Fact]
    public void Get_GivenKeyButIncorrectType_ThrowsKeyException()
    {
        var dict = new DynamicDictionary();
        dict.Set("my-key", 1234);

        Assert.Throws<KeyNotFoundException>(() => dict.Get<string>("my-key"));
    }

    [Fact]
    public void Get_GivenIncorrectKeyButCorrectType_ThrowsKeyException()
    {
        var dict = new DynamicDictionary();
        dict.Set("my-key", 1234);

        Assert.Throws<KeyNotFoundException>(() => dict.Get<int>("not-my-key"));
    }

    [Fact]
    public void Set_GivenNullValue_ThrowsArgumentNullException()
    {
        var dict = new DynamicDictionary();
        Assert.Throws<ArgumentNullException>(() => dict.Set<int?>("key", null));
    }

    [Fact]
    public void Set_GivenSameKeysAndDifferentTypes_InsertsBothValues()
    {
        var dict = new DynamicDictionary();
        dict.Set("my-key", 1234);
        dict.Set<string>("my-key", "pepperoni");

        Assert.Equal(1234, dict.Get<int>("my-key"));
        Assert.Equal("pepperoni", dict.Get<string>("my-key"));
    }

    [Fact]
    public void Set_GivenSameKeysAndSameTypes_OverwritesOldValue()
    {
        var dict = new DynamicDictionary();
        dict.Set("my-key", 1234);
        dict.Set("my-key", 5678);

        Assert.Equal(5678, dict.Get<int>("my-key"));
    }
}
