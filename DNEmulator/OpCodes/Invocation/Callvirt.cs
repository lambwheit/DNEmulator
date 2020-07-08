using DNEmulator.Abstractions;
using dnlib.DotNet.Emit;

namespace DNEmulator.OpCodes.Invocation
{
    public class Callvirt : Call
    {
        public override Code Code => Code.Callvirt;
        public override EmulationRequirements Requirements => EmulationRequirements.None;

        public override EmulationResult Emulate(Context ctx)
        {
            return base.Emulate(ctx);
        }
    }
}
