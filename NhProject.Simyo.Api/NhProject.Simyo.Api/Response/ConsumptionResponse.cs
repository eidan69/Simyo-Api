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
    public class ConsumptionResponse:SimyoResponse
    {

        public List<Consumption> Consumptions = new List<Consumption>();

        public ConsumptionResponse(string jsonSimyo)
            : base(jsonSimyo)
        {
            //Obtenemos la respuesta si no hay error
            if (this.Success)
            {
                JObject jsonLinq = JObject.Parse(jsonSimyo);
                if (jsonLinq["response"]["consumptionsByCycle"] != null)
                {
                    //Montamos el array principal
                    JArray consumptionByCycleArray = JArray.Parse((string)jsonLinq["response"]["consumptionsByCycle"].ToString());
                    //Recorremos cada uno
                    foreach (JObject consumptionByCycle in consumptionByCycleArray)
                    {
                        //Creamos el consumption temporal
                        Consumption temporalConsumption = new Consumption();
                        temporalConsumption.StartDate = (long)consumptionByCycle["startDate"];
                        temporalConsumption.EndDate = (long)consumptionByCycle["endDate"];
                        temporalConsumption.BillCycleNumber = (int)consumptionByCycle["billCycleNumber"];
                        temporalConsumption.ChargeTotal = (string)consumptionByCycle["chargeTotal"];
                        //Comprobamos si los valores 0 o 1 existen antes de añadirlos
                        if (consumptionByCycle["balance"]!=null)
                            temporalConsumption.Balance = (string)consumptionByCycle["balance"];
                        if (consumptionByCycle["voice"] != null)
                            temporalConsumption.Voice = new Charge((string)consumptionByCycle["voice"]["chargeTotal"], (long)consumptionByCycle["voice"]["count"]);
                        if (consumptionByCycle["data"] != null)
                            temporalConsumption.Data = new Charge((string)consumptionByCycle["data"]["chargeTotal"], (long)consumptionByCycle["data"]["count"]);
                        if (consumptionByCycle["sms"] != null)
                            temporalConsumption.Sms = new Charge((string)consumptionByCycle["sms"]["chargeTotal"], (long)consumptionByCycle["sms"]["count"]);
                        if (consumptionByCycle["mms"] != null)
                            temporalConsumption.Mms = new Charge((string)consumptionByCycle["mms"]["chargeTotal"], (long)consumptionByCycle["mms"]["count"]);
                        if (consumptionByCycle["dataBundle"] != null)
                            temporalConsumption.DataBundle = new Bundle((long)consumptionByCycle["dataBundle"]["limit"], (long)consumptionByCycle["dataBundle"]["spent"]);
                        if (consumptionByCycle["voicePremium"] != null)
                            temporalConsumption.VoicePremium = new Charge((string)consumptionByCycle["voicePremium"]["chargeTotal"], (long)consumptionByCycle["voicePremium"]["count"]);
                        if (consumptionByCycle["smsPremium"] != null)
                            temporalConsumption.SmsPremium = new Charge((string)consumptionByCycle["smsPremium"]["chargeTotal"], (long)consumptionByCycle["smsPremium"]["count"]);
                        if (consumptionByCycle["voiceOutgoingRoaming"] != null)
                            temporalConsumption.VoiceOutgoingRoaming = new Charge((string)consumptionByCycle["voiceOutgoingRoaming"]["chargeTotal"], (long)consumptionByCycle["voiceOutgoingRoaming"]["count"]);
                        if (consumptionByCycle["voiceIngoingRoaming"] != null)
                            temporalConsumption.VoiceIngoingRoaming = new Charge((string)consumptionByCycle["voiceIngoingRoaming"]["chargeTotal"], (long)consumptionByCycle["voiceIngoingRoaming"]["count"]);
                        if (consumptionByCycle["smsRoaming"] != null)
                            temporalConsumption.SmsRoaming = new Charge((string)consumptionByCycle["smsRoaming"]["chargeTotal"], (long)consumptionByCycle["smsRoaming"]["count"]);
                        if (consumptionByCycle["mmsRoaming"] != null)
                            temporalConsumption.MmsRoaming = new Charge((string)consumptionByCycle["mmsRoaming"]["chargeTotal"], (long)consumptionByCycle["mmsRoaming"]["count"]);
                        if (consumptionByCycle["dataRoaming"] != null)
                            temporalConsumption.DataRoaming = new Charge((string)consumptionByCycle["dataRoaming"]["chargeTotal"], (long)consumptionByCycle["dataRoaming"]["count"]);
                        if (consumptionByCycle["voiceBundle"] != null)
                            temporalConsumption.VoiceBundle = new Bundle((long)consumptionByCycle["voiceBundle"]["limit"], (long)consumptionByCycle["voiceBundle"]["spent"]);
                        
                        //Añadimos el consumption a la lista
                        Consumptions.Add(temporalConsumption);
                    }
                }
            }
        }

        protected override bool checkSpecificIntegrity(JObject jsonLinq)
        {
            bool correct = true;
            correct = correct && (jsonLinq["response"] != null);
            return correct;
        }

        protected override string getSpecificErrorString()
        {
            switch (Header.code)
            {
                case "610":
                    return "El sistema no consigue encontrar tu número";
                    break;
                default:
                    return String.Empty;
            }
        }
    }
}
