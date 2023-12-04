public class big_heart : pickup_mother, IHeart_pickup
{
    public int GetHeart()
    {
        Playsound();
        return 5; //heart return
    }
}
