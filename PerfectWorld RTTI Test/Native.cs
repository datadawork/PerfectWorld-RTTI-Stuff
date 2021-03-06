﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace PerfectWorld_RTTI_Test {
    internal static class Native {
        #region Native Imports

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle,
            int dwProcessId);

        internal static SafeProcessHandle OpenProcess(Process proc) =>
            new SafeProcessHandle(OpenProcess(ProcessAccessFlags.All, false, proc.Id), true);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress, [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress, [Out, MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [DllImport("dbghelp.dll", SetLastError = true, PreserveSig = true)]
        public static extern int UnDecorateSymbolName(
            [In] [MarshalAs(UnmanagedType.LPStr)] string DecoratedName,
            [Out] StringBuilder UnDecoratedName,
            [In] [MarshalAs(UnmanagedType.U4)] int UndecoratedLength,
            [In] [MarshalAs(UnmanagedType.U4)] UnDecorateFlags Flags);

        #endregion

        #region Enums

        [Flags]
        internal enum ProcessAccessFlags : uint {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [Flags]
        public enum UnDecorateFlags {
            UNDNAME_COMPLETE = 0x0000, // Enable full undecoration
            UNDNAME_NO_LEADING_UNDERSCORES = 0x0001, // Remove leading underscores from MS extended keywords
            UNDNAME_NO_MS_KEYWORDS = 0x0002, // Disable expansion of MS extended keywords
            UNDNAME_NO_FUNCTION_RETURNS = 0x0004, // Disable expansion of return type for primary declaration
            UNDNAME_NO_ALLOCATION_MODEL = 0x0008, // Disable expansion of the declaration model
            UNDNAME_NO_ALLOCATION_LANGUAGE = 0x0010, // Disable expansion of the declaration language specifier
            UNDNAME_NO_MS_THISTYPE = 0x0020,
            // NYI Disable expansion of MS keywords on the 'this' type for primary declaration
            UNDNAME_NO_CV_THISTYPE = 0x0040,
            // NYI Disable expansion of CV modifiers on the 'this' type for primary declaration
            UNDNAME_NO_THISTYPE = 0x0060, // Disable all modifiers on the 'this' type
            UNDNAME_NO_ACCESS_SPECIFIERS = 0x0080, // Disable expansion of access specifiers for members
            UNDNAME_NO_THROW_SIGNATURES = 0x0100,
            // Disable expansion of 'throw-signatures' for functions and pointers to functions
            UNDNAME_NO_MEMBER_TYPE = 0x0200, // Disable expansion of 'static' or 'virtual'ness of members
            UNDNAME_NO_RETURN_UDT_MODEL = 0x0400, // Disable expansion of MS model for UDT returns
            UNDNAME_32_BIT_DECODE = 0x0800, // Undecorate 32-bit decorated names
            UNDNAME_NAME_ONLY = 0x1000, // Crack only the name for primary declaration;
            // return just [scope::]name.  Does expand template params
            UNDNAME_NO_ARGUMENTS = 0x2000, // Don't undecorate arguments to function
            UNDNAME_NO_SPECIAL_SYMS = 0x4000,
            // Don't undecorate special names (v-table, vcall, vector xxx, metatype, etc)
        }

        #endregion
    }
}