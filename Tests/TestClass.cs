namespace Loxifi.MethodInvoke.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    internal class TestClass
    {
        public int TestReturn1(int x) => 1;

        public int TestReturn2() => 2;

        public int TestReturn3(string a, string b = "") => 3;
    }
}
