using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> Samples = new List<float>();
            List<float> SignalFrequencies = new List<float>();
            List<float> FrequenciesAmplitudes = new List<float>();
            List<float> FrequenciesPhaseShifts = new List<float>();
            bool Periodic = false;
            OutputTimeDomainSignal = new Signal(Samples, Periodic, SignalFrequencies, FrequenciesAmplitudes, FrequenciesPhaseShifts);
            double pi = Math.PI;
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            for (int k = 0; k < N; k++)
            {
                Complex harmonic = new Complex();
                for (int n = 0; n < N; n++)
                {
                    double p = (k * 2 * pi * n) / N;
                    double realP =  Math.Cos(p);
                    double imaginaryP = Math.Sin(p);
                    harmonic += new Complex(realP, imaginaryP) * Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[n], InputFreqDomainSignal.FrequenciesPhaseShifts[n]);
                }
                harmonic /= N;
                OutputTimeDomainSignal.FrequenciesAmplitudes.Add((float)harmonic.Magnitude);
                OutputTimeDomainSignal.FrequenciesPhaseShifts.Add((float)harmonic.Phase);
                OutputTimeDomainSignal.Samples.Add((float)((harmonic.Magnitude)));
                OutputTimeDomainSignal.SamplesIndices.Add(k);
            }
        }
    }
}
