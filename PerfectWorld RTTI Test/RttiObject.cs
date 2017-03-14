using System;
using System.Linq;
using System.Text;

namespace PerfectWorld_RTTI_Test {
    public class RttiObject {
        public IntPtr BaseAddress;
        public RttiCompleteObjectLocator ObjectLocator;

        public string RawName => ObjectLocator.Type.Name;
        public string DemangledName => Helper.DemangleName(RawName);
        public string Name => DemangledName.Replace("class ", "");

        public BaseClass[] BaseClassArray => ObjectLocator.ClassHierarchy.BaseClassArray;

        public RttiObject(IntPtr dwAddress, bool isPointer = true) {
            if (isPointer) dwAddress = Core.Memory.Read<IntPtr>(dwAddress);
            var vtable = Core.Memory.Read<IntPtr>(dwAddress);
            var locator = Core.Memory.Read<IntPtr>(vtable - 0x4);
            BaseAddress = dwAddress;
            ObjectLocator = new RttiCompleteObjectLocator(locator);
        }

        public bool isValid() {
            var name = ObjectLocator.Type.Name;
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3) return false;
            var num = ObjectLocator.ClassHierarchy.numBaseClasses;
            if (num < 1 || num > 0x1F) return false;
            return true;
        }

        #region RttiClasses

        public class RttiCompleteObjectLocator {
            public uint Signature;
            public uint VFTableOffset;
            public uint cdOffset;
            public TypeDescriptor Type;
            public RttiClassHierarchyDescriptor ClassHierarchy;

            public RttiCompleteObjectLocator(IntPtr dwAddress) {
                var bStruct = Core.Memory.Read<byte>(dwAddress, 0x14);
                Signature = BitConverter.ToUInt32(bStruct, 0);
                VFTableOffset = BitConverter.ToUInt32(bStruct, 0x4);
                cdOffset = BitConverter.ToUInt32(bStruct, 0x8);
                Type = new TypeDescriptor((IntPtr)BitConverter.ToInt32(bStruct, 0xC));
                ClassHierarchy = new RttiClassHierarchyDescriptor((IntPtr)BitConverter.ToInt32(bStruct, 0x10));
            }
        }

        public class TypeDescriptor {
            public IntPtr pVFTable;
            public string Name;

            public TypeDescriptor(IntPtr dwAddress) {
                pVFTable = Core.Memory.Read<IntPtr>(dwAddress);
                Name = Core.Memory.ReadString(dwAddress + 0x8, Encoding.ASCII);
            }
        }

        public class RttiClassHierarchyDescriptor {
            public uint Signature;
            public uint Attributes;
            public uint numBaseClasses;
            public BaseClass[] BaseClassArray;

            public RttiClassHierarchyDescriptor(IntPtr dwAddress) {
                var bStruct = Core.Memory.Read<byte>(dwAddress, 0x10);
                Signature = BitConverter.ToUInt32(bStruct, 0);
                Attributes = BitConverter.ToUInt32(bStruct, 0x4);
                numBaseClasses = BitConverter.ToUInt32(bStruct, 0x8);
                var baseArray = (IntPtr)BitConverter.ToInt32(bStruct, 0xC);
                if (numBaseClasses > 1 && numBaseClasses < 0x1F) {
                    BaseClassArray = new BaseClass[numBaseClasses-1];
                    for (var i = 1; i < numBaseClasses; i++) {
                        var addr = Core.Memory.Read<IntPtr>(baseArray + (i * 0x4));
                        BaseClassArray[i-1] = new BaseClass(addr);
                    }
                    BaseClassArray = BaseClassArray.Where(bc => 
                        !string.IsNullOrWhiteSpace(bc.Type.Name) && bc.Type.Name.Length >= 3
                    ).ToArray();
                } else BaseClassArray = new BaseClass[0];
            }
        }

        public class RttiBaseClassDescriptor {
            public uint numContainedClasses;
            public TypeDescriptor Type;

            public RttiBaseClassDescriptor(IntPtr dwAddress) {
                Type = new TypeDescriptor(Core.Memory.Read<IntPtr>(dwAddress));
                numContainedClasses = Core.Memory.Read<uint>(dwAddress + 0x4);
            }
        }

        public class BaseClass {
            public uint numContainedClasses;
            public TypeDescriptor Type;

            public string RawName => Type.Name;
            public string DemangledName => Helper.DemangleName(RawName);
            public string Name => DemangledName.Replace("class ", "");

            public BaseClass(IntPtr dwAddress) {
                Type = new TypeDescriptor(Core.Memory.Read<IntPtr>(dwAddress));
                numContainedClasses = Core.Memory.Read<uint>(dwAddress + 0x4);
            }
        }

        #endregion
    }
}