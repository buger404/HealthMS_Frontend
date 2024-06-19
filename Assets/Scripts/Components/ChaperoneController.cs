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
    public TMP_Dropdown HospitalDropdown;
    public MilListView ListView;
    public void Bootstrap()
    {
        Instance = this;
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
        var chaperones = await Server.Get<ChaperoneModel[]>("chaperone/list", 
                ("token", AuthController.Token),
                ("hospital", id.ToString())
            );
        ListView.Clear();
        foreach (var c in chaperones)
        {
            ListView.Add(c);
        }
    }
}
