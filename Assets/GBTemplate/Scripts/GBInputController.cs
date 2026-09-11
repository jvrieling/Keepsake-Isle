using UnityEngine;

namespace GBTemplate
{
    public class GBInputController : MonoBehaviour
    {
        public float DeadZone = 0.01f;
        public bool Up;
        public bool UpJustPressed;
        public float UpPressedTime;
        public bool Down;
        public bool DownJustPressed;
        public float DownPressedTime;
        public bool Left;
        public bool LeftJustPressed;
        public float LeftPressedTime;
        public bool Right;
        public bool RightJustPressed;
        public float RightPressedTime;
        public bool ButtonA;
        public bool ButtonAJustPressed;
        public bool ButtonAJustReleased;
        public float ButtonAPressedTime;
        public bool ButtonB;
        public bool ButtonBJustPressed;
        public bool ButtonBJustReleased;
        public float ButtonBPressedTime;
        public bool ButtonSelect;
        public bool ButtonSelectJustPressed;
        public float ButtonSelectPressedTime;
        public bool ButtonStart;
        public bool ButtonStartJustPressed;
        public float ButtonStartPressedTime;

        private InputMaster m_Controls;

        public void Awake()
        {
            m_Controls = new InputMaster();
        }

        public void OnEnable()
        {
            m_Controls.Enable();
        }

        public void OnDisable()
        {
            m_Controls.Disable();
        }

        void Update()
        {
            //Update times
            if (!Up)
            {
                UpPressedTime = 0;
            }
            else
            {
                UpPressedTime += Time.unscaledDeltaTime;
            }
            if (!Down)
            {
                DownPressedTime = 0;
            }
            else
            {
                DownPressedTime += Time.unscaledDeltaTime;
            }
            if (!Left)
            {
                LeftPressedTime = 0;
            }
            else
            {
                LeftPressedTime += Time.unscaledDeltaTime;
            }
            if (!Right)
            {
                RightPressedTime = 0;
            }
            else
            {
                RightPressedTime += Time.unscaledDeltaTime;
            }
            if (!ButtonA)
            {
                ButtonAJustReleased = (ButtonAPressedTime > 0);
                ButtonAPressedTime = 0;
            }
            else
            {
                ButtonAPressedTime += Time.unscaledDeltaTime;
            }
            if (!ButtonB)
            {
                ButtonBJustReleased = (ButtonBPressedTime > 0);
                ButtonBPressedTime = 0;
            }
            else
            {
                ButtonBPressedTime += Time.unscaledDeltaTime;
            }
            if (!ButtonSelect)
            {
                ButtonSelectPressedTime = 0;
            }
            else
            {
                ButtonSelectPressedTime += Time.unscaledDeltaTime;
            }
            if (!ButtonStart)
            {
                ButtonStartPressedTime = 0;
            }
            else
            {
                ButtonStartPressedTime += Time.unscaledDeltaTime;
            }

            //Update buttons
            ButtonA = (m_Controls.Player.A.ReadValue<float>() > 0);
            ButtonAJustPressed = m_Controls.Player.A.triggered;
            ButtonB = (m_Controls.Player.B.ReadValue<float>() > 0);
            ButtonBJustPressed = m_Controls.Player.B.triggered;
            ButtonSelect = (m_Controls.Player.Select.ReadValue<float>() > 0);
            ButtonSelectJustPressed = m_Controls.Player.Select.triggered;
            ButtonStart = (m_Controls.Player.Start.ReadValue<float>() > 0);
            ButtonStartJustPressed = m_Controls.Player.Start.triggered;

            //Update directions
            if (Left == false)
            {
                Left = (m_Controls.Player.Move.ReadValue<Vector2>().x < -DeadZone);

                if (Left)
                {
                    LeftJustPressed = true;
                }
            }
            else
            {
                Left = (m_Controls.Player.Move.ReadValue<Vector2>().x < -DeadZone);

                LeftJustPressed = false;
            }

            if (Right == false)
            {
                Right = (m_Controls.Player.Move.ReadValue<Vector2>().x > DeadZone);

                if (Right)
                {
                    RightJustPressed = true;
                }
            }
            else
            {
                Right = (m_Controls.Player.Move.ReadValue<Vector2>().x > DeadZone);

                RightJustPressed = false;
            }

            if (Up == false)
            {
                Up = (m_Controls.Player.Move.ReadValue<Vector2>().y > DeadZone);

                if (Up)
                {
                    UpJustPressed = true;
                }
            }
            else
            {
                Up = (m_Controls.Player.Move.ReadValue<Vector2>().y > DeadZone);

                UpJustPressed = false;
            }

            if (Down == false)
            {
                Down = (m_Controls.Player.Move.ReadValue<Vector2>().y < -DeadZone);

                if (Down)
                {
                    DownJustPressed = true;
                }
            }
            else
            {
                Down = (m_Controls.Player.Move.ReadValue<Vector2>().y < -DeadZone);

                DownJustPressed = false;
            }
        }
    }
}