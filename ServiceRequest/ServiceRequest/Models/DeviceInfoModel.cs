using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequest.Models
{
    public class DeviceInfoModel
    {
        public string Name { get; set; }
        public string ModelName { get; set; }
        public string OsVersionName { get; set; }
        public string OsVersion { get; set; }
        public double FreeSpace { get; set; }
        public double TotalSpace { get; set; }
    }
}
