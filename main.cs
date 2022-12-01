using System.Diagnostics;

namespace aoc2022 {
    public class Program {

        public static void Bench(String prefix, Func<string> fun) {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            long count = 0, time = 0;
            while (stopwatch.ElapsedMilliseconds < 999) {
                fun();
                count++;
            }
            stopwatch.Stop();
            time = stopwatch.ElapsedMilliseconds;
            long its = (1000 * count) / time;
            long npi = (time * 1000000) / count;
            Console.WriteLine(prefix + npi + " ns, " + its + " it/s");
        }

        public static void Main(string[] args) {
            List<string> inputDays = new List<string>();
            bool runAll = false;
            bool benchmark = false;
            string classPrefix = "Day";
            for (int i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case "all": runAll = true; break;
                    case "bench": benchmark = true; break;
                    case "vis": classPrefix = "Vis"; break;
                    default: inputDays.AddRange(args[i].Split(",")); break;
                }
            }
            if (inputDays.Count() == 0) {
                string[] files = Directory.GetFiles("input/");
                foreach (var f in files) {
                    int idx = f.LastIndexOf('.');
                    inputDays.Add(f.Substring(idx - 2, 2));
                }
                if (!runAll) {
                    inputDays.Sort();
                    inputDays.RemoveRange(0, inputDays.Count() - 1);
                }
            }

            Console.WriteLine("Selected days: " + String.Join(",", inputDays));
            foreach (var day in inputDays) {
                string className = "aoc2022." + classPrefix + day;
                Type? type = Type.GetType(className);
                if (type != null) {
                    Solution? sol = (Solution?)Activator.CreateInstance(type);
                    List<String> input = File.ReadAllLines("input/day" + day + ".txt").ToList();
                    sol!.parse(input);
                    if (benchmark) {
                        Bench("Day " + day + " part 1: ", sol.part1);
                        Bench("Day " + day + " part 2: ", sol.part2);
                    } else {
                        Console.WriteLine("Day " + day + " part 1: " + sol.part1());
                        Console.WriteLine("Day " + day + " part 1: " + sol.part2());
                    }
                } else {
                    Console.WriteLine("Class " + className + " not found");
                }
            }
        }
    }
}
