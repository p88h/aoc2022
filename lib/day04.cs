namespace aoc2022 {
    public class Day04 : Solution {
        private List<int[]> data = new List<int[]>();
        public void parse(List<string> input) {
            foreach (var s in input) {
                var t = s.Split(',').ToArray();
                var t1 = t[0].Split('-').ToArray();
                var t2 = t[1].Split('-').ToArray();
                data.Add(new int[] { int.Parse(t1[0]), int.Parse(t1[1]), int.Parse(t2[0]), int.Parse(t2[1]) });
            }
        }

        public string part1() {
            int sum = 0;
            foreach (var a in data) if ((a[0] <= a[2] && a[1] >= a[3]) || (a[2] <= a[0] && a[3] >= a[1])) sum++;
            return sum.ToString();
        }

        public string part2() {
            long sum = 0;
            foreach (var a in data) if (!((a[1] < a[2]) || (a[0] > a[3]))) sum++;
            return sum.ToString();
        }
    }
}
