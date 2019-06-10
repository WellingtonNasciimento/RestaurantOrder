using RestaurantOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RestaurantOrderTest
{
    [TestClass]
    public class OrderTest
    {
        [TestMethod]
        public void IncluirPedido()
        {
            //Arranjo
            string timeDay = "morning";
            List<string> listaDeCodigos = new List<string>() { "1", "2", "3", "4" };
            string restuladoEsperado =  "eggs, toast, coffee, error";

            //Inclui o pedido
            Order order = new Order(timeDay, listaDeCodigos);

            //Assert
            Assert.AreEqual(restuladoEsperado, order.CodigosIcluidos);
        }
    }
}