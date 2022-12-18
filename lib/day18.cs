using System;
using System.Text;

namespace aoc2022 {
    public class Day18 : Solution {

        public List<int[]> cubes = new List<int[]>();
        public int min = 100, max = 0;

        public void parse(List<string> input) {
            foreach (var s in input) {
                var tmp = new int[3];
                var sn = s.Split(',');
                for (int i = 0; i < 3; i++) {
                    tmp[i] = int.Parse(sn[i]);
                    max = Math.Max(max, tmp[i]);
                    min = Math.Min(min, tmp[i]);
                }
                cubes.Add(tmp);
            }
            min--; max++;
        }

        public string part1() {
            var sides = new HashSet<int>();
            foreach (var cube in cubes) {
                for (int side = 0; side < 6; side++) {
                    int code = 0;
                    for (int dim = 0; dim < 3; dim++) {
                        code = code * 100 + cube[dim] * 2;
                        if (dim == side / 2) code += (side % 2 == 0 ? 1 : -1);
                    }
                    sides.Add(code);
                }
            }
            var connected = cubes.Count * 6 - sides.Count;
            var exposed = sides.Count - connected;
            return exposed.ToString();
        }

        public List<(int,int,int)> trace = new List<(int, int, int)>();

        public string part2() {
            var blocks = new HashSet<int>();
            foreach (var cube in cubes) blocks.Add(cube[0] * 10000 + cube[1] * 100 + cube[2]);
            var steam = new List<(int, int, int)> { (min, min, min), (min, min, max), (min, max, min), (min, max, max), 
                                                    (max, min, min), (max, min, max), (max, max, min), (max, max, max) };
            var dirs = new List<(int, int, int)> { (-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1) };
            var visited = new HashSet<int> { 10000 * min + 100 * min + min };
            var sides = new HashSet<int>();
            trace.Clear();
            while (steam.Count > 0) {
                trace.AddRange(steam);
                var more = new List<(int, int, int)>(steam.Count);
                foreach (var (x, y, z) in steam) {
                    foreach (var (dx, dy, dz) in dirs) {
                        if (x + dx < min || y + dy < min || z + dz < min) continue;
                        if (x + dx > max || y + dy > max || z + dz > max) continue;
                        int code = (x + dx) * 10000 + (y + dy) * 100 + (z + dz);
                        if (visited.Contains(code)) continue;
                        if (blocks.Contains(code)) {
                            sides.Add((x * 2 + dx) * 10000 + (y * 2 + dy) * 100 + (z * 2 + dz));
                            continue;
                        }
                        visited.Add(code);
                        more.Add((x + dx, y + dy, z + dz));
                    }
                }
                steam = more;
            }
            return sides.Count.ToString();
        }
    }
}
