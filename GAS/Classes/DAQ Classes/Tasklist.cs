using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAS.Classes.DAQ_Classes
{
    class Tasklist
    {
        List<string> tasklist = new List<string>();
        public string[] ReadDOTasks()
        {
            foreach (string s in DaqSystem.Local.Tasks)
            {
                try
                {
                    using (NationalInstruments.DAQmx.Task t = DaqSystem.Local.LoadTask(s))
                    {
                        t.Control(TaskAction.Verify);

                        if (t.DOChannels.Count > 0 && t.Timing.SampleTimingType == SampleTimingType.OnDemand)
                        {
                            tasklist.Add(s);
                        }
                    }
                }
                catch (DaqException)
                {
                }
            }
            string[] tasks = tasklist.ToArray();
            return tasks;
        }
        public string[] ReadDITasks()
        {
            List<string> tasklist = new List<string>();
            foreach (string s in DaqSystem.Local.Tasks)
            {
                try
                {
                    using (NationalInstruments.DAQmx.Task t = DaqSystem.Local.LoadTask(s))
                    {
                        t.Control(TaskAction.Verify);

                        if (t.DIChannels.Count > 0 && t.Timing.SampleTimingType == SampleTimingType.OnDemand)
                        {
                            tasklist.Add(s);
                        }
                    }
                }
                catch (DaqException)
                {
                }
            }
            string[] tasks = tasklist.ToArray();
            return tasks;
        }
        public static bool DO { get; set; }
        
    }
}
