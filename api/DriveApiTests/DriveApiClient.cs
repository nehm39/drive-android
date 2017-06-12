using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriveApi;

namespace DriveApiTests
{
    class DriveApiClient
    {
        IService service;

        public DriveApiClient()
        {
            this.service = new Service();
        }
    }
}
