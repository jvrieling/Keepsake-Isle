using GBTemplate;
using UnityEngine;

public class GBSetup : MonoBehaviour
{
    private GBConsoleController gb;
    void Start()
    {
        gb = GBConsoleController.GetInstance();

        gb.Display.UpdateColorPalette(0);
    }
}
