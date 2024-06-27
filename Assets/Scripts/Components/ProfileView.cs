using System;
using System.Collections;
using System.Collections.Generic;
using Milease.Core.UI;
using TMPro;
using UnityEngine;

public class ProfileView : MonoBehaviour, IBootstrap
{
    public static ProfileView Instance;

    public GameObject NoOrderText;
    public TMP_Text Title, Money;
    public MilListView OrderList;

    public void Bootstrap()
    {
        Instance = this;
    }

    public async void UpdateProfile()
    {
        var user = await Server.Get<UserModel>("/users/info", ("token", AuthController.Token));
        Title.text = (DateTime.Now.Hour switch
        {
            < 6 or >= 19 => "晚上",
            >= 6 and < 11 => "上午",
            >= 11 and <= 12 => "中午",
            > 12 and < 19 => "下午"
        }) + "好，" + user.username + "！";
        Money.text = $"余额：{user.money:F2}元";
    }
    
    public async void UpdateView()
    {
        UpdateProfile();
        
        var reservations = 
            await Server.Get<ReservationModel[]>("/reservation/list", ("token", AuthController.Token));
        
        OrderList.Clear();

        foreach (var item in reservations)
        {
            OrderList.Add(item);
        }

        RefreshNoOrderText();
    }

    public void RefreshNoOrderText()
    {
        NoOrderText.SetActive(OrderList.Items.Count == 0);
    }

    public async void Logout()
    {
        var res = await Server.Post<StatusModel>("/logout", null, ("token", AuthController.Token));
        if (res.status != "succeed")
        {
            DialogController.Show("登出失败！", "未知错误，请稍后重试。");
            return;
        }

        DialogController.Show("登出成功", "即将返回登录界面。");
        
        AuthController.Token = "";
        CanvasManager.SwitchCanvas("LoginCanvas");
    }

    public async void Recharge()
    {
        var res = await Server.Post<UserModel>("/users/recharge", null, 
            ("token", AuthController.Token), ("money", "500.0"));

        DialogController.Show("充值成功", "500元已到账。");
        
        Money.text = $"余额：{res.money:F2}元";
    }
}
