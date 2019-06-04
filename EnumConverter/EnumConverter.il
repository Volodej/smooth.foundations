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

.class public abstract auto ansi sealed beforefieldinit Smooth.Conversion.EnumConverter
    extends [mscorlib]System.Object
{
    .method public hidebysig static int32  ToInt32RuntimeCheck<TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0001:  ldtoken    !!TEnum
      IL_0002:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0003:  callvirt   instance bool [mscorlib]System.Type::get_IsEnum()
      IL_0004:  brtrue.s   IL_0021

      IL_0011:  ldstr      "'{0}' is not an enum."
      IL_0012:  ldtoken    !!TEnum
      IL_0013:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0014:  call       string [mscorlib]System.String::Format(string, object)
      IL_0015:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
      IL_0016:  throw

      IL_0021:  ldarg.0
      IL_0022:  conv.i4
      IL_0023:  ret
    }

    .method public hidebysig static int64  ToInt64RuntimeCheck<TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0001:  ldtoken    !!TEnum
      IL_0002:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0003:  callvirt   instance bool [mscorlib]System.Type::get_IsEnum()
      IL_0004:  brtrue.s   IL_0021

      IL_0011:  ldstr      "'{0}' is not an enum."
      IL_0012:  ldtoken    !!TEnum
      IL_0013:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0014:  call       string [mscorlib]System.String::Format(string, object)
      IL_0015:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
      IL_0016:  throw

      IL_0021:  ldarg.0
      IL_0022:  conv.i8
      IL_0023:  ret
    }

    .method public hidebysig static uint32  ToUInt32RuntimeCheck<TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0001:  ldtoken    !!TEnum
      IL_0002:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0003:  callvirt   instance bool [mscorlib]System.Type::get_IsEnum()
      IL_0004:  brtrue.s   IL_0021

      IL_0011:  ldstr      "'{0}' is not an enum."
      IL_0012:  ldtoken    !!TEnum
      IL_0013:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0014:  call       string [mscorlib]System.String::Format(string, object)
      IL_0015:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
      IL_0016:  throw

      IL_0021:  ldarg.0
      IL_0022:  conv.u4
      IL_0023:  ret
    }

    .method public hidebysig static uint64  ToUInt64RuntimeCheck<TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0001:  ldtoken    !!TEnum
      IL_0002:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0003:  callvirt   instance bool [mscorlib]System.Type::get_IsEnum()
      IL_0004:  brtrue.s   IL_0021

      IL_0011:  ldstr      "'{0}' is not an enum."
      IL_0012:  ldtoken    !!TEnum
      IL_0013:  call       class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
      IL_0014:  call       string [mscorlib]System.String::Format(string, object)
      IL_0015:  newobj     instance void [mscorlib]System.InvalidOperationException::.ctor(string)
      IL_0016:  throw

      IL_0021:  ldarg.0
      IL_0022:  conv.u8
      IL_0023:  ret
    }

    .method public hidebysig static int8  ToInt8<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i
      IL_0002:  ret
    }

    .method public hidebysig static int16  ToInt16<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i2
      IL_0002:  ret
    }

    .method public hidebysig static int32  ToInt32<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i4
      IL_0002:  ret
    }

    .method public hidebysig static int64  ToInt64<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.i8
      IL_0002:  ret
    }

    .method public hidebysig static uint8  ToUInt8<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u
      IL_0002:  ret
    }

    .method public hidebysig static uint16  ToUInt16<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u2
      IL_0002:  ret
    }

    .method public hidebysig static uint32  ToUInt32<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u4
      IL_0002:  ret
    }

    .method public hidebysig static uint64  ToUInt64<valuetype 
        .ctor ([mscorlib]System.Enum) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  2
      IL_0000:  ldarg.0
      IL_0001:  conv.u8
      IL_0002:  ret
    }
} 