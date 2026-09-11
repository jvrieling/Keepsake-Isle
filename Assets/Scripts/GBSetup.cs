using GBTemplate;
using UnityEngine;

public class GBSetup : MonoBehaviour
{
    private GBConsoleController gb;
    void Start()
    {
        gb = GBConsoleController.GetInstance();
    }

    void Update()
    {
        if (gb.Input.ButtonAJustPressed)
        {
            Debug.Log("A!!");
        }
    }
}
