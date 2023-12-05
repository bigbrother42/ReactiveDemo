using BaseDemo.ClientBase;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDemo.Base
{
    public class ClientApiService : ApiServiceBase
    {
        protected SqLiteDbContext SqLiteDbContext;

        public ClientApiService()
        {
            SqLiteDbContext = new SqLiteDbContext(GlobalData.DbConnection);
        }
    }
}
