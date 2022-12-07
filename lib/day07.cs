namespace aoc2022 {
    public class Day07 : Solution {

        public class Node {
            public string name;
            public int size;
            public int index;
            public Node? parent;
            public Dictionary<string, Node> entries = new Dictionary<string, Node>();
            public Node(Node? parent, string name, int size) {
                this.name = name;
                this.size = size;
                this.parent = parent;
            }
            public int Fix() {
                foreach (var entry in entries.Values) size += entry.Fix();
                return size;
            }
        }

        public List<Node> allNodes = new List<Node>();

        public void parse(List<string> data) {
            Node root = new Node(null, "/", 0);
            Node current = root;
            allNodes = new List<Node>() { root };
            foreach (string cmd in data) {
                if (cmd == "$ cd /") {
                    current = root;
                } else if (cmd == "$ cd ..") {
                    current = current.parent!;
                } else if (cmd.StartsWith("$ cd ")) {
                    string name = cmd.Substring(5);
                    current = current.entries[name];
                } else if (cmd == "$ ls") {
                    // ignore
                } else {
                    var parts = cmd.Split(' ').ToArray();
                    int size = 0;
                    if (parts[0] != "dir") size=int.Parse(parts[0]);
                    Node child = new Node(current, parts[1], size);
                    current.entries.Add(parts[1], child);
                    child.index = allNodes.Count;
                    allNodes.Add(child);
                }
            }
            root.Fix();
        }

        public string part1() {
            long total = 0;
            foreach (Node n in allNodes) if (n.entries.Count > 0 && n.size <= 100000L) total += n.size;
            return total.ToString();
        }

        public string part2() {
            long freeSpace = 70000000 - allNodes[0].size;
            long minNeeded = 30000000 - freeSpace;
            long min = allNodes[0].size;
            foreach (Node n in allNodes) if (n.size < min && n.size > minNeeded) min = n.size;
            return min.ToString();
        }
    }
}
