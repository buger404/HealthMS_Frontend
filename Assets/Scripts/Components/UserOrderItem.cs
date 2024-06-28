using System;
using System.Collections.Generic;
using Milease.Core.Animator;
using Milease.Core.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UserOrderItem : MilListViewItem
{
    public TMP_Text Username, Price;
    
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
        var data = (Binding as ReservationModel)!;
        Username.text = data.username;
        Price.text = $"￥{data.price:F2}";
    }

    public override void AdjustAppearance(float pos)
    {

    }
}
