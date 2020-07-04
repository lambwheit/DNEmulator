using DNEmulator.Abstractions;
using DNEmulator.EmulationResults;
using DNEmulator.Exceptions;
using DNEmulator.Values;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Linq;
using System.Reflection;

namespace DNEmulator.OpCodes.Invocation
{
    public class Callvirt : Call
    {
        public override Code Code => Code.Callvirt;
        public override EmulationRequirements Requirements => EmulationRequirements.None;

        public override EmulationResult Emulate(Context ctx)
        {
            base.Emulate(ctx);
            return new NormalResult();
        }
    }
}
