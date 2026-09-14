using System;
using UnityEngine;

namespace ChickenCoop.Util
{
    public static class VectorExtensions
    {
        public static Vector2Int Vector2Int(this Vector3Int v3)
        {
            return new Vector2Int(v3.x, v3.y);
        }

        public static Vector3Int Vector3Int(this Vector2Int v2)
        {
            return new Vector3Int(v2.x, v2.y, 0);
        }

        public static Vector3Int RoundToInt(this Vector3 v3)
        {
            return new Vector3Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y), Mathf.RoundToInt(v3.z));
        }

        public static Vector3Int RoundToInt(this Vector2 v2)
        {
            return new Vector3Int(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y), 0);
        }

        public static byte[] Vector2ToBytes(this Vector2Int vector)
        {
            byte[] bytes = new byte[8];
            BitConverter.GetBytes(vector.x).CopyTo(bytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(bytes, 4);
            return bytes;
        }

        public static Vector2Int BytesToVector2(this byte[] bytes)
        {
            int x = BitConverter.ToInt32(bytes, 0);
            int y = BitConverter.ToInt32(bytes, 4);
            return new Vector2Int(x, y);
        }
    }
}