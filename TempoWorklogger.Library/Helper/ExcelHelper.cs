using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoWorklogger.Library.Helper
{
    public static class ExcelHelper
    {
        public static int GetColumnIndex(ReadOnlySpan<char> col)
        {
            // Span<char> b64Result = stackalloc char[((b64Length + 4) * 2)];
            // col = AB
            // var lastCharPosition = col.lastChar().GetExcelColumnIndex(); // 2
            // var position = col.lengh * 26 - 26 + lastCharPosition; // 2 * 26 - 26 + 2 = 28
            // var index = position - 1;
            // TODO: if col.lenght > 1 => for GetExcelColumnIndex(col[i]) * 26

            var index = 0;
            if (col.Length > 1)
            {
                throw new NotImplementedException("Only from A-Z columns is supported");
                //var lastIndex = col.Length - 1;
                //for (int i = 0; i < lastIndex; i++)
                //{
                //    var range = GetColumnIndex(col[i].ToString()) + 1; // speed and memory bad perfomence
                    
                //    index += range * 26;
                //}

                // index += IntPow(26, (uint)(lastIndex)); if lenght > 2
                //index += 25 * (col.Length - 1); // shift it based on level
                //index += GetColumnIndex(col[lastIndex].ToString()); // last is in last level range
                //return index;
            }
            
            var letter = col[0];
            for (char c = 'A'; c < letter; c++)
            {
                index++;
            }

            return index;
        }
        private static int IntPow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;
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
