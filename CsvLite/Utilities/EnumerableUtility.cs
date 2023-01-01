namespace CsvLite.Utilities;

public static class EnumerableUtility
{
    public static IEnumerable<T> Cache<T>(this IEnumerable<T> enumerable, List<T> cache, int? cacheLimit)
    {
        lock (cache)
        {
            foreach (var item in cache)
            {
                yield return item;
            }

            using var enumerator = enumerable.GetEnumerator();
            for (var i = 0; i < cache.Count; i++)
            {
                if (!enumerator.MoveNext()) yield break;
            }

            if (cacheLimit is not { } limit)
            {
                while (enumerator.MoveNext())
                {
                    cache.Add(enumerator.Current);
                    
                    yield return enumerator.Current;
                }
                
                yield break;
            }

            for (var i = cache.Count; i < limit; i++)
            {
                if (!enumerator.MoveNext()) yield break;

                cache.Add(enumerator.Current);

                yield return enumerator.Current;
            }

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}