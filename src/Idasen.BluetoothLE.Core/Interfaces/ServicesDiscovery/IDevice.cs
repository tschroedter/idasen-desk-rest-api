﻿using System ;
using System.Collections.Generic ;
using Windows.Devices.Bluetooth ;
using Windows.Devices.Bluetooth.GenericAttributeProfile ;
using Idasen.BluetoothLE.Core.Interfaces.ServicesDiscovery.Wrappers ;
// ReSharper disable UnusedMemberInSuper.Global

namespace Idasen.BluetoothLE.Core.Interfaces.ServicesDiscovery
{
    /// <summary>
    ///     Represents a Bluetooth LE device.
    ///     Add the moment only a limited set of properties and functions are
    ///     exposed.
    /// </summary>
    public interface IDevice
        : IDisposable
    {
        /// <summary>
        ///     Defines a provider for push-based notification about the
        ///     connection status..
        /// </summary>
        IObservable < BluetoothConnectionStatus > ConnectionStatusChanged { get ; }

        /// <summary>
        ///     Gets the Gatt communication connection status of the device.
        /// </summary>
        GattCommunicationStatus GattCommunicationStatus { get ; }

        /// <summary>
        ///     Gets the name of the Bluetooth LE device.
        /// </summary>
        string Name { get ; }

        /// <summary>
        ///     Gets the device Id.
        /// </summary>
        string Id { get ; }

        /// <summary>
        ///     Gets a value that indicates whether the device is currently paired.
        /// </summary>
        bool IsPaired { get ; }

        /// <summary>
        ///     Gets the connection status of the device.
        /// </summary>
        BluetoothConnectionStatus ConnectionStatus { get ; }

        /// <summary>
        ///     Gets the read-only list of GATT services supported by the device.
        /// </summary>
        IReadOnlyDictionary < IGattDeviceServiceWrapper , IGattCharacteristicsResultWrapper > GattServices { get ; }

        /// <summary>
        ///     Defines a provider for push-based notification about Gatt services
        ///     being refreshed.
        /// </summary>
        IObservable < GattCommunicationStatus > GattServicesRefreshed { get ; }

        /// <summary>
        ///     The device address.
        /// </summary>
        ulong BluetoothAddress { get ; }

        /// <summary>
        ///     The address type.
        /// </summary>
        string BluetoothAddressType { get ; }

        /// <summary>
        ///     Connects to the device if the device is disconnected.
        /// </summary>
        void Connect ( ) ;
    }
}