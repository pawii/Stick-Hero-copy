
public class RedSquare : PartOfPlatform
{
    public static readonly float Width;


    static RedSquare()
    {
        Width = 0.3f;
    }


    protected override void HandleOnMovePlatform()
    {
        if (state == States.Behind)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
