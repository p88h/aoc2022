using System.Collections;

namespace aoc2022 {
    public class Day20 : Solution {
        public List<(long, long)> data = new List<(long, long)>();
        public (long, long) zero;
        public int size;

        public void parse(List<string> input) {
            for (int i = 0; i < input.Count; i++) data.Add((i, int.Parse(input[i])));
            zero = data.Find(p => p.Item2 == 0);
            size = data.Count;
        }

        public class BucketList<T> : IEnumerable<T> where T : notnull {
            public int size; // sweet spot is around square root of N
            public List<List<T>> buckets = new List<List<T>> { new List<T>() };
            public Dictionary<T, int> idx = new Dictionary<T, int>();
            public BucketList(IEnumerable<T> source, int siz = 16) { size = siz; foreach (T value in source) Add(value); }
            public void Add(T value) {
                if (buckets[buckets.Count - 1].Count == size) buckets.Add(new List<T>());
                buckets[buckets.Count - 1].Add(value);
                idx[value] = buckets.Count - 1;
            }
            public void Shift(T value, int ofs) {
                int b = idx[value];
                ofs += buckets[b].IndexOf(value);
                buckets[b].Remove(value);
                while (ofs > buckets[b].Count) {
                    ofs -= buckets[b].Count;
                    b = (b + 1) % buckets.Count;
                }
                buckets[b].Insert(ofs, value);
                idx[value] = b;
            }
            public int IndexOf(T value) {
                int b = idx[value];
                int pos = buckets[b].IndexOf(value);
                for (int i = 0; i < b; i++) pos += buckets[i].Count;
                return pos;
            }
            public T Get(int index) {
                int b = 0;
                while (index >= buckets[b].Count) index -= buckets[b++].Count;
                return buckets[b][index];
            }

            public IEnumerator<T> GetEnumerator() { foreach (var bucket in buckets) foreach (T item in bucket) yield return item; }
            IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        }

        void mix(List<(long, long)> order, BucketList<(long, long)> values) {
            int len = order.Count;
            foreach (var n in order) {
                long shift = n.Item2 >= 0 ? n.Item2 % (len - 1) : (len - 1) - ((-n.Item2) % (len - 1));
                values.Shift(n, (int)shift);
            }
        }

        string result(BucketList<(long, long)> values) {
            int zpos = values.IndexOf(zero);
            long sum = values.Get((zpos + 1000) % size).Item2 + values.Get((zpos + 2000) % size).Item2 + values.Get((zpos + 3000) % size).Item2;
            return sum.ToString();
        }

        public string part1() {
            var order = new List<(long, long)>(data);
            var temp = new BucketList<(long, long)>(data, 64);
            mix(order, temp);
            return result(temp);
        }

        public string part2() {
            var order = data.Select(n => (n.Item1, n.Item2 * 811589153)).ToList();
            var temp = new BucketList<(long, long)>(order, 64);
            for (int i = 0; i < 10; i++) mix(order, temp);
            return result(temp);
        }
    }
}

