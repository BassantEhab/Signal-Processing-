using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }
        public List<List<float>> intervals { get; set; }

        public override void Run()
        {
            if (InputLevel <= 0)
                InputLevel = (int)Math.Pow(2, InputNumBits);
            else
                InputNumBits = (int)Math.Log(InputLevel, 2);
            float minAmp = InputSignal.Samples[0], maxAmp = InputSignal.Samples[0];
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] < minAmp)
                    minAmp = InputSignal.Samples[i];
                if (InputSignal.Samples[i] > maxAmp)
                    maxAmp = InputSignal.Samples[i];
            }
            float delta = (maxAmp - minAmp) / InputLevel;
            intervals = new List<List<float>>();
            for (int i = 0; i < InputLevel; i++)
            {
                List<float> l = new List<float>();
                l.Add(minAmp + i * delta);
                if (i == InputLevel - 1)
                    l.Add(maxAmp);
                else
                    l.Add(minAmp + (i + 1) * delta);
                l.Add((l[0] + l[1]) / 2);
                intervals.Add(l);
            }
            OutputIntervalIndices = new List<int>();
            List<float> newSignals = new List<float>();
            bool Periodic = false;
            OutputQuantizedSignal = new Signal(newSignals, Periodic);
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < intervals.Count; j++)
                {
                    if (InputSignal.Samples[i] >= intervals[j][0] && InputSignal.Samples[i] <= intervals[j][1])
                    {
                        OutputIntervalIndices.Add(j + 1);
                        OutputQuantizedSignal.Samples.Add(intervals[j][2]);
                        break;
                    }
                }
            }
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++) 
            {
                OutputEncodedSignal.Add(Convert.ToString((OutputIntervalIndices[i] - 1), 2).PadLeft(InputNumBits,'0'));
                OutputSamplesError.Add(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);
            }
        }
    }
}
