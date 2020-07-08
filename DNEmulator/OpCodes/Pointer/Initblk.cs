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
    public class Initblk : OpCodeEmulator
    {
        public override Code Code => Code.Initblk;

        public override EmulationRequirements Requirements => EmulationRequirements.None;

        private delegate void InitBlock(IntPtr destination, IntPtr source, int length);
        private static InitBlock _init;
        static Initblk()
        {
            var dynamicMethod = new DynamicMethod(string.Empty, typeof(void), new Type[] { typeof(IntPtr), typeof(IntPtr), typeof(int) });
            var ilGenerator = dynamicMethod.GetILGenerator();
            //Emitting address
            ilGenerator.Emit(SRE.OpCodes.Ldarg_0);
            //Emitting initialiser
            ilGenerator.Emit(SRE.OpCodes.Ldarg_1);
            //Emitting length
            ilGenerator.Emit(SRE.OpCodes.Ldarg_2);
            ilGenerator.Emit(SRE.OpCodes.Initblk);
            ilGenerator.Emit(SRE.OpCodes.Ret);
            _init = (InitBlock)dynamicMethod.CreateDelegate(typeof(InitBlock));
        }
        public override EmulationResult Emulate(Context ctx)
        {
            var thirdValue = ctx.Stack.Pop();
            var secondValue = ctx.Stack.Pop();
            var firstValue = ctx.Stack.Pop();

            if (!(firstValue is NativeValue address && secondValue is NativeValue initialiser && thirdValue is I4Value length))
                throw new InvalidStackException();

            _init(address.Value, initialiser.Value, length.Value);
            return new NormalResult();
        }
    }
}
