using Maya.Ext.Rop;
using NPOI.SS.UserModel;
using TempoWorklogger.Library.Helper;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Library.Mapper.Db
{
    public static class WorklogFromNPOI
    {
        public static Result<Worklog, (Exception Exception, int RowNr)> MapRowByImportToWorklog(this IRow row, ImportMap importMap)
        {
            try
            {
                var worklog = new Worklog();

                foreach (var item in importMap.ColumnDefinitions)
                {
                    if (item.IsStatic)
                    {
                        worklog = worklog.MapColumnDefinitionStaticDataToWorklog(item);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Position))
                    {
                        continue;
                    }

                    var cellIndex = ExcelHelper.GetColumnIndex(item.Position);
                    worklog = worklog.MapColumnDefinitionExcelRowToWorklog(item, row.GetCell(cellIndex));
                }
                
                FixStartEndDate(worklog);
                
                worklog.TimeSpentSeconds = WorklogHelper.CalculateTimeSpentSeconds(worklog.TimeSpentSeconds, worklog.StartTime, worklog.EndTime);

                return Result<Worklog, (Exception, int)>.Succeeded(worklog);
            }
            catch (Exception e)
            {
                return Result<Worklog, (Exception, int)>.Failed((e, row.RowNum));
            }
        }

        private static void FixStartEndDate(Worklog worklog)
        {
            if (worklog.EndTime.Year == CommonConstants.Zero && worklog.StartTime.Year != CommonConstants.Zero)
            {
                worklog.EndTime = new DateTime(
                    worklog.StartTime.Year,
                    worklog.StartTime.Month,
                    worklog.StartTime.Day,
                    worklog.EndTime.Hour,
                    worklog.EndTime.Minute,
                    worklog.EndTime.Second);
            }

            if (worklog.StartTime.Year == CommonConstants.Zero && worklog.EndTime.Year != CommonConstants.Zero)
            {
                worklog.StartTime = new DateTime(
                    worklog.EndTime.Year,
                    worklog.EndTime.Month,
                    worklog.EndTime.Day,
                    worklog.StartTime.Hour,
                    worklog.StartTime.Minute,
                    worklog.StartTime.Second);
            }
        }

        /// <summary>
        /// Map column definition static value to Worklog model
        /// It is not returning new instance!
        /// </summary>
        /// <param name="worklog"></param>
        /// <param name="columnDefinition"></param>
        /// <returns></returns>
        public static Worklog MapColumnDefinitionStaticDataToWorklog(this Worklog worklog, ColumnDefinition columnDefinition)
        {
            if (columnDefinition.Name == nameof(Worklog.IssueKey))
            {
                worklog.IssueKey = columnDefinition.Value;
            }

            if (columnDefinition.Name == nameof(Worklog.StartTime))
            {
                var startTime = TimeOnly.Parse(columnDefinition.Value);
                worklog.StartTime = new DateTime(
                    worklog.StartTime.Year,
                    worklog.StartTime.Month,
                    worklog.StartTime.Day,
                    startTime.Hour,
                    startTime.Minute,
                    startTime.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.EndTime))
            {
                var endTime = TimeOnly.Parse(columnDefinition.Value);
                worklog.EndTime = new DateTime(
                    worklog.EndTime.Year,
                    worklog.EndTime.Month,
                    worklog.EndTime.Day,
                    endTime.Hour,
                    endTime.Minute,
                    endTime.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.TimeSpentSeconds))
            {
                worklog.TimeSpentSeconds = Convert.ToInt32(columnDefinition.Value);
            }

            if (columnDefinition.Name == nameof(Worklog.StartDate))
            {
                var startDate = DateOnly.Parse(columnDefinition.Value);
                worklog.StartTime = new DateTime(
                    startDate.Year,
                    startDate.Month,
                    startDate.Day,
                    worklog.StartTime.Hour,
                    worklog.StartTime.Minute,
                    worklog.StartTime.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.EndDate))
            {
                var endDate = DateOnly.Parse(columnDefinition.Value);
                worklog.EndTime = new DateTime(
                    endDate.Year,
                    endDate.Month,
                    endDate.Day,
                    worklog.EndTime.Hour,
                    worklog.EndTime.Minute,
                    worklog.EndTime.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.Description))
            {
                worklog.Description = columnDefinition.Value;
            }

            //if (columnDefinition.Name == nameof(Worklog.AuthorAccountId))
            //{
            //    worklog.AuthorAccountId = columnDefinition.Value;
            //}

            if (columnDefinition.Name.StartsWith(ModelsConstant.AttributePrefix))
            {
                var attrKeyVal = new CustomAttributeKeyVal();
                worklog.Attributes.Add(attrKeyVal.MapFromColumnDefinitionStaticData(columnDefinition));
            }

            return worklog;
        }

        /// <summary>
        /// Map cell values from excel to Worklog model
        /// It is not returning new instance!
        /// </summary>
        /// <param name="worklog"></param>
        /// <param name="columnDefinition"></param>
        /// <returns></returns>
        public static Worklog MapColumnDefinitionExcelRowToWorklog(this Worklog worklog, ColumnDefinition columnDefinition, ICell cell)
        {

            if (columnDefinition.Name == nameof(Worklog.IssueKey))
            {
                worklog.IssueKey = cell.ToString();
            }

            if (columnDefinition.Name == nameof(Worklog.StartTime))
            {
                worklog.StartTime = new DateTime(
                    worklog.StartTime.Year,
                    worklog.StartTime.Month,
                    worklog.StartTime.Day,
                    cell.DateCellValue.Hour,
                    cell.DateCellValue.Minute,
                    cell.DateCellValue.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.StartDate))
            {
                worklog.StartTime = new DateTime(
                    cell.DateCellValue.Year,
                    cell.DateCellValue.Month,
                    cell.DateCellValue.Day,
                    worklog.StartTime.Hour,
                    worklog.StartTime.Minute,
                    worklog.StartTime.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.EndTime))
            {
                worklog.EndTime = new DateTime(
                    worklog.EndTime.Year,
                    worklog.EndTime.Month,
                    worklog.EndTime.Day,
                    cell.DateCellValue.Hour,
                    cell.DateCellValue.Minute,
                    cell.DateCellValue.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.EndDate))
            {
                worklog.EndTime = new DateTime(
                    cell.DateCellValue.Year,
                    cell.DateCellValue.Month,
                    cell.DateCellValue.Day,
                    worklog.EndTime.Hour,
                    worklog.EndTime.Minute,
                    worklog.EndTime.Second);
            }

            if (columnDefinition.Name == nameof(Worklog.TimeSpentSeconds))
            {
                worklog.TimeSpentSeconds = Convert.ToInt32(cell);
            }

            if (columnDefinition.Name == nameof(Worklog.Description))
            {
                worklog.Description = cell.ToString();
            }

            //if (columnDefinition.Name == nameof(Worklog.AuthorAccountId))
            //{
            //    worklog.AuthorAccountId = cell.ToString();
            //}


            if (columnDefinition.Name.StartsWith(ModelsConstant.AttributePrefix))
            {
                var attrKeyVal = new CustomAttributeKeyVal();
                worklog.Attributes.Add(attrKeyVal.MapFromColumnDefinitionExcelCell(columnDefinition, cell));
            }

            return worklog;
        }
    }
}
