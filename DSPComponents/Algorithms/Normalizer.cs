using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            int maxSize = 0;
            float min = InputSignal.Samples[0], max = InputSignal.Samples[0];
            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                if (min > InputSignal.Samples[i])
                    min = InputSignal.Samples[i];
                if (max < InputSignal.Samples[i])
                    max = InputSignal.Samples[i];
            }
            List<float> outputSignalSamples = new List<float>();
            bool Periodic = false;
            OutputNormalizedSignal = new Signal(outputSignalSamples, Periodic);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                OutputNormalizedSignal.Samples.Add((InputMaxRange - InputMinRange) / (max - min)*(InputSignal.Samples[i]-max)+InputMaxRange);
            }
        }
    }
}
