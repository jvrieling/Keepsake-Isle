using System.Collections;
using UnityEngine;
using TMPro;
using System;

namespace GBTemplate
{
    public class DialogBoxController : MonoBehaviour
    {
        public bool Active;
        public bool SkipDialog;
        public TMP_Text TMPText;
        public AudioClip TypingSound;

        public float TextSpeed = 15;
        public float FastTextSpeed = 100;

        public float TargetPanelWidth = 152;
        public float PanelAnimSpeed = 1000;
        public RectTransform Panel;
        public GameObject CursorObject;

        private bool speedUpSentence;
        private GBConsoleController gb;

        // Start is called before the first frame update
        void Start()
        {
            gb = GBConsoleController.GetInstance();
            Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            CursorObject.SetActive(false);
            TMPText.text = string.Empty;
        }

        public void ShowDialog(DialogData dialog)
        {
            Active = true;
            SkipDialog = false;
            StartCoroutine(StepThroughDialog(dialog));
        }

        private IEnumerator TypeText(DialogSentence sentence)
        {
            float t = 0;
            int charIndex = 0;
            int lastIndex = 0;

            CursorObject.SetActive(false);

            while (charIndex < sentence.Text.Length)
            {
                if (charIndex > 3 && !speedUpSentence && (gb.Input.ButtonAJustPressed || gb.Input.ButtonBJustPressed || gb.Input.ButtonStartJustPressed))
                {
                    speedUpSentence = true;
                }

                float currentTextSpeed = speedUpSentence ? FastTextSpeed : TextSpeed;

                t += currentTextSpeed * Time.deltaTime;
                charIndex = Mathf.FloorToInt(t);
                charIndex = Mathf.Clamp(charIndex, 0, sentence.Text.Length);

                string text = sentence.Text.Substring(0, charIndex).Replace("#", "\n");
                text += "<color=#00000000>" + sentence.Text.Substring(charIndex).Replace("#", "\n") + "</color>";

                TMPText.text = text;

                if (charIndex != lastIndex && TypingSound != null && !speedUpSentence)
                {
                    if (sentence.TypeSound != null)
                    {
                        gb.Sound.PlaySound(sentence.TypeSound);
                    }
                    else
                    {
                        gb.Sound.PlaySound(TypingSound);
                    }

                    lastIndex = charIndex;
                }

                if (SkipDialog)
                {
                    charIndex = sentence.Text.Length;
                }

                yield return null;
            }

            TMPText.text = sentence.Text.Replace("#", "\n");
            speedUpSentence = false;

            CursorObject.SetActive(true);
        }

        private IEnumerator StepThroughDialog(DialogData data)
        {
            TMPText.text = string.Empty;

            //Open up panel
            float currentWidth = 0;
            while (currentWidth < TargetPanelWidth)
            {
                currentWidth += PanelAnimSpeed * Time.deltaTime;
                currentWidth = Mathf.Clamp(currentWidth, 0, TargetPanelWidth);
                Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
                yield return null;
            }

            //Cycle through sentences
            foreach (DialogSentence sentence in data.Sentences)
            {
                yield return StartCoroutine(TypeText(sentence));
                yield return new WaitUntil(() => (gb.Input.ButtonAJustPressed || gb.Input.ButtonBJustPressed || gb.Input.ButtonStartJustPressed || SkipDialog));
            }

            TMPText.text = string.Empty;
            CursorObject.SetActive(false);

            //Close!
            while (currentWidth > 0)
            {
                currentWidth -= PanelAnimSpeed * Time.deltaTime;
                currentWidth = Mathf.Clamp(currentWidth, 0, TargetPanelWidth);
                Panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth);
                yield return null;
            }

            Active = false;
            DialogComplete();
        }

        //EVENTS
        public event Action onDialogComplete;
        public void DialogComplete()
        {
            if (onDialogComplete != null)
            {
                onDialogComplete();
            }
        }
    }
}