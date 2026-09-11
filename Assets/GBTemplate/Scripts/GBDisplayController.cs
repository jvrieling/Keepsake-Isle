using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GBTemplate
{
    public class GBDisplayController : MonoBehaviour
    {
        public Renderer ScreenRenderer;
        public ColorPalette[] Palettes;
        public int CurrentPalette;
        public bool Fading;

        public void UpdateColorPalette(int palNum)
        {
            ScreenRenderer.material.SetColor("_Darkest", Palettes[palNum].Darkest);
            ScreenRenderer.material.SetColor("_Dark", Palettes[palNum].Dark);
            ScreenRenderer.material.SetColor("_Light", Palettes[palNum].Light);
            ScreenRenderer.material.SetColor("_Lightest", Palettes[palNum].Lightest);
        }

        public void PaletteCycleNext()
        {
            CurrentPalette++;

            if (CurrentPalette > Palettes.Length - 1)
            {
                CurrentPalette = 0;
            }

            UpdateColorPalette(CurrentPalette);
        }

        public void PaletteCyclePrev()
        {
            CurrentPalette--;

            if (CurrentPalette < 0)
            {
                CurrentPalette = Palettes.Length - 1;
            }

            UpdateColorPalette(CurrentPalette);
        }

        public IEnumerator FadeToBlack(float fadeSpeed)
        {
            Fading = true;
            ScreenRenderer.material.SetFloat("_Fade", 0);

            while (Fading)
            {                
                float value = ScreenRenderer.material.GetFloat("_Fade");

                value = Mathf.Min(0, value - (fadeSpeed * Time.deltaTime));
                value = Mathf.Clamp(value, -1, 0);
                Fading = (value > -1);

                ScreenRenderer.material.SetFloat("_Fade", value);

                yield return null;
            }

            yield break;
        }

        public IEnumerator FadeFromBlack(float fadeSpeed)
        {
            Fading = true;
            ScreenRenderer.material.SetFloat("_Fade", -1);

            while (Fading)
            {
                float value = ScreenRenderer.material.GetFloat("_Fade");

                value = Mathf.Max(-1, value + (fadeSpeed * Time.deltaTime));
                value = Mathf.Clamp(value, -1, 0);
                Fading = (value < 0);

                ScreenRenderer.material.SetFloat("_Fade", value);

                yield return null;
            }

            yield break;
        }

        public IEnumerator FadeToWhite(float fadeSpeed)
        {
            Fading = true;
            ScreenRenderer.material.SetFloat("_Fade", 0);

            while (Fading)
            {
                float value = ScreenRenderer.material.GetFloat("_Fade");

                value = Mathf.Max(0, value + (fadeSpeed * Time.deltaTime));
                value = Mathf.Clamp01(value);
                Fading = (value < 1);

                ScreenRenderer.material.SetFloat("_Fade", value);

                yield return null;
            }

            yield break;
        }

        public IEnumerator FadeFromWhite(float fadeSpeed)
        {
            Fading = true;
            ScreenRenderer.material.SetFloat("_Fade", 1);

            while (Fading)
            {
                float value = ScreenRenderer.material.GetFloat("_Fade");

                value = Mathf.Min(1, value - (fadeSpeed * Time.deltaTime));
                value = Mathf.Clamp01(value);
                Fading = (value > 0);

                ScreenRenderer.material.SetFloat("_Fade", value);

                yield return null;
            }

            yield break;
        }
    }
}