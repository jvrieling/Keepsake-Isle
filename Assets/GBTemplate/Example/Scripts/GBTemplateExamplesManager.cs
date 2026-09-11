using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBTemplate
{
    public class GBTemplateExamplesManager : MonoBehaviour
    {
        public eExampleType CurrentExample;

        public GameObject MainMenuCanvas;
        public GameObject InputTestGroup;
        public GameObject InputTestCanvas;
        public GameObject OptionsMenuCanvas;
        public GameObject ConfirmMenuCanvas;
        public DialogBoxController DialogBox;
        public List<DialogData> DialogList;
        public GameObject FXGroup;
        public AudioClip ExampleMusic;

        private SimpleMenuController mainMenu;
        private bool whiteFade = false;

        public enum eExampleType
        {
            MainMenu,
            InputTest,
            OptionsMenu,
            ConfirmMenu,
            DialogExample
        }

        private GBConsoleController gb;

        // Start is called before the first frame update
        void Start()
        {
            gb = GBConsoleController.GetInstance();

            mainMenu = MainMenuCanvas.GetComponent<SimpleMenuController>();
            mainMenu.onMenuSelect += MainMenuSelection;

            DialogBox.onDialogComplete += DialogJustFinished;

            SetCurrentExample(-1);

            //Have some music playing!
            gb.Sound.PlayMusic(ExampleMusic);
        }

        private void OnDestroy()
        {
            mainMenu.onMenuSelect -= MainMenuSelection;
            DialogBox.onDialogComplete -= DialogJustFinished;
        }

        private void MainMenuSelection()
        {
            //A button press from the main menu was detected,
            //let's set the example based on the option index of the menu
            //... unless it's the last option:
            if (mainMenu.CurrentOption == 4)
            {
                StartCoroutine(FadeTest());
                whiteFade = !whiteFade;
            }
            else
            {
                SetCurrentExample(mainMenu.CurrentOption);
            }
        }

        public void SetCurrentExample(int value)
        {
            MainMenuCanvas.SetActive(false);
            InputTestGroup.SetActive(false);
            InputTestCanvas.SetActive(false);
            OptionsMenuCanvas.SetActive(false);
            ConfirmMenuCanvas.SetActive(false);

            switch (value)
            {
                case -1: MainMenuCanvas.SetActive(true);
                    break;
                case 0: InputTestGroup.SetActive(true);
                    InputTestCanvas.SetActive(true);
                    break;
                case 1: OptionsMenuCanvas.SetActive(true);
                    break;
                case 2: ConfirmMenuCanvas.SetActive(true);
                    break;
                case 3: ShowRandomExampleDialog();
                    break;
            }
        }

        private void ShowRandomExampleDialog()
        {
            int rnd = Random.Range(0, DialogList.Count);
            DialogBox.gameObject.SetActive(true);
            DialogBox.ShowDialog(DialogList[rnd]);
        }

        private void DialogJustFinished()
        {
            DialogBox.gameObject.SetActive(false);
            SetCurrentExample(-1); //Back to the Main Menu
        }

        public IEnumerator FadeTest()
        {
            if (whiteFade)
            {
                yield return gb.Display.StartCoroutine(gb.Display.FadeToWhite(2));
                
                //Insert you action / scene transition here,
                //in this case we rotate the starfield in the background
                RotateFXGroup();

                yield return gb.Display.StartCoroutine(gb.Display.FadeFromWhite(2));

                mainMenu.State = SimpleMenuController.eMenuState.Active;
            }
            else
            {
                yield return gb.Display.StartCoroutine(gb.Display.FadeToBlack(2));

                //Insert you action / scene transition here,
                //in this case we rotate the starfield in the background
                RotateFXGroup();

                yield return gb.Display.StartCoroutine(gb.Display.FadeFromBlack(2));

                mainMenu.State = SimpleMenuController.eMenuState.Active;
            }
        }

        private void RotateFXGroup()
        {
            FXGroup.transform.Rotate(new Vector3(0, 0, 90));
        }
    }
}