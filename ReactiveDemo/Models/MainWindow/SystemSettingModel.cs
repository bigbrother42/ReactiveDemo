using DataDemo.WebDto;
using ServiceDemo.Common.SQLite;
using SharedDemo.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models.MainWindow
{
    public class SystemSettingModel
    {
        private SystemConfigService _systemConfigService = new SystemConfigService();

        public async Task SaveSystemColor(string colorStr)
        {
            await _systemConfigService.InsertOrUpdateSystemConfigAsync(new SystemConfigWebDto
            {
                UserId = LoginInfo.UserBasicInfo.UserId,
                FunctionNo = "LN010001",
                ItemNo = "SystemColor",
                ItemName = "theme name of system",
                ConfigContent = colorStr,
                UpdateAt = DateTime.Now,
                UpdateBy = LoginInfo.UserBasicInfo.UserId,
                CreateAt = DateTime.Now,
                CreateBy = LoginInfo.UserBasicInfo.UserId
            });
        }
    }
}
