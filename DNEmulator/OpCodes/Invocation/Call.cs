using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System.Linq;
using System.Reflection;

namespace DNEmulator.OpCodes.Invocation
{
    public class Call : OpCodeEmulator
    {
        public override Code Code => Code.Call;
        public override EmulationRequirements Requirements => EmulationRequirements.None;

        public override EmulationResult Emulate(Context ctx)
        {
            if (!(ctx.Instruction.Operand is IMethod iMethod))
                throw new InvalidILException(ctx.Instruction.ToString());

            var method = (iMethod is MethodDef methodDef) ? methodDef : iMethod.ResolveMethodDef();
            if((method.IsPinvokeImpl || !method.HasBody) 
                && !(ctx.Emulator.DynamicContext is null) 
                && ctx.Emulator.DynamicContext.AllowInvocation)
            {
                var methodInfo = ctx.Emulator.DynamicContext.LookupMember<MethodInfo>(iMethod.MDToken.ToInt32());
                var parameters = ctx.Stack.PopObjects(methodInfo.GetParameters().Length).Reverse().ToArray();
                var thisPtr = method.IsStatic ? null : ((ObjectValue)ctx.Stack.Pop()).Value;
                object returnValue = methodInfo.Invoke(thisPtr, parameters);
                if (method.ReturnType.ElementType != ElementType.Void)
                    ctx.Stack.Push(Value.FromObject(returnValue));
                return new NormalResult();
            }
            var emulator = new CILEmulator(method, ctx.Emulator.DynamicContext, ctx.Stack.Pop(method.Parameters.Count).Reverse());
            emulator.Emulate();

            if (method.ReturnType.ElementType != ElementType.Void)
                ctx.Stack.Push(emulator.ValueStack.Pop());

            return new NormalResult();
        }
    }
}
