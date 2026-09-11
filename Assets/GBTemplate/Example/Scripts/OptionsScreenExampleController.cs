using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace GBTemplate
{
    public class OptionsScreenExampleController : MonoBehaviour
    {
        /* 0 - COLOR PALETTE
         * 1 - SOUND VOLUME
         * 2 - MUSIC VOLUME
         * 3 - EXIT
         */

        public TextMeshProUGUI textColorValue;
        public TextMeshProUGUI textSoundValue;
        public TextMeshProUGUI textMusicValue;
        public UnityEvent OnMenuBack;

        private GBConsoleController gb;
        private SimpleMenuController menu;
        
        // Start is called before the first frame update
        void Start()
        {
            gb = GBConsoleController.GetInstance();

            menu = GetComponent<SimpleMenuController>();
            menu.onMenuSelect += MenuSelected;

            UpdateTextValues();
        }

        private void OnDestroy()
        {
            menu.onMenuSelect -= MenuSelected;
        }

        // Update is called once per frame
        void Update()
        {
            //Color palette selection
            if (menu.CurrentOption == 0)
            {                
                if (gb.Input.RightJustPressed)
                {
                    gb.Display.PaletteCycleNext();
                    UpdateTextValues();
                }

                if (gb.Input.LeftJustPressed)
                {
                    gb.Display.PaletteCyclePrev();
                    UpdateTextValues();
                }
            }

            //Sound controller, sound effects volume
            if (menu.CurrentOption == 1)
            {
                if (gb.Input.RightJustPressed)
                {
                    gb.Sound.CurrentSoundVolume += 0.1f;
                    gb.Sound.CurrentSoundVolume = Mathf.Clamp01(gb.Sound.CurrentSoundVolume);

                    gb.Sound.UpdateSoundVolume(gb.Sound.CurrentSoundVolume);
                    UpdateTextValues();
                }

                if (gb.Input.LeftJustPressed)
                {
                    gb.Sound.CurrentSoundVolume -= 0.1f;
                    gb.Sound.CurrentSoundVolume = Mathf.Clamp01(gb.Sound.CurrentSoundVolume);

                    gb.Sound.UpdateSoundVolume(gb.Sound.CurrentSoundVolume);
                    UpdateTextValues();
                }
            }

            //Sound controller, music volume
            if (menu.CurrentOption == 2)
            {
                if (gb.Input.RightJustPressed)
                {
                    gb.Sound.CurrentMusicVolume += 0.1f;
                    gb.Sound.CurrentMusicVolume = Mathf.Clamp01(gb.Sound.CurrentMusicVolume);

                    gb.Sound.UpdateMusicVolume(gb.Sound.CurrentMusicVolume);
                    UpdateTextValues();
                }

                if (gb.Input.LeftJustPressed)
                {
                    gb.Sound.CurrentMusicVolume -= 0.1f;
                    gb.Sound.CurrentMusicVolume = Mathf.Clamp01(gb.Sound.CurrentMusicVolume);

                    gb.Sound.UpdateMusicVolume(gb.Sound.CurrentMusicVolume);
                    UpdateTextValues();
                }
            }
        }

        private void UpdateTextValues()
        {
            textColorValue.text = gb.Display.CurrentPalette.ToString("00");
            textSoundValue.text = (gb.Sound.CurrentSoundVolume * 10).ToString("00");
            textMusicValue.text = (gb.Sound.CurrentMusicVolume * 10).ToString("00");
        }

        private void MenuSelected()
        {
            if (menu.CurrentOption == 3)
            {
                OnMenuBack.Invoke();
            }
        }
    }
}