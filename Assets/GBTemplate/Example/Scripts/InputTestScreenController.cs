using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GBTemplate
{
    public class InputTestScreenController : MonoBehaviour
    {
        public Image imageUp;
        public Image imageDown;
        public Image imageLeft;
        public Image imageRight;
        public Image imageSelect;
        public Image imageStart;
        public Image imageA;
        public Image imageB;
        public Color inputColorInactive;
        public Color inputColorActive;

        public UnityEvent onHoldSelect;

        private GBConsoleController gb;

        // Start is called before the first frame update
        void Start()
        {
            //Getting the instance of the console controller, so we can access its functions
            gb = GBConsoleController.GetInstance();
        }

        // Update is called once per frame
        void Update()
        {
            //Gamepad buttons update
            imageUp.color = gb.Input.Up ? inputColorActive : inputColorInactive;
            imageDown.color = gb.Input.Down ? inputColorActive : inputColorInactive;
            imageLeft.color = gb.Input.Left ? inputColorActive : inputColorInactive;
            imageRight.color = gb.Input.Right ? inputColorActive : inputColorInactive;
            imageSelect.color = gb.Input.ButtonSelect ? inputColorActive : inputColorInactive;
            imageStart.color = gb.Input.ButtonStart ? inputColorActive : inputColorInactive;
            imageA.color = gb.Input.ButtonA ? inputColorActive : inputColorInactive;
            imageB.color = gb.Input.ButtonB ? inputColorActive : inputColorInactive;

            //Hold select to exit
            if (gb.Input.ButtonSelectPressedTime > 2)
            {
                onHoldSelect.Invoke();
            }
        }
    }
}