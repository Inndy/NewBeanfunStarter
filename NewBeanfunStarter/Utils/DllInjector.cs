using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NewBeanfunStarter
{
    public class DllInjector
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, ref IntPtr lpNumberOfBytesWritten);

        public enum AllocationType
        {
            MEM_COMMIT = 0x1000,
            MEM_RESERVE = 0x2000,
            MEM_RESET = 0x80000
        }

        public enum ProtectionConstants
        {
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_NOACCESS = 1
        }

        public static bool InjectDLL(Process process, string dll)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(dll);

            if (process.HasExited)
                return false;

            IntPtr hProcess = process.Handle;
            if (hProcess == IntPtr.Zero)
                return false;
            IntPtr hModule = GetModuleHandle("kernel32.dll");
            IntPtr procAddress = GetProcAddress(hModule, "LoadLibraryW");
            int LoadLibraryW_Offset = (int)procAddress - (int)hModule;
            hModule = IntPtr.Zero;
            
            foreach (ProcessModule mod in process.Modules) {
                if (mod.ModuleName.ToLower() == "kernel32.dll" || mod.ModuleName.ToLower() == "kernel32")
                {
                    hModule = mod.BaseAddress;
                    break;
                }
            }
            procAddress = IntPtr.Add(hModule, LoadLibraryW_Offset);
            IntPtr lpBaseAddress = VirtualAllocEx(hProcess, IntPtr.Zero, buffer.Length + 1, 0x1000, 0x40);
            if (lpBaseAddress == IntPtr.Zero)
                return false;
            IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
            WriteProcessMemory(hProcess, lpBaseAddress, buffer, (uint)buffer.Length, ref lpNumberOfBytesWritten);
            if (Marshal.GetLastWin32Error() != 0)
                return false;
            if (CreateRemoteThread(hProcess, IntPtr.Zero, IntPtr.Zero, procAddress, lpBaseAddress, 0, IntPtr.Zero) == IntPtr.Zero)
                return false;
            return true;
        }
    }
}
