using DNEmulator.Values;
using dnlib.DotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DNEmulator.Tests.Invocation
{
    [TestClass()]
    public class CallvirtTest
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var assembly = typeof(CallvirtTest).Assembly;
            var module = ModuleDefMD.Load(assembly.Location);
            Resolver.Resolve(module);
            var dynamicContext = new DynamicContext(assembly)
            {
                AllowInvocation = true,
            };
            var cilEmulator = new CILEmulator(module.FindNormal("DNEmulator.Tests.Invocation.CallvirtTest").FindMethod("TestMethod"),
                              dynamicContext);
            cilEmulator.Emulate();
            var value = cilEmulator.ValueStack.Pop();
            Assert.AreEqual(typeof(I4Value), value.GetType());
            var emulatedToString = (I4Value)value;
            var toString = TestMethod();
            Assert.AreEqual(toString, (char)emulatedToString.Value);
        }

     
        private char TestMethod()
        {
            var obj = new Obj("Hello");
            return obj.First();
        }

        public class Obj
        {
            public string _str;
            public Obj(string str)
            {
                _str = str;
            }
            public char First() => _str[0];

        }
    }
}
