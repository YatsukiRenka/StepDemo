using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace StepDemo
{
    public class Program
    {
        private static void Main()
        {
            Program program = new Program();
            Dictionary<int, bool> slotDict = new Dictionary<int, bool>();
            List<double> slotList = new List<double>();
            Random random = new Random();
            for (int i = 1; i <= 5; i++)
            {
                int slotValid = random.Next(100);
                int slotValue = random.Next(10, 20);
                slotList.Add(slotValue);
                if (slotValid <= 50)
                {
                    slotDict.Add(i, false);
                }
                else
                {
                    slotDict.Add(i, true);
                }
            }
            program.StepToZero(slotDict, slotList);
            Console.ReadLine();
        }

        public void StepToZero(Dictionary<int, bool> slotDict, List<double> slotList)
        {
            Console.WriteLine("请输入步长x(0.1 <= x <= 10)：");
            string stepString = Console.ReadLine();
            double x = double.Parse(stepString);
            if (x < 0.1)
            {
                x = 0.1;
                Console.WriteLine("步长小于0.1，设置为0.1");
            }
            if (x > 10)
            {
                x = 10;
                Console.WriteLine("步长大于10，设置为10");
            }

            Console.WriteLine("请输入单步延时t(s)(t >= 0.1)：");
            string stepDelay = Console.ReadLine();
            double t = double.Parse(stepDelay);
            if (t < 0.1)
            {
                t = 0.1;
                Console.WriteLine("单步延时小于0.1s，设置为0.1s");
            }

            //slotVals<slot, currentVal>
            List<int> validSlot = new List<int>();
            Dictionary<int, double> slotVals = new Dictionary<int, double>();
            for (int slot = 1; slot <= slotList.Count; slot++)
            {
                if (slotDict[slot] == false)
                {
                    Console.WriteLine($"Slot[{slot}] 状态为[Invalid]");
                    continue;
                }
                validSlot.Add(slot);
                slotVals.Add(slot, slotList[slot - 1]);
                Console.WriteLine($"Slot[{slot}] 状态为[Valid]");
            }

            //slotEndValArray<slot, slotEndValArray>
            Dictionary<int, double[]> slotEndValArray = new Dictionary<int, double[]>();
            foreach (var slot in slotVals.Keys)
            {
                slotEndValArray.Add(slot, Tools.CalculateArray(slotVals[slot], 0.0, -x));

                var endValArray = slotEndValArray[slot];
                string eValTips = string.Empty;
                foreach (var eVal in endValArray)
                {
                    eValTips += Math.Round(eVal, 2) + "、";
                }
                eValTips = eValTips.TrimEnd('、');

                Console.WriteLine($"Slot[{slot}] 下降阶梯:[{eValTips}]");
            }

            //maxLength
            int maxLength = 0;
            foreach (var slot in slotEndValArray.Keys)
            {
                if (slotEndValArray[slot].Length > maxLength)
                {
                    maxLength = slotEndValArray[slot].Length;
                }
            }

            Console.WriteLine("阶梯过程开始");
            Console.WriteLine();

            Stopwatch sw = Stopwatch.StartNew();

            //动态下阶梯
            for (int j = 0; j < maxLength; j++)
            {
                for (int i = 0; i < validSlot.Count; i++)
                {
                    if (slotEndValArray[validSlot[i]].Length >= (maxLength - j))
                    {
                        double stepValue = 0;
                        if (slotEndValArray[validSlot[i]].Length == maxLength)
                        {
                            stepValue = slotEndValArray[validSlot[i]][j];
                        }
                        else
                        {
                            stepValue = slotEndValArray[validSlot[i]][j - (maxLength - slotEndValArray[validSlot[i]].Length)];
                        }
                        Console.WriteLine($"Slot[{validSlot[i]}] 当前阶梯值:[{stepValue}] 已运行时间:[{sw.ElapsedMilliseconds}]ms");
                    }
                }
                Thread.Sleep(Convert.ToInt16(t * 1000));
            }

            Console.WriteLine();
            if (sw.ElapsedMilliseconds * 1000 < 60)
            {
                double time = sw.ElapsedMilliseconds / 1000;
                Console.WriteLine($"阶梯过程结束 用时[{time}]s");
            }
            else
            {
                Console.WriteLine($"阶梯过程结束 用时[{sw.ElapsedMilliseconds}]ms");
            }

            sw.Stop();
            sw.Reset();
        }
    }
}
