using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Runtime.InteropServices;

namespace DNEmulator.OpCodes.Misc
{
    public class Ldobj : OpCodeEmulator
    {
        public override Code Code => Code.Ldobj;
        public override EmulationRequirements Requirements => EmulationRequirements.MemberLoading;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Stack.Pop() is NativeValue address))
                throw new InvalidStackException();

            if (!(ctx.Instruction.Operand is ITypeDefOrRef typeDefOrRef))
                throw new InvalidILException(ctx.Instruction.ToString());

            var type = ctx.Emulator.DynamicContext.LookupMember<Type>(typeDefOrRef.MDToken.ToInt32());
            
            ctx.Stack.Push(new ObjectValue(Marshal.PtrToStructure(address.Value, type)));
            return new NormalResult();
        }
    }
}
