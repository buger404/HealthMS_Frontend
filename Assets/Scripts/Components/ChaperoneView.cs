using System;
using System.Collections;
using System.Collections.Generic;
using LeTai.Asset.TranslucentImage;
using Milease.Core;
using Milease.Core.Animator;
using Milease.Core.UI;
using Milease.Enums;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChaperoneView : MonoBehaviour, IBootstrap
{
    public static ChaperoneView Instance;

    public static ChaperoneModel CurrentChaperone;
    
    public TMP_Text Name, Phone, Hospital, Price, Money, BuyBtnText, Rate;
    public Image BuyBtnBack, Panel;
    public TranslucentImage Cover;

    public Transform CloseBtn;
    
    public MilListView CommentList;
    
    public GameObject NoCommentText, Canvas;
    
    private MilInstantAnimator showAnimator, hideAnimator;
    
    public void Bootstrap()
    {
        Instance = this;
        showAnimator =
            Cover.Milease("color", ColorUtils.WhiteClear, Color.white, 0.25f)
                .Then(
                    Panel.rectTransform.Milease("sizeDelta", 
                        new Vector2(3265.505f, 0f), new Vector2(3265.505f, 1966f), 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out)
                )
                .Then(
                    CloseBtn.Milease("localScale", 
                        Vector3.zero, Vector3.one, 
                        0.5f, 0f, EaseFunction.Back, EaseType.Out)
                )
                .UsingResetMode(RuntimeAnimationPart.AnimationResetMode.ResetToInitialState);
        
        hideAnimator =
            Cover.Milease("color", Color.white, ColorUtils.WhiteClear,0.25f)
                .While(
                    Panel.rectTransform.Milease("anchoredPosition", 
                        Vector3.zero, new Vector3(0f, -800f, 0f), 
                        0.25f, 0f, EaseFunction.Back, EaseType.In)
                )
                .Then(
                    new Action(() =>
                    {
                        Canvas.SetActive(false);
                    }).AsMileaseKeyEvent()
                )
                .UsingResetMode(RuntimeAnimationPart.AnimationResetMode.ResetToInitialState);
    }

    public async void Show(ChaperoneModel chaperone)
    {
        CurrentChaperone = chaperone;
        
        Name.text = chaperone.name;
        Hospital.text = ChaperoneController.Hospitals.Find(x => x.id == chaperone.hospital).name
                        + $"   {chaperone.startHour}:00 ~ {chaperone.endHour}:00";
        Phone.text = chaperone.phone;
        Price.text = $"￥{chaperone.price}";
        Rate.text = chaperone.GetRating().ToString("F1") + "分";
        
        var data = await Server.Get<UserModel>("/users/info", ("token", AuthController.Token));
        Money.text = $"余额  {data.money:F2}元";

        if (data.money >= chaperone.price)
        {
            BuyBtnBack.color = ColorUtils.RGB(235, 68, 80);
            BuyBtnText.text = "立即下单预约";
        }
        else
        {
            BuyBtnBack.color = ColorUtils.RGB(128, 128, 128);
            BuyBtnText.text = "余额不足";
        }

        var comments = await Server.Get<CommentModel[]>("/feedback/list",
            ("token", AuthController.Token), ("chaperone", chaperone.id));
        CommentList.Clear();
        foreach (var c in comments)
        {
            CommentList.Add(c);
        }
        
        NoCommentText.SetActive(CommentList.Items.Count == 0);
        
        Canvas.SetActive(true);
        hideAnimator.Reset();
        showAnimator.Play();
    }

    public void Hide()
    {
        hideAnimator.Play();
    }

    public async void Order()
    {
        var response = await Server.Post<StatusModel>("/reservation/submit", null,
            ("token", AuthController.Token), ("chaperone", CurrentChaperone.id));
        if (response.status == "failed")
        {
            DialogController.Show("预约失败", response.message);
            return;
        }
        DialogController.Show("预约成功！", response.message);
    }
}
