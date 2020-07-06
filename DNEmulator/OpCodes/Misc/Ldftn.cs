using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Reflection;

namespace DNEmulator.OpCodes.Misc
{
    public class Ldftn : OpCodeEmulator
    {
        public override Code Code => Code.Ldftn;

        public override EmulationRequirements Requirements => EmulationRequirements.MemberLoading;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IMethod iMethod))
                throw new InvalidILException(ctx.Instruction.ToString());

            var methodInfo = ctx.Emulator.DynamicContext.LookupMember<MethodInfo>(iMethod.MDToken.ToInt32());
            ctx.Stack.Push(new NativeValue(methodInfo.MethodHandle.GetFunctionPointer()));
            return new NormalResult();
        }
    }
}
