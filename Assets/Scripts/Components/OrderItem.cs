using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core.Animator;
using Milease.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderItem : MilListViewItem
{
    public TMP_Text Title, Hospital, Phone;
    
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
        Title.text = "陪诊师 · " + data.chaperoneInfo.name + $"（{data.time:M月d日}）";
        Hospital.text = ChaperoneController.Hospitals.Find(x => x.id == data.chaperoneInfo.hospital).name
                        + $"  {data.chaperoneInfo.startHour}:00 ~ {data.chaperoneInfo.endHour}:00";
        Phone.text = data.chaperoneInfo.phone;
    }

    public override void AdjustAppearance(float pos)
    {

    }

    public async void CancelOrder()
    {
        var data = (Binding as ReservationModel)!;
        var res = await Server.Post<StatusModel>("/reservation/cancel", null,
            ("token", AuthController.Token), ("reservation", data.id));
        
        if (res.status == "failed")
        {
            DialogController.Show("退单失败", res.message);
            return;
        }

        ProfileView.Instance.UpdateProfile();
        ProfileView.Instance.OrderList.Remove(data);
        ProfileView.Instance.RefreshNoOrderText();
        
        DialogController.Show("退单成功", "已将订单金额退还至您的账户余额中。");
    }

    public void ShowChaperoneDetail()
    {
        ChaperoneView.Instance.Show((Binding as ReservationModel)!.chaperoneInfo);
    }

    public void FinishOrder()
    {
        FeedbackView.Instance.Show((Binding as ReservationModel)!);
    }
}
