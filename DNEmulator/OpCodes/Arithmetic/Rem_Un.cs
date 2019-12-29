﻿using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Enumerations;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet.Emit;
using System;

namespace DNEmulator.OpCodes.Arithmetic
{
    public class Rem_Un : IOpCode
    {
        public Code Code => Code.Rem_Un;

        public EmulationResult Emulate(Context ctx)
        {
            var secondValue = ctx.Stack.Pop();
            var firstValue = ctx.Stack.Pop();

            switch (firstValue.ValueType)
            {
                case DNValueType.Int32 when secondValue.ValueType == DNValueType.Int32:
                    ctx.Stack.Push(new I4Value((int)((uint)((I4Value)firstValue).Value % (uint)((I4Value)secondValue).Value)));
                    break;
                case DNValueType.Int32 when secondValue.ValueType == DNValueType.Native:
                    ctx.Stack.Push(new NativeValue(new IntPtr((long)((uint)((I4Value)firstValue).Value % (ulong)((NativeValue)secondValue).Value))));
                    break;
                case DNValueType.Int64 when secondValue.ValueType == DNValueType.Int64:
                    ctx.Stack.Push(new I8Value((long)((ulong)((I8Value)firstValue).Value % (ulong)((I8Value)secondValue).Value)));
                    break;
                case DNValueType.Native when secondValue.ValueType == DNValueType.Native:
                    ctx.Stack.Push(new NativeValue(new IntPtr((long)((ulong)((NativeValue)firstValue).Value % (ulong)((NativeValue)secondValue).Value))));
                    break;
                case DNValueType.Native when secondValue.ValueType == DNValueType.Int32:
                    ctx.Stack.Push(new NativeValue(new IntPtr((long)((ulong)((NativeValue)firstValue).Value % (uint)((I4Value)secondValue).Value))));
                    break;
                default:
                    throw new InvalidILException(ctx.Instruction.ToString());
            }


            return new NormalResult();
        }
    }
}