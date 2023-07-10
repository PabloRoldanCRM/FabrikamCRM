using Microsoft.VisualStudio.TestTools.UnitTesting;
using PL101.plugin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace PL101.UT
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void DeserializarJSON()
        {
            string json = "{\"id\":\"594e81e2-3d0f-4ca3-951d-e72c2ca6ab48\",\"folio\":\"YNWRJNF0257\",\"fecha\":\"2023-07-06T11:17:15.2791959-06:00\",\"interpretacion\":\"PabloCesarRoldanPerezconRFCROPP960802HabilitadoenelcoreconelfolioYNWRJNF0257el06/07/2023\"}";
            string json2c = "{\r\n\t\"id\": \"594e81e2-3d0f-4ca3-951d-e72c2ca6ab48\",\r\n\t\"folio\": \"YNWRJNF0257\",\r\n\t\"fecha\": \"2023-07-06T11:17:15.2791959-06:00\",\r\n\t\"interpretacion\": \"Pablo Cesar Roldan Perez con RFC ROPP960802 Habilitado en el core con el folio YNWRJNF0257 el 06/07/2023\"\r\n}";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ResponseClass objresponse = serializer.Deserialize<ResponseClass>(json2c);
        }

        [TestMethod]
        public void SerializarJSON()
        {
            var obj = new ResponseClass()
            {
                id = "1",
                folio = "XXXXXXXX",
                fecha = DateTime.Now,
                interpretacion = "ABC 123"

            };
            StringBuilder sb = new StringBuilder();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(obj, sb);
            string serializedJSON = sb.ToString();
        }
    }
}
