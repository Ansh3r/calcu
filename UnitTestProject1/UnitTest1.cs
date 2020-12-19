using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Calc.Interfaces;
using Calc.Models;

namespace TestCalculator
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Sum_10and20_30returned()
        {
            ICalculate calc = new Calculate();
            var expected = calc.Parse(new Expression("10+20", calc));
            Assert.AreEqual(expected.Result, 30);
        }
        [TestMethod]
        public void Error_plus()
        {
            ICalculate calc = new Calculate();
            var expected = calc.Parse(new Expression("1+", calc));
            Assert.AreEqual(expected.HasError, true);
        }
        [TestMethod]
        public void TestMinus()
        {
            ICalculate calc = new Calculate();
            var expected = calc.Parse(new Expression("2-1", calc));
            Assert.AreEqual(expected.Result, 1);
        }
        [TestMethod]
        public void TestMinus2()
        {
            ICalculate calc = new Calculate();
            var expected = calc.Parse(new Expression("1-2", calc));
            Assert.AreEqual(expected.Result, -1);
        }
        [TestMethod]
        public void TestDivision()
        {
            ICalculate calc = new Calculate();
            var res = calc.Parse(new Expression("50/5", calc));
            Assert.AreEqual(res.Result, 10);
        }
        [TestMethod]
        public void TestMultiplication()
        {
            ICalculate calc = new Calculate();
            var expected = calc.Parse(new Expression("10*2", calc));
            Assert.AreEqual(expected.Result, 20);
        }
        [TestMethod]
        public void TestBrackets()
        {
            ICalculate calc = new Calculate();
            var expected = calc.Parse(new Expression("2*(1+2)", calc));
            Assert.AreEqual(expected.Result, 6);
        }
    }
}
