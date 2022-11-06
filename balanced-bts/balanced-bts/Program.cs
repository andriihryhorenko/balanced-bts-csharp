using balanced_bts.BTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace balanced_bts
{
    internal class Program
    {

       

        static void Main(string[] args)
        {

            Thread.Sleep(3);
          
            //default
            var numberOfDatasets = 100;
            var step = 1000;
            var datasetL = 100;

            var arguments = new Dictionary<string, string>();

            foreach (string argument in args)
            {
                string[] splitted = argument.Split('=');

                if (splitted.Length == 2)
                {
                    arguments[splitted[0]] = splitted[1];
                }
            }

            var value = Environment.GetEnvironmentVariable("DATASETSN");
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out numberOfDatasets);
            }

            value = Environment.GetEnvironmentVariable("STEP");
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out step);
            }

            value = Environment.GetEnvironmentVariable("FIRSTLEN");
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out datasetL);
            }

            if (arguments.Any())
            {
                if (arguments.ContainsKey("-n"))
                {
                    int.TryParse(arguments["-n"], out numberOfDatasets);
                }

                if (arguments.ContainsKey("-s"))
                {
                    int.TryParse(arguments["-s"], out step);
                }

                if (arguments.ContainsKey("-l"))
                {
                    int.TryParse(arguments["-l"], out datasetL);
                }
            }


            for (var i=0;i<numberOfDatasets;i++)
            {
                var timeResult = GenerateBalancedTree(datasetL, false);
                datasetL = datasetL +  step;

                timeResult.ToPrint();
            }
            

            
        }


        static TimeResult GenerateBalancedTree(int datasetL=100, bool printTree = false)
        {
            var result = new TimeResult(datasetL);
            var sw1 = new Stopwatch();
            var dataset = GenerateDataset(datasetL);

            var bTree1 = new BinaryTree();

            sw1.Start();
            foreach (var item in dataset)
            {
                bTree1.Add(item);
                if (printTree)
                {
                    Console.Write(item.ToString() + ",");
                }
            }

            if (printTree)
            {
                Console.WriteLine();
                Console.WriteLine("TWay building");
            }
            var inOrderTravert = new List<Node>();
            bTree1.TraverseInOrder(bTree1.Root, ref inOrderTravert);


            var balanced1 = new BinaryTree();
            balanced1.BuildBalancedTree(inOrderTravert.Select(x=>x.Data).ToList());
            sw1.Stop();
            if (printTree)
                balanced1.Print();

            result.TreeWay = sw1.Elapsed;


            if (printTree)
            {
                Console.WriteLine();
                Console.WriteLine("CountingSort building");
            }

            sw1.Start();
            var bTreeB1 = new BinaryTree();
            bTreeB1.BuildBalancedTree(CountingSort(dataset).ToList());
            sw1.Stop();

            result.CountingSort = sw1.Elapsed;

            if (printTree)
                bTreeB1.Print();

            if (printTree)
            {
                Console.WriteLine();
                Console.WriteLine("CommonSort building");
            }

            sw1.Start();
            var bTreeB2 = new BinaryTree();
            bTreeB2.BuildBalancedTree(dataset.OrderBy(x=>x).ToList());
            sw1.Stop();

            result.CommonSort = sw1.Elapsed;

            if (printTree)
                bTreeB2.Print();

            return result;

        }

        static int[] GenerateDataset(int length)
        {
            Random randomGenerator = new Random();

            int[] result = new int[length];

            for (var i= 0; i < length; i++){
                result[i] = randomGenerator.Next(1, length);
            }

            return result;
        }


        public static int[] CountingSort(int[] array)
        {
            var size = array.Length;
            var maxElement = GetMaxVal(array, size);
            var occurrences = new int[maxElement + 1];
            for (int i = 0; i < maxElement + 1; i++)
            {
                occurrences[i] = 0;
            }
            for (int i = 0; i < size; i++)
            {
                occurrences[array[i]]++;
            }
            for (int i = 0, j = 0; i <= maxElement; i++)
            {
                while (occurrences[i] > 0)
                {
                    array[j] = i;
                    j++;
                    occurrences[i]--;
                }
            }
            return array;
        }

        public static int GetMaxVal(int[] array, int size)
        {
            var maxVal = array[0];
            for (int i = 1; i < size; i++)
                if (array[i] > maxVal)
                    maxVal = array[i];
            return maxVal;
        }


        public class TimeResult
        {
            public int Length;
            public TimeSpan TreeWay { get; set; }
            public TimeSpan CountingSort { get; set; }

            public TimeSpan CommonSort { get; set; }

            public TimeResult(int length)
            {
                Length = length;
            }

            public void ToPrint()
            {
                Console.WriteLine($"[L:{this.Length}] TreeWay:{this.TreeWay}, CountingSort:{this.CountingSort}, CommonSort:{this.CommonSort}");
            }

        }
    }
}
