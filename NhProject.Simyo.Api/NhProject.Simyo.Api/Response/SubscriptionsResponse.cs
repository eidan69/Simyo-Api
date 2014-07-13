/*Copyright 2014 Alberto Ferrero López

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/

using Newtonsoft.Json.Linq;
using NhProject.Simyo.Api.Response.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhProject.Simyo.Api.Response
{

    /// <summary>
    /// Representa a la respuesta recibida al solicitar las subscripciones (lineas)
    /// </summary>
    public class SubscriptionsResponse:SimyoResponse
    {

        public List<Subscription> Subscriptions;

        public SubscriptionsResponse(string jsonSimyo)
            : base(jsonSimyo)
        {
            //Obtenemos la respuesta si no hay error
            if (this.Success)
            {
                JObject jsonlinq = JObject.Parse(jsonSimyo);
                //http://james.newtonking.com/json/help/index.html?topic=html/DeserializeWithLinq.htm
                JArray subscriptionsArray = JArray.Parse((string)jsonlinq["response"]["subcriptions"].ToString());
                Subscriptions = subscriptionsArray.Select(p => new Subscription { 
                    SubscriberId = (string)p["subscriberId"],
                    Msisdn = (string)p["msisdn"],
                    BillCycleType = (int)p["billCycleType"],
                    RegisterDate = (long)p["registerDate"],
                    PayType = (string)p["payType"],
                    MainProductId = (string)p["mainProductId"]
                }).ToList();
            }
        }

        protected override bool checkSpecificIntegrity(JObject jsonLinq)
        {
            bool correct = true;
            correct = correct && (jsonLinq["response"] != null);
            correct = correct && (jsonLinq["response"]["subcriptions"] != null);
            return correct;
        }

        protected override string getSpecificErrorString()
        {
            return String.Empty;            
        }
    }
}
