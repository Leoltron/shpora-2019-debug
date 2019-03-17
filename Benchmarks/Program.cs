using BenchmarkDotNet.Running;
using JPEG;

namespace Benchmarks
{
    internal class Program
    {
        public static void Main(string[] args)
        {
//            BenchmarkRunner.Run<MemoryTraffic>();
//            BenchmarkRunner.Run<StructVsClassBenchmark>();
            BenchmarkRunner.Run<BenchmarkAdapter>();
//            BenchmarkRunner.Run<BitCountBenchmark>();
//            BenchmarkRunner.Run<ByteArrayEqualityBenchmark>();
//            BenchmarkRunner.Run<SortedVsUnsorted>();
//            BenchmarkRunner.Run<NewConstraintBenchmark>();
//            BenchmarkRunner.Run<MaxBenchmark>();
        }
    }
}
