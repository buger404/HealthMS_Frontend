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

public class CProfileView : MonoBehaviour, IBootstrap
{
    public static CProfileView Instance;

    public Image Cover, Panel;
    public GameObject Canvas;

    public GameObject LeaveBtn;
    public TMP_Text SubmitBtnText;
    public TMP_InputField Name, Phone, Price, SHour, EHour;
    public TMP_Dropdown Hospital;

    public bool EditMode = false;
    
    private MilInstantAnimator showAnimator, hideAnimator;
    public void Bootstrap()
    {
        Instance = this;
        showAnimator =
            Cover.Milease("color", Color.clear, Color.black, 0.25f)
                .Then(
                    Panel.rectTransform.Milease("sizeDelta", 
                        new Vector2(2729f, 0f), new Vector2(2729f, 2500f), 
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

    public async void Show()
    {
        var user = await Server.Get<UserModel>("users/info", ("token", AuthController.Token));
        EditMode = !string.IsNullOrEmpty(user.partTime);
        LeaveBtn.SetActive(EditMode);
        SubmitBtnText.text = EditMode ? "确认陪诊师信息" : "加入陪诊师兼职！";
        
        Hospital.options.Clear();
        foreach (var hospital in ChaperoneController.Hospitals)
        {
            Hospital.options.Add(new TMP_Dropdown.OptionData(hospital.name));
        }
        
        var chaperone = EditMode
            ? await Server.Get<ChaperoneModel>("/chaperone/info",
                ("token", AuthController.Token), ("chaperone", user.partTime))
            : new ChaperoneModel();
        Name.text = chaperone.name;
        Phone.text = chaperone.phone;
        Hospital.value = chaperone.hospital;
        SHour.text = chaperone.startHour.ToString();
        EHour.text = chaperone.endHour.ToString();
        Price.text = chaperone.price.ToString("F2");
        
        Canvas.SetActive(true);
        
        hideAnimator.Reset();
        showAnimator.Play();
    }

    public async void Quit()
    {
        var res = await Server.Post<StatusModel>("/chaperone/quit", null,
            ("token", AuthController.Token));
        if (res.status == "failed")
        {
            DialogController.Show("失败", res.message);
            return;
        }
        
        PartTimeView.Instance.RefreshPage();
        
        DialogController.Show("成功", res.message);
        hideAnimator.Play();
    }
    
    public async void Submit()
    {
        if (string.IsNullOrEmpty(Price.text))
        {
            DialogController.Show("错误", "价格设定不能为空。");
            return;
        }
        
        if (string.IsNullOrEmpty(SHour.text) || string.IsNullOrEmpty(EHour.text))
        {
            DialogController.Show("错误", "兼职时间段必须填写完整。");
            return;
        }
        
        var body = new
        {
            hospital = Hospital.value,
            name = Name.text,
            phone = Phone.text,
            price = decimal.Parse(Price.text),
            startHour = int.Parse(SHour.text),
            endHour = int.Parse(EHour.text)
        };

        var res = await Server.Post<StatusModel>(EditMode ? "/chaperone/update" : "/chaperone/join", body,
            ("token", AuthController.Token));
        if (res.status == "failed")
        {
            DialogController.Show("失败", res.message);
            return;
        }
        
        PartTimeView.Instance.RefreshPage();
        
        DialogController.Show("成功", res.message);
        hideAnimator.Play();
    }
}
