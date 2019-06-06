using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Booki.Helper
{
    public static class JsonHelper
    {
        //public static string Serialize2Json<T>(this List<T> lista)
        //{
        //    var json = JsonConvert.SerializeObject(lista, typeof (T), Formatting.Indented, new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        //        PreserveReferencesHandling = PreserveReferencesHandling.Objects
        //    });

        //    return json;
        //}

        public static string Serialize2Json<T>(this T objecto)
        {
            return Serialize2JsonBase(objecto, true, false, true);
        }

        public static string Serialize2Json<T>(this T objecto, bool showReferenceId)
        {
            return Serialize2JsonBase(objecto, showReferenceId, false, true);
        }

        public static string Serialize2Json<T>(this T objecto, bool showReferenceId, bool typeNameHandling)
        {
            return Serialize2JsonBase(objecto, showReferenceId, typeNameHandling, true);
        }

        public static string Serialize2Json<T>(this T objecto, bool showReferenceId, bool typeNameHandling, bool indented)
        {
            return Serialize2JsonBase(objecto, showReferenceId, typeNameHandling, indented);
        }

        private static string Serialize2JsonBase<T>(this T objecto, bool showReferenceId, bool typeNameHandling, bool indented)
        {
            var jsonSetting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = showReferenceId ? PreserveReferencesHandling.Objects : PreserveReferencesHandling.None,

                TypeNameHandling = typeNameHandling ? TypeNameHandling.All : TypeNameHandling.None

            };

            var json = JsonConvert.SerializeObject(objecto, typeof(T), (indented ? Formatting.Indented : Formatting.None), jsonSetting);

            return json;
        }




        public static T DeserializeFromJson<T>(this string objecto)
        {
            return objecto.DeserializeFromJsonBase<T>(true, false);
        }

        public static T DeserializeFromJson<T>(this string objecto, bool typeNameHandling)
        {
            return objecto.DeserializeFromJsonBase<T>(true, typeNameHandling);
        }

        private static T DeserializeFromJsonBase<T>(this string objecto, bool showReferenceId, bool typeNameHandling)
        {
            if (objecto == null)
                objecto = "[]";

            if (objecto == "[]")
            {
                if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) return default(T);

                if (!typeof(T).IsGenericType) return default(T);

                var template = typeof(T).GetGenericTypeDefinition();
                if (template == typeof(IEnumerable<>))
                {
                    return
                        (T)
                            Activator.CreateInstance(
                                typeof(Enumerable).MakeGenericType(typeof(T).GetGenericArguments()));
                }
                if (template == typeof(IList<>))
                {
                    return
                        (T)
                            Activator.CreateInstance(
                                typeof(List<>).MakeGenericType(typeof(T).GetGenericArguments()));
                }
                if (template == typeof(List<>))
                {
                    return
                        (T)
                            Activator.CreateInstance(
                                typeof(List<>).MakeGenericType(typeof(T).GetGenericArguments()));
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(objecto, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = showReferenceId ? PreserveReferencesHandling.Objects : PreserveReferencesHandling.None,
                    TypeNameHandling = typeNameHandling ? TypeNameHandling.All : TypeNameHandling.None
                });
            }

            return default(T);
        }



    }
}

