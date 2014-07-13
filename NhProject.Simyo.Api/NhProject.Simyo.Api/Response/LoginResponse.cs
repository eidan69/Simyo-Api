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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhProject.Simyo.Api.Response
{

    /// <summary>
    /// Representa a la respuesta recibida al realizar Login
    /// </summary>
    public class LoginResponse:SimyoResponse
    {

        public string SessionId;
        public string CustomerId;

        public LoginResponse(string jsonSimyo):base(jsonSimyo)
        {
            //Obtenemos la respuesta si no hay error
            if (this.Success)
            {
                JObject jsonlinq = JObject.Parse(jsonSimyo);
                SessionId = (string)jsonlinq["response"]["sessionId"];
                CustomerId = (string)jsonlinq["response"]["customerId"];
            }
        }

        protected override bool checkSpecificIntegrity(JObject jsonLinq)
        {
            bool correct = true;
            correct = correct && (jsonLinq["response"] != null);
            correct = correct && (jsonLinq["response"]["sessionId"] != null);
            correct = correct && (jsonLinq["response"]["customerId"] != null);
            return correct;
        }

        protected override string getSpecificErrorString()
        {
            switch (Header.code)
            {
                case "201":
                case "202":
                    return "Usuario o Contraseña incorrectos";
                    break;
                default:
                    return String.Empty;
            }
        }
    }
}
