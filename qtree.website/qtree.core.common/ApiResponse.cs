using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace qtree.core.common
{
    public interface IResponseApi<T>
    {
        string Status { get; set; }
        string Code { get; set; }
        bool Error { get; set; }
        T[] Data { get; }
    }
    public static class QtreeExtensions
    {
        public static string Serialize<TEntity>(this IResponseApi<TEntity> response)
        {
            return JsonConvert.SerializeObject(response);
        }
    }

    public class ResponseApi<T> : IResponseApi<T>
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        /// <summary>
        /// Data is always returned as an array (iEnumerable)
        /// </summary>
        [JsonProperty("data")]
        public T[] Data
        {
            get
            {
                if (_data == null)
                    return new T[0];

                return (typeof(IEnumerable<T>).IsAssignableFrom(typeof(T)) ?
                            _data as T[] :
                            new T[] { _data });

            }
        }
        private T _data { get; set; }
        public string Result { get; internal set; }

        public ResponseApi(HttpStatusCode pStatusCode = HttpStatusCode.OK, bool pError = false, object dataObject = null)
        {
            Status = $"{pStatusCode}";
            Code = $"{(int)pStatusCode}";
            Error = pError;

            try
            {
                if (dataObject != null)
                {
                    _data = (T)dataObject;
                }
            }
            catch (MissingMethodException)
            {
                // do nothing the data object can be without any instanciated element
            }
        }
    }

    /// <summary>
    /// ResponseManager - Handle responses and transform them into  response format    
    /// </summary>
    public static class ApiResponse
    {
        private static string ErrorMessageSomethingWentWrong = "";

        private static bool isErrorResponse(HttpStatusCode statusCode)
        {
            return ((int)statusCode) > 300;
        }

        /// <summary>
        /// ResponseManager.Empty > no dataobject to wrap
        /// Deafault : 
        ///     message = "no data" 
        ///     error = "true"
        ///     status = "NotFound"
        ///     code = 404
        /// </summary>
        /// <returns></returns>
        public static IResponseApi<T> Empty<T>(HttpStatusCode statusCode = HttpStatusCode.NotFound, string message = "no data")
        {
            return new ResponseApi<T>(statusCode, isErrorResponse(statusCode), message);
        }
        public static IResponseApi<T> Empty<T>(string message)
        {
            return Empty<T>(HttpStatusCode.NotFound, message);

        }

        /// <summary>
        /// ResponseManager.DataResponse > map a dataobject that is retrieved from services to the "data" object in the response
        /// Default:
        ///     error = false
        ///     status = HttpStatusCode.OK
        ///     code = 200
        ///     data = Mapped(dataObject)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResponseApi<T> DataResponse<T>(T dataObject, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "")
        {
            return DataResponse(dataObject, message, statusCode);
        }
        public static IResponseApi<T> DataResponse<T>(T dataObject, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var isError = isErrorResponse(statusCode);
            if ((dataObject is String) && !string.IsNullOrEmpty(message))
            {
                //.LogError($"Error occured on API call, error:{message}");
                message = $"{dataObject}";
            }
            return new ResponseApi<T>(statusCode, isError, dataObject);
        }

        /// <summary>
        /// ResponseManager.Warning >> Response where the dataobject is not returned to the user    
        /// Deafault:
        ///     message = "Deze actie is niet toegstaan."
        ///     error = "true"
        ///     status = HttpStatusCode.MethodNotAllowed
        ///     code = 405
        ///     data = 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IResponseApi<T> Warning<T>(HttpStatusCode statusCode = HttpStatusCode.MethodNotAllowed, string message = "This action is not allowed.")
        {
            return Empty<T>(statusCode, message);
        }

        /// <summary>
        /// ResponseManager.Error >> Response where the dataobject is not returned to the user
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject">the exception or any other dataobject that needs to be in the data object for the client</param>
        /// <param name="statusCode">default InternalServerError</param>
        /// <param name="message">when provided this will be the message
        /// when not provided and the dataobject is a string, the message will be the value of the dataobject</param>
        /// when an exception is passed    
        /// <returns>Response object where data object is absent (not initialized)</returns>
        public static IResponseApi<T> Error<T>(T dataObject, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string message = "")
        {
            return new ResponseApi<T>(statusCode, true, dataObject);
        }

        public static IResponseApi<T> Error<T>(T dataObject, string message)
        {
            return Error(dataObject, HttpStatusCode.InternalServerError, message);
        }
    }


    public class CustomDataContractResolver : DefaultContractResolver
    {
        public static readonly CustomDataContractResolver Instance = new CustomDataContractResolver();

        public CustomDataContractResolver()
        {

        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var Tobject = ApiResponse.Empty<string>(HttpStatusCode.Accepted, "test");
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == Tobject.GetType())
            {
                if (property.PropertyName.Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    property.PropertyName = Tobject.GetType().Name.ToLowerInvariant();
                }
            }
            return property;
        }
    }

}

