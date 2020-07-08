using dnlib.DotNet;
namespace DNEmulator.Tests
{
    public static class Resolver
    {
        public static void Resolve(ModuleDef module)
        {
            var asmResolver = new AssemblyResolver();
            var modCtx = new ModuleContext(asmResolver);
            asmResolver.DefaultModuleContext = modCtx;
            module.Context = modCtx;
            asmResolver.EnableTypeDefCache = true;
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                var def = asmResolver.Resolve(asmRef, module);
                asmResolver.AddToCache(def);
            }
        }
    }
}
