namespace aoc2022 {
    public class Vis20 : Solution {
        private ASCIIRay renderer = new ASCIIRay(1920, 1080, 60, 32, "Day20");
        private Day20 solver = new Day20();
        public void parse(List<string> input) {
            solver.parse(input);
        }

        public string part1() {
            var order = new List<(long, long)>(solver.data.GetRange(0, 500));
            var temp = new Day20.BucketList<(long, long)>(order);
            int idx = -1, shift = 0, len = order.Count;
            HashSet<(long, long)> done = new HashSet<(long, long)>();
            int slow = 10, totswaps = 0, totshifts = 0, balcnt = 0;
            renderer.loop(cnt => {
                if (shift == 0) {
                    if (idx >= 0) done.Add(order[idx]);
                    if (idx == len - 1) return true;
                    var n = order[++idx];
                    shift = (int)(n.Item2 >= 0 ? n.Item2 % (len - 1) : (len - 1) - ((-n.Item2) % (len - 1)));
                    totshifts += shift;
                    if (idx > 0 && idx % temp.size == 0) {
                        balcnt++;
                        temp = new Day20.BucketList<(long, long)>(temp);
                    }
                } else if (cnt % slow == 0) {
                    int b = temp.idx[order[idx]];
                    int nb = (b + 1) % temp.buckets.Count;
                    if (temp.buckets[b][temp.buckets[b].Count - 1] == order[idx] && temp.buckets[nb].Count > 0 && shift > 0) {
                        shift -= temp.buckets[nb].Count;
                        temp.Shift(order[idx], temp.buckets[nb].Count);
                    } else {
                        if (shift > 0) {
                            temp.Shift(order[idx], 1);
                            shift--;
                        } else {
                            temp.Shift(order[idx], len - 2);
                            shift++;
                        }
                        totswaps++;
                    }
                }
                if (cnt % 300 == 0 && slow > 1) slow--;
                int tot = 0;
                renderer.SetColor(200, 200, 200, 255);
                renderer.WriteXY(0, temp.buckets.Count, String.Format("Total swaps executed {0} vs {1} shift total. Rebalanced: {2} times",
                                                                           totswaps, totshifts, balcnt));
                for (int b = 0; b < temp.buckets.Count; b++) {
                    renderer.SetColor(240, 200, 200, 255);
                    renderer.WriteXY(0, b, tot + "-" + (tot + temp.buckets[b].Count) + ": ");
                    renderer.SetColor(120, 120, 160, 255);
                    tot += temp.buckets[b].Count;
                    for (int i = 0; i < temp.buckets[b].Count; i++) {
                        if (temp.buckets[b][i] == order[idx]) {
                            renderer.SetColor(200, 250, 200, 255);
                            renderer.WriteXY(12 + i * 6, b, temp.buckets[b][i].Item2.ToString());
                        } else if (done.Contains(temp.buckets[b][i])) {
                            renderer.SetColor(60, 60, 160, 255);
                            renderer.WriteXY(12 + i * 6, b, temp.buckets[b][i].Item2.ToString());
                        } else {
                            renderer.SetColor(120, 80, 120, 255);
                            renderer.WriteXY(12 + i * 6, b, temp.buckets[b][i].Item2.ToString());
                        }
                    }
                }
                return false;
            });
            return "";
        }

        public string part2() {
            return "";
        }
    }
}
