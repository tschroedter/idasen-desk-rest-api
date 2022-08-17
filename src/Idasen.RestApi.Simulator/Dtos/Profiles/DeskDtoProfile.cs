using AutoMapper ;
using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Simulator.Desks ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.Simulator.Dtos.Profiles ;

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