﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace WhaleLand.Extensions.Redis
{
    static class JsonHelper
    {
        /// <summary>
        /// 将对象转为json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isDateFomat"></param>
        /// <param name="dateFomat"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return ToJson(obj, false, string.Empty);
        }

        /// <summary>
        /// 将对象转为json格式字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isDateFomat"></param>
        /// <param name="dateFomat"></param>
        /// <returns></returns>
        public static string ToJson(object obj, bool isDateFomat, string dateFomat)
        {
            if (obj == null) return string.Empty;
            IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter()
            {
                DateTimeFormat = (isDateFomat && !string.IsNullOrEmpty(dateFomat)) ? dateFomat : "yyyy-MM-dd HH:mm:ss"
            };
            JsonConverter[] converter = new JsonConverter[] { dateTimeConverter };
            return JsonConvert.SerializeObject(obj, converter);
        }

        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static T FromJsonSafe<T>(this string json, T defValue = default(T))
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return defValue;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return defValue;
            }
        }
    }
}