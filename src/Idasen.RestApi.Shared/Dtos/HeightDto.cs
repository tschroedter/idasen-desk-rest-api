﻿namespace Idasen.RestApi.Shared.Dtos ;

public class HeightDto
{
    public uint Height { get ; set ; }

    public override string ToString ( )
    {
        return $"[Height: {Height}]" ;
    }
}