﻿#region Using Statements
using DataLayer.Entities;
using DataLayer.Entities.HomeDevices;
using SmartHome.Communications.Messages;
using SmartHome.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmartHome.BusinessEntities.BusinessHomeDevice;
using SmartHome.Communications.Modules;
#endregion

namespace SmartHome.BusinessEntities.BusinessHomeDevice
{
    public static class BusinessHomeDevice
    {
        private static Dictionary<Type, MethodInfo[]> _homeDeviceOperations;
        private static Dictionary<Type, MethodInfo[]> _HomeDeviceOperations
        {
            get
            {
                if(_homeDeviceOperations == null)
                    GetExecutableMethods();
                return _homeDeviceOperations;
            }
        }

        public static HomeDevice CreateHomeDevice(string homeDeviceType)
        {
            try
            {
                Type deviceType = typeof(HomeDevice).Assembly.GetTypes().First(t => t.Name == homeDeviceType);

                return (HomeDevice)Activator.CreateInstance(deviceType);
            }
            catch (Exception)
            {
                throw new ArgumentException("The type " + homeDeviceType + " not exist in the system");
            }
        }

        public static PinPort[] GetPinPorts(this HomeDevice homeDevice)
        {
            if (!homeDevice.InUse)
                return null;
            if (!homeDevice.ProductTag.HasValue)//no es un producto
                return homeDevice.Connector.GetPinPort();
            else //es un producto
            {
                List<PinPort> pinPorts = new List<PinPort>();
                //TODO
                return pinPorts.ToArray();
            }
        }

        public static OperationMessage RefreshState(this HomeDevice homeDevice)
        {
            if (homeDevice is WallPlug)
                return (homeDevice as WallPlug).RefreshState();
            else if (homeDevice is Light)
                return (homeDevice as Light).RefreshState();
            else if (homeDevice is Dimmable)
                return (homeDevice as Dimmable).RefreshState();
            else if (homeDevice is HumiditySensor)
                return (homeDevice as HumiditySensor).RefreshState();
            else if (homeDevice is PowerSensor)
                return (homeDevice as PowerSensor).RefreshState();
            else if (homeDevice is PresenceSensor)
                return (homeDevice as PresenceSensor).RefreshState();
            else if (homeDevice is RGBLight)
                return (homeDevice as RGBLight).RefreshState();
            else if (homeDevice is SwitchButton)
                return (homeDevice as SwitchButton).RefreshState();
            else if (homeDevice is TemperatureSensor)
                return (homeDevice as TemperatureSensor).RefreshState();
            else
                throw new ArgumentException("HomeDevice not valid");
        }

        public static MethodInfo[] GetHomeDeviceMethodOperations(this HomeDevice homeDevice)
        {
            Type HomeDeviceType = homeDevice.HomeDeviceType;
            if (HomeDeviceType == null || !typeof(HomeDevice).IsAssignableFrom(HomeDeviceType))
                throw new ArgumentException("This homeDevice is not a valid home Device");

            return _HomeDeviceOperations[HomeDeviceType];
        }

        public static string[] GetHomeDeviceNameOperations(this HomeDevice homeDevice)
        {
            return GetHomeDeviceMethodOperations(homeDevice).Select(m => m.Name).ToArray();
        }

        public static MethodInfo GetArgsOperation(this HomeDevice homeDevice, string nameOperation)
        {
            return GetHomeDeviceMethodOperations(homeDevice).First(m => m.Name == nameOperation);
        }

        public static OperationMessage GetAddressableOperation(this HomeDevice homeDevice, OperationMessage message)
        {
            if (homeDevice.InUse)
                message.DestinationAddress = (ushort)homeDevice.Connector.Node.Address;

            return message;
        }

        public static List<PropertyParam> GetStateValue(this HomeDevice homeDevice)
        {
            List<PropertyParam> PropiertyValues = new List<PropertyParam>();

            var filterProperties = homeDevice.HomeDeviceType.GetProperties().Where(p => p.GetCustomAttributes(true)
                                            .OfType<PropertyAttribute>()
                                            .Where(a => !a.Internal).Count() > 0);

            foreach (var item in filterProperties)
            {
                Type type = item.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }

                PropiertyValues.Add(new PropertyParam()
                {
                    Name = item.Name,
                    Type = type,
                    Value = item.GetValue(homeDevice, null)
                });
            }

            return PropiertyValues;
        }

        private static void GetExecutableMethods()
        {
            _homeDeviceOperations = new Dictionary<Type, MethodInfo[]>();
            foreach (var homeDeviceType in HomeDevice.HomeDeviceTypes)
            {
                var methods = homeDeviceType.GetExtensionMethods(Assembly.GetAssembly(Type.GetType("BusinessHomeDevice")))
                    .Where(m => m.GetCustomAttributes(true)
                                            .OfType<OperationAttribute>()
                                            .Where(a => !a.Internal).Count() > 0)
                    .ToArray();

                _homeDeviceOperations.Add(homeDeviceType, methods);
            }
        }

        private static IEnumerable<MethodInfo> GetExtensionMethods(this Type type, Assembly extensionsAssembly)
        {
            var query = from t in extensionsAssembly.GetTypes()
                        where !t.IsGenericType && !t.IsNested
                        from m in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                        where m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false)
                        where m.GetParameters()[0].ParameterType == type
                        select m;

            return query;
        }
    }

    public class PropertyParam
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
