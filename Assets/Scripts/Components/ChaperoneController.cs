using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Milease.Core.UI;
using TMPro;
using UnityEngine;

public class ChaperoneController : MonoBehaviour, IBootstrap
{
    public static ChaperoneController Instance;
    public static List<HospitalModel> Hospitals = new();
    public static List<ChaperoneModel> Chaperones = new();
    
    public TMP_Dropdown HospitalDropdown;
    public MilListView ListView, SortMode;
    public void Bootstrap()
    {
        Instance = this;
        SortMode.Add("默认排序");
        SortMode.Add("最高评分");
        SortMode.Add("价格最低");
        SortMode.Add("最多人约");
        SortMode.Select(0, true);
        SortMode.SlideTo(0);
    }

    public async void LoadHospitals()
    {
        var hospitals = await Server.Get<HospitalModel[]>("hospital/list", ("token", AuthController.Token));
        Hospitals = hospitals.ToList();
        Instance.HospitalDropdown.options.Clear();
        Instance.HospitalDropdown.options.Add(new TMP_Dropdown.OptionData("全部医院"));
        foreach (var h in hospitals)
        {
            Instance.HospitalDropdown.options.Add(new TMP_Dropdown.OptionData(h.name));
        }

        Instance.HospitalDropdown.value = 0;
    }

    public async void LoadChaperones()
    {
        var id = HospitalDropdown.value == 0 ? -1 : Hospitals[HospitalDropdown.value - 1].id;
        Chaperones = (await Server.Get<ChaperoneModel[]>("chaperone/list", 
                ("token", AuthController.Token),
                ("hospital", id.ToString())
            )).ToList();
        SortModeItem.SortChaperoneAndDisplay();
    }
}
