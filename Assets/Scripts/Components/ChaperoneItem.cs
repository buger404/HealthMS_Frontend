using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core.Animator;
using Milease.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChaperoneItem : MilListViewItem
{
    public TMP_Text Name, Price, Hospital, Reserved, WorkHour, Rating;
    
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
        var data = (Binding as ChaperoneModel)!;
        Name.text = "陪诊师 · " + data.name;
        Hospital.text = ChaperoneController.Hospitals.Find(x => x.id == data.hospital).name;
        Price.text = "￥" + data.price.ToString("F2");
        WorkHour.text = $"{data.startHour}:00 ~ {data.endHour}:00";
        Reserved.text = data.reserved + "人预约过";
        Rating.text = data.GetRating().ToString("F1") + "分";
    }

    public override void AdjustAppearance(float pos)
    {

    }
}
