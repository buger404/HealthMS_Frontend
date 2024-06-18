using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core.Animator;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public enum UIState
    {
        Deactive, Active
    }
    
    public static readonly List<TabButton> Buttons = new();
    private static TabButton lstSelected;

    public Image Back, Icon;
    public TMP_Text Text;
    
    public GameObject BindPanel;

    public bool DefaultSelection;

    private MilStateAnimator animator = new();
    
    private void Awake()
    {
        Buttons.Add(this);
        animator.AddState(UIState.Deactive, 0.25f,
            new[]
            {
                Back.MilState("color", Color.black.Opacity(0.01f)),
                Icon.MilState("color", Color.black),
                Text.MilState("color", Color.black)
            });
        
        animator.AddState(UIState.Active, 0.25f,
            new[]
            {
                Back.MilState("color", Color.white),
                Icon.MilState("color", Color.white),
                Text.MilState("color", Color.white)
            });

        animator.SetDefaultState(DefaultSelection ? UIState.Active : UIState.Deactive);
        
        BindPanel.SetActive(DefaultSelection);

        if (DefaultSelection)
        {
            lstSelected = this;
        }
    }

    public void Active()
    {
        animator.Transition(UIState.Active);
        BindPanel.SetActive(true);
        lstSelected?.Deactive();
        lstSelected = this;
    }

    public void Deactive()
    {
        animator.Transition(UIState.Deactive);
        BindPanel.SetActive(false);
    }
}
