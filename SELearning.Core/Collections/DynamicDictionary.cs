namespace SELearning.Core.Collections;

/// <summary>
/// An implementation of IDynamicDictionary that treats the string key and the
/// type as a composite key. That is, <code>Set<int>("key", 1)</code> and
/// <code>Set<string>("key", "hello")</code> will insert two different values
/// despite having the same string key. The same applies to Get.
/// </summary>
public class DynamicDictionary : IDynamicDictionary
{
    private IDictionary<(string, string), object> _dict = new Dictionary<(string, string), object>();

    public T Get<T>(string key)
    {
        var fullKey = (key, nameof(T));

        if (!_dict.ContainsKey(fullKey))
            throw new KeyNotFoundException($"there was no key '{key}' of type {nameof(T)} in the dictionary");

        return (T)_dict[fullKey];
    }

    public void Set<T>(string key, T value)
        => _dict.Add((key, nameof(T)), value ?? throw new ArgumentNullException(nameof(value)));
}
