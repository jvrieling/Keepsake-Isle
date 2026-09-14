using UnityEngine;

namespace ChickenCoop.Util
{ 
    public static class ChickenGizmos
    {
        public static void HighlightCell(Vector3Int cell, float alpha = 0.5f)
        {
            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, alpha);
            Gizmos.DrawCube(cell + new Vector3(0.5f, 0.5f, 0), new Vector3(1, 1, 0.1f));
        }
    }
}