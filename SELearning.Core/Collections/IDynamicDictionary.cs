namespace SELearning.Core.Collections;

/// <summary>
/// A dictionary with string keys that can store arbitrarily typed values. The
/// </summary>
// We have split this into two interfaces so that you can, at compile-time,
// restrict some users (i.e. methods) to only read from the dictionary, not
// write to it.
public interface IDynamicDictionary : IDynamicDictionaryRead
{
    public void Set<T>(string key, T value);
}

public interface IDynamicDictionaryRead
{
    public T Get<T>(string key);
}
