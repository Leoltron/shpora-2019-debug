using BenchmarkDotNet.Attributes;

namespace JPEG
{
    public class BenchmarkProvider
    {
        [Benchmark]
        public void Main()
        {
            Program.Main();
        }
    }
}
