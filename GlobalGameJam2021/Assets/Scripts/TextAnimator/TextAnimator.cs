using CharTween;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimator : MonoBehaviour, IPointerClickHandler {

    private const string PAUSE_CHARACTER = "~";

    private enum RevealMode { Character, Word, Sentence }

    public bool EmptyDialog => string.IsNullOrEmpty(dirtyText);

    [SerializeField] private float timePerReveal = 0.01f;
    [SerializeField] private RevealMode revealMode;
    [SerializeField] private CharAnimations.AnimationMode defaultAnimationMode = CharAnimations.AnimationMode.Punch;
    [SerializeField] private bool skipAnimationOnClick = true;
    [SerializeField] private bool playOnAwake;

    private TextMeshProUGUI text;
    private Coroutine showTextRoutine;
    private bool isSkipping;
    private CharAnimations charAnimations;
    private List<Tween> charTweens = new List<Tween>();

    public bool IsPlaying { get; private set; }

    // This is the dirty text that gets passed into the ShowText function, it contains all tags.
    private string dirtyText;
    // This is the result of the ParseText function and is what the animation routine iterates over.
    // It doesnt contain any html/custom tags but does contain break characters.
    private StringStyle[] cleanTextToReveal;
    // This gets shown in the text field and doesnt contain custom tags.
    private List<StringStyle> textFieldStyles = new List<StringStyle>();

    private class StringStyle {

        public string Text;
        public CharAnimations.AnimationMode AnimationMode;
        public float RevealSpeed;
        public int AnimationStrength = 1;

        public StringStyle(string text, CharAnimations.AnimationMode animationMode, float revealSpeed, int animationStrength) {
            Text = text;
            AnimationMode = animationMode;
            RevealSpeed = revealSpeed;
            AnimationStrength = animationStrength;
        }
    }

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();

        CharTweener tweener = text.GetCharTweener();
        charAnimations = new CharAnimations(tweener);

        if (playOnAwake) {
            Show();
        }
    }

    public void Show(Action onComplete = null) {
        ShowText(text.text, onComplete);
    }

    public void ShowText(string text, Action onComplete = null) {
        if (!gameObject.activeInHierarchy || !enabled) { return; }

        if (showTextRoutine != null) {
            StopCoroutine(showTextRoutine);
        }

        if (text != dirtyText) {
            dirtyText = text;
            cleanTextToReveal = EmptyDialog ? new StringStyle[0] : ParseText(dirtyText);
        }

        showTextRoutine = StartCoroutine(ShowText(onComplete));
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!skipAnimationOnClick) { return; }
        Skip();
    }

    public void Skip() {
        isSkipping = true;
    }

    private IEnumerator ShowText(Action onComplete) {
        ResetTextField();

        IsPlaying = true;

        TMP_TextInfo textInfo = text.textInfo;
        yield return new WaitForEndOfFrame();

        int index = 0;
        int consecutiveSameAnimationModes = 0;
        CharAnimations.AnimationMode prevAnimationMode = CharAnimations.AnimationMode.None;
        for (int i = 0; i < cleanTextToReveal.Length; i++) {
            for (int j = 0; j < cleanTextToReveal[i].Text.Length; j++) {
                if (cleanTextToReveal[i].Text == PAUSE_CHARACTER) {
                    continue;
                }

                text.maxVisibleCharacters++;

                CharAnimations.AnimationMode currentAnimationMode = cleanTextToReveal[i].AnimationMode;
                charAnimations.AnimateCharacter(index, currentAnimationMode, consecutiveSameAnimationModes, cleanTextToReveal[i].AnimationStrength, ref charTweens);

                consecutiveSameAnimationModes = currentAnimationMode == prevAnimationMode ? consecutiveSameAnimationModes + 1 : 0;
                prevAnimationMode = cleanTextToReveal[i].AnimationMode;

                index++;
            }

            if (!isSkipping && cleanTextToReveal[i].RevealSpeed > 0) {
                yield return new WaitForSeconds(cleanTextToReveal[i].RevealSpeed);
            }
        }

        onComplete?.Invoke();
        IsPlaying = false;
    }

    private void ResetTextField() {
        text.text = GetCleanedTextFieldText();
        text.maxVisibleCharacters = 0;

        charTweens.ForEach(x => x.SetLoops(0));
        charTweens.ForEach(x => x.Kill(true));
        charTweens.Clear();

        isSkipping = false;
    }

    private StringStyle[] ParseText(string text) {
        string[] words = TextAnimatorHelper.SplitOnCharacters(text, ' ');
        List<StringStyle> styles = new List<StringStyle>();

        CharAnimations.AnimationMode mode = defaultAnimationMode;
        float revealSpeed = timePerReveal;
        int animationStrength = 1;
        float pauseDuration = 0;

        for (int i = 0; i < words.Length; i++) {
            if (TextAnimatorHelper.ContainsOpeningTag(words[i])) {
                string value = TextAnimatorHelper.GetTagValueFromWord(words[i], "<", ">");
                string[] parameters = value.Split(',');

                for (int j = 0; j < parameters.Length; j++) {
                    if (parameters[j].Contains("speed")) {
                        TextAnimatorHelper.TryGetSpeed(parameters[j], ref revealSpeed);
                    } else {
                        TextAnimatorHelper.TryGetAnimationModeAndStrength(parameters[j], ref mode, ref animationStrength);
                    }
                }
            } else if (words[i].Contains("<pause=")) {
                if (TextAnimatorHelper.TryGetPauseDuration(words[i], ref pauseDuration)) {
                    styles.Add(new StringStyle(PAUSE_CHARACTER, CharAnimations.AnimationMode.None, pauseDuration, 1));
                    continue;
                }
            }

            styles.Add(new StringStyle(words[i], mode, revealSpeed, animationStrength));

            if (TextAnimatorHelper.ContainsClosingTag(words[i])) {
                mode = defaultAnimationMode;
                revealSpeed = timePerReveal;
                animationStrength = 1;
            }
        }

        // Remove all html tags and split the words into characters or sentences if needed.
        List<StringStyle> cleanedStyles = new List<StringStyle>();
        textFieldStyles.Clear();
        string prevCleanedStyleWord = "";
        string prevTextFieldStyleWord = "";
        for (int i = 0; i < styles.Count; i++) {
            switch (revealMode) {
                case RevealMode.Character:
                    foreach (string s in TextAnimatorHelper.RemoveAllHtmlTags(styles[i].Text)) {
                        cleanedStyles.Add(new StringStyle(s, styles[i].AnimationMode, styles[i].RevealSpeed, styles[i].AnimationStrength));
                    }

                    foreach (string s in TextAnimatorHelper.RemoveAnimationTags(styles[i].Text)) {
                        textFieldStyles.Add(new StringStyle(s, styles[i].AnimationMode, styles[i].RevealSpeed, styles[i].AnimationStrength));
                    }
                    break;

                case RevealMode.Word:
                    cleanedStyles.Add(new StringStyle(string.Join("", TextAnimatorHelper.RemoveAllHtmlTags(styles[i].Text)), styles[i].AnimationMode, styles[i].RevealSpeed, styles[i].AnimationStrength));
                    textFieldStyles.Add(new StringStyle(string.Join("", TextAnimatorHelper.RemoveAnimationTags(styles[i].Text)), styles[i].AnimationMode, styles[i].RevealSpeed, styles[i].AnimationStrength));
                    break;

                case RevealMode.Sentence:
                    string textWithoutHtml = string.Join("", TextAnimatorHelper.RemoveAllHtmlTags(styles[i].Text));
                    prevCleanedStyleWord += textWithoutHtml;
                    if (textWithoutHtml.Contains(".")) {
                        cleanedStyles.Add(new StringStyle(prevCleanedStyleWord, styles[i].AnimationMode, styles[i].RevealSpeed, styles[i].AnimationStrength));
                        prevCleanedStyleWord = "";
                    }

                    string textWithoutAnimTags = string.Join("", TextAnimatorHelper.RemoveAnimationTags(styles[i].Text));
                    prevTextFieldStyleWord += textWithoutAnimTags;
                    if (textWithoutAnimTags.Contains(".")) {
                        textFieldStyles.Add(new StringStyle(prevTextFieldStyleWord, styles[i].AnimationMode, styles[i].RevealSpeed, styles[i].AnimationStrength));
                        prevTextFieldStyleWord = "";
                    }
                    break;
            }
        }

        return cleanedStyles.ToArray();
    }

    public string[] GetCleanedTextAsArray() {
        return GetCleanedWords(cleanTextToReveal);
    }

    private string GetCleanedTextFieldText() {
        if (revealMode == RevealMode.Character) {
            // Join words by spaces.
            return string.Join(" ", GetCleanedWords(textFieldStyles.ToArray()));
        } else {
            // Join words/sentences without a separator because the words/sentences itself already contain spaces.
            return string.Join("", GetCleanedWords(textFieldStyles.ToArray()));
        }
    }

    private string[] GetCleanedWords(StringStyle[] styles) {
        List<string> cleanedWords = new List<string>();

        string word = "";
        for (int i = 0; i < styles.Length; i++) {
            if (string.IsNullOrEmpty(styles[i].Text) || string.IsNullOrWhiteSpace(styles[i].Text)) {
                cleanedWords.Add(word);
                word = "";
            } else if (styles[i].Text != PAUSE_CHARACTER) {
                word += styles[i].Text;

                if (revealMode != RevealMode.Character) {
                    cleanedWords.Add(word);
                    word = "";
                }
            }
        }

        return cleanedWords.ToArray();
    }

#if UNITY_EDITOR
    private string debugText;

    [ContextMenu("Debug")]
    public void OnDebugTextClicked() {
        if (string.IsNullOrEmpty(debugText) || string.IsNullOrWhiteSpace(debugText)) {
            debugText = text.text;
        }
        ShowText(debugText);
    }
#endif
}