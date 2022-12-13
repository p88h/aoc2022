using System;

namespace aoc2022 {
    public class Day13 : Solution {

        class Packet : IComparable<Packet> {
            public List<Packet>? items;
            public int value;
            public Packet(int v) { value = v; }
            public Packet() { items = new List<Packet>(); }
            public Packet(Packet wrap) { items = new List<Packet> { wrap }; }
            public void Add(Packet p) { items!.Add(p); }
            public override string ToString() {
                if (items == null) return value.ToString();
                return '[' + string.Join(',', items) + ']';
            }
            public int CompareTo(Packet? other) {
                if (items == null && other!.items == null) return value.CompareTo(other.value);
                if (items != null && other!.items != null) {
                    int idx = 0;
                    while (idx < items.Count || idx < other.items.Count) {
                        if (idx >= items.Count) return -1;
                        if (idx >= other.items.Count) return 1;
                        int ret = items[idx].CompareTo(other.items[idx]);
                        if (ret != 0) return ret;
                        idx++;
                    }
                    return 0;
                }
                if (other!.items != null) return -other.CompareTo(this);
                if (items != null) {
                    if (items.Count == 0) return -1;
                    int ret = items[0].CompareTo(other);
                    if (ret != 0) return ret;
                    if (items.Count > 1) return 1;
                }
                return 0;
            }
        }

        List<Packet> packets = new List<Packet>();
        Packet two = new Packet(new Packet(new Packet(2))), six = new Packet(new Packet(new Packet(6)));

        public void parse(List<string> input) {
            List<Packet> stack = new List<Packet>();
            foreach (var s in input) {
                int pos = 0;
                Packet? root = null;
                while (pos < s.Length) {
                    if (s[pos] == '[') {
                        Packet cur = new Packet();
                        if (root != null) {
                            root.Add(cur);
                            stack.Add(root);
                        }
                        root = cur;
                    } else if (s[pos] == ']') {
                        if (stack.Count > 0) {
                            root = stack[stack.Count - 1];
                            stack.RemoveAt(stack.Count - 1);
                        }
                    } else if (s[pos] != ',') {
                        int npos = pos + 1;
                        while (s[npos] != ',' && s[npos] != ']') npos++;
                        root!.Add(new Packet(int.Parse(s[pos..npos])));
                        pos = npos - 1;
                    }
                    pos++;
                }
                if (root != null) packets.Add(root);
            }
            packets.Add(six);
            packets.Add(two);
        }

        public string part1() {
            int tot = 0;
            for (int i = 0; i < packets.Count; i += 2) {
                if (packets[i].CompareTo(packets[i + 1]) < 0) tot += i / 2 + 1;
            }
            return tot.ToString();
        }
        public string part2() {
            int ret = 1;
            packets.Sort();
            for (int i = 0; i < packets.Count; i++) if (packets[i] == two || packets[i] == six) ret *= i + 1;
            return ret.ToString();
        }
    }
}
