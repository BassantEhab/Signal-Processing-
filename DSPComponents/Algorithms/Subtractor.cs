using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            Signal signal1 = InputSignal1;
            Signal signal2 = InputSignal2;
            if (signal1.Samples.Count != signal2.Samples.Count)
            {
                if (signal1.Samples.Count > signal2.Samples.Count)
                {
                    int def = signal1.Samples.Count - signal2.Samples.Count;
                    for (int i = 0; i < def; i++)
                        signal2.Samples.Add(0);
                }
                else
                {
                    int def = signal2.Samples.Count - signal1.Samples.Count;
                    for (int i = 0; i < def; i++)
                        signal1.Samples.Add(0);
                }
            }

            MultiplySignalByConstant m = new MultiplySignalByConstant();
            m.InputSignal = signal2;
            m.InputConstant = -1;
            m.Run();

            Signal newSignal2 = m.OutputMultipliedSignal;

            Adder a = new Adder();
            a.InputSignals = new List<Signal>();
            a.InputSignals.Add(signal1);
            a.InputSignals.Add(newSignal2);
            a.Run();

            OutputSignal = a.OutputSignal;
        }
    }
}