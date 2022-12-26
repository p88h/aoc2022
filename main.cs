using System.Diagnostics;
using System.Collections.Concurrent;

namespace aoc2022 {
    public class Program {

        public static long Bench(String prefix, Func<string> fun) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            long count = 0;
            while (stopwatch.ElapsedMilliseconds < 999) {
                fun();
                count++;
            }
            stopwatch.Stop();
            long time_usec = stopwatch.ElapsedMilliseconds * 1000, time = time_usec;
            string unit = " ms";
            if (count > 500000) {
                time *= 1000;
                unit = " ns";
            } else if (count > 500) {
                unit = " µs";
            } else {
                time /= 1000;
            }
            long tpi = time / count;
            long upi = time_usec / count;
            int[] scale_ms = { 1, 5, 10, 20, 30, 40, 50, 60, 70, 80 };
            var sb = "[----------] ".ToArray();
            for (int i = 0; i < scale_ms.Length; i++) if ((upi + 500) / 1000 > scale_ms[i]) sb[i + 1] = '*';
            Console.WriteLine(prefix + tpi + unit + ",\t " + new string(sb) + count + " ips");
            return upi;
        }

        public static void Main(string[] args) {
            List<string> inputDays = new List<string>();
            bool runAll = false, benchmark = false, parallel = false;
            string classPrefix = "Day";
            for (int i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case "all": runAll = true; break;
                    case "bench": benchmark = true; break;
                    case "parallel": parallel = true; break;
                    case "vis": classPrefix = "Vis"; break;
                    case "rec": ViewerOptions.recordVideo = true; break;
                    default: inputDays.AddRange(args[i].Split(",")); break;
                }
            }
            if (inputDays.Count() == 0) {
                string[] files = Directory.GetFiles("input/");
                foreach (var f in files) {
                    int idx = f.LastIndexOf('.');
                    inputDays.Add(f.Substring(idx - 2, 2));
                }
                inputDays.Sort();
                if (!runAll) {
                    inputDays.RemoveRange(0, inputDays.Count() - 1);
                }
            } else {
                inputDays.Sort();
            }

            Console.WriteLine("Selected days: " + String.Join(",", inputDays));
            ConcurrentBag<long> tot0 = new ConcurrentBag<long>(), tot1 = new ConcurrentBag<long>(), tot2 = new ConcurrentBag<long>();
            int maxCores = Math.Min(8, Environment.ProcessorCount + 1) / 2;
            var opts = new ParallelOptions() { MaxDegreeOfParallelism = parallel ? maxCores : 1 };
            if (parallel && maxCores > 1) Console.WriteLine("Parallel execution: " + maxCores + " threads");
            Parallel.ForEach(inputDays, opts, day => {
                string className = "aoc2022." + classPrefix + day;
                Type? type = Type.GetType(className);
                if (type != null) {
                    Solution? sol = (Solution?)Activator.CreateInstance(type);
                    List<String> input = File.ReadAllLines("input/day" + day + ".txt").ToList();
                    sol!.parse(input);
                    if (benchmark) {
                        tot0.Add(Bench("Day " + day + " parser: ", () => {
                            Solution? s = (Solution?)Activator.CreateInstance(type);
                            s!.parse(input);
                            return "";
                        }));
                        tot1.Add(Bench("Day " + day + " part 1: ", sol.part1));
                        tot2.Add(Bench("Day " + day + " part 2: ", sol.part2));
                    } else {
                        Console.WriteLine("Day " + day + " part 1: " + sol.part1());
                        Console.WriteLine("Day " + day + " part 2: " + sol.part2());
                    }
                } else {
                    Console.WriteLine("Class " + className + " not found");
                }
            });
            if (benchmark && inputDays.Count > 1) {
                Console.WriteLine("Total parse : " + tot0.Sum() / 1000 + " ms");
                Console.WriteLine("Total part 1: " + tot1.Sum() / 1000 + " ms");
                Console.WriteLine("Total part 2: " + tot2.Sum() / 1000 + " ms");
                Console.WriteLine("Total AOC   : " + (tot0.Sum() + tot1.Sum() + tot2.Sum()) / 1000 + " ms");
            }
        }
    }
}
