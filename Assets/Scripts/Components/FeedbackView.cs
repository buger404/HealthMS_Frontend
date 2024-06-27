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

public class FeedbackView : MonoBehaviour, IBootstrap
{
    public static FeedbackView Instance;
    
    public static ReservationModel CurrentReservation;

    public bool Like = false;
    
    public Image Cover, Panel;
    public TMP_InputField Comment;
    public Image LikeBtnBack, DisLikeBtnBack, LikeBtnIcon, DisLikeBtnIcon;
    public TMP_Text LikeBtnText, DisLikeBtnText, Title;

    public Sprite ActiveBtnBack, DeactiveBtnBack;
    
    public GameObject Canvas;
    
    private MilInstantAnimator showAnimator, hideAnimator;
    public void Bootstrap()
    {
        Instance = this;
        showAnimator =
            Cover.Milease("color", Color.clear, Color.black, 0.25f)
                .Then(
                    Panel.rectTransform.Milease("sizeDelta", 
                        new Vector2(2729f, 0f), new Vector2(2729f, 1832.214f), 
                        0.25f, 0f, EaseFunction.Quad, EaseType.Out)
                )
                .UsingResetMode(RuntimeAnimationPart.AnimationResetMode.ResetToInitialState);
        
        hideAnimator =
            Cover.Milease("color", Color.black, Color.clear,0.25f)
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
    
    public void Show(ReservationModel reservation)
    {
        CurrentReservation = reservation;
        
        Title.text = $"请评价 陪诊师 · {reservation.chaperoneInfo.name} 本次服务";
        Comment.text = "";
        Like = true;

        UpdateLikeBtn();
        
        Canvas.SetActive(true);
        hideAnimator.Reset();
        showAnimator.Play();
    }

    private void UpdateLikeBtn()
    {
        LikeBtnBack.sprite = (Like ? ActiveBtnBack : DeactiveBtnBack);
        DisLikeBtnBack.sprite = (!Like ? ActiveBtnBack : DeactiveBtnBack);
        
        LikeBtnIcon.color = (Like ? Color.white : Color.black);
        DisLikeBtnIcon.color = (!Like ? Color.white : Color.black);
        
        LikeBtnText.color = (Like ? Color.white : Color.black);
        DisLikeBtnText.color = (!Like ? Color.white : Color.black);
    }

    public void OnClickLike()
    {
        Like = true;
        UpdateLikeBtn();
    }
    
    public void OnClickDisLike()
    {
        Like = false;
        UpdateLikeBtn();
    }

    public async void SubmitFeedback()
    {
        var res = await Server.Post<StatusModel>("/reservation/checkout",
            new
            {
                reservation = CurrentReservation.id,
                praise = Like,
                comment = Comment.text
            }, ("token", AuthController.Token));

        if (res.status == "failed")
        {
            DialogController.Show("评价失败", res.message);
            return;
        }
        
        ProfileView.Instance.OrderList.Remove(CurrentReservation);
        ProfileView.Instance.RefreshNoOrderText();
        
        DialogController.Show("评价成功", "感谢您为本次服务做出评价！");
        
        hideAnimator.Play();
    }
}
