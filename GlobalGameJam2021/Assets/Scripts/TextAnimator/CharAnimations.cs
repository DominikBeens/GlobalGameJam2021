using CharTween;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimations {

    [Serializable]
    public enum AnimationMode {
        None,
        Punch,
        Shake,
        Breathe,
        Wave,
        Fade,
        ScaleIn
    }

    private CharTweener tweener;

    public CharAnimations(CharTweener tweener) {
        this.tweener = tweener;
    }

    public void AnimateCharacter(int index, AnimationMode mode, int consecutiveSameAnimationModes, int animationStrength, ref List<Tween> charTweens) {
        tweener.SetLocalScale(index, Vector3.one);
        tweener.DOLocalMoveY(index, 0, 0);

        float strength = GetAnimationStrengthMultiplier(animationStrength);

        switch (mode) {
            case AnimationMode.None:                                                                                            break;
            case AnimationMode.Punch:       PunchCharacter(index, strength, ref charTweens);                                    break;
            case AnimationMode.Shake:       ShakeCharacter(index, strength, ref charTweens);                                    break;
            case AnimationMode.Breathe:     BreatheCharacter(index, strength, consecutiveSameAnimationModes, ref charTweens);   break;
            case AnimationMode.Wave:        WaveCharacter(index, strength, consecutiveSameAnimationModes, ref charTweens);      break;
            case AnimationMode.Fade:        FadeCharacter(index, strength, ref charTweens);                                     break;
            case AnimationMode.ScaleIn:     ScaleInCharacter(index, strength, ref charTweens);                                  break;
        }
    }

    private void PunchCharacter(int index, float strength, ref List<Tween> charTweens) {
        charTweens.Add(tweener.DOPunchScale(index, Vector3.one * (0.5f * strength), 0.1f));
    }

    private void ShakeCharacter(int index, float strength, ref List<Tween> charTweens) {
        charTweens.Add(tweener.DOShakePosition(index, 1337f, Mathf.Clamp(1.5f * strength, 0.8f, 3f), 5, 90, true, true));
    }

    private void BreatheCharacter(int index, float strength, int consecutiveSameAnimationModes, ref List<Tween> charTweens) {
        //if (consecutiveSameAnimationModes > 0) {
        //    Vector3 startScale = tweener.GetLocalScale(index - 1);
        //    tweener.DOScale(index, startScale, 0);
        //}
        charTweens.Add(tweener.DOScale(index, 1 + (0.25f * strength), 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad));
    }

    private void WaveCharacter(int index, float strength, int consecutiveSameAnimationModes, ref List<Tween> charTweens) {
        charTweens.Add(tweener.DOLocalMoveY(index, 4f * strength, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetDelay(0.03f * consecutiveSameAnimationModes));
    }

    // Not combinable with <color> tags.
    private void FadeCharacter(int index, float strength, ref List<Tween> charTweens) {
        tweener.DOFade(index, 1f, 0);
        charTweens.Add(tweener.DOFade(index, 1 - (0.9f * strength), 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad));
        charTweens.Add(tweener.DOScale(index, 1 - (0.2f * strength), 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad));
    }

    private void ScaleInCharacter(int index, float strength, ref List<Tween> charTweens) {
        tweener.SetLocalScale(index, Vector3.zero);
        tweener.DOLocalMoveY(index, -15, 0);

        float speedIn = 0.3f;
        float speedOut = 0.1f;

        charTweens.Add(tweener.DOScaleY(index, Mathf.Clamp(2f * strength, 1.25f, 4), speedIn).SetEase(Ease.OutCubic).OnComplete(() => {
            tweener.DOScaleY(index, 1f, speedOut).SetEase(Ease.OutBack);
        }));
        charTweens.Add(tweener.DOScaleX(index, Mathf.Clamp(1.2f * strength, 1.05f, 2), speedIn).SetEase(Ease.OutCubic).OnComplete(() => {
            tweener.DOScaleX(index, 1f, speedOut).SetEase(Ease.OutBack);
        }));
        charTweens.Add(tweener.DOLocalMoveY(index, 20 * strength, speedIn).SetEase(Ease.OutCubic).OnComplete(() => {
            tweener.DOLocalMoveY(index, 0, speedOut).SetEase(Ease.OutBack);
        }));
    }

    private float GetAnimationStrengthMultiplier(int strength) {
        switch (strength) {
            case 2:
                return 0.4f;
            case 3:
                return 2f;
            default:
                return 1;
        }
    }
}