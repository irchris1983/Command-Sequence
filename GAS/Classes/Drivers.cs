using System.Threading;
using ATM;
using NationalInstruments.DAQmx;
using FGV;
using System.Collections.Generic;

namespace Drivers
{
    public class Driver1
    {
        public bool exit = false;
        List<string> AI = new List<string>();
        
        public void DAQAI()
        {
            //foreach (string s in DaqSystem.Local.Tasks)
            //{
            //    try
            //    {
            //        using (NationalInstruments.DAQmx.Task t = DaqSystem.Local.LoadTask(s))
            //        {
            //            t.Control(TaskAction.Verify);

            //            if (t.AIChannels.Count > 0)
            //            {
            //                AI.Add(s);
            //            }
            //        }
            //    }
            //    catch (DaqException)
            //    {
            //    }
            //}
            //string taskName = AI[0];
            NationalInstruments.DAQmx.Task analogReadTask = DaqSystem.Local.LoadTask("Voltage_Read_Multi");
            AnalogMultiChannelReader AI_Channel = new AnalogMultiChannelReader(analogReadTask.Stream);
            do
            {
                Global.AI = AI_Channel.ReadSingleSample();
            } while (exit == false);
        }
        public void DAQDI()
        {
            NationalInstruments.DAQmx.Task digitalReadTask = DaqSystem.Local.LoadTask("Digital_Read_Multi");
            DigitalMultiChannelReader DI_Channel = new DigitalMultiChannelReader(digitalReadTask.Stream);
            do
            {
                Global.DI = DI_Channel.ReadSingleSampleSingleLine();
            } while (exit == false);
        }
        public void DAQDO()
        {
            NationalInstruments.DAQmx.Task digitalWriteTask = DaqSystem.Local.LoadTask("Digital_Write_Single");
            DigitalSingleChannelWriter DO_Channel = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
            do
            {
                DO_Channel.WriteSingleSampleSingleLine(true, Global.DO);
            } while (exit == false);
        }
    }
}

    public class Driver2
    {
    public bool exit = false;
    
    public void DAQAI()
    {
        NationalInstruments.DAQmx.Task analogReadTask = DaqSystem.Local.LoadTask("Voltage_Read_Single");
        AnalogSingleChannelReader AI_Channel = new AnalogSingleChannelReader(analogReadTask.Stream); // If task has only 1 channel
        do
        {
            Global.AI_Single = AI_Channel.ReadSingleSample();
        } while (exit == false);
      }
    }
    

    
