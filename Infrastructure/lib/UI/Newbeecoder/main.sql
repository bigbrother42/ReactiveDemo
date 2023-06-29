Type=SqlExpress
<--支持SqlServer、MySql、Sqlite数据库-->
【查询】
--条件查询返回数量
var qRet = db.Query<ViewSocre>().Where(p => p.ID == 1).Count();
--条件查询返回第一条
var qRet = db.Query<ViewSocre>().Where(p => p.StudentName == "赵可").FirstOrDefault();
var qRet = DbService.Default.QueryFirstOrDefault<ViewSocre>(p => p.StudentName == "赵可").Data;
--Top记录查询
var query = db.Query<ViewSocre>();
var qRet = query.OrderBy(p => p.ID, Data.OrderByType.Asc).Top(1);
--使用In和NotIn查询
var qRet = db.Query<ViewSocre>().Where(p => p.StudentName.In("赵可")).ToList();
var qRet = db.Query<ViewSocre>().Where(p => p.StudentName.NotIn("赵可")).ToList();
--查询符合条件所有数据
DbService.Default.FindAll<ViewSocre>(p => p.StudentName == "赵可").ToList();
--多个条件分页查询
var query = db.Query<ViewSocre>();
query.And(p => p.StudentName.Like($"%{this.SearchStudentName}%"));
query.And(p => (p.StartDate.Between(SearchStartDate.Value, SearchEndDate.Value)
var qRet = query.OrderBy(p => p.ID, OrderByType.Asc).Page(this.PageNo, this.PageSize);
【新增】
--新增记录
DbService.Default.Insert(model);
db.Insert<ScoreInfo>().Values(model).Execute().Code;
--新增符合条件的多条记录
db.Insert<ScoreInfo>().Values(items.Select(p=>new ScoreInfo() { ID = model.ID, GradeNo == 3}).ToList()).Execute().Code
【更新】
--更新表
DbService.Default.Update(model);;
db.Update<ScoreInfo>().SetModel(model).Execute().Code;
--更新表多条记录
var qRet = db.Query<CarModelInfo>().ToList();
db.Update<ScoreInfo>().SetModels(qRet.Data).Execute().Code
--更新符合条件的学生名字
var qRet = db.Query<ScoreInfo>().Where(p => p.StudentName == "赵可").FirstOrDefault();
var eRet = db.Update<ScoreInfo>().Set(new { StudentName == "赵三" }, p => p.ID == qRet.ID).Execute();
--更新多个字段
var models = Items.SelectMany(g => g.StudentName.Select(x => { var model = new ViewSocre(); x.PropertyCopyTo(model); return model; }));
DbService.Default.CreateDb().Update<ScoreInfo>().SetModels(models).SetUpdateProperties("StudentName", "Chinese").Execute().Code
【删除】
--删除表记录
db.Delete<ScoreInfo>().Where(p=>p.ID == item.ID).Execute().Code;
DbService.Default.Delete<ScoreInfo>(p => p.ID == model.ID);
【Excel导入导出】
--导出Excel
DataGridExportHelper.ExportAsync<ViewSocre>(new DataAccessMgrPage().Table);
--导入数据
ExcelImportHelper.Load<ScoreImportModel>(fd.FileName);