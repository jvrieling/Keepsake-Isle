using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GBTemplate
{
    public class ConfirmExampleController : MonoBehaviour
    {
        public GameObject ConfirmMenuGroup;
        public GameObject ConfirmOKGroup;
        public UnityEvent OnOKSelected;
        public UnityEvent OnCancelSelected;
        public bool OKSelected;
        public AudioClip SoundOK;
        public AudioClip SoundCancel;
        public AudioClip SoundTransition;

        private SimpleMenuController confirmMenu;
        private float messageTimer;
        private GBConsoleController gb;

        // Start is called before the first frame update
        void Start()
        {
            gb = GBConsoleController.GetInstance();

            confirmMenu = ConfirmMenuGroup.GetComponent<SimpleMenuController>();
            confirmMenu.onMenuSelect += MenuSelection;
        }

        private void OnDestroy()
        {
            confirmMenu.onMenuSelect -= MenuSelection;
        }

        private void OnEnable()
        {
            OKSelected = false;
            messageTimer = 0;

            ConfirmMenuGroup.SetActive(true);
            ConfirmOKGroup.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (OKSelected)
            {
                if (messageTimer > 3)
                {
                    OnOKSelected.Invoke();

                    gb.Sound.PlaySound(SoundTransition);
                }

                messageTimer += Time.unscaledDeltaTime;
            }
        }

        private void MenuSelection()
        {
            //0 - OK
            //1 - Cancel

            if (confirmMenu.CurrentOption == 0)
            {
                OKSelected = true;
                ConfirmMenuGroup.SetActive(false);
                ConfirmOKGroup.SetActive(true);

                gb.Sound.PlaySound(SoundOK);
            }
            else
            {
                OnCancelSelected.Invoke();

                gb.Sound.PlaySound(SoundCancel);
            }
        }
    }
}