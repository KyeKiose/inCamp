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
        private string name;
        private string date;
        private string hours;
        public Employee(string n, string d, string h)
        {
        Name = n;
        Date = d;
        Hours = h;
        }
        
        public string Name { get; set; }
        public string Date {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
        public string Hours {
            get 
            {
                return hours;
            }
            set
            {
                hours = value;
            }
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
        List<List<string>> list;
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
            list = new List<List<string>>();
            names = new List<string>();
            List<string> namesBuf = new List<string>();
            dates = new List<string>();
            List<string> datesBuf = new List<string>();
            List<string> oneDayHours = new List<string>();
            foreach (Employee e in input.list)
            {
                names.Add(e.Name);
                namesBuf.Add(e.Name);
                dates.Add(e.Date);
                datesBuf.Add(e.Date);
            }
            names = namesBuf.Union(names).ToList();
            names.Sort();
            dates = datesBuf.Union(dates).ToList();
            int counter = 0;
            foreach (string n in names)
            {
                list.Add(new List<string>());
                list[counter].Add(n);
                counter++;
            }
            List<Employee> sortedInput = input.list.OrderBy(x => x.Date).ThenBy(x => x.Name).ToList();
            output = new List<List<string>>();
            output.Add(new List<string>());
            output[0].Add("Name / Date");
            for (int i = 0; i < dates.Count; i++)
            {
                output[0].Add(dates[i]);
            }
            for (int i = 0; i < names.Count; i++)
            {
                output.Add(new List<string>());
                output[i + 1].Add(names[i]);
                for (int j = 0; j < dates.Count; j++)
                {
                    output[i + 1].Add("0");
                }
            }
            for (int i = 1; i < output[0].Count; i++)
            {
                string currentDate = output[0][i];
                for (int j = 1; j < output.Count; j++)
                {
                    string currentName = output[j][0];
                    var buf = sortedInput.Find(x => x.Name.Contains(currentName) && x.Date.Contains(currentDate));
                    if (buf != null)
                    {
                        output[j][i] = buf.Hours;
                    }
                }
            }
            for (int i = 1; i < output[0].Count; i++)
            {
                output[0][i] = Convert.ToDateTime(dates[i - 1]).ToString("yyyy-MM-dd");
            }
        }
        private void WriteFile(string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                for (int i = 0; i < names.Count + 1; i++)
                {
                    for (int j = 0; j <= dates.Count; j++)
                    {
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