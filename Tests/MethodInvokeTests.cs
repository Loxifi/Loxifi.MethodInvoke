namespace Loxifi.MethodInvoke.Tests
{
    [TestClass]
    public class MethodInvokeTests
    {
        [TestMethod]
        public void TestExact()
        {
            int result = new TestClass().Invoke<int>(nameof(TestClass.TestReturn1), 0);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestOptionalA()
        {
            int result = new TestClass().Invoke<int>(nameof(TestClass.TestReturn3), "A");
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestOptionalB()
        {
            int result = new TestClass().Invoke<int>(nameof(TestClass.TestReturn3), "A", "B");
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void TestNone()
        {
            int result = new TestClass().Invoke<int>(nameof(TestClass.TestReturn2));
            Assert.AreEqual(2, result);
        }
    }
}