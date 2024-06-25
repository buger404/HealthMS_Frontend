public static class ChaperoneExtension
{
    public static double GetRating(this ChaperoneModel chaperone)
    {
        if (chaperone.reserved == 0)
        {
            return 10.0;
        }
        return chaperone.praised * 1.0 / chaperone.reserved * 10.0;
    }
}
