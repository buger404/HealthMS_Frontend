using System;

[Serializable]
public class ReservationModel
{
    public string id;
    public string chaperone;
    public string username;
    public decimal price;
    public ChaperoneModel chaperoneInfo;
}
