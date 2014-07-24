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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NhProject.Simyo.Api.Response
{
    public abstract class SimyoResponse
    {
        public Header Header;
        public bool Success = false;
        private string _rawResponse;

        public SimyoResponse(string json)
        {
            //En el constructor de MainReponse comprobamos la integridad e inicializamos el header
            //http://james.newtonking.com/json/help/index.html
            try
            {
                JObject jsonLinq = JObject.Parse(json);
                if (checkHeaderIntegrity(jsonLinq))
                {
                    //Integridad de datos correcta
                    Header = JsonConvert.DeserializeObject<Response.Header>(jsonLinq["header"].ToString());

                    //Analizamos los datos  de la respuesta
                    if (Header.code == "100")
                    {
                        //Segun Simyo todo está bien, comprobamos si tenemos lo que necesitamos
                        Success = checkResponseIntegrity(jsonLinq);
                        if (Success == false)
                        {
                            Header.code = "NH002"; //Establecemos un código propio para indicar que hay un error en el cuerpo
                        }
                    }
                }
                else
                {
                    //Integridad errónea
                    Header = new Header();
                    Header.code = "NH001"; //Establecemos un código propio para indicar que hay un error en la cabecera
                }
            }
            catch (Exception exception)
            {
                //Error en la conexión
                Header = new Header();
                Header.code = "NH003"; //Establecemos un código propio para indicar que hay un error en la conexión
                //TODO enviar el mensaje de la excepción
                
            }

            //Guardamos la consulta en bruto
            this._rawResponse = json;
        }

        #region Integridad

        /// <summary>
        /// Comprueba la integridad de la cabecera de la respuesta
        /// </summary>
        /// <param name="jsonLinq"></param>
        /// <returns></returns>
        private bool checkHeaderIntegrity(JObject jsonLinq)
        {
            bool correct = true;
            correct = correct && (jsonLinq["header"] != null);
            correct = correct && (jsonLinq["header"]["code"] != null);
            correct = correct && (jsonLinq["header"]["description"] != null);
            correct = correct && (jsonLinq["header"]["sessionId"] != null);
            return correct;
        }

        /// <summary>
        /// Comprueba la integridad del cuerpo de la resupesta comprobando que existen todos los elementos
        /// </summary>
        /// <param name="jsonLinq">Objeto del tipo JObject, un json-linq</param>
        /// <returns></returns>
        protected bool checkResponseIntegrity(JObject jsonLinq)
        {
            bool correct = true;            
            correct = correct && checkSpecificIntegrity(jsonLinq);
            return correct;
        }

        /// <summary>
        /// Integridad específica de cada respuesta
        /// </summary>
        /// <param name="jsonLinq"></param>
        /// <returns></returns>
        protected abstract bool checkSpecificIntegrity(JObject jsonLinq);

        #endregion

        #region Mensajes de error

        public string getErrorString()
        {
            string to_return = getSpecificErrorString();
            if (to_return == String.Empty)
                to_return = getGeneralErrorString();
            return to_return;
        }

        /// <summary>
        /// Analiza el código de estado y devuelve el error correspondiente - Errores generales para todas las consultas
        /// </summary>
        /// <returns></returns>
        protected string getGeneralErrorString()
        {
            string to_return = "";
            switch (Header.code)
            {
                case "631":
                    to_return = "Error al reconocer tu cuenta";
                    break;
                case "995":
                    to_return = "Tiempo de espera para la respuesta excedido";
                    break;
                case "997":
                    to_return = "Error en la sesión";
                    break;
                case "999":
                    to_return = "Error genérico";
                    break;
                case "0001":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0002":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0003":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0004":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0005":
                    to_return = "La gran API de Simyo parece estar en mantenimiento, en unos minutos debería de volver a funcionar";
                    break;
                case "0006":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0007":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0008":
                    to_return = "Ahora mismo no se puede realizar esta petición, hemos informado del error";
                    break;
                case "0009":
                    to_return = "Error genérico en la API de Simyo";
                    break;
                case "0010":
                    to_return = "Servicio no disponible (de momento)";
                    break;
                case "NH001":
                case "NH002":
                    to_return = "Parece que Simyo ha tocado algo y no ha devuelto lo que esperábamos. Ya estamos trabajando en ello.";
                    break;
                case "NH003":
                    to_return = "Es posible que el servidor de Simyo esté caido, no conseguimos que responda. Prueba en unos minutos.";
                    break;
                default:
                    to_return = "Error genérico no identificado";
                    break;
            }
            //TODO si llegamos a este punto, sería interesante enviar el error genérico para analizar
            return to_return;
        }

        /// <summary>
        /// Analiza el código de estado y devuelve el error correspondiente
        /// </summary>
        /// <returns></returns>
        abstract protected string getSpecificErrorString();

        #endregion
    }
}
