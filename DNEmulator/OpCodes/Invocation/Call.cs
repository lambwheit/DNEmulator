﻿using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Linq;

namespace DNEmulator.OpCodes.Invocation
{
    public class Call : IOpCode
    {
        public Code Code => Code.Call;

        public EmulationResult Emulate(Context ctx)
        {
            var method = ((IMethod)ctx.Instruction.Operand).ResolveMethodDef();
            var emulator = new Emulator(method, ctx.Stack.Pop(method.Parameters.Count).Reverse());
            emulator.Emulate();
            if (method.ReturnType.ElementType != ElementType.Void)
                ctx.Stack.Push(emulator.ValueStack.Pop());

            return new NormalResult();
        }
    }
}
