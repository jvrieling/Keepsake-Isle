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

    void Update()
    {
        if (gb.Input.ButtonAJustPressed)
        {
            Debug.Log("A!!");
        }

        if (gb.Input.ButtonStartJustPressed)
        {
            Debug.Log("!!Start!!");
        }
    }
}
