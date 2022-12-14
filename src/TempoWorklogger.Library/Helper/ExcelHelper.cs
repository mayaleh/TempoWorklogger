using System.Linq;

namespace TempoWorklogger.Library.Helper
{
    public static class ExcelHelper
    {
        public static int GetColumnIndex(ReadOnlySpan<char> columnName)
        {
            var columnIndex = 0;

            foreach (char c in columnName)
            {
                columnIndex *= 26;
                columnIndex += (c - 'A' + 1);
            }

            return columnIndex - 1;
        }

        [Obsolete]
        public static int GetColumnIndexV1(ReadOnlySpan<char> col)
        {
            var index = 0;
            if (col.Length > 1)
            {
                throw new NotImplementedException("Only from A-Z columns is supported");
            }
            
            var letter = col[0];
            for (char c = 'A'; c < letter; c++)
            {
                index++;
            }

            return index;
        }

        private static string GetExcelColumnName(int columnNumber) // TODO not number but index (index = number - 1)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
