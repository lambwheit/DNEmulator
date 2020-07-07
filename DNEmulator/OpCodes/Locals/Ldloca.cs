using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet.Emit;
using System;
using System.Runtime.InteropServices;

namespace DNEmulator.OpCodes.Locals
{
    public class Ldloca : OpCodeEmulator
    {
        public override Code Code => Code.Ldloca;

        public override EmulationRequirements Requirements => EmulationRequirements.None;

        public unsafe override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is Local local))
                throw new InvalidILException(ctx.Instruction.ToString());

            object value = ctx.Emulator.LocalMap.Get(local).GetValue();
            var handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            try
            {
                var localPtr = GCHandle.ToIntPtr(handle);
                ctx.Stack.Push(new NativeValue(localPtr));
            }
            finally
            {
                handle.Free();
            }
            return new NormalResult();
        }
    }
}
