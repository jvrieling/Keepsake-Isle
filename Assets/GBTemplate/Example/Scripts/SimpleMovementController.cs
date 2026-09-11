using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBTemplate
{
    public class SimpleMovementController : MonoBehaviour
    {
        public float MoveSpeed;
        public float PositionLimitX;
        public float PositionLimitY;

        private GBConsoleController gb;

        // Start is called before the first frame update
        void Start()
        {
            gb = GBConsoleController.GetInstance();
        }

        // Update is called once per frame
        void Update()
        {
            float MoveX = 0;
            float MoveY = 0;

            //Movement
            if (gb.Input.Up && !gb.Input.Down)
            {
                MoveY = MoveSpeed;
            }
            if (gb.Input.Down && !gb.Input.Up)
            {
                MoveY = -MoveSpeed;
            }
            if (gb.Input.Right && !gb.Input.Left)
            {
                MoveX = MoveSpeed;
            }
            if (gb.Input.Left && !gb.Input.Right)
            {
                MoveX = -MoveSpeed;
            }

            Vector3 movement = new Vector3(MoveX * Time.deltaTime, MoveY * Time.deltaTime);
            transform.localPosition += movement;
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -PositionLimitX, PositionLimitX), Mathf.Clamp(transform.localPosition.y, -PositionLimitY, PositionLimitY));
        }
    }
}