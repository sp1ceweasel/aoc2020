using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve15();
        }

        public static void Solve1()
        {
            var path = System.IO.Directory.GetCurrentDirectory() + @"\input1.txt";
            var lines = File.ReadAllLines(path);
            var num = new List<int>();

            foreach (var line in lines)
            {
                try
                {
                    num.Add(Convert.ToInt32(line));
                }
                catch (Exception e)
                {
                }
            }

            for (int i = 0; i < num.Count; i++)
            for (int j = i + 1; j < num.Count; j++)
                if (num[i] + num[j] == 2020)
                    Console.WriteLine((num[i] * num[j]).ToString());

            for (int i = 0; i < num.Count; i++)
            for (int j = i + 1; j < num.Count; j++)
            for (int k = j + 1; k < num.Count; k++)
                if (num[i] + num[j] + num[k] == 2020)
                    Console.WriteLine((num[i] * num[j] * num[k]).ToString());
        }

        static public void Solve2()
        {
            var path = System.IO.Directory.GetCurrentDirectory() + @"\input2.txt";
            var lines = File.ReadAllLines(path);

            int result = 0;
            int result2 = 0;

            foreach (var line in lines)
            {
                try
                {
                    var rule = line.Split(':')[0];
                    var pw = line.Split(':')[1].Trim();
                    var low = Convert.ToInt32(rule.Split('-')[0]);
                    var remain = rule.Split('-')[1];
                    var high = Convert.ToInt32(remain.Split(' ')[0]);
                    var letter = remain.Split(' ')[1];

                    int i = pw.IndexOf(letter);
                    int count = 0;

                    if ((low <= pw.Length && pw[low - 1] == letter[0]) ^ (high <= pw.Length && pw[high - 1] == letter[0]))
                        result2++;

                    while (i != -1)
                    {
                        pw = pw.Substring(i + 1);
                        i = pw.IndexOf(letter);
                        count++;
                    }

                    if (count <= high && count >= low)
                        result++;
                }
                catch (Exception e)
                {
                    int x = 0;
                }
            }

            Console.WriteLine("Part1: " + result.ToString());
            Console.WriteLine("Part2: " + result2.ToString());
        }

        static public int HitsIn3(string[] lines, int dx, int dy)
        {
            int x = 0;
            int y = 0;

            int count = 0;

            do
            {
                if (lines[y][x % lines[y].Length] == '#')
                    count++;
                if (lines[y].Trim().Length == 0)
                    Console.WriteLine("bam");
                x += dx;
                y += dy;
            } while (y < lines.Length);

            return count;
        }

        static public void Solve3()
        {
            var path = System.IO.Directory.GetCurrentDirectory() + @"\input3.txt";
            var lines = File.ReadAllLines(path);


            Console.WriteLine("Part1: " + HitsIn3(lines, 3, 1).ToString());
            Console.WriteLine("Part2: " +
                              (HitsIn3(lines, 1, 1) *
                               HitsIn3(lines, 3, 1) *
                               HitsIn3(lines, 5, 1) *
                               HitsIn3(lines, 7, 1) *
                               HitsIn3(lines, 1, 2)
                              ).ToString());
        }


        static public void Solve4()
        {
            var path = System.IO.Directory.GetCurrentDirectory() + @"\input4.txt";
            var lines = File.ReadAllLines(path);


            string content = "";
            int count = 0;
            int count2 = 0;

            foreach (var line in lines)
            {
                content += " " + line;
                if (line.Trim() == "")
                {
                    if (content.Contains("byr:") &&
                        content.Contains("iyr:") &&
                        content.Contains("eyr:") &&
                        content.Contains("hgt:") &&
                        content.Contains("hcl:") &&
                        content.Contains("ecl:") &&
                        content.Contains("pid:")
                    )
                    {
                        count++;

                        Regex reg = new Regex(@"(\w+):([#\w]+)");
                        var res = reg.Matches(content);

                        bool fail = false;

                        foreach (Match g in res)
                        {
                            if (g.Groups[1].ToString() == "byr")
                            {
                                int val = Convert.ToInt32(g.Groups[2].ToString());
                                if (val < 1920 || val > 2002)
                                    fail = true;
                            }

                            if (g.Groups[1].ToString() == "iyr")
                            {
                                int val = Convert.ToInt32(g.Groups[2].ToString());
                                if (val < 2010 || val > 2020)
                                    fail = true;
                            }

                            if (g.Groups[1].ToString() == "eyr")
                            {
                                int val = Convert.ToInt32(g.Groups[2].ToString());
                                if (val < 2020 || val > 2030)
                                    fail = true;
                            }

                            if (g.Groups[1].ToString() == "hgt")
                            {
                                string hgt = g.Groups[2].ToString();

                                if (hgt.EndsWith("in"))
                                {
                                    hgt = hgt.Replace("in", "");
                                    int val = Convert.ToInt32(hgt);
                                    if (val < 59 || val > 76)
                                        fail = true;
                                }
                                else if (hgt.EndsWith("cm"))
                                {
                                    hgt = hgt.Replace("cm", "");
                                    int val = Convert.ToInt32(hgt);
                                    if (val < 150 || val > 193)
                                        fail = true;
                                }
                                else fail = true;
                            }

                            if (g.Groups[1].ToString() == "hcl")
                            {
                                string txt = g.Groups[2].ToString();
                                var ex = new Regex("^#[0-9a-f]{6}$");
                                if (!ex.Match(txt).Success)
                                    fail = true;
                            }

                            if (g.Groups[1].ToString() == "ecl")
                            {
                                string txt = g.Groups[2].ToString();
                                var ex = new Regex("^(amb|blu|brn|gry|grn|hzl|oth){1}$");
                                if (!ex.Match(txt).Success)
                                    fail = true;
                            }

                            if (g.Groups[1].ToString() == "pid")
                            {
                                string txt = g.Groups[2].ToString();
                                var ex = new Regex("^[0-9]{9}$");
                                if (!ex.Match(txt).Success)
                                    fail = true;
                            }
                        }

                        if (!fail)
                            count2++;
                    }

                    content = "";
                }
            }

            Console.WriteLine("Part1: " + count.ToString());
            Console.WriteLine("Part2: " + count2.ToString());
            Console.ReadLine();
        }

        static public void Solve5()
        {
            var path = System.IO.Directory.GetCurrentDirectory() + @"\input5.txt";
            var lines = File.ReadAllLines(path);

            int result = 0;
            bool[] seats = new bool[1024];

            foreach (var line in lines)
            {
                try
                {
                    var asBin = line.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1");
                    int val = Convert.ToInt32(asBin, 2);
                    seats[val] = true;
                    if (val > result)
                        result = val;
                }
                catch (Exception e)
                {
                    int x = 0;
                }
            }

            Console.WriteLine("Day 5, Part1: " + result.ToString());
            for (int i = 8 + 1; i < 1024 - 8 - 1; i++)
                if (!seats[i] && seats[i - 1] && seats[i + 1])
                    Console.WriteLine("Day 5, Part2: Possible Solution:" + i);
        }

        static public bool Search7(Dictionary<string, List<c7tuple>> map, string key)
        {
            if (key == "shiny gold")
                return true;
            if (map.ContainsKey(key))
            {
                foreach (var child in map[key])
                    if (Search7(map, child.color))
                        return true;
            }

            return false;
        }

        static public int Search7Count(Dictionary<string, List<c7tuple>> map, string key)
        {
            if (map.ContainsKey(key))
            {
                int res = 0;
                foreach (var child in map[key])
                {
                    res += child.count;
                    res += child.count * Search7Count(map, child.color);
                    Console.WriteLine(child.color + " " + child.count);
                }

                return res;
            }

            return 0;
        }

        public class c7tuple
        {
            public string color;
            public int count;
        }

        static public void Solve7()
        {
            var lines = File.ReadAllLines("input7.txt");

            var map = new Dictionary<string, List<c7tuple>>();

            foreach (var line in lines)
            {
                Regex reg = new Regex(@"(?<left>(\w+\s\w+)) bags contain(\s(?<count>\d+)\s(?<right>(\w+\s?)+)\s(bag|bags)[,\.])+");
                var res = reg.Match(line);
                if (res.Success)
                {
                    Console.WriteLine(res.ToString());
                    map[res.Groups["left"].Value] = new List<c7tuple>();
                    for (int i = 0; i < res.Groups["right"].Captures.Count; i++)
                        map[res.Groups["left"].Value].Add(new c7tuple()
                        {
                            color = res.Groups["right"].Captures[i].ToString(),
                            count = Convert.ToInt32(res.Groups["count"].Captures[i].ToString())
                        });
                }
            }

            var count = 0;
            foreach (var key in map.Keys)
                if (Search7(map, key))
                    count++;
            Console.WriteLine("a: " + (count - 1));
            Console.WriteLine("b: " + Search7Count(map, "shiny gold"));
            Console.ReadKey();
        }

        static public void Solve6()
        {
            var lines = File.ReadAllText(@"input6.txt");

            int count = 0;
            int count2 = 0;

            var reg = new Regex(@"(([a-z]+)\r\n)+\r\n");

            foreach (var match in reg.Matches(lines))
            {
                var dict = new Dictionary<char, int>();
                foreach (var cap in (match as Match).Groups[2].Captures)
                {
                    Console.WriteLine(cap);
                    foreach (char c in cap.ToString())
                        if (dict.ContainsKey(c))
                            dict[c]++;
                        else
                            dict[c] = 1;
                }

                Console.WriteLine(dict.Keys.Count);
                count += dict.Keys.Count;
                int numPersons = (match as Match).Groups[2].Captures.Count;

                foreach (var k in dict.Keys)
                    if (dict[k] == numPersons)
                        count2++;
            }

            Console.WriteLine("a: " + count);
            Console.WriteLine("b: " + count2);
            Console.ReadLine();
        }

        public static void Find8Stop(string[] lines, HashSet<int> visited, int i, int acc, bool reportInfiniteLoop, bool flipNopJmp)
        {
            while (i < lines.Length)
            {
                var line = lines[i];
                if (visited.Contains(i))
                {
                    if (reportInfiniteLoop)
                        Console.WriteLine("stopped at " + i + " with acc " + acc);
                    return;
                }

                visited.Add(i);

                if (line.StartsWith("nop"))
                {
                    if (flipNopJmp)
                    {
                        Find8Stop(lines, new HashSet<int>(visited), i + Convert.ToInt32(line.Substring(3)), acc, reportInfiniteLoop, false);
                    }

                    i++;
                }
                else if (line.StartsWith("acc"))
                {
                    acc += Convert.ToInt32(line.Substring(3));
                    i++;
                }
                else if (line.StartsWith("jmp"))
                {
                    if (flipNopJmp)
                    {
                        Find8Stop(lines, new HashSet<int>(visited), i + 1, acc, reportInfiniteLoop, false);
                    }

                    i += Convert.ToInt32(line.Substring(3));
                }
                else
                    i++;
            }

            Console.WriteLine("teminated with acc " + acc);
        }

        public static void Solve8()
        {
            var lines = File.ReadAllLines("input8.txt");
            Find8Stop(lines, new HashSet<int>(), 0, 0, true, false);
            Find8Stop(lines, new HashSet<int>(), 0, 0, false, true);
            Console.ReadLine();
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static void Solve9()
        {
            var nums = (from e in File.ReadAllLines("input9.txt") select Convert.ToInt64(e)).ToArray();
            int preamble = 25;
            Int64 res = 0;

            for (int i = preamble; i < nums.Length; i++)
            {
                bool found = false;
                var sum = nums[i];
                for (int j = i - 1; j >= i - preamble && !found; j--)
                for (int k = j - 1; k >= i - preamble && !found; k--)
                    if (nums[j] + nums[k] == sum)
                        found = true;

                if (!found)
                {
                    Console.WriteLine("9a: " + sum);
                    res = sum;
                    break;
                }
            }

            for (int i = 0; i < nums.Length - 1; i++)
            {
                Int64 sum = nums[i];
                for (int j = i + 1; j < nums.Length - 1; j++)
                {
                    sum += nums[j];
                    if (sum == res)
                    {
                        var cset = SubArray(nums, i, j - i + 1);
                        Console.WriteLine("9b: " + (cset.Max() + cset.Min()).ToString());
                        break;
                    }
                }
            }


            Console.ReadLine();
        }


        public static Int64 SearchIn10(Int64[] nums, int pos, Int64 valAtPos, Int64 max, Int64[] burned)
        {
            if (pos >= 0 && burned[pos] > -1)
                return burned[pos];

            Int64 res = 0;
            for (int i = pos + 1; i < nums.Length; i++)
            {
                if (nums[i] - valAtPos <= 3)
                    res += SearchIn10(nums, i, nums[i], max, burned);
                else
                    break;
            }

            if (max - valAtPos <= 3)
                res++;

            if (pos >= 0)
                burned[pos] = res;
            return res;
        }


        public static void Solve10()
        {
            var nums = (from e in File.ReadAllLines("input10.txt") select Convert.ToInt64(e)).OrderBy(ele => ele).ToArray();
            int jrange = 3;

            var tab = new int[jrange + 1];
            var device = nums.Max() + 3;

            long jolt = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                var diff = nums[i] - jolt;
                if (diff <= jrange)
                {
                    tab[diff]++;
                    jolt = nums[i];
                }
                else
                {
                    Console.WriteLine("no connection possible at {%1} with value {%2}", i, diff);
                }
            }

            var burned = (from i in nums select -1L).ToArray();

            Console.WriteLine("10a " + tab[1] * (tab[3] + 1));
            Console.WriteLine("10b " + SearchIn10(nums, -1, 0, device, burned));
            Console.ReadLine();
        }

        public static void Solve11()
        {
            var seats = (from row in File.ReadAllLines("input11.txt") select row.ToCharArray()).ToArray();
            var lastseats = seats;
            int count = 0;
            bool changed = false;

            do
            {
                changed = false;
                lastseats = seats.Select(row => row.Select(ele => ele).ToArray()).ToArray();
                for (int i = 0; i < lastseats.Length; i++)
                for (int j = 0; j < lastseats[i].Length; j++)
                {
                    seats[i][j] = lastseats[i][j];

                    if (lastseats[i][j] == 'L' && occupiedNeighbours(lastseats, i, j,1) == 0)
                    {
                        seats[i][j] = '#';
                        changed = true;
                    }

                    if (lastseats[i][j] == '#' && occupiedNeighbours(lastseats, i, j,1) >= 4)
                    {
                        seats[i][j] = 'L';
                        changed = true;
                    }
                }

                count = 0;
                for (int i = 0; i < seats.Length; i++)
                for (int j = 0; j < seats[i].Length; j++)
                    if (seats[i][j] == '#')
                        count++;

                Console.WriteLine("11a " + count);
            } while (changed);

            Console.WriteLine("11a " + count);
            //            Console.WriteLine("10b " + SearchIn10(nums, -1, 0, device, burned));

            seats = (from row in File.ReadAllLines("input11.txt") select row.ToCharArray()).ToArray();
            lastseats = seats;
            count = 0;
            changed = false;

            do
            {
                changed = false;
                lastseats = seats.Select(row => row.Select(ele => ele).ToArray()).ToArray();

                //foreach (var row in lastseats)
                //    Console.WriteLine(row);

                for (int i = 0; i < lastseats.Length; i++)
                for (int j = 0; j < lastseats[i].Length; j++)
                {
                    seats[i][j] = lastseats[i][j];

                    if (lastseats[i][j] == 'L' && occupiedNeighbours(lastseats, i, j, seats.Length) == 0)
                    {
                        seats[i][j] = '#';
                        changed = true;
                    }

                    if (lastseats[i][j] == '#' && occupiedNeighbours(lastseats, i, j, seats.Length) >= 5)
                    {
                        seats[i][j] = 'L';
                        changed = true;
                    }
                }

                count = 0;
                for (int i = 0; i < seats.Length; i++)
                for (int j = 0; j < seats[i].Length; j++)
                    if (seats[i][j] == '#')
                        count++;

                Console.WriteLine("11b " + count);
            } while (changed);

            Console.WriteLine("11b " + count);


            Console.ReadLine();
        }

        private static int occupiedNeighbours(char[][] seats, int i, int j, int range)
        {
            int count = 0;
            for (int y = -1; y < 2; y++)
            for (int x = -1; x < 2; x++)
                if (!(y == 0 && x == 0))
                    for (int r = 1; r <= range; r++)
                        if (y*r + i >= 0 && y*r + i < seats.Length && x*r + j >= 0 && x*r + j < seats[y*r+i].Length)
                        {
                            //Console.WriteLine(x+" "+y+" "+ seats[y][x]);

                            if (seats[y*r+i][x*r+j] == '#')
                            {
                                count++;
                                break;
                            }

                            if (seats[y*r+i][x*r+j] == 'L')
                                break;
                        }

            //Console.WriteLine("neighbours " + count);
            return count;
        }

        public static void SolveBarl()
        {
            var seats = File.ReadAllText("inputbarl.txt");
            var reg = new Regex(@"\d+\s+(L|B)\s+(([\w-]+\s)+)(D\s?\d+)\s.*\n");

            var matches = reg.Matches(seats);

            int count = 0;
            string content = "";

            foreach (Match line in matches)
            {
                content += line.Groups[2] + "; " + line.Groups[4] + "; 10g\n";
                count++;
                //if (count > 10)
                 //   break;
            }
            File.WriteAllText("barl",content);

            Console.ReadLine();
        }

        public static void Solve15()
        {
            var nums = (from e in File.ReadAllText("input15.txt").Split(',') select Convert.ToInt32(e)).ToArray();

            Dictionary<int, int> lastSpokenBeforeLast = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length-1; i++)
                    lastSpokenBeforeLast[nums[i]] = i;

            int last = nums.Last();
            int current = 0;

            for (int i = nums.Length; i < 30000000; i++)
            {
                if (lastSpokenBeforeLast.ContainsKey(last))
                     current = i - 1 - lastSpokenBeforeLast[last];
                else
                    current = 0;

                lastSpokenBeforeLast[last] = i - 1;
                last = current;

                if ( i == 2020-1)
                   Console.WriteLine("15a: " + current);
            }

            Console.WriteLine("15b: " + current);
            Console.ReadLine();
        }
    }
}