using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core;
using Milease.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    private static GameObject lstSelectedPanel;
    
    public List<GameObject> Canvas = new();
    public GameObject StartupCanvas;
    public Image LoadingCover;
    
    private void Awake()
    {
        Instance = this;
        foreach (var can in Canvas)
        {
            can.SetActive(false);
        }
        StartupCanvas.SetActive(true);
        lstSelectedPanel = StartupCanvas;
    }

    public static void SwitchCanvas(string canvas)
    {
        new Action(() => Instance.LoadingCover.gameObject.SetActive(true)).AsMileaseKeyEvent()
            .While(
                Instance.LoadingCover.Milease("color", ColorUtils.WhiteClear, Color.white, 0.25f)
            )
            .ThenOneByOne(
                new Action(() =>
                {
                    lstSelectedPanel?.SetActive(false);
                    var can = Instance.Canvas.Find(x => string.Equals(x.name, canvas, StringComparison.CurrentCultureIgnoreCase));
                    can.SetActive(true);
                    lstSelectedPanel = can;
                }).AsMileaseKeyEvent(),
                Instance.LoadingCover.Milease("color", Color.white, ColorUtils.WhiteClear,0.25f),
                new Action(() => Instance.LoadingCover.gameObject.SetActive(false)).AsMileaseKeyEvent()
            ).UsingResetMode(RuntimeAnimationPart.AnimationResetMode.ResetToInitialState)
            .Play();
    }
}
