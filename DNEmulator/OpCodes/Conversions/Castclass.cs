using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;

namespace DNEmulator.OpCodes.Conversions
{

    public class Castclass : OpCodeEmulator
    {
        public override Code Code => Code.Castclass;

        public override EmulationRequirements Requirements => EmulationRequirements.MemberLoading;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IType iType))
                throw new InvalidILException(ctx.Instruction.ToString());

            object obj = ctx.Stack.Pop().GetValue();
            var type = ctx.Emulator.DynamicContext.LookupMember<Type>(iType.MDToken.ToInt32());

            if (!type.IsAssignableFrom(obj.GetType()))
                throw new InvalidCastException();

            ctx.Stack.Push(Value.FromObject(Convert.ChangeType(obj, type)));

            return new NormalResult();
        }
    }
}

