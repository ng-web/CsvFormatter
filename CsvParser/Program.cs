using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CsvParser
{
    class Program
    {
        static string FormatLine(string[] lineValues, int columns)
        {
            string line = "";
            for (int i = 0; i < columns; i++)
            {
                line += String.Format("{0}\t", lineValues[i]);
            }
            return line;
        }

        static string ProcessLines(int colCount, string lineValues, Version node)
        {
            string[] currentLine = lineValues.Split('\t');
            string newLine = "";
            //var count = 0;
            for (int i = 0; i < colCount; i++)
            {
                var currValue = currentLine[i];
                //check each column of each line to see which column is currency and remove white space

                var fieldType = node.Orderedfields.Type[i];
                //check each column of each line to see which column is currency and remove white space
                if ("currency".Equals(fieldType.ToLower()))
                {
                    currentLine[i] = Regex.Replace(currValue, " ", String.Empty);
                }
                currentLine[i] = Regex.Replace(currentLine[i], "\"", String.Empty);
            }

            for (int i = 0; i < colCount; i++)
            {
                newLine += String.Format("{0}\t", currentLine[i]); 
            }
            return newLine.TrimEnd();
        }
 
        public void FileWatcher()
        {
            var source = @"C:\DIPV4";
            var target = @"C:\Test Destination";
            while (true)
            {
                using (var folderWatcher = new FileSystemWatcher(source))
                {
                    folderWatcher.Filter = "*.csv";

                    Console.WriteLine("Watching " + source);
                    var change = folderWatcher.WaitForChanged(WatcherChangeTypes.Created, 1000 * 60);

                    if (!change.TimedOut)
                    {
                        Console.WriteLine("File detected: " + change.Name);
                        Console.WriteLine("Moving to: " + target);
                        File.Move(Path.Combine(source, change.Name), Path.Combine(target, change.Name));
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Dip dipConfig = null;
            var xmlString = File.ReadAllText(@"DipConfig.xml");
            int count, docLength = 0;

            

            string file = @"C:\DIPV4\apptest.csv";
            string destinationPath = @"C:\Processed Files";
            string destinationFile = @"C:\Processed Files\receipts.csv";

            string[] lines = File.ReadAllLines(file);

            using (var stream = new StringReader(xmlString))
            {
                var serializer = new XmlSerializer(typeof(Dip));
                dipConfig = (Dip)serializer.Deserialize(stream);
            }

            var colCount = 0;
           
            docLength = lines.Length;
            string[] newLines = new string[docLength];

            for (int i = 0; i < docLength; i++)
            {
                colCount = lines[i].Split('\t').Count();
                var node = dipConfig.Versions.Version.FirstOrDefault(q => q.NumberOfFields == colCount.ToString());
                if (node != null)
                {
                    newLines[i] = ProcessLines(colCount, lines[i], node);
                    
                } else{
                    //Exception Code
                    break;
                }
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            try
            {
                File.WriteAllLines(destinationFile, newLines);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Could not create file" + ex);
            }
            
            
        //Console.ReadLine();
        }

    }
}