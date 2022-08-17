using FluentValidation ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.Shared.Dtos.Validators ;

[ UsedImplicitly ]
public class HeightDtoValidator
    : AbstractValidator < HeightDto >
{
    public HeightDtoValidator ( )
    {
        RuleFor ( m => m.Height ).Must ( x => x is >= MinHeight and <= MaxHeight )
                                 .WithMessage ( $"Height must be between {MinHeight} and {MaxHeight}" ) ;
    }

    private const uint MinHeight = 6200u ;
    private const uint MaxHeight = 12700u ;
}