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
    public class Charge
    {
        /// <summary>
        /// Coste total de las llamadas. Unidad: Euros con 6 decimales.
        /// </summary>
        public string ChargeTotal;
        /// <summary>
        /// Duración total de llamadas en segundos.
        /// </summary>
        public long Count;

        public Charge(string chargeTotal, long count)
        {
            ChargeTotal = chargeTotal;
            Count = count;
        }
    }
}
