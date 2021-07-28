using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            if (InputSignal2 == null) //for auto correlation
                InputSignal2 = InputSignal1;

            List<float> signal1 = new List<float>();
            List<float> signal2 = new List<float>();
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
                signal1.Add(InputSignal1.Samples[i]);
            for (int i = 0; i < InputSignal2.Samples.Count; i++)
                signal2.Add(InputSignal2.Samples[i]);

            if (signal1.Count != signal2.Count) //for different sizes
            {
                int cross_length = signal1.Count + signal2.Count - 1;
                for (int i = signal1.Count; i < cross_length; i++)
                    signal1.Add(0);
                for (int i = signal2.Count; i < cross_length; i++)
                    signal2.Add(0);
            }

            double normSum = 0;
            double sum1 = 0;
            double sum2 = 0;
            for (int i = 0; i < signal1.Count; i++)
            {
                sum1 += signal1[i] * signal1[i];
                sum2 += signal2[i] * signal2[i];
            }
            normSum = Math.Sqrt(sum1 * sum2) / signal1.Count;
            List<Complex> InputSignal1Complex = new List<Complex>();
            InputSignal1Complex = DFT(signal1);
            for (int i = 0; i < InputSignal1Complex.Count; i++)
                InputSignal1Complex[i] = new Complex(InputSignal1Complex[i].Real, InputSignal1Complex[i].Imaginary * -1);

            List<Complex> InputSignal2Complex = new List<Complex>();
            InputSignal2Complex = DFT(signal2);

            List<Complex> SignalsMultiplication = new List<Complex>();

            for (int i = 0; i < InputSignal1Complex.Count; i++)
                SignalsMultiplication.Add(InputSignal1Complex[i] * InputSignal2Complex[i]);
            List<Complex> answer = new List<Complex>();
            answer = IDFT(SignalsMultiplication);

            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normSum));
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