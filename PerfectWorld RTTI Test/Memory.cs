using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using static PerfectWorld_RTTI_Test.Native;

namespace PerfectWorld_RTTI_Test {
    public class Memory : IDisposable {
        public Process Process;
        public SafeProcessHandle ProcessHandle;
        private IntPtr Handle;

        public Memory(Process proc) {
            Process = proc;
            ProcessHandle = OpenProcess(proc);
            if (Marshal.GetLastWin32Error() > 0) throw new Win32Exception(Marshal.GetLastWin32Error());
            Handle = ProcessHandle.DangerousGetHandle();
        }

        #region Read

        public byte[] ReadBytes(IntPtr dwAddress, int count) {
            var buffer = new byte[count];
            var ret = new byte[count];
            if (!ReadProcessMemory(Handle, dwAddress, buffer, count, out IntPtr bytesRead)) {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            Array.Copy(buffer, ret, bytesRead.ToInt32());
            return ret;
        }

        public T Read<T>(IntPtr dwAddress) where T : struct {
            object ret;
            var size = Marshal.SizeOf(typeof(T));
            switch (typeof(T).Name) {
                case "Int32":
                    ret = BitConverter.ToInt32(ReadBytes(dwAddress, size), 0);
                    break;
                case "UInt32":
                    ret = BitConverter.ToUInt32(ReadBytes(dwAddress, size), 0);
                    break;
                case "Int64":
                    ret = BitConverter.ToInt64(ReadBytes(dwAddress, size), 0);
                    break;
                case "UInt64":
                    ret = BitConverter.ToUInt64(ReadBytes(dwAddress, size), 0);
                    break;
                case "Int16":
                    ret = BitConverter.ToInt16(ReadBytes(dwAddress, size), 0);
                    break;
                case "UInt16":
                    ret = BitConverter.ToUInt16(ReadBytes(dwAddress, size), 0);
                    break;
                case "IntPtr":
                    ret = new IntPtr(BitConverter.ToInt32(ReadBytes(dwAddress, size), 0));
                    break;
                case "Byte":
                    ret = ReadBytes(dwAddress, 1)[0];
                    break;
                case "Boolean":
                    ret = BitConverter.ToBoolean(ReadBytes(dwAddress, 1), 0);
                    break;
                case "Char":
                    ret = BitConverter.ToChar(ReadBytes(dwAddress, size), 0);
                    break;
                default:
                    ret = default(T);
                    break;
            }
            return (T) ret;
        }

        public T[] Read<T>(IntPtr dwAddress, int count) where T : struct {
            var ret = new T[count];
            var size = Marshal.SizeOf(typeof(T));
            for (var i = 0; i < count; i++) {
                ret[i] = Read<T>(dwAddress + (i * size));
            }
            return ret;
        }

        #endregion

        #region IDisposable

        public void Dispose() {
            Process?.Dispose();
            ProcessHandle?.Dispose();
        }

        #endregion
    }
}