using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvParser
{
    public abstract class CsvableBase
    {
        public virtual string ToCsv()
        {
            string output = "";

            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                output += properties[i].GetValue(this).ToString();
                if (i != properties.Length - 1)
                {
                    output += "\t";
                }
            }

            return output;
        }

        private string PreProcess(string input)
        {
            input = input.Replace('ı', 'i')
                .Replace('ç', 'c')
                .Replace('ö', 'o')
                .Replace('ş', 's')
                .Replace('ü', 'u')
                .Replace('ğ', 'g')
                .Replace('İ', 'I')
                .Replace('Ç', 'C')
                .Replace('Ö', 'O')
                .Replace('Ş', 'S')
                .Replace('Ü', 'U')
                .Replace('Ğ', 'G')
                .Replace("\"", String.Empty)
                .Trim();
            //if (input.Contains("\t"))
            //{
            //    input = "\"" + input + "\"";
            //}
            return input;
        }
    }
}
