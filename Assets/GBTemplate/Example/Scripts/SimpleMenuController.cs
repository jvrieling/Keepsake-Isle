using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBTemplate
{
    public class SimpleMenuController : MonoBehaviour
    {
        public int CurrentOption;
        public eInputType InputType;
        public eMenuState State;
        public float InitializationTime = 0.25f;
        public int DefaultOption;
        public bool DisableOnSelect = true;

        public List<GameObject> MenuHighlights;

        public AudioClip SoundMenuMove;
        public AudioClip SoundMenuSelect;

        public enum eInputType
        {
            Vertical,
            Horizontal
        }

        public enum eMenuState
        {
            Inactive,
            Active,
            Init
        }

        private float initTimer;
        private GBConsoleController gb;

        // Start is called before the first frame update
        void Start()
        {
            gb = GBConsoleController.GetInstance();
        }

        // Update is called once per frame
        void Update()
        {
            if (State == eMenuState.Init)
            {
                if (initTimer > InitializationTime)
                {
                    State = eMenuState.Active;
                    UpdateSelection();
                }

                initTimer += Time.unscaledDeltaTime;
            }

            if (State == eMenuState.Active)
            {
                if (InputType == eInputType.Vertical)
                {
                    if (gb.Input.DownJustPressed)
                    {
                        OptionAdd();
                    }

                    if (gb.Input.UpJustPressed)
                    {
                        OptionSub();
                    }
                }
                else
                {
                    if (gb.Input.RightJustPressed)
                    {
                        OptionAdd();
                    }

                    if (gb.Input.LeftJustPressed)
                    {
                        OptionSub();
                    }
                }

                if (gb.Input.ButtonAJustPressed || gb.Input.ButtonBJustPressed || gb.Input.ButtonStartJustPressed)
                {
                    MenuSelect();
                }
            }

            
        }

        public void InitMenu()
        {
            initTimer = 0;
            State = eMenuState.Init;
            CurrentOption = DefaultOption;
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            foreach (GameObject obj in MenuHighlights)
            {
                obj.SetActive(false);
            }

            MenuHighlights[CurrentOption].SetActive(true);
        }

        private void OptionAdd()
        {
            CurrentOption++;

            if (CurrentOption > MenuHighlights.Count - 1)
            {
                CurrentOption = 0;
            }

            MenuMoveCursor();
        }

        private void OptionSub()
        {
            CurrentOption--;

            if (CurrentOption < 0)
            {
                CurrentOption = MenuHighlights.Count - 1;
            }

            MenuMoveCursor();
        }

        private void OnEnable()
        {
            InitMenu();
        }

        //EVENTS
        public event Action onMenuSelect;
        public void MenuSelect()
        {
            if (onMenuSelect != null)
            {
                onMenuSelect();
            }

            if (DisableOnSelect)
            {
                State = eMenuState.Inactive;
            }

            if (SoundMenuSelect != null)
            {
                gb.Sound.PlaySound(SoundMenuSelect);
            }
        }

        public event Action onMenuMoveCursor;
        public void MenuMoveCursor()
        {
            if (onMenuMoveCursor != null)
            {
                onMenuMoveCursor();
            }

            UpdateSelection();
            if (SoundMenuMove != null)
            {
                gb.Sound.PlaySound(SoundMenuMove);
            }
        }
    }
}