using Maya.Ext.Rop;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using TempoWorklogger.Library.Helper;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Model.Tempo;

namespace TempoWorklogger.Library.Mapper.Tempo
{
    internal static class WorklogMapper
    {
        const int Zero = 0;

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

                worklog.TimeSpentSeconds = CalculateTimeSpentSeconds(worklog.TimeSpentSeconds, worklog.StartTime, worklog.EndTime);

                return Result<Worklog, (Exception, int)>.Succeeded(worklog);
            }
            catch (Exception e)
            {
                return Result<Worklog, (Exception, int)>.Failed((e, row.RowNum));
            }
        }

        private static int CalculateTimeSpentSeconds(int actualSeconds, TimeOnly startTime, TimeOnly endTime)
        {
            if (actualSeconds == Zero && startTime.Ticks != endTime.Ticks)
            {
                return (int)(endTime - startTime).TotalSeconds;
            }

            return actualSeconds;
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
                worklog.StartTime = TimeOnly.Parse(columnDefinition.Value);
            }

            if (columnDefinition.Name == nameof(Worklog.EndTime))
            {
                worklog.EndTime = TimeOnly.Parse(columnDefinition.Value);
            }

            if (columnDefinition.Name == nameof(Worklog.TimeSpentSeconds))
            {
                worklog.TimeSpentSeconds = Convert.ToInt32(columnDefinition.Value);
            }

            if (columnDefinition.Name == nameof(Worklog.StartDate))
            {
                worklog.StartDate = DateOnly.Parse(columnDefinition.Value);
            }

            if (columnDefinition.Name == nameof(Worklog.Description))
            {
                worklog.Description = columnDefinition.Value;
            }

            if (columnDefinition.Name == nameof(Worklog.AuthorAccountId))
            {
                worklog.AuthorAccountId = columnDefinition.Value;
            }

            if (columnDefinition.Name.StartsWith(nameof(AttributeKeyVal)))
            {
                var attrKeyVal = new AttributeKeyVal();
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
                worklog.StartTime = TimeOnly.FromDateTime(cell.DateCellValue);
            }

            if (columnDefinition.Name == nameof(Worklog.EndTime))
            {
                worklog.EndTime = TimeOnly.FromDateTime(cell.DateCellValue);
            }

            if (columnDefinition.Name == nameof(Worklog.TimeSpentSeconds))
            {
                worklog.TimeSpentSeconds = Convert.ToInt32(cell);
            }

            if (columnDefinition.Name == nameof(Worklog.StartDate))
            {
                worklog.StartDate = DateOnly.FromDateTime(cell.DateCellValue);
            }

            if (columnDefinition.Name == nameof(Worklog.Description))
            {
                worklog.Description = cell.ToString();
            }

            if (columnDefinition.Name == nameof(Worklog.AuthorAccountId))
            {
                worklog.AuthorAccountId = cell.ToString();
            }

            if (columnDefinition.Name.StartsWith(nameof(AttributeKeyVal)))
            {
                var attrKeyVal = new AttributeKeyVal();
                worklog.Attributes.Add(attrKeyVal.MapFromColumnDefinitionExcelCell(columnDefinition, cell));
            }

            return worklog;
        }
    }
}
