using AutoMapper ;
using Idasen.RestApi.Fake.Desks ;
using Idasen.RestApi.Shared.Dtos ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.Fake.Dtos.Profiles ;

[ UsedImplicitly ]
internal class DeskDtoProfile
    : Profile
{
    public DeskDtoProfile ( )
    {
        CreateMap < FakeDesk , DeskDto > ( )
           .ForMember ( dest => dest.BluetoothAddress ,
                        opt => opt.MapFrom ( src => src.BluetoothAddress ) )
           .ForMember ( dest => dest.BluetoothAddressType ,
                        opt => opt.MapFrom ( src => src.BluetoothAddressType ) )
           .ForMember ( dest => dest.DeviceName ,
                        opt => opt.MapFrom ( src => src.DeviceName ) )
           .ForMember ( dest => dest.Name ,
                        opt => opt.MapFrom ( src => src.Name ) ) ;
    }
}