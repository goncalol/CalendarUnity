using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Messages
{
    public static class JsonHelper
    {
        public static T[] FromJsonArray<T>(string json)
        {
            var concated = "{ \"Items\": " + json + " }";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(concated);
            return wrapper.Items;
        }

        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
