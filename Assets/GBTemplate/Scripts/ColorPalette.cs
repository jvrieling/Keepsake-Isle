using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GBTemplate/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    public Color Darkest;
    public Color Dark;
    public Color Light;
    public Color Lightest;
}
