using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet.Emit;
using System;
using System.Reflection.Emit;
using SRE = System.Reflection.Emit;

namespace DNEmulator.OpCodes.Pointer
{
    public class Cpblk : OpCodeEmulator
    {
        public override Code Code => Code.Cpblk;

        public override EmulationRequirements Requirements => EmulationRequirements.None;

        private delegate void CopyBlock(IntPtr destination, IntPtr source, int length);
        private static CopyBlock _copy;
        static Cpblk()
        {
            var dynamicMethod = new DynamicMethod(string.Empty, typeof(void), new Type[] { typeof(IntPtr), typeof(IntPtr), typeof(int) });
            var ilGenerator = dynamicMethod.GetILGenerator();
            //Emitting destination address
            ilGenerator.Emit(SRE.OpCodes.Ldarg_0);
            //Emitting source address
            ilGenerator.Emit(SRE.OpCodes.Ldarg_1);
            //Emitting length
            ilGenerator.Emit(SRE.OpCodes.Ldarg_2);
            ilGenerator.Emit(SRE.OpCodes.Cpblk);
            ilGenerator.Emit(SRE.OpCodes.Ret);
            _copy = (CopyBlock)dynamicMethod.CreateDelegate(typeof(CopyBlock));
        }
        public override EmulationResult Emulate(Context ctx)
        {
            var thirdValue = ctx.Stack.Pop();
            var secondValue = ctx.Stack.Pop();
            var firstValue = ctx.Stack.Pop();

            if (!(firstValue is NativeValue destination && secondValue is NativeValue source && thirdValue is I4Value length))
                throw new InvalidStackException();

            _copy(destination.Value, source.Value, length.Value);
            return new NormalResult();
        }
    }
}
