using System;

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
    }
}