namespace aoc2022 {
    public class Day08 : Solution {
        private List<int[]> data = new List<int[]>();
        public void parse(List<string> input) {
            foreach (var s in input) {
                int[] arr = new int[s.Length];
                for (int i = 0; i < s.Length; i++) arr[i] = s[i] - '0' + 1;
                data.Add(arr);
            }
        }

        public string part1() {
            int sum = 0; int dim = data.Count - 1;
            List<int[]> visible = new List<int[]>();
            foreach (var s in data) visible.Add(new int[data[0].Length]);
            for (int i = 0; i < data.Count; i++) {
                int maxlr = 0, maxrl = 0, maxtb = 0, maxbt = 0;
                for (int j = 0; j < data.Count; j++) {
                    if (data[i][j] > maxlr) { maxlr = data[i][j]; visible[i][j] = 1; }
                    if (data[i][dim - j] > maxrl) { maxrl = data[i][dim - j]; visible[i][dim - j] = 1; }
                    if (data[j][i] > maxtb) { maxtb = data[j][i]; visible[j][i] = 1; }
                    if (data[dim - j][i] > maxbt) { maxbt = data[dim - j][i]; visible[dim - j][i] = 1; }
                }
            }
            for (int i = 0; i < data.Count; i++) for (int j = 0; j < data.Count; j++) sum += visible[i][j];
            return sum.ToString();
        }

        public string part2slow() {
            int dim = data.Count - 1;
            List<int[]> visible = new List<int[]>();
            foreach (var s in data) visible.Add(new int[data[0].Length]);
            for (int i = 0; i < data.Count; i++) {
                int[] cntlr = new int[11], cntrl = new int[11], cnttb = new int[11], cntbt = new int[11];
                for (int j = 0; j < data.Count; j++) {
                    visible[i][j]+=cntlr[data[i][j]];
                    for (int h = 1; h < 11; h++) cntlr[h] = h <= data[i][j] ? 1 : cntlr[h] + 1;
                    visible[i][dim-j]+=100*cntrl[data[i][dim-j]];
                    for (int h = 1; h < 11; h++) cntrl[h] = h <= data[i][dim-j] ? 1 : cntrl[h] + 1;
                    visible[j][i]+=100*100*cnttb[data[j][i]];
                    for (int h = 1; h < 11; h++) cnttb[h] = h <= data[j][i] ? 1 : cnttb[h] + 1;
                    visible[dim-j][i]+=100*100*100*cntbt[data[dim-j][i]];
                    for (int h = 1; h < 11; h++) cntbt[h] = h <= data[dim-j][i] ? 1 : cntbt[h] + 1;
                }
            }
            int max = 0;
            for (int i = 0; i < data.Count; i++) for (int j = 0; j < data.Count; j++) {
                int prod = 1, v = visible[i][j];
                for (int k = 0; k < 4; k++) { prod *= v%100; v/=100; }
                if (prod > max) max = prod;
            }
            return max.ToString();
        }

        public string part2() {
            long max = 0;
            int len = data.Count;
            for (int i = 0; i < data.Count; i++) {
                for (int j = 0; j < data.Count; j++) {
                    int tmp = 0, prod = 1;
                    for (int k = i - 1; k >= 0; k--) { tmp++; if (data[k][j] >= data[i][j]) break; }
                    prod *= tmp; tmp = 0;
                    for (int k = i + 1; k < len; k++) { tmp++; if (data[k][j] >= data[i][j]) break; }
                    prod *= tmp; tmp = 0;
                    for (int k = j - 1; k >= 0; k--) { tmp++; if (data[i][k] >= data[i][j]) break; }
                    prod *= tmp; tmp = 0;
                    for (int k = j + 1; k < len; k++) { tmp++; if (data[i][k] >= data[i][j]) break; }
                    prod *= tmp; tmp = 0;
                    if (prod > max) max = prod;
                }
            }
            return max.ToString();
        }
    }
}
