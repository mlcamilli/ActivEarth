using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ActivEarth.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivEarth.Tests
{
    [TestClass]
    public class BMICalculatorTest
    {
        [TestMethod]
        public void TestCalculateBMI()
        {
            Assert.AreEqual(23, BMICalculator.CalculateBMI(72, 175));
        }
        [TestMethod]
        public void TestGetBMIResult()
        {
            Assert.AreEqual("underweight", BMICalculator.GetBMIResult(16));
            Assert.AreEqual("normal", BMICalculator.GetBMIResult(23));
            Assert.AreEqual("overweight", BMICalculator.GetBMIResult(26));
            Assert.AreEqual("obese", BMICalculator.GetBMIResult(32));
        }
    }
}
