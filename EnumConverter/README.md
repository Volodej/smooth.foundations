## Description
Utility lib for boxing-free conversion of enums to primitive types.

All conversion methods are not using overflow checks.

Support conversion of enums to this types:
- System.Byte
- System.SByte
- System.Int16
- System.UInt16
- System.Int32
- System.UInt32
- System.Int64
- System.UInt64 

## Building
To build this lib run `ilasm EnumConverter.il /dll`