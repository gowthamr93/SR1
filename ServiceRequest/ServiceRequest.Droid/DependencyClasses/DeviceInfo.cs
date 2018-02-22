using ServiceRequest.DependencyInterfaces;
using ServiceRequest.Droid.DependencyClasses;
using ServiceRequest.Models;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceInfo))]
namespace ServiceRequest.Droid.DependencyClasses
{
    public class DeviceInfo:IDeviceInfo
    {
        public DeviceInfoModel GetDeviceInfo()
        {
            return new DeviceInfoModel()
            {
                Name = Android.OS.Build.Manufacturer,
                ModelName = Android.OS.Build.Model,
                OsVersionName = Android.OS.Build.VERSION.Sdk,
                OsVersion = Android.OS.Build.VERSION.Sdk,
            };
        }
    }
}