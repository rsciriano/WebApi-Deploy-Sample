using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Queries.ViewModels
{
    public class SystemInfoViewModel
    {
        public string OSVersion { get; set; }

        public string DatabaseVersionString { get; set; }

        public string MachineName { get; internal set; }
        public string DatabaseType { get; internal set; }
    }
}
