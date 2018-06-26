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
                //line += (i < lineValues.Length - 1) ? lineValues[i] + "\t" : lineValues[i] + "\r\n";
                //lineValues[i].Remove(line.Length - 1);  //removes trailing tab if necessary
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

                foreach (var fieldType in node.Orderedfields.Type)
                {
                    Console.WriteLine(currValue);
                    if (!String.IsNullOrEmpty(currValue))
                    {
                        if ("currency".Equals(fieldType.ToLower()))
                        {
                            currentLine[i] = Regex.Replace(currValue, " ", String.Empty);
                        }
                    }
                    //count++;
                }
                currentLine[i] = Regex.Replace(currentLine[i], "\"", String.Empty);
            }

            for (int i = 0; i < colCount; i++)
            {
                newLine += String.Format("{0}\t", lineValues[i]); 
                //newLine += (i < currentLine.Length - 1) ? currentLine[i] + "\t" : currentLine[i] + "\r\n";
                //lineValues[i].Remove(line.Length - 1);  //removes trailing tab if necessary
            }
            return newLine;
        }

        
        public static List<string> GetAllCsv()
        {
            string FolderPath = @"C:\DIPV4";
            DirectoryInfo di = new DirectoryInfo(FolderPath);

            // Get only the CSV files.
            List<string> csvFiles = di.GetFiles("*.csv")
                                      .Where(file => file.Name.EndsWith(".csv"))
                                      .Select(file => file.Name).ToList();

            //Console.WriteLine(csvFiles.Count);
            return csvFiles;
        }

        //static string ReadAllLinesCsv()
        //{
        //    string[] lines = File.ReadAllLines(filename);
        //    foreach (string line in lines)
        //    {
        //        string[] col = line.Split(new char[] { ',' });
        //        // process col[0], col[1], col[2]
        //    }
        //}

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
           

            

            //getting the # of fields in version
            

            
            

            //for (int i = 0; i < docLength; i++) //change to docLength
            //{
            //    //remove all quotes
            //    lines[i] = Regex.Replace(lines[i], "\"", String.Empty);
            //    //if (lines[i].Contains(allowedChars))
            //    //{
            //    //    lines[i] = Regex.Replace(lines[i], " ", String.Empty);
            //    //}
            //    //Console.WriteLine(lines[i]);
            //    //Console.ReadLine();
            //}
            File.WriteAllLines(@"C:\Test Destination\test.csv", newLines);
            
        Console.ReadLine();
        }

                /*
                 * Modify code to:
                 * 1. Get number of columns in row
                 * 2. Search config file for corresponding version number, based on number of fields and number of columns
                 * 3. write new line accordingly
                 * 4. if corresponding config is not found, then write to exception file and/or logs
                 * 5. check out the empty values (how to set a value if needs be
                 * 
                 * 
                 */
                //gets # of columns in row
            //    var colCount = lines[0].Split('\t').Count();
            //    lines = lines[0].Split('\t');

            //    for (int i = 0; i < lines.Length; i++) //change to docLength
            //    {
            //        //remove all quotes
            //        lines[i] = Regex.Replace(lines[i], "\"", String.Empty);
            //        Console.WriteLine(lines[i]);
            //        Console.WriteLine();
            //    }

            //    Console.WriteLine();
            //    Console.WriteLine("No. of columns: {0}", colCount);
            //    Console.WriteLine("No. of Lines: {0}", docLength);
            //    Console.WriteLine();

            //using (var stream = new StringReader(xmlString))
            //{
            //    var serializer = new XmlSerializer(typeof(Dip));
            //    dipConfig = (Dip)serializer.Deserialize(stream);
            //}

            ////getting the # of fields in version
            //var node = dipConfig.Versions.Version.FirstOrDefault(q => q.NumberOfFields == colCount.ToString());
            //if (node != null)
            //    Console.WriteLine("No. of fields: {0}", node.NumberOfFields);

            //    string[] newLines = new string[docLength];
            //    for (int i = 0; i < docLength; i++)
            //    {
            //        lineValues = lines;
            //        //check column number here and check for corresponding node object.
            //        //if null, write to exception file, and log the failure 
            //        //if not null, then continue the logic below. 
            //        count = 0;
            //        var currValue = "";
            //        if (lineValues.Length == int.Parse(node.NumberOfFields))
            //        {
            //            foreach (var fieldType in node.Orderedfields.Type)
            //            {
            //                currValue = lineValues[count];
            //                if (!String.IsNullOrEmpty(currValue))
            //                {
            //                    if ("currency".Equals(fieldType.ToLower()))
            //                    {
            //                        lineValues[count] = Regex.Replace(currValue, " ", String.Empty);
            //                    }
            //                }
            //                //else
            //                //{
            //                //    lineValues[count] = "null"; //if this is whats causing onBase to fail
            //                //}
            //                count++;
            //                //Console.WriteLine("The Count is: " + count);
            //                //Console.WriteLine("Number : " + lineValues[1]);
            //                //Console.WriteLine();
            //            }
            //            newLines[i] = FormatLine(lineValues, colCount);

            //            //    if(colCount==26)
            //            //    {
            //            //        File.WriteAllLines(@"C:\Test Destination\V4\testver4.csv",newLines);
            //            //    }
            //            //    else if (colCount==23)
            //            //    {
            //            //        File.WriteAllLines(@"C:\Test Destination\V3\testver3.csv", newLines);
            //            //    }
            //            //}
            //            //else
            //            //{

            //            //   File.WriteAllLines(@"C:\Test Destination\Ex\testex.csv", newLines);
            //            //    //Console.WriteLine("The Line is: " + i);
            //            //    //Console.WriteLine();      
            //            //}
            //        }

            //        //write to csv file newLines array **To Do**

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.InnerException);
            //}

            //foreach (var version in dipConfig.Versions.Version)
            //{
            //    //foreach (var i in version.Orderedfields.Type) //iterates through the field types
            //    //{
            //    //    Console.WriteLine(i);
            //    //}
            //    Console.WriteLine(version.NumberOfFields);

            //    foreach (var fieldType in version.Orderedfields.Type)
            //    {
            //        Console.WriteLine(fieldType);
            //    }
            //}

            //        //  nodename = versionNode.Name;

            //    }

            //}
            //file.Close();
            //System.Console.WriteLine("There were {0} lines.", counter);

            //Console.WriteLine(xmlcontents);


    }
}