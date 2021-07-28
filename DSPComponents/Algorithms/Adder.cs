using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int maxSize = 0;
            for(int i=0;i<InputSignals.Count;i++)
            {
                if (InputSignals[i].Samples.Count > maxSize)
                    maxSize = InputSignals[i].Samples.Count;
            }
            for (int i = 0; i < InputSignals.Count; i++)
            {
                int def = maxSize - InputSignals[i].Samples.Count;
                for (int j = 0; j < def; j++)
                    InputSignals[j].Samples.Add(0);
            }
            List<float> outputSignalSamples = new List<float>();
            bool Periodic = false;
            OutputSignal = new Signal(outputSignalSamples, Periodic);
            for (int i=0; i<maxSize;i++)
            {
                float sum = 0;
                for (int j = 0; j < InputSignals.Count; j++)
                {
                    sum += InputSignals[j].Samples[i];
                }
                OutputSignal.Samples.Add(sum);
            }
        }
    }
}