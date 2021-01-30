using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public static class TextAnimatorHelper {

    public static string[] SplitOnCharacters(string text, char c) {
        string[] splitText = text.Split(c);
        for (int i = 0; i < splitText.Length; i++) {
            splitText[i] += c;
        }
        return splitText;
    }

    public static bool ContainsOpeningTag(string s) {
        string[] enums = Enum.GetNames(typeof(CharAnimations.AnimationMode));
        for (int i = 0; i < enums.Length; i++) {
            if (s.Contains($"<{enums[i].ToLower()}")) {
                return true;
            }
        }
        if (s.Contains($"<speed")) {
            return true;
        }
        return false;
    }

    public static bool ContainsClosingTag(string s) {
        string[] enums = Enum.GetNames(typeof(CharAnimations.AnimationMode));
        for (int i = 0; i < enums.Length; i++) {
            if (s.Contains($"</{enums[i].ToLower()}")) {
                return true;
            }
        }
        if (s.Contains($"</speed")) {
            return true;
        }
        return false;
    }

    public static bool TryGetSpeed(string s, ref float speed) {
        string value = GetTagValueFromWord(s, "speed", null);
        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out speed)) {
            return true;
        }
        return false;
    }

    public static bool TryGetPauseDuration(string s, ref float duration) {
        string value = GetTagValueFromWord(s, "<pause=", ">");
        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out duration)) {
            return true;
        }
        return false;
    }

    public static bool TryGetAnimationModeAndStrength(string s, ref CharAnimations.AnimationMode mode, ref int animationStrength) {
        if (char.IsDigit(s[s.Length - 1])) {
            string digit = s[s.Length - 1].ToString();
            s = s.Substring(0, s.Length - 1);
            int.TryParse(digit, out animationStrength);
        }

        if (Enum.TryParse(s, true, out mode)) {
            return true;
        }
        return false;
    }

    public static string GetTagValueFromWord(string word, string startTag, string endTag) {
        int startIndex = word.IndexOf(startTag);
        int endIndex = string.IsNullOrEmpty(endTag) ? word.Length : word.IndexOf(endTag, startIndex);
        string value = word.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
        return value;
    }

    public static string[] RemoveAllHtmlTags(string text) {
        List<string> cleanedText = new List<string>();
        bool skipCharacter = false;
        foreach (char c in text) {
            if (c == '<') {
                skipCharacter = true;
            }

            if (!skipCharacter) {
                cleanedText.Add(c.ToString());
            }

            if (c == '>') {
                skipCharacter = false;
            }
        }
        return cleanedText.ToArray();
    }

    public static string[] RemoveAnimationTags(string text) {
        List<string> cleanedText = new List<string>();
        bool skipCharacter = false;

        // Split text on opening tags ('<') and keep the separator in there.
        string[] splitText = Regex.Split(text, @"(?=<)");

        for (int i = 0; i < splitText.Length; i++) {
            bool containsExpectedTag = ContainsOpeningTag(splitText[i]) || ContainsClosingTag(splitText[i]);
            foreach (char c in splitText[i]) {
                if (c == '<' && containsExpectedTag) {
                    skipCharacter = true;
                }

                if (!skipCharacter) {
                    cleanedText.Add(c.ToString());
                }

                if (c == '>' && containsExpectedTag) {
                    skipCharacter = false;
                }
            }
        }

        return cleanedText.ToArray();
    }

    public static Color32[] GetCharacterColors(TMP_TextInfo textInfo, int characterIndex) {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[characterIndex];
        int meshIndex = charInfo.materialReferenceIndex;

        Color32[] vertexColors = (Color32[])textInfo.meshInfo[meshIndex].colors32.Clone();
        if (vertexColors != null) {
            for (int k = 0; k < vertexColors.Length; k++) {
                vertexColors[k].a = 255;
            }
            return vertexColors;
        }

        return null;
    }

    public static void SetCharacterColors(TMP_TextInfo textInfo, int characterIndex, Color32[] colors) {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[characterIndex];
        int meshIndex = charInfo.materialReferenceIndex;
        int vertexIndex = charInfo.vertexIndex;

        Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
        if (vertexColors != null) {
            vertexColors[vertexIndex + 0] = colors[vertexIndex + 0];
            vertexColors[vertexIndex + 1] = colors[vertexIndex + 1];
            vertexColors[vertexIndex + 2] = colors[vertexIndex + 2];
            vertexColors[vertexIndex + 3] = colors[vertexIndex + 3];
        }
    }
}
