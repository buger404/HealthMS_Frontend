using System.Collections;
using System.Collections.Generic;
using Milease.Core.UI;
using TMPro;
using UnityEngine;

public class PartTimeView : MonoBehaviour, IBootstrap
{
    public static PartTimeView Instance;
    
    public GameObject JoinPage, ProfilePage, NoOrderText;
    public TMP_Text Title, OverView, OrderTitle;
    
    public MilListView OrderList;
    
    public void Bootstrap()
    {
        Instance = this;
    }

    public async void RefreshPage()
    {
        var user = await Server.Get<UserModel>("users/info", ("token", AuthController.Token));
        var hasPartTime = !string.IsNullOrEmpty(user.partTime);
        ProfilePage.SetActive(hasPartTime);
        JoinPage.SetActive(!hasPartTime);

        if (hasPartTime)
        {
            var chaperone = await Server.Get<ChaperoneModel>("/chaperone/info",
                    ("token", AuthController.Token), ("chaperone", user.partTime));
            Title.text = $"陪诊师 · {chaperone.name}";
            OverView.text =
                $"当前评分：{chaperone.GetRating():F1}（{chaperone.praised}人好评，{chaperone.finished - chaperone.praised}人差评）";

            var orders = await Server.Get<ReservationModel[]>("/chaperone/orders", ("token", AuthController.Token));
            OrderTitle.text = $"未处理订单（{orders.Length}）";
            OrderList.Clear();
            foreach (var order in orders)
            {
                OrderList.Add(order);
            }
            NoOrderText.SetActive(orders.Length == 0);
        }
    }
    
    public void JoinChaperone()
    {
        CProfileView.Instance.Show();
    }

    public async void ViewDetail()
    {
        var user = await Server.Get<UserModel>("users/info", ("token", AuthController.Token));
        var chaperone = await Server.Get<ChaperoneModel>("/chaperone/info",
            ("token", AuthController.Token), ("chaperone", user.partTime));
        ChaperoneView.Instance.Show(chaperone);
    }
}
