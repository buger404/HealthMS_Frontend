using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core;
using Milease.Core.Animator;
using Milease.Enums;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour, IBootstrap
{
    public static DialogController Instance;

    public TMP_Text Title, Content, ButtonText;
    public GameObject Dialog;
    public Image Panel, Cover, ButtonBack;

    private Action callback;
    
    private MilInstantAnimator showAnimator, hideAnimator;

    public static void Show(string title, string content, Action callback = null)
    {
        Instance.Title.text = title;
        Instance.Content.text = content;
        Instance.callback = callback;
        Instance.Dialog.SetActive(true);
    }

    private void OnEnable()
    {
        hideAnimator.Reset();
        showAnimator.Play();
    }

    public void OnClick()
    {
        hideAnimator.Play();
    }
    
    public void Bootstrap()
    {
        Instance = this;
        showAnimator =
            Cover.Milease("color", Color.clear, Color.black.Opacity(0.5f), 0.25f)
                .Then(
                    Panel.rectTransform.MileaseAdditive("sizeDelta", 
                        new Vector3(0f, -1211.48f, 0f), Vector3.zero, 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out),
                    Panel.Milease("color", 
                        ColorUtils.WhiteClear, Color.white, 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out),
                    Title.Milease("color", 
                        Color.clear, Color.black, 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out),
                    Content.Milease("color", 
                        Color.clear, ColorUtils.RGB(127, 127, 127), 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out),
                    ButtonBack.Milease("color", 
                        ColorUtils.WhiteClear, Color.white, 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out),
                    ButtonText.Milease("color", 
                        ColorUtils.WhiteClear, Color.white, 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out)
                ).UsingResetMode(RuntimeAnimationPart.AnimationResetMode.ResetToInitialState);
        
        hideAnimator =
            Cover.Milease("color", Color.black.Opacity(0.5f), Color.clear,0.25f)
                .While(
                    Panel.rectTransform.Milease("anchoredPosition", 
                        Vector3.zero, new Vector3(0f, -800f, 0f), 
                        0.25f, 0f, EaseFunction.Back, EaseType.In)
                )
                .Then(
                    new Action(() =>
                    {
                        Dialog.SetActive(false);
                        callback?.Invoke();
                    }).AsMileaseKeyEvent()
                )
                .UsingResetMode(RuntimeAnimationPart.AnimationResetMode.ResetToInitialState);
    }

}
