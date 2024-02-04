using DataDemo.WebDto;
using ServiceDemo.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDemo.Common.SQLite
{
    public class SystemConfigService : ClientApiService
    {
        public async Task<SystemConfigWebDto> InsertOrUpdateSystemConfigAsync(SystemConfigWebDto param)
        {
            if (param == null) return null;

            var existSystemConfigItem = SqLiteDbContext.SystemConfig.FirstOrDefault(o => o.UserId == param.UserId && o.FunctionNo == param.FunctionNo && o.ItemNo == param.ItemNo);

            if (existSystemConfigItem == null)
            {
                // insert
                await SqLiteDbContext.SystemConfig.AddAsync(param);
            }
            else
            {
                // update
                existSystemConfigItem.ConfigContent = param.ConfigContent;
            }

            await SqLiteDbContext.SaveChangesAsync();

            return param;
        }

        public string GetSystemConfigContent(SystemConfigWebDto param)
        {
            if (param == null) return null;

            var result = string.Empty;
            var existSystemConfigItem = SqLiteDbContext.SystemConfig.FirstOrDefault(o => o.UserId == param.UserId && o.FunctionNo == param.FunctionNo && o.ItemNo == param.ItemNo);
            if (existSystemConfigItem != null)
            {
                result = existSystemConfigItem.ConfigContent;
            }

            return result;
        }
    }
}
