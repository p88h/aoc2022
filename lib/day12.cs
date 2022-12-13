using System;

namespace aoc2022 {
    public class Day12 : Solution {
        public char[,] map = new char[0, 0];
        public int[,] distance = new int[0, 0], prev = new int[0, 0];
        public List<(int, int)> lows = new List<(int, int)>();
        public int w, h, sx, sy, ex, ey, gen;
        public void parse(List<string> input) {
            w = input[0].Length;
            h = input.Count;
            map = new char[w, h];
            distance = new int[w, h];
            prev = new int[w, h];
            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    map[x, y] = input[y][x];
                    distance[x, y] = 0;
                    if (map[x, y] == 'S') { sx = x; sy = y; map[x, y] = 'a'; }
                    if (map[x, y] == 'E') { ex = x; ey = y; map[x, y] = 'z'; }
                    if (map[x, y] == 'a') { lows.Add((x, y)); }
                }
            }
        }

        int bfs(List<(int, int)> start) {
            List<(int, int)> stack = start;
            List<(int, int)> dirs = new List<(int, int)> { (-1, 0), (1, 0), (0, -1), (0, 1) };
            gen += 10000;
            int dst = ++gen;
            foreach ((int sx, int sy) in start) distance[sx, sy] = dst;
            while (stack.Count > 0) {
                List<(int, int)> next = new List<(int, int)>();
                dst++;
                foreach ((int cx, int cy) in stack) {
                    foreach ((int dx, int dy) in dirs) {
                        int nx = cx + dx, ny = cy + dy;
                        if (nx >= 0 && nx < w && ny >= 0 && ny < h) {
                            if (distance[nx, ny] < gen && map[nx, ny] <= map[cx, cy] + 1) {
                                prev[nx,ny] = cy * 1000 + cx;
                                if ((nx, ny) == (ex, ey)) return dst - gen;
                                distance[nx, ny] = dst;
                                next.Add((nx, ny));
                            }
                        }
                    }
                }
                stack = next;
            }
            return -1;
        }

        public string part1() { return bfs(new List<(int, int)> { (sx, sy) }).ToString(); }
        public string part2() { return bfs(lows).ToString(); }
    }
}
