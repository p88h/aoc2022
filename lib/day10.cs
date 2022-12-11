using System.Text;

namespace aoc2022 {
    public class Day10 : Solution {

        public enum Operation { NOOP, ADDX }

        public class Instruction {
            public Operation op;
            public int arg;
            public Instruction(Operation operation, int argument) {
                op = operation;
                arg = argument;
            }
        }

        public List<Instruction> program = new List<Instruction>();
        public void parse(List<string> input) {
            foreach (var s in input) {
                var parts = s.Split(' ');
                int val = parts.Length > 1 ? int.Parse(parts[1]) : 0;
                program.Add(new Instruction(Enum.Parse<Operation>(parts[0].ToUpper()), val));
            }
        }

        public abstract class CPU {
            public int x = 1;
            public abstract void tick();
            public void exec(Instruction ins) {
                switch (ins.op) {
                    case Operation.NOOP: tick(); break;
                    case Operation.ADDX: tick(); tick(); x += ins.arg; break;
                }
            }
        }

        public class CPU1 : CPU {
            public int cycle = 20, str = 0;
            public override void tick() { if ((++cycle % 40) == 0) str += (cycle - 20) * x; }
        }

        public string part1() {
            CPU1 cpu = new CPU1();
            foreach (var ins in program) cpu.exec(ins);
            return cpu.str.ToString();
        }

        public class CPU2 : CPU {
            public int cycle = 0;
            public StringBuilder sb = new StringBuilder();
            public override void tick() {
                if (cycle % 40 == 0) sb.Append('\n');
                int posx = (cycle++ % 40);
                if (posx == x || posx == x - 1 || posx == (x + 1) % 40) {
                    sb.Append('█');
                } else {
                    sb.Append(' ');
                }
            }
        }

        public string part2() {
            CPU2 cpu = new CPU2();
            foreach (var ins in program) cpu.exec(ins);
            return cpu.sb.ToString();
        }
    }
}
