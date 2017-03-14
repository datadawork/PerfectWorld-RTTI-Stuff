using System;
using System.IO;
using System.Text;

namespace PerfectWorld_RTTI_Test {
    public static class RttiInfo {
        public class RttiCompleteObjectLocator {
            public uint Signature;
            public uint VFTableOffset;
            public uint cdOffset;
            public TypeDescriptor pTypeDescriptor;
            //public RttiClassHierarchyDescriptor pClassHierarchyDescriptor;
        }

        public class TypeDescriptor {
            public IntPtr pVFTable;
            public string Name;
        }

        public class RttiClassHierarchyDescriptor {
            public uint Signature;
            public uint Attributes;
            public uint numBaseClasses;
            public RttiCompleteObjectLocator[] pBaseClassArray;
        }

        public static RttiCompleteObjectLocator GetCompleteObjectLocator(IntPtr dwBaseAddress) {
            var ret = new RttiCompleteObjectLocator();
            var objBase = Core.Memory.Read<IntPtr>(dwBaseAddress);
            var locatorAddr = Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(objBase) - 0x4);
            var typePtr = Core.Memory.Read<IntPtr>(locatorAddr + 0xC);
            ret.pTypeDescriptor = new TypeDescriptor {
                Name = Core.Memory.ReadString(typePtr + 0x8, Encoding.ASCII)
            };
            return string.IsNullOrWhiteSpace(ret.pTypeDescriptor.Name) ? null : ret;
        }
    }
}