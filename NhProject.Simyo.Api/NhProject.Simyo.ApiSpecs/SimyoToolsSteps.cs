using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace NhProject.Simyo.ApiSpecs
{
    [Binding]
    public class SimyoToolsSteps
    {
        private string resultString = "";

        [Given(@"Queremos llamar a un método de SimyoTools")]
        public void GivenQueremosLlamarAUnMetodoDeSimyoTools()
        {
        }

        [When(@"Cuando llamamos a getPrice con (.*) como string")]
        public void WhenCuandoLlamamosAGetPriceConComoString(string p0)
        {
            resultString = NhProject.Simyo.Api.SimyoTools.getPrice(p0);
        }

        [Then(@"El resultado obtenido debe de ser el (.*)")]
        public void ThenElResultadoObtenidoDebeDeSerEl(string p0)
        {
            Assert.AreEqual(p0, resultString);
        }


    }
}
