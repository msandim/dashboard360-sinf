using System;
using FirstREST.Lib_Primavera;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FirstREST.Tests
{
    [TestClass]
    public class MaleToFemaleRatioTest
    {
        [TestMethod]
        public void GetMaleToFemaleRatio()
        {
            float ratio = PriIntegration.GetMaleToFemaleRatio();
            //Assert.AreEqual(1, ratio);
        }
    }
}
