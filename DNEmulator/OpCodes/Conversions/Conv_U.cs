﻿using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Enumerations;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet.Emit;
using System;

namespace DNEmulator.OpCodes.Conversions
{
    public class Conv_U : IOpCode
    {
        public Code Code => Code.Conv_U;

        public EmulationResult Emulate(Context ctx)
        {
            var firstValue = ctx.Stack.Pop();

            switch (firstValue.ValueType)
            {
                case DNValueType.Int32:
                    ctx.Stack.Push(new NativeValue((IntPtr)(long)(ulong)(UIntPtr)((I4Value)firstValue).Value));
                    break;
                case DNValueType.Int64:
                    ctx.Stack.Push(new NativeValue((IntPtr)(long)(ulong)(UIntPtr)((I8Value)firstValue).Value));
                    break;
                case DNValueType.Real:
                    ctx.Stack.Push(new NativeValue((IntPtr)(long)(ulong)(UIntPtr)((R8Value)firstValue).Value));
                    break;
                default:
                    throw new InvalidILException(ctx.Instruction.ToString());
            }

            return new NormalResult();
        }
    }
}
