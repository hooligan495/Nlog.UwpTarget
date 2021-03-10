using NLog.Uwp.Target.Streams;
using Xunit;

namespace NLog.Uwp.Tests.Target.Streams
{
    public class FileStreamCacheTests
    {
        [Fact]
        public void FileStreamCache_Empty()
        {
            FileStreamCache cache = FileStreamCache.Empty;
            Assert.Equal(0, cache.Size);
            Assert.Null(cache.Factory);
        }
    }
}
