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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhProject.Simyo.Api.Response.Objects
{
    public class Consumption
    {
        /// <summary>
        /// Fecha inicio periodo facturación
        /// </summary>
        public long StartDate;
        /// <summary>
        /// Fecha fin periodo facturación
        /// </summary>
        public long EndDate;
        /// <summary>
        /// Antigüedad del ciclo. El valor 1 corresponde al ciclo actual, el 2 al ciclo anterior y así sucesivamente.
        /// </summary>
        public int BillCycleNumber;
        /// <summary>
        /// Consumo total del ciclo. Unidad: Euros con 6 decimales.
        /// </summary>
        public string ChargeTotal;
        /// <summary>
        /// Saldo en caso de usuario prepago y ciclo actual. Unidad: Euros con 6 decimales o null
        /// </summary>
        public string Balance = null;
        public Charge Voice = null;
        public Charge Data = null;
        public Charge Sms = null;
        public Charge Mms = null;
        public Bundle DataBundle = null;
        public Charge VoicePremium = null;
        public Charge SmsPremium = null;
        public Charge VoiceOutgoingRoaming = null;
        public Charge VoiceIngoingRoaming = null;
        public Charge SmsRoaming = null;
        public Charge MmsRoaming = null;
        public Charge DataRoaming = null;
        public Bundle VoiceBundle = null;


        public string GetStartDateFormated()
        {
            return "";
        }
        public string GetEndDateFormated()
        {
            return "";
        }
    }
}
