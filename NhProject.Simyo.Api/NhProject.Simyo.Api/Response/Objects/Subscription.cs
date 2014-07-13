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
    public class Subscription
    {
        /// <summary>
        /// Identificación de la linea
        /// </summary>
        public string SubscriberId { get; set; }
        /// <summary>
        /// MSISDN de la subscripción.
        /// </summary>
        public string Msisdn { get; set; }
        /// <summary>
        /// Tipo de ciclo de la subscripción. Valores de 1 a 28.
        /// Actualmente se recibirán tan solo valore 1 o 20.
        /// </summary>
        public int BillCycleType { get; set; }
        /// <summary>
        /// Fecha de creación del usuario representada como
        /// número de milisegundos desde el 1 de enero de
        /// 1970.
        /// </summary>
        public long RegisterDate { get; set; }
        /// <summary>
        /// Modo de pago: 
        /// 01 : Postpaid
        /// 02 : Prepaid
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// Identificador de tarifa
        /// </summary>
        public string MainProductId { get; set; }
    }
}
