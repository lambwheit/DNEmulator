using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Reflection;

namespace DNEmulator.OpCodes.Misc
{
    public class Ldtoken : OpCodeEmulator
    {
        public override Code Code => Code.Ldtoken;
        public override EmulationRequirements Requirements => EmulationRequirements.MemberLoading;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IDnlibDef dnlibDef))
                throw new InvalidILException(ctx.Instruction.ToString());
            var member = ctx.Emulator.DynamicContext.LookupMember<MemberInfo>(dnlibDef.MDToken.ToInt32());
            object handle = null;
            switch(member)
            {
                case Type type:
                    handle = type.TypeHandle;
                    break;
                case MethodBase methodBase:
                    handle = methodBase.MethodHandle;
                    break;
                case FieldInfo fieldInfo:
                    handle = fieldInfo.FieldHandle;
                    break;

            }
            ctx.Stack.Push(new ObjectValue(handle));
            return new NormalResult();
        }
    }
}
