using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;

namespace DNEmulator.OpCodes.Misc
{
    public class Isinst : OpCodeEmulator
    {
        public override Code Code => Code.Isinst;

        public override EmulationRequirements Requirements => EmulationRequirements.MemberLoading;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IType iType))
                throw new InvalidILException(ctx.Instruction.ToString());

            object obj = ctx.Stack.Pop().GetValue();
            var type = ctx.Emulator.DynamicContext.LookupMember<Type>(iType.MDToken.ToInt32());

            if (!type.IsAssignableFrom(obj.GetType()))
                ctx.Stack.Push(new ObjectValue(null));
            else
                ctx.Stack.Push(Value.FromObject(Convert.ChangeType(obj, type)));

            return new NormalResult();
        }
    }
}
