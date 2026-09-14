using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChickenCoop.Util
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }
            int randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }
    }
}