using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> Samples = new List<float>();
            List<float> SignalFrequencies = new List<float>();
            List<float> FrequenciesAmplitudes = new List<float>();
            List<float> FrequenciesPhaseShifts = new List<float>();
            bool Periodic = false;
            OutputFreqDomainSignal = new Signal(Samples, Periodic, SignalFrequencies, FrequenciesAmplitudes, FrequenciesPhaseShifts);
            double pi = Math.PI;
            int N = InputTimeDomainSignal.Samples.Count;
            StreamWriter sw = new StreamWriter("E:\\Test.txt");
            sw.WriteLine("1");
            sw.WriteLine("0");
            sw.WriteLine(N);
            for (int k = 0; k < N; k++)
            {
                Complex harmonic = new Complex();
                for (int n = 0; n < N; n++)
                {
                    double p = (-k * 2 * pi * n) / N;
                    double realP = InputTimeDomainSignal.Samples[n] * Math.Cos(p);
                    double imaginaryP = InputTimeDomainSignal.Samples[n] * Math.Sin(p);
                    harmonic += new Complex(realP, imaginaryP);
                }
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)harmonic.Magnitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)harmonic.Phase);
                OutputFreqDomainSignal.Frequencies.Add(k);
                sw.WriteLine( k + " " + harmonic.Magnitude + " " + harmonic.Phase);
            }
            sw.Close();
        }
    }
}
 