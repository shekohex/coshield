using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
namespace Shield
{
    public class Main
    {
        private string baseURL = "https://my-back-end-demo.000webhostapp.com/hacks.php?gethacks=true";
        public List<string> runningProcesses = new List<string>();
        private List<string> data = new List<string>();
        private List<string> hacks = new List<string>();
        private string coMainPath = Directory.GetParent(Environment.CurrentDirectory).ToString();
        private Process[] AllRunningProcesses()
        {
            Process[] processlist = Process.GetProcesses();
            return processlist;
        }
        private List<string> Init()
        {
            var api = new ApiParser();
             data = api.getHacks(baseURL);

            foreach (Process thisprocess in AllRunningProcesses())
            {
                runningProcesses.Add(thisprocess.ProcessName);
            }
            foreach (var hack in data)
            {
                if (runningProcesses.Contains(hack)) hacks.Add(hack);
            }
            return hacks;
        }
        public void KillHack(string hackName)
        {
            foreach (var process in Process.GetProcessesByName(hackName))
            {
                process.Kill();
            }
        }
        public void Run()
        {
            foreach (var hack in this.Init())
            {
                this.KillHack(hack);
            }

        }
    }
}
