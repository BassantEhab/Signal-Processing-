using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            bool auto = false;
            if (InputSignal2 == null) //for auto correlation
            {
                InputSignal2 = InputSignal1;
                auto = true;
            }

            List<double> signal1 = new List<double>();
            List<double> signal2 = new List<double>();
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
                signal1.Add(InputSignal1.Samples[i]);
            for (int i = 0; i < InputSignal2.Samples.Count; i++)
                signal2.Add(InputSignal2.Samples[i]);

            if (signal1.Count != signal2.Count) //for different sizes in cross
            {
                int lengthC = signal1.Count + signal2.Count - 1;
                for (int i = signal1.Count; i < lengthC; i++)
                    signal1.Add(0);
                for (int i = signal2.Count; i < lengthC; i++)
                    signal2.Add(0);
            }

            double firstEle;
            for (int i = 0; i < signal2.Count; i++)
            {
                double sum = 0;
                if (i != 0)  
                {
                    if (InputSignal1.Periodic)
                        firstEle = signal2[0];
                    else
                        firstEle = 0;
                    for (int j = 0; j < signal2.Count - 1; j++)
                    {
                        signal2[j] = signal2[j + 1];
                        sum += signal2[j] * signal1[j];
                    }
                    signal2[signal2.Count - 1] = firstEle;
                    sum += signal2[signal2.Count - 1] * signal1[signal1.Count - 1];
                }
                else
                    for (int j = 0; j < signal2.Count; j++)
                        sum += signal1[j] * signal2[j];
                OutputNonNormalizedCorrelation.Add((float)sum / signal2.Count);
            }

            double normSum = 0; double sum1 = 0; double sum2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
                sum1 += Math.Pow(InputSignal1.Samples[i], 2);
            for (int i = 0; i < InputSignal2.Samples.Count; i++)
                sum2 += Math.Pow(InputSignal2.Samples[i], 2);
            if (auto)
                normSum = Math.Sqrt(sum1 * sum2) / (InputSignal1.Samples.Count);
            else
                normSum = Math.Sqrt(sum1 * sum2) / (InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1);
            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normSum));
        }
    }
}