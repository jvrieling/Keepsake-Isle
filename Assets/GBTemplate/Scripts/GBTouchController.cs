using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GBTemplate
{
    public class GBTouchController : MonoBehaviour
    {
        public bool usingTouchControls;
        public GameObject touchControlsGroup;
        public Image touchControlImgUp;
        public Image touchControlImgDown;
        public Image touchControlImgLeft;
        public Image touchControlImgRight;
        public Image touchControlImgA;
        public Image touchControlImgB;
        public Image touchControlImgSelect;
        public Image touchControlImgStart;
        public List<Image> touchControlBGObjects;
        public Button touchButtonColor;
        public Button touchButtonVolume;
        public Sprite sprButtonVolumeOn;
        public Sprite sprButtonVolumeOff;

        private GBConsoleController gb;
        
        // Start is called before the first frame update
        void Start()
        {
            //Getting the instance of the console controller, so we can access its functions
            gb = GBConsoleController.GetInstance();
      
            usingTouchControls = Application.isMobilePlatform;
        }

        // Update is called once per frame
        void Update()
        {
            if (usingTouchControls)
            {
                touchControlsGroup.SetActive(true);
                UpdateTouchControls();
                UpdateTopButtons();
            }
            else
            {
                touchControlsGroup.SetActive(false);
            }
        }

        private void UpdateTouchControls()
        {
            //Get colors from the currently selected palette
            Color touchColorInactive = gb.Display.Palettes[gb.Display.CurrentPalette].Dark;
            touchColorInactive.a = 1;
            Color touchColorActive = gb.Display.Palettes[gb.Display.CurrentPalette].Lightest;
            touchColorActive.a = 1;

            //Set Background Objects Color
            foreach (Image img in touchControlBGObjects)
            {
                img.color = touchColorInactive;
            }

            //Set active colors for pressed inputs
            touchControlImgUp.color = gb.Input.Up ? touchColorActive : touchColorInactive;
            touchControlImgDown.color = gb.Input.Down ? touchColorActive : touchColorInactive;
            touchControlImgLeft.color = gb.Input.Left ? touchColorActive : touchColorInactive;
            touchControlImgRight.color = gb.Input.Right ? touchColorActive : touchColorInactive;
            touchControlImgSelect.color = gb.Input.ButtonSelect ? touchColorActive : touchColorInactive;
            touchControlImgStart.color = gb.Input.ButtonStart ? touchColorActive : touchColorInactive;
            touchControlImgA.color = gb.Input.ButtonA ? touchColorActive : touchColorInactive;
            touchControlImgB.color = gb.Input.ButtonB ? touchColorActive : touchColorInactive;
        }

        public void CycleColorPalette()
        {
            gb.Display.PaletteCycleNext();
        }

        private void UpdateTopButtons()
        {
            //Get colors from the currently selected palette
            Color touchColorInactive = gb.Display.Palettes[gb.Display.CurrentPalette].Dark;
            touchColorInactive.a = 1;
            Color touchColorActive = gb.Display.Palettes[gb.Display.CurrentPalette].Lightest;
            touchColorActive.a = 1;

            ColorBlock colBlock = new ColorBlock();
            colBlock.normalColor = touchColorInactive;
            colBlock.pressedColor = touchColorActive;
            colBlock.highlightedColor = touchColorInactive;
            colBlock.selectedColor = touchColorInactive;
            colBlock.colorMultiplier = 1;
            colBlock.fadeDuration = 0;
            
            touchButtonColor.colors = colBlock;
            touchButtonVolume.colors = colBlock;
        }

        public void ToggleAudioVolume()
        {
            if (gb.Sound.GlobalAudioVolume > 0)
            {
                //Mute
                gb.Sound.UpdateGlobalVolume(0);
            }
            else
            {
                //Unmute
                gb.Sound.UpdateGlobalVolume(1);
            }

            //Update button image
            ((Image)touchButtonVolume.targetGraphic).sprite = gb.Sound.GlobalAudioVolume > 0 ? sprButtonVolumeOn : sprButtonVolumeOff;
        }
    }
}