
using DNEmulator.Tests.Misc;
using DNEmulator.Values;
using dnlib.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DNEmulator.Tests.Conversions
{
    [TestClass]
    public class Isinst 
    {

        [TestMethod()]
        public void IsDateTime()
        {
            var assembly = typeof(Isinst).Assembly;
            var module = ModuleDefMD.Load(assembly.Location);
            Resolver.Resolve(module);
            var dynamicContext = new DynamicContext(assembly);
            var cilEmulator = new CILEmulator(module.FindNormal("DNEmulator.Tests.Conversions.Isinst").FindMethod("TestMethod"),
                              dynamicContext);
            cilEmulator.Emulate();
            var value = cilEmulator.ValueStack.Pop();
            Assert.AreEqual(typeof(I4Value), value.GetType());
            var emulatedIsDateTime = (I4Value)value;
            bool isDateTime = TestMethod();
            Assert.AreEqual(isDateTime, emulatedIsDateTime.Value != 0);
        }

        public bool TestMethod()
        {
            object date = "20.04.1889";
            return date is string;
        }
    }
}
