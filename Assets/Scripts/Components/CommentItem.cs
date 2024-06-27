using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core.Animator;
using Milease.Core.UI;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentItem : MilListViewItem
{
    public TMP_Text Title, Content, LikeCount, SendTime;
    public Image LikeBtnBack;
    
    protected override IEnumerable<MilStateParameter> ConfigDefaultState()
    {
        return Array.Empty<MilStateParameter>();
    }

    protected override IEnumerable<MilStateParameter> ConfigSelectedState()
    {
        return Array.Empty<MilStateParameter>();
    }

    public override void OnSelect(PointerEventData eventData)
    {

    }

    protected override void OnInitialize()
    {

    }

    protected override void OnTerminate()
    {

    }

    protected override MilInstantAnimator ConfigClickAnimation()
    {
        return null;
    }

    public override void UpdateAppearance()
    {
        var data = (Binding as CommentModel)!;
        Title.text = data.username + "  " + (data.praised ? "<color=#EB4450>推荐此陪诊师" : "<b>不推荐此陪诊师");
        Content.text = data.comment;
        LikeCount.text = data.likes.ToString();
        SendTime.text = data.sendTime.ToString("yyyy.MM.dd HH:mm");
        LikeBtnBack.color = data.liked ? ColorUtils.RGB(235, 68, 80) : ColorUtils.RGB(128, 128, 128);
    }

    public async void Like()
    {
        var data = (Binding as CommentModel)!;
        var res = await Server.Post<StatusModel>("/feedback/" + (data.liked ? "unlike" : "like"), null,
            ("token", AuthController.Token), ("feedback", data.id));
        
        if (res.status == "failed")
        {
            DialogController.Show("失败", res.message);
            return;
        }

        data.liked = !data.liked;
        data.likes += (data.liked ? 1 : -1);
        UpdateAppearance();
    }
    
    public override void AdjustAppearance(float pos)
    {

    }
}
