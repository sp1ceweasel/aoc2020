using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve8();
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

                                if (hgt.EndsWith("cm"))
                                {
                                    hgt = hgt.Replace("cm", "");
                                    int val = Convert.ToInt32(hgt);
                                    if (val < 150 || val > 193)
                                        fail = true;
                                }
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
    }
}