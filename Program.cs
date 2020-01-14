using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace ConsoleApp1
{
    public class Program
    {
        const int _numberOfLoops = 1000000;

        [MemoryDiagnoser]
        public class AddingToCollectionBenchmark
        {
            [Benchmark]
            public List<object> Boxing()
            {
                var list = new List<object>(_numberOfLoops);
                for (var i = 0; i < _numberOfLoops; i++)
                {
                    list.Add(i);
                }

                return list;
            }

            [Benchmark]
            public List<int> WithoutBoxing()
            {
                var list = new List<int>(_numberOfLoops);
                for (var i = 0; i < _numberOfLoops; i++)
                {
                    list.Add(i);
                }

                return list;
            }
        }

        [MemoryDiagnoser]
        public class ReadingFromCollectionBenchmark
        {
            AddingToCollectionBenchmark _addingToCollection = new AddingToCollectionBenchmark();
            private List<object> _boxedList;
            private List<int> _unboxedList;

            public ReadingFromCollectionBenchmark()
            {
                _boxedList = _addingToCollection.Boxing();
                _unboxedList = _addingToCollection.WithoutBoxing();
            }

            [Benchmark]
            public void Unboxing()
            {
                for (var i = 0; i < _numberOfLoops; i++)
                {
                    var unbox = (int)_boxedList[i];
                }
            }

            [Benchmark]
            public void WithoutUnboxing()
            {
                for (var i = 0; i < _numberOfLoops; i++)
                {
                    int simple = _unboxedList[i];
                }
            }
        }

        enum SomeEnum
        {
            A = 1,
            B = 2
        }

        static void Main(string[] args)
        {
            //var converted = new[] {1, 2}.Cast<SomeEnum>(); //toArray
            //var working = new[] {1, 2}.Cast<object>().Cast<SomeEnum>();
            BenchmarkRunner.Run(typeof(Program).Assembly, ManualConfig
                .Create(DefaultConfig.Instance)
                .With(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile));

            //var boxing = new AddingToCollectionBenchmark();
            //boxing.Boxing();
            //boxing.WithoutBoxing();
        }
    }
}
