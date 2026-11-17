
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PotionScreenEffect : MonoBehaviour
{
    [Header("Screen Overlay")]
    [SerializeField] private Image overlay;

    [Header("Transition Text")]
    [SerializeField] private TMP_Text transitionText;

    [Header("Flash Colors")]
    [SerializeField] private Color firstFlashColor = Color.red;
    [SerializeField] private Color secondFlashColor = new Color(0.5f, 0f, 0.8f);
    [SerializeField] private Color finalFlashColor = Color.black;

    [Header("Text Intervals")]
    [SerializeField] private float firstFadeDuration = 0.15f;
    [SerializeField] private float firstTextDuration = 1.0f;

    [SerializeField] private float secondFadeDuration = 0.2f;
    [SerializeField] private float secondTextDuration = 1.5f;

    [SerializeField] private float thirdFadeDuration = 0.25f;
    [SerializeField] private float thirdTextDuration = 1.5f;

    [SerializeField] private float finalFadeDuration = 1.5f;
    [SerializeField] private float blackScreenDuration = 1.5f;
    [SerializeField] private float revealDuration = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip firstFlashSound;
    [SerializeField] private AudioClip secondFlashSound;
    [SerializeField] private AudioClip thirdFlashSound;
    [SerializeField] private AudioClip finalFlashSound;

    [SerializeField, Range(0f, 1f)]
    private float audioVolume = 1f;

    [Header("Scene Transition")]
    [SerializeField] private string destinationScene = "Distorted Reality";

    [SerializeField] private string destinationSpawnPoint = "DistortedRealitySpawn";

    private void Awake()
    {
        if (overlay == null)
            overlay = GetComponent<Image>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SetAlpha(0f);

        if (transitionText != null)
        {
            transitionText.text = "";
            transitionText.gameObject.SetActive(false);
        }
    }

    public void StartPotionTransition()
    {
        StartCoroutine(PotionTransition());
    }

    private IEnumerator PotionTransition()
    {
        // FIRST FLASH
        SetOverlayColor(firstFlashColor);
        ShowText("Something is wrong...");
        PlaySound(firstFlashSound);

        yield return FadeTo(0.8f, firstFadeDuration);
        yield return new WaitForSeconds(firstTextDuration);

        // SECOND FLASH
        SetOverlayColor(secondFlashColor);
        ShowText("This isn't a cure I'm building...");
        PlaySound(secondFlashSound);

        yield return FadeTo(0.15f, secondFadeDuration);
        yield return new WaitForSeconds(secondTextDuration);

        // THIRD FLASH
        SetOverlayColor(secondFlashColor);
        ShowText("What am I doing?");
        PlaySound(thirdFlashSound);

        yield return FadeTo(0.9f, thirdFadeDuration);
        yield return new WaitForSeconds(thirdTextDuration);

        // FINAL FLASH
        SetOverlayColor(finalFlashColor);
        ShowText("This isn't a disease...");
        PlaySound(finalFlashSound);

        yield return FadeTo(1f, finalFadeDuration);

        // Store destination spawn point
        SceneTransitionData.spawnPointName = destinationSpawnPoint;

        // Immediately stop all transition audio
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Load destination scene
        SceneManager.LoadScene(destinationScene);

        // Wait before revealing the new scene
        yield return new WaitForSeconds(blackScreenDuration);

        HideText();

        yield return FadeTo(0f, revealDuration);
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = overlay.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsed / duration
            );

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetOverlayColor(Color newColor)
    {
        if (overlay == null)
            return;

        Color color = overlay.color;
        color.r = newColor.r;
        color.g = newColor.g;
        color.b = newColor.b;

        overlay.color = color;
    }

    private void SetAlpha(float alpha)
    {
        if (overlay == null)
            return;

        Color color = overlay.color;
        color.a = alpha;
        overlay.color = color;
    }

    private void ShowText(string message)
    {
        if (transitionText == null)
            return;

        transitionText.gameObject.SetActive(true);
        transitionText.text = message;
    }

    private void HideText()
    {
        if (transitionText == null)
            return;

        transitionText.text = "";
        transitionText.gameObject.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.PlayOneShot(clip, audioVolume);
    }
}
