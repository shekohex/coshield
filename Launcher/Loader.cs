using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Timers;

namespace Launcher
{
    public partial class Loader : Form
    {
        public static string DllName;

        public Loader()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
            DllName = "ShieldHook.dll";
            this.Text = "Conquer Loader.";
        }
           

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            PROCESS_INFORMATION lpProcessInformation = new PROCESS_INFORMATION();
            SECURITY_ATTRIBUTES structure = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES security_attributes2 = new SECURITY_ATTRIBUTES();
            STARTUPINFO lpStartupInfo = new STARTUPINFO();
            structure.nLength = Marshal.SizeOf(structure);
            security_attributes2.nLength = Marshal.SizeOf(security_attributes2);
            if (!CreateProcess(Application.StartupPath + @"\Conquer.exe", " blacknull", ref structure, ref security_attributes2, false, 0x4000000, IntPtr.Zero, null, ref lpStartupInfo, out lpProcessInformation))
            {
                MessageBox.Show("We Cannot Find Conquer.exe ! .. please put the loader in Conuqer Folder :) .. \n  Ty For ur Time ;)", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
                Environment.Exit(0);
            }
            else
            {               
                worker.ReportProgress(10);
                Thread.Sleep(1000);               
                InjectDLL(lpProcessInformation.hProcess, DllName, worker);                           
            }
        }
        
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(3000);
            Application.Exit();
        }
        #region Injection
        public static bool InjectDLL(IntPtr hProcess, string strDLLName, BackgroundWorker worker)
        {
            IntPtr ptr;
            string str;
            int num = strDLLName.Length + 1;
            IntPtr lpBaseAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)num, 0x1000, 0x40);
            worker.ReportProgress(20);
            if ((lpBaseAddress == IntPtr.Zero) && (lpBaseAddress == IntPtr.Zero))
            {
                str = "Unable to allocate memory to target process.\n";
                str = str + "Error code: " + Marshal.GetLastWin32Error();
                return false;
            }
            WriteProcessMemory(hProcess, lpBaseAddress, strDLLName, (UIntPtr)num, out ptr);
            worker.ReportProgress(50);
            if (Marshal.GetLastWin32Error() != 0x514)
            {
                str = "Please run it as an administrator";
                str = str + "Error code: " + Marshal.GetLastWin32Error();
            }
            else if (Marshal.GetLastWin32Error() != 0)
            {
                str = "Unable to write memory to process.";
                str = str + "Error code: " + Marshal.GetLastWin32Error();
                return false;
            }
            UIntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            worker.ReportProgress(60);
            if (procAddress == ((UIntPtr)0))
            {
                str = "Unable to find address of \"LoadLibraryA\".\n";
                MessageBox.Show(str + "Error code: " + Marshal.GetLastWin32Error());
                return false;
            }
            IntPtr handle = CreateRemoteThread(hProcess, IntPtr.Zero, 0, procAddress, lpBaseAddress, 0, out ptr);
            worker.ReportProgress(80);
            if (handle == IntPtr.Zero)
            {
                str = "Unable to load dll into memory.";
                str = str + "Error code: " + Marshal.GetLastWin32Error();
                return false;
            }
            long num2 = WaitForSingleObject(handle, 0x2710);
            worker.ReportProgress(90);
            switch (num2)
            {
                case 0x80L:
                case 0x102L:
                case 0xffffffffL:
                    CloseHandle(handle);
                    return false;
            }
            Thread.Sleep(0x3e8);
            VirtualFreeEx(hProcess, lpBaseAddress, (UIntPtr)0, 0x8000);
            worker.ReportProgress(100);
            CloseHandle(handle);
            return true;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public int bInheritHandle;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes, ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);
        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern int WaitForSingleObject(IntPtr handle, int milliseconds);
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, string lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);

        #endregion
    }
}
