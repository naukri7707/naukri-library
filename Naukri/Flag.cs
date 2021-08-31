using System;

namespace Naukri
{
    [Flags]
    public enum Flag : uint
    {
        None = 0U,
        Bit00 = 1U << 00,
        Bit01 = 1U << 01,
        Bit02 = 1U << 02,
        Bit03 = 1U << 03,
        Bit04 = 1U << 04,
        Bit05 = 1U << 05,
        Bit06 = 1U << 06,
        Bit07 = 1U << 07,
        Bit08 = 1U << 08,
        Bit09 = 1U << 09,
        Bit10 = 1U << 10,
        Bit11 = 1U << 11,
        Bit12 = 1U << 12,
        Bit13 = 1U << 13,
        Bit14 = 1U << 14,
        Bit15 = 1U << 15,
        Bit16 = 1U << 16,
        Bit17 = 1U << 17,
        Bit18 = 1U << 18,
        Bit19 = 1U << 19,
        Bit20 = 1U << 20,
        Bit21 = 1U << 21,
        Bit22 = 1U << 22,
        Bit23 = 1U << 23,
        Bit24 = 1U << 24,
        Bit25 = 1U << 25,
        Bit26 = 1U << 26,
        Bit27 = 1U << 27,
        Bit28 = 1U << 28,
        Bit29 = 1U << 29,
        Bit30 = 1U << 30,
        Bit31 = 1U << 31,
        Everything = uint.MaxValue
    }
}