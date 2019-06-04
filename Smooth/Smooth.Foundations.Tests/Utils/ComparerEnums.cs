namespace Smooth.Tests.Utils
{
    public enum ByteEnum : byte
    {
        Value1 = 1,
        Value2 = 2,
        Value3 = 3
    }

    public enum ShortEnum : short
    {
        Value1 = 0x0100,
        Value2 = 0x0200,
        Value3 = 0x0300
    }

    public enum IntEnum
    {
        Value1 = 0x010000,
        Value2 = 0x020000,
        Value3 = 0x030000
    }

    public enum LongEnum : long
    {
        Value1 = 0x0100000000,
        Value2 = 0x0200000000,
        Value3 = 0x0300000000
    }
}