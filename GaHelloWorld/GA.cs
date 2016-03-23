using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaHelloWorld
{
    class Chromosome
    {
        public string strValue { get; set; }
        public int fitValue { get; set; }
    }

    class GA
    {
        List<Chromosome> Parents;
        List<Chromosome> Children;

        int PopulationSize = 2048;
        double ElitePct = 0.1;
        double MutationPct = 0.2;

        Random rand = new Random();

        private string Target { get; set; }

        public GA(string target)
        {
            Target = target;
            Parents = new List<Chromosome>();
            Children = new List<Chromosome>();
        }

        public char randchr()
        {
            return (char)rand.Next(32, 127);
        }

        public string randStr(string TargetStr)
        {
            string result = string.Empty;
            for (int i = 0; i < TargetStr.Length; i++)
            {
                result += randchr();
            }

            return result;
        }

        public int CalculatFit(string strValue, string TargetStr)
        {
            int fit = 0;
            for (int i = 0; i < TargetStr.Length; i++)
            {
                fit += Math.Abs(strValue[i] - TargetStr[i]);
            }
            return fit;
        }

        public void InitPopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Chromosome chromosome = new Chromosome();
                chromosome.strValue = randStr(Target);
                chromosome.fitValue = CalculatFit(chromosome.strValue, Target);
                Parents.Add(chromosome);
            }

            Parents = Parents.OrderBy(o => o.fitValue).ToList();

            //foreach (var item in Parents)
            //{
            //    Console.WriteLine("str:{0}\tfit:{1}", item.strValue, item.fitValue);
            //}
        }

        public void Mate()
        {
            int esize = (int)(ElitePct * PopulationSize);

            for (int i = 0; i < esize; i++)
            {
                Children.Add(Parents[i]);
            }

            int next = PopulationSize - esize;
            for (int i = 0; i < next; i++)
            {
                int i1 = rand.Next(0, PopulationSize / 2);
                int i2 = rand.Next(0, PopulationSize / 2);
                int spop = rand.Next(0, Target.Length - 1);

                string str1 = Parents[i1].strValue.Substring(0, spop);
                string str2 = Parents[i2].strValue.Substring(spop, Target.Length - spop);

                Chromosome chromosome = new Chromosome();
                chromosome.strValue = str1 + str2;

                double mutation = rand.NextDouble();
                if (mutation < MutationPct)
                {
                    int mutationPosition = rand.Next(0, Target.Length);
                    string combinStr = string.Empty;
                    if (mutationPosition != Target.Length)
                    {
                        combinStr = chromosome.strValue.Substring(0, mutationPosition) + randchr() + chromosome.strValue.Substring(mutationPosition + 1, Target.Length - mutationPosition - 1); 
                    }
                    else
                    {
                        combinStr = chromosome.strValue.Substring(0, mutationPosition) + randchr();
                    }
                    chromosome.strValue = combinStr;
                }

                chromosome.fitValue = CalculatFit(chromosome.strValue, Target);
                Children.Add(chromosome);
            }

            Children = Children.OrderBy(o => o.fitValue).ToList();
        }

        public void Run()
        {
            int Count = 0;
            InitPopulation();
            int temp = 0;
            while (true)
            {
                Count++;
                Children.Clear();
                Mate();

                if (temp != Children[0].fitValue)
                {
                    Console.WriteLine("[{0}] str:{1}\tfit:{2}", Count, Children[0].strValue, Children[0].fitValue);
                }
                //Console.WriteLine("[{0}] str:{1}\tfit:{2}", Count, Children[0].strValue, Children[0].fitValue);
                if (Children[0].fitValue == 0)
                {
                    break;
                }

                Parents.Clear();
                foreach (Chromosome item in Children)
                {
                    Parents.Add(item);
                }
                temp = Children[0].fitValue;
            }
        }
    }
}
