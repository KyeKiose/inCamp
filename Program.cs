using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
namespace task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = @"acme_worksheet.csv";
            string outputPath = @"output.csv";
            Input input = new Input(inputPath);
            Output output = new Output(input, outputPath);
        }
    }
    public class Employee
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Hours { get; set; }
        public Employee(string n, string d, string h)
        {
        Name = n;
        Date = d;
        Hours = h;
        }
    }
    public class Input
    {
        public List<Employee> list;
        public Input(string path)
        {
            list = new List<Employee>();
            ReadFile(path);
        }
        private void ReadFile(string path)
        {
            int counter = 0;
            using (TextFieldParser str = new TextFieldParser(path, System.Text.Encoding.Default))
            {
                str.TextFieldType = FieldType.Delimited;
                str.SetDelimiters(",");
                while (!str.EndOfData)
                {
                    string[] fields = str.ReadFields();
                    if (counter != 0)
                    {
                        list.Add(new Employee(fields[0], fields[1], fields[2]));
                    }
                    counter++;
                }
            }
        }
    }
    public class Output
    {
        List<List<string>> output;
        List<string> names;
        List<string> dates;
        public Output(Input input, string path)
        {
            FormationTable(input);
            WriteFile(path);
        }
        private void FormationTable(Input input)
        {
            names = new List<string>();
            dates = new List<string>();
            foreach (Employee e in input.list)
            {
                names.Add(e.Name);
                dates.Add(e.Date);
            }
            names = names.Union(names).ToList();
            names.Sort();
            dates = dates.Union(dates).ToList();
            List<Employee> sortedInput = input.list.OrderBy(x => x.Date).ThenBy(x => x.Name).ToList();
            output = new List<List<string>>();
            output.Add(new List<string>());
            output[0].Add("Name / Date");

            for (int j = 0; j < names.Count; j++)
            {
                output.Add(new List<string>());
                output[j + 1].Add(names[j]);
            }
            for (int i = 1; i < dates.Count + 1; i++)
            {
                output[0].Add(dates[i - 1]);
                string currentDate = output[0][i];
                for (int j = 1; j < output.Count; j++)
                {
                    string currentName = output[j][0];
                    var buf = sortedInput.Find(x => x.Name.Contains(currentName) && x.Date.Contains(currentDate));
                    if (buf != null)
                    {
                        output[j].Add(buf.Hours);
                    }
                    else
                    {
                        output[j].Add("0");
                    }
                }
            }
        }
        private void WriteFile(string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                for (int i = 0; i < output.Count; i++)
                {
                    for (int j = 0; j < output[0].Count; j++)
                    {
                        if(j != 0 && i == 0)
                        {
                            output[i][j] = Convert.ToDateTime(dates[j - 1]).ToString("yyyy-MM-dd");
                        }
                        outputFile.Write(output[i][j]);
                        if(j != dates.Count)
                        {
                            outputFile.Write(",");
                        }
                    }
                    outputFile.Write("\n");
                }
            }
        }
    }
}