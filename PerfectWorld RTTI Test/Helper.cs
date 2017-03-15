using System;
using System.Runtime.InteropServices;
using System.Text;
using static PerfectWorld_RTTI_Test.Native;

namespace PerfectWorld_RTTI_Test {
    public static class Helper {
        public static string ByteArrayToString(byte[] ba) {
            var hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static byte[] StringToByteArray(string hex) {
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars / 2];
            for (var i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static int ReverseBytes(int val) {
            var bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static bool isValidClassName(string name) {
            return !string.IsNullOrWhiteSpace(name) && name.Length >= 3;
        }

        public static string DemangleName(string name) {
            if (!name.StartsWith(".", StringComparison.Ordinal)) return "";
            name = "??_R0" + name.Substring(1, name.Length-1) + "@8";
            var builder = new StringBuilder(255);
            UnDecorateSymbolName(name, builder, builder.Capacity, UnDecorateFlags.UNDNAME_COMPLETE);
            var outname = builder.ToString();
            var i = outname.IndexOf("`RTTI Type Descriptor'", StringComparison.Ordinal);
            if (i == -1) return "";
            outname = outname.Substring(0, i-1);
            return outname;
        }
    }
}