using System.Collections.Generic;

namespace Common.AsyncBundles.Bundles
{
    public interface IAsyncRefs
    {
        IEnumerable<object> Refs { get; }
        string Name { get; }
    }
}