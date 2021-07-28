using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> Samples = new List<float>();
            List<float> SignalFrequencies = new List<float>();
            List<float> FrequenciesAmplitudes = new List<float>();
            List<float> FrequenciesPhaseShifts = new List<float>();
            bool Periodic = false;
            OutputConvolvedSignal = new Signal(Samples, Periodic, SignalFrequencies, FrequenciesAmplitudes, FrequenciesPhaseShifts);
            int maxLen = Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count);


            int minIndx = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];

            for (int i = InputSignal1.Samples.Count; i < maxLen; i++)
                InputSignal1.Samples.Add(0);
            for (int i = InputSignal2.Samples.Count; i < maxLen; i++)
                InputSignal2.Samples.Add(0);

            List<Complex> InputSignal1Complex = new List<Complex>();
            InputSignal1Complex = DFT(InputSignal1.Samples);
            List<Complex> InputSignal2Complex = new List<Complex>();
            InputSignal2Complex = DFT(InputSignal2.Samples);
            List<Complex> SignalsMultiplication = new List<Complex>();

            for (int i = 0; i < maxLen; i++)
                SignalsMultiplication.Add(InputSignal1Complex[i] * InputSignal2Complex[i]);

            List<Complex> answer = new List<Complex>();
            answer = IDFT(SignalsMultiplication);

            for (int i = 0; i < maxLen; i++)
            {
                OutputConvolvedSignal.Samples.Add((float)answer[i].Real / maxLen);
                OutputConvolvedSignal.SamplesIndices.Add(minIndx++);
            }

        }

        private List<Complex> DFT(List<float> har)
        {
            List<Complex> harmonics = new List<Complex>();

            double pi = Math.PI;
            int N = har.Count;
            for (int k = 0; k < N; k++)
            {
                Complex harmonic = new Complex();
                for (int n = 0; n < N; n++)
                {
                    double p = (-k * 2 * pi * n) / N;
                    double realP = har[n] * Math.Cos(p);
                    double imaginaryP = har[n] * Math.Sin(p);
                    harmonic += new Complex(realP, imaginaryP);
                }
                harmonics.Add(new Complex(harmonic.Real, harmonic.Imaginary));
            }
            return harmonics;
        }

        private List<Complex> IDFT(List<Complex> har)
        {
            double pi = Math.PI;
            int N = har.Count;
            List<Complex> harmonics = new List<Complex>();
            for (int k = 0; k < N; k++)
            {
                Complex harmonic = new Complex();
                for (int n = 0; n < N; n++)
                {
                    double p = (k * 2 * pi * n) / N;
                    double realP = Math.Cos(p);
                    double imaginaryP = Math.Sin(p);
                    harmonic += new Complex(realP, imaginaryP) * Complex.FromPolarCoordinates(har[n].Real, har[n].Imaginary);
                }
                harmonics.Add(harmonic);
            }
            return harmonics;
        }
    }
        

        
    
}
