using BenchmarkDotNet.Attributes;

namespace JPEG
{
    public class BenchmarkAdapter
    {
        [Benchmark]
        public void Main()
        {
            Program.Main();
        }
    }
}
