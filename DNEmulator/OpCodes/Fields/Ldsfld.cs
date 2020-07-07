using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace DNEmulator.OpCodes.Fields
{
    public class Ldsfld : OpCodeEmulator
    {
        public override Code Code => Code.Ldsfld;
        public override EmulationRequirements Requirements => EmulationRequirements.None;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IField iField))
                throw new InvalidILException(ctx.Instruction.ToString());

            var field = (iField is FieldDef fieldDef) ? fieldDef : iField.ResolveFieldDef();

            if (!field.IsStatic)
                throw new InvalidILException(ctx.Instruction.ToString());

            ctx.Stack.Push(ctx.Emulator.FieldMap.Get(field));
            return new NormalResult();
        }
    }
}
