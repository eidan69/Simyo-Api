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
using Org.BouncyCastle.Crypto.Parameters;
using RestSharp;
using Org.BouncyCastle.Security;
using System.Text;
using NhProject.Simyo.Api.Response;
using NhProject.Simyo.Api.Response.Objects;

namespace NhProject.Simyo.Api
{

    /// <summary>
    /// Clase Singleton encargada de la comunicación con la api de Simyo
    /// </summary>
    public class SimyoApi
    {
        #region Elementos Singleton

        /// <summary>
        /// Instancia privada
        /// </summary>
        private static SimyoApi _instance;

        private SimyoApi(){}

        public static SimyoApi Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SimyoApi();

                return _instance;
            }
        }

        #endregion

        #region Delegados y Eventos

        public delegate void SimyoLoginResponseDelegate(LoginResponse response);
        public event SimyoLoginResponseDelegate LoginSuccess;
        public event SimyoLoginResponseDelegate LoginError;

        public delegate void SimyoLogoutResponseDelegate(LogoutResponse response);
        public event SimyoLogoutResponseDelegate LogoutSuccess;

        public delegate void SimyoSubscriptionResponseDelegate(SubscriptionsResponse response);
        public event SimyoSubscriptionResponseDelegate SubscriptionsSuccess;
        public event SimyoSubscriptionResponseDelegate SubscriptionError;

        public delegate void SimyoConsumptionResponseDelegate(ConsumptionResponse response);
        public event SimyoConsumptionResponseDelegate ConsumptionSuccess;
        public event SimyoConsumptionResponseDelegate ConsumptionError;

        #endregion

        #region propiedades

        /// <summary>
        /// Url base de la api de simyo
        /// </summary>
        private const string _simyoBaseUrlConstant= "https://api.simyo.es/api/";
        /// <summary>
        /// Clave privada
        /// </summary>
        private static string _privateKey;
        /// <summary>
        /// Clave pública
        /// </summary>
        private static string _publicKey;
        /// <summary>
        /// Clave para el cifrado 3des o desede
        /// </summary>
        private static string _desedeKey;
        /// <summary>
        /// Bandera que indica si la configuración de la api se ha establecido
        /// </summary>
        private static bool _apiDataSetted = false;

        #endregion

        #region Métodos

        public void setApiData(string privateKey, string publicKey, string desedeKey)
        {
            _privateKey = privateKey;
            _publicKey = publicKey;
            _desedeKey = desedeKey;
            _apiDataSetted = true;
        }

        public void DoLogin(string user, string password)
        {
            if(_apiDataSetted==false)
                return;
            //Componemos la url de consulta
            string rest_url = _simyoBaseUrlConstant + "login";
            //Solicitamos la url firmada
            rest_url = this.addSigToUrl(rest_url);
            //Solicitamos el password cifrado
            string password_3des = this.get3desCypher(password);

            //Montamos el objeto Rest
            var client = new RestClient(rest_url); //Creamos el cliente
            var request = new RestRequest(RestSharp.Method.POST); //Creamos los parámetros
            request.AddParameter("publicKey", _publicKey);
            request.AddParameter("user", user);
            request.AddParameter("password", password_3des);

            //Hacemos la llamada
            client.ExecuteAsync(request, (response, handler) =>
            {
                //Analizamos el resultado
                string content = response.Content;
                LoginResponse login_response = new LoginResponse(content);
                //Según la respuesta, determinamos a qué evento hay que llamar
                if(login_response.Success==true){
                    LoginSuccess(login_response);
                }
                else
                {
                    LoginError(login_response);
                }
            });
        }

        public void DoLogout(string sessionId)
        {
            if (_apiDataSetted == false)
                return;
            //Componemos la url de consulta
            string rest_url = _simyoBaseUrlConstant + "logout/";
            rest_url = this.addGetParameterToUrl(rest_url, "sessionId", sessionId);
            rest_url = this.addGetParameterToUrl(rest_url, "publicKey", _publicKey);
            //Solicitamos la url firmada
            rest_url = this.addSigToUrl(rest_url);

            //Montamos el objeto Rest
            var client = new RestClient(rest_url); //Creamos el cliente
            var request = new RestRequest(RestSharp.Method.GET); //Creamos la consulta

            //Hacemos la llamada
            client.ExecuteAsync(request, (response, handler) =>
            {
                //Analizamos el resultado
                string content = response.Content;
                LogoutResponse sub_response = new LogoutResponse(content);
                //En este caso llamamos siempre al evento
                LogoutSuccess(sub_response);
            });
        }

        public void GetUserSubscriptions(string sessionId, string customerId)
        {
            if (_apiDataSetted == false)
                return;
            //Componemos la url de consulta
            string rest_url = _simyoBaseUrlConstant + "subscriptions/" + customerId;
            rest_url = this.addGetParameterToUrl(rest_url, "sessionId", sessionId);
            rest_url = this.addGetParameterToUrl(rest_url, "publicKey", _publicKey);
            //Solicitamos la url firmada
            rest_url = this.addSigToUrl(rest_url);

            //Montamos el objeto Rest
            var client = new RestClient(rest_url); //Creamos el cliente
            var request = new RestRequest(RestSharp.Method.GET); //Creamos la consulta

            //Hacemos la llamada
            client.ExecuteAsync(request, (response, handler) =>
            {
                //Analizamos el resultado
                string content = response.Content;
                SubscriptionsResponse subs_response = new SubscriptionsResponse(content);
                //Según la respuesta, determinamos a qué evento hay que llamar
                if (subs_response.Success == true)
                {
                    SubscriptionsSuccess(subs_response);
                }
                else
                {
                    SubscriptionError(subs_response);
                }
            });
        }

        /// <summary>
        /// Devuelve información resumida del consumo del usuario
        /// </summary>
        /// <param name="sessionId">Id de la sesión</param>
        /// <param name="customerId">Id del cliente</param>
        /// <param name="subscription">Línea</param>
        /// <param name="billCycle">Ciclo que se desea recuperar. 1 = Ciclo actual, 2 = ciclo anterior... Máximo = 6 ciclos.</param>
        /// <param name="billCycleCount">Numero de ciclos anteriores al indicado en el billCycle de los que se desea la información. Si no se indica nada se considera por defecto 1 (ciclo actual)</param>
        public void GetConsumptionByCycle(string sessionId, string customerId, Subscription subscription, int billCycle = 1, int billCycleCount = 1)
        {
            if (_apiDataSetted == false)
                return;
            //Controlamos los datos
            if (billCycle <= 0)
                billCycle = 1;
            else if (billCycle > 6)
                billCycle = 6;

            if (billCycleCount <= 0)
                billCycleCount = 1;

            //Componemos la url de consulta
            string rest_url = _simyoBaseUrlConstant + "consumptionByCycle/" + customerId;
            rest_url = this.addGetParameterToUrl(rest_url, "sessionId", sessionId);
            rest_url = this.addGetParameterToUrl(rest_url, "publicKey", _publicKey);
            rest_url = this.addGetParameterToUrl(rest_url, "msisdn", subscription.Msisdn);
            rest_url = this.addGetParameterToUrl(rest_url, "billCycleType", Convert.ToString(subscription.BillCycleType));
            rest_url = this.addGetParameterToUrl(rest_url, "registerDate", Convert.ToString(subscription.RegisterDate));
            rest_url = this.addGetParameterToUrl(rest_url, "billCycle", Convert.ToString(billCycle));
            rest_url = this.addGetParameterToUrl(rest_url, "billCycleCount", Convert.ToString(billCycleCount));
            rest_url = this.addGetParameterToUrl(rest_url, "payType", subscription.PayType);
            //Solicitamos la url firmada
            rest_url = this.addSigToUrl(rest_url);

            //Montamos el objeto Rest
            var client = new RestClient(rest_url); //Creamos el cliente
            var request = new RestRequest(RestSharp.Method.GET); //Creamos la consulta

            //Hacemos la llamada
            client.ExecuteAsync(request, (response, handler) =>
            {
                //Analizamos el resultado
                string content = response.Content;
                ConsumptionResponse sub_response = new ConsumptionResponse(content);
                //Según la respuesta, determinamos a qué evento hay que llamar
                if (sub_response.Success == true)
                {
                    ConsumptionSuccess(sub_response);
                }
                else
                {
                    ConsumptionError(sub_response);
                }
            });
        }

        #region Tratamiento de Urls

        /// <summary>
        /// Devuelve la url de la consulta con la firma añadida
        /// </summary>
        /// <param name="url">url base</param>
        /// <returns>url con la firma</returns>
        private string addSigToUrl(string url)
        {
            string api_sig = this.getHmacSha256(url);
            string url_final = this.addGetParameterToUrl(url, "apiSig", api_sig);
            return url_final;
        }        

        /// <summary>
        /// Añade un valor get a la url
        /// </summary>
        /// <param name="url">Url base</param>
        /// <param name="name">Nombre de la clave</param>
        /// <param name="value">Valor a añadir</param>
        /// <returns>Url resultante</returns>
        private string addGetParameterToUrl(string url, string name, string value)
        {
            //Determinamos el caracter a concatenar
            char character_to_add;
            if (url.Contains('?'))
                character_to_add = '&';
            else
                character_to_add = '?';

            //Concatenamos la información
            url = url + character_to_add + name + '=' + value;
            return url;
        }

        #endregion

        #region Criptografía

        /// <summary>
        /// Calcula el código HmacSha256 empleado para la firma
        /// </summary>
        /// <param name="url">url base</param>
        /// <returns>Código para la firma</returns>
        private string getHmacSha256(string url)
        {
            string url_to_encode = _privateKey + url;           //Añadimos la clave privada al inicio
            url_to_encode = url_to_encode.ToLower();                      //Pasamos a minúsculas
            //Codificamos en SHA256
            Byte[] private_key_bytes = Encoding.UTF8.GetBytes(SimyoApi._privateKey);
            var hashmanager = new System.Security.Cryptography.HMACSHA256(private_key_bytes); //Creamos el cifrador partiendo de la clave privada en bytes
            byte[] sha256hash = hashmanager.ComputeHash(Encoding.UTF8.GetBytes(url_to_encode)); //Obtenemos los bytes de la url codificada
            string sha256hash_hex = BitConverter.ToString(sha256hash).Replace("-", String.Empty).ToLower();  //Los pasamos a hexadecimal
            return sha256hash_hex;
        }

        /// <summary>
        /// Cifra la cadena facilitada empleando el algoritmo de 3des
        /// </summary>
        /// <param name="text">texto a cifrar</param>
        /// <returns>Cadena cifrada y codificada en base64</returns>
        private string get3desCypher(string text)
        {
            var cipher = CipherUtilities.GetCipher("DesEde"); //Creamos el cifrador
            byte[] byte_key = Encoding.UTF8.GetBytes(SimyoApi._desedeKey); //Preparamos la clave (que mania en .net con hacerlo todo por bytes)
            var param_key = new DesEdeParameters(byte_key); //Añadimos la clave de cifrado
            byte[] data = Encoding.UTF8.GetBytes(text); //Pasamos el texto a array de bytes para poder procesarlo
            cipher.Init(true, param_key); //Arrancamos el motor del cifrador
            byte[] data_encrypted = cipher.DoFinal(data); //Ciframos
            string string_encrypted_b64 = Convert.ToBase64String(data_encrypted); //Convertimos el resultado en base64
            string string_encrypted = Uri.EscapeUriString(string_encrypted_b64);  //Escapamos los caracteres para Uri por si acaso
            return string_encrypted;
        }

        #endregion

        #endregion
    }
}
