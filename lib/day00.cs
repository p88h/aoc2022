namespace aoc2022 {
    public class Day00 : Solution
    {
        private List<int> data = new List<int>();
        public void parse(List<string> input)
        {
            foreach (var s in input) {
                data.Add(int.Parse(s));
            }
        }

        public string part1()
        {
            int sum = 0;
            foreach (var n in data) {
                sum += n;
            }
            return sum.ToString();
        }

        public string part2()
        {
            int prod = 1;
            foreach (var n in data) {
                prod *= n;
            }
            return prod.ToString();
        }
    }
}
