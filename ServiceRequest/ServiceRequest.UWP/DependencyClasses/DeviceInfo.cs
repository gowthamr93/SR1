using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Models;
using ServiceRequest.UWP.DependencyClasses;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceInfo))]
namespace ServiceRequest.UWP.DependencyClasses
{
    public class DeviceInfo : IDeviceInfo
    {
        public DeviceInfoModel GetDeviceInfo()
        {
            EasClientDeviceInformation deviceInfo;
            //
            deviceInfo = new EasClientDeviceInformation();
            //deviceInfo.OperatingSystem
            return new DeviceInfoModel()
            {
                Name = deviceInfo.SystemManufacturer,
                ModelName = deviceInfo.SystemProductName,
                OsVersionName = AnalyticsInfo.VersionInfo.DeviceFamily,
                OsVersion = GetFamilyVersion(AnalyticsInfo.VersionInfo.DeviceFamilyVersion)
            };
        }

        private string GetFamilyVersion(string deviceFamilyVersion)
        {
            ulong v = ulong.Parse(deviceFamilyVersion);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);
            return $"{v1}.{v2}.{v3}.{v4}";
        }
    }
}
