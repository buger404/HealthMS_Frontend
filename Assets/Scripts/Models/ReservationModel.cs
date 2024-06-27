using System;

[Serializable]
public class ReservationModel
{
    public string id;
    public string chaperone;
    public decimal price;
    public ChaperoneModel chaperoneInfo;
}
