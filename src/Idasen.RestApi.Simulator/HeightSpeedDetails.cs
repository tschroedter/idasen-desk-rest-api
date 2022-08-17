namespace Idasen.RestApi.Simulator ;

public class HeightSpeedDetails
{
    public HeightSpeedDetails ( DateTimeOffset now ,
                                uint           height ,
                                int            speed )
    {
        Now    = now ;
        Height = height ;
        Speed  = speed ;
    }

    public uint           Height { get ; }
    public DateTimeOffset Now    { get ; }
    public int            Speed  { get ; }
}