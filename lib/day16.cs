using System.Text.RegularExpressions;

namespace aoc2022 {
    public class Day16 : Solution {
        public List<int[]> graph = new List<int[]>(), cgraph = new List<int[]>();
        public List<int> flows = new List<int>();
        public int start, valves = 0;

        public void compress_distances() {
            for (int origin = 0; origin < graph.Count; origin++) {
                int[] dist = new int[graph.Count];
                cgraph.Add(dist);
                List<int> stack = new List<int> { origin };
                if (flows[origin] == 0 && origin != start) continue;
                if (flows[origin] > 0) valves++;
                while (stack.Count > 0) {
                    List<int> nstack = new List<int> { };
                    foreach (var src in stack) {
                        foreach (var dst in graph[src]) {
                            if (dst != origin && dist[dst] == 0) {
                                dist[dst] = dist[src] + 1;
                                nstack.Add(dst);
                            }
                        }
                    }
                    stack = nstack;
                }
            }
        }

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
            compress_distances();
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

        public struct State {
            public int ta, tb, pa, pb, fa, fb, acc, bits;
        }

        public List<State> generate(State s) {
            List<State> results = new List<State>();
            for (int i = 0; i < valves; i++) {
                if ((s.bits & (1 << i)) == 0) {
                    // move and swap: copy b to a, rest assigned below
                    State d = new State { ta = s.tb, pa = s.pb, fa = s.fb };
                    int cost = cgraph[s.pa][i] + 1;  // time to go from pa to i 
                    d.bits = s.bits | (1 << i);  // .. and open the valve i
                    d.acc = s.acc + s.fa * cost; // time flows for a, so add it to the accumulator
                    d.pb = i;                    // a becomes b and is now at i
                    d.fb = s.fa + flows[i];      // flow of a (now b) is increased
                    d.tb = s.ta + cost;          // the new timepoint for a (now b)
                    // swap if unordered
                    if (d.tb < d.ta) d = new State { 
                        ta = d.tb, pa = d.pb, fa = d.fb, 
                        tb = d.ta, pb = d.pa, fb = d.fa, 
                        acc = d.acc, bits = d.bits };
                    results.Add(d);
                }
            }
            if (s.ta == s.tb) {
                List<State> nresults = new List<State>();
                foreach (State t in results) nresults.AddRange(generate(t));
                results = nresults;
            }
            return results;
        }

        public string part2() {
            int max = 0;
            int[] best = new int[1 << 15];
            List<List<State>> stacks = new List<List<State>>();
            stacks.Add(new List<State> { new State { pa = start, pb = start } });
            for (int i = 1; i < 26; i++) stacks.Add(new List<State>());
            for (int time = 0; time < 26; time++) {
                foreach (State s in stacks[time]) {
                    // drop out of time 
                    int projection = s.acc + s.fa * (26 - s.ta);
                    if (s.tb <= 26) projection += s.fb * (26 - s.tb);
                    if (projection > max) max = projection;
                    // this may be a bit wonky, since it doesn't really consider where we are or anything, 
                    // Just what's the best projection for a combination of valves up to this point in time. 
                    // But hey, it produces good results on some inputs fast.
                    // If we use longer codes : (s.pa << 21) + (s.pb << 15) + s.bits (and larger best cache)
                    // like part1 does, then this runs in about 1.5 seconds as opposed to 40 ms. That is fully 
                    // correct then, since the time factor is accounted into projection - same code with higher
                    // time will always produce a lower projection.
                    int code = s.bits;
                    if (best[code] > projection + 1) continue;
                    best[code] = projection + 1;
                    foreach (State t in generate(s)) if (t.tb < 26) stacks[t.ta].Add(t);
                }
            }
            return max.ToString();
        }
    }
}
