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

namespace NhProject.Simyo.Api
{
    public class SimyoTools
    {
        /// <summary>
        /// Convierte la cantidad de bytes dada en la unidad mayor posible (KB, MB, GB...)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>String</returns>
        public static string fromBytesToXBytes(long bytes)
        {
            //Calculamos la cantidad
            int saltos = 0;
            for (; saltos < 5 && bytes >= 1024;saltos++ )
                bytes = bytes / 1024;

            //Ahora que tenemos la cantidad, con el número de saltos obtenemos la unidad
            string unit;
            switch (saltos)
            {
                case 0:
                    unit = "B";
                    break;
                case 1:
                    unit = "KB";
                    break;
                case 2:
                    unit = "MB";
                    break;
                case 3:
                    unit = "GB";
                    break;
                case 4:
                    unit = "TB";
                    break;
                default:
                    unit = "";
                    break;
            }
            string result = String.Format("{0} {1}", bytes, unit);
            return result;

        }

        /// <summary>
        /// Convierte una cantidad de segundos en un string que exprese dias, horas, minutos y segundos
        /// </summary>
        /// <param name="totalSeconds"></param>
        /// <returns>String</returns>
        public static string fromSecondsToXTime(long totalSeconds)
        {
            //Despiezamos la marca de tiempo
            TimeSpan timespan = TimeSpan.FromSeconds(totalSeconds);
            int days = timespan.Days;
            int hours = timespan.Hours;
            int minutes = timespan.Minutes;
            int seconds = timespan.Seconds;
            //Componemos el retorno
            string toReturn = "";
            if (days != 0)
                toReturn += days + " dias ";
            if (hours != 0)
                toReturn += hours + " horas ";
            if (minutes != 0)
                toReturn += minutes + " minutos ";
            
            toReturn += seconds + " segundos";

            return toReturn;
        }

        /// <summary>
        /// Convierte una longitud de segundos desde la marca de tiempo unix a DateTime
        /// Obtenido de http://stackoverflow.com/questions/249760/how-to-convert-unix-timestamp-to-datetime-and-vice-versa
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Dado una cantidad, la redondea y devuelve un string con la moneda
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string getPrice(double quantity, string currency = "€")
        {
            //Redondeamos el precio
            quantity = Math.Round(quantity, 2);
            //Convertimos a string
            string finalResult = Convert.ToString(quantity)+" "+currency;
            return finalResult;
        }

        /// <summary>
        /// Dada una cantidad, la redondea y devuelve un string con la moneda
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string getPrice(string quantity, string currency = "€")
        {
            //Cambiamos punto por coma
            quantity = quantity.Replace(".", ",");
            //Convertimos a double
            double quantity_double = Convert.ToDouble(quantity);
            return SimyoTools.getPrice(quantity_double, currency);
        }
    }
}
