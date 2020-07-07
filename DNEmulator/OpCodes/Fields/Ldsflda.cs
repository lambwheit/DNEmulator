using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Reflection;

namespace DNEmulator.OpCodes.Fields
{
    public class Ldsflda : OpCodeEmulator
    {
        public override Code Code => Code.Ldsflda;

        public override EmulationRequirements Requirements => EmulationRequirements.MemberLoading;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IField iField))
                throw new InvalidILException(ctx.Instruction.ToString());

            var fieldInfo = ctx.Emulator.DynamicContext.LookupMember<FieldInfo>(iField.MDToken.ToInt32());
            ctx.Stack.Push(new NativeValue(fieldInfo.FieldHandle.Value));

            return new NormalResult();
        }
    }
}
