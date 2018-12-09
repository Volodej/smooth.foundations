.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )    
  .ver 4:0:0:0
}
.assembly 'EnumConverter'
{
  .hash algorithm 0x00008004
  .ver  1:0:0:0
}

.class public auto ansi beforefieldinit Smooth.Conversion.EnumConverter
    extends [mscorlib]System.Object
{
    /// <summary>
    /// Test
    /// </summary>
    .method public hidebysig static int32  ToInt32Unsafe<TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i4
      IL_0002:  ret
    }

    .method public hidebysig static int64  ToInt64Unsafe<TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i8
      IL_0002:  ret
    }

    .method public hidebysig static int8  ToInt8<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i
      IL_0002:  ret
    }

    .method public hidebysig static int16  ToInt16<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i2
      IL_0002:  ret
    }

    .method public hidebysig static int32  ToInt32<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i4
      IL_0002:  ret
    }

    .method public hidebysig static int64  ToInt64<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i8
      IL_0002:  ret
    }

    .method public hidebysig static uint8  ToUInt8<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u
      IL_0002:  ret
    }

    .method public hidebysig static uint16  ToUInt16<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u2
      IL_0002:  ret
    }

    .method public hidebysig static uint32  ToUInt32<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u4
      IL_0002:  ret
    }

    .method public hidebysig static uint64  ToUInt64<valuetype 
        .ctor ([mscorlib]System.ValueType, [mscorlib]System.IConvertible) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u8
      IL_0002:  ret
    }
} 