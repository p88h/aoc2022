using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day16 : Solution {
        public List<int[]> graph = new List<int[]>();
        public List<int> flows = new List<int>();
        public int start;

        public void parse(List<string> input) {
            Regex rx = new Regex(@"^Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? (.*)$", RegexOptions.Compiled);
            List<(string l, int f, string[] d)> parsed = new List<(string, int, string[])>();
            for (int i = 0; i < input.Count; i++) {
                Match m = rx.Match(input[i]);
                parsed.Add((m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[3].Value.Split(", ")));
            }
            parsed = parsed.OrderByDescending(t => t.f).ToList();
            Dictionary<string, int> labelIndex = new Dictionary<string, int>();
            foreach (var (l, f, _) in parsed) {
                labelIndex[l] = labelIndex.Count;
                flows.Add(f);
            }
            foreach (var (l, _, d) in parsed) graph.Add(d.Select(s => labelIndex[s]).ToArray());
            start = labelIndex["AA"];
        }

        public string part1() {
            List<(int, int, int, int)> states = new List<(int, int, int, int)> { (start, 0, 0, 0) };
            int[] best = new int[4194304];
            int skipcnt = 0;
            int max = 0;
            for (int round = 1; round <= 29; round++) {
                List<(int, int, int, int)> nstates = new List<(int, int, int, int)>();
                foreach (var (n, bits, flow, acc) in states) {
                    int code = (n << 16) + bits;
                    int projected = acc + flow * (30 - round + 1);
                    if (best[code] > projected + 1) {
                        skipcnt++;
                        continue;
                    }
                    // open valve
                    if (flows[n] > 0 && (bits & (1 << n)) == 0) {
                        int nbits = bits | (1 << n);
                        int nflow = flow + flows[n];
                        code = (n << 16) + nbits;
                        projected = acc + flow + nflow * (30 - round);
                        if (projected + 1 > best[code]) {
                            nstates.Add((n, nbits, nflow, acc + flow));
                            best[code] = projected + 1;
                            if (projected > max) max = projected;
                        }
                    }
                    // go somewhere
                    foreach (int dst in graph[n]) {
                        code = (dst << 16) + bits;
                        projected = acc + flow * (30 - round + 1);
                        if (projected + 1 > best[code]) {
                            nstates.Add((dst, bits, flow, acc + flow));
                            best[code] = projected + 1;
                            if (projected > max) max = projected;
                        }
                    }
                }
                states = nstates;
            }
            return max.ToString();
        }

        public string part2() {
            List<(int, int, int, int, int)> states = new List<(int, int, int, int, int)> { (start, start, 0, 0, 0) };
            // elephant-sized.
            int[] best = new int[134217728];
            int max = 0;
            for (int round = 1; round <= 25; round++) {
                List<(int, int, int, int, int)> nstates = new List<(int, int, int, int, int)>();
                Console.WriteLine("Round: " + round + " States: " + states.Count + " Max " + max);
                int pc = 0, pbits = 0, pprev = -1;
                states = states.OrderByDescending(f => (f.Item1 << 21) + (f.Item2 << 15) + f.Item3).ToList();
                foreach (var (n, m, bits, flow, acc) in states) {
                    int code = (n << 21) + (m << 15);
                    int projected = acc + flow * (26 - round + 1);
                    if (best[code+bits] > projected + 1) continue;
                    if (pc == code && (pbits & bits) == bits && projected < pprev) continue;
                    pc = code;
                    pbits = bits;
                    pprev = projected;
                    List<(int, int, int)> tmp = new List<(int, int, int)>();
                    // elephant opens the valve, because now it can.
                    if (flows[m] > 0 && (bits & (1 << m)) == 0) {
                        int nbits = bits | (1 << m);
                        tmp.Add((m, nbits, flow + flows[m]));
                    }
                    // elephant moves everywhere it can, because it's an elephant.
                    foreach (int dst in graph[m]) tmp.Add((dst, bits, flow));
                
                    foreach (var (em, ebits, eflow) in tmp) {
                        // open valve
                        if (flows[n] > 0 && (ebits & (1 << n)) == 0) {
                            int nbits = ebits | (1 << n);
                            int nflow = eflow + flows[n];
                            code = n <= em ? (n << 21) + (em << 15) : (em << 21) + (n << 15);
                            code += nbits;
                            // elephant's flow isn't flowing yet
                            projected = acc + flow + nflow * (26 - round);
                            if (projected + 1 > best[code]) {
                                nstates.Add((n, em, nbits, nflow, acc + flow));
                                best[code] = projected + 1;
                                if (projected > max) max = projected;
                            }
                        }
                        // go somewhere
                        foreach (int dst in graph[n]) {
                            code = dst <= em ? (dst << 21) + (em << 15) : (em << 21) + (dst << 15);
                            code += ebits;
                            projected = acc + flow + eflow * (26 - round);
                            if (projected + 1 > best[code]) {
                                nstates.Add((dst, em, ebits, eflow, acc + flow));
                                best[code] = projected + 1;
                                if (projected > max) max = projected;
                            }
                        }
                    }
                }
                states = nstates;
            }
            return max.ToString();
        }
    }
}
