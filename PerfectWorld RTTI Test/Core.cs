using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace PerfectWorld_RTTI_Test {
    public static class Core {

        public static bool isLoaded;
        public static Memory Memory;

        private static IntPtr BaseAddress;

        public static bool Load() {
            var proc = Process.GetProcessesByName("elementclient");
            if (proc.Length <= 0) return false;

            Memory = new Memory(proc[0]);
            isLoaded = true;
            return true;
        }

        public static void Unload() {
            Memory?.Dispose();
            isLoaded = false;
        }

        public static IntPtr GetBaseAddress(bool log = false) {
            if (BaseAddress != IntPtr.Zero) return BaseAddress;

            var data = File.ReadAllBytes(Memory.Process.MainModule.FileName);
            var bstr = Helper.ByteArrayToString(data);

            if (log) Logging.Log("Searching for Base Address...");
            var match = Regex.Match(bstr, "A1(.{8})5332DB8B48");
            if (match.Success) {
                var valInt = BitConverter.ToInt32(Helper.StringToByteArray(match.Groups[1].Value), 0);
                if (log) Logging.Log($"Found: {valInt:X8}\n{new string('-', 50)}");
                BaseAddress = new IntPtr(valInt);
                return BaseAddress;
            }
            if (log) Logging.Log($"Nothing found\n{new string('-', 50)}");
            return IntPtr.Zero;
        }
    }
}