using System;
using System.IO;

namespace Plot3dReader
{
    class Program
    {
        private static FileInfo[] ReadFile()
        {
            DirectoryInfo d = new DirectoryInfo(@"D:\PyrosimFile\importCAD_3\importCAD_3");
            FileInfo[] fileList = d.GetFiles("*.txt");
            return fileList;
        }
        static void Main(string[] args)
        {
            string Folder = @"D:\PyrosimFile\importCAD_3\importCAD_3";
            string MeshNumber = args[0];
            string TargetSecond = args[1];
            double TemperatureThreshold = 70.0;

            foreach (var OutputFile in ReadFile())
            {
                string[] splitName = OutputFile.Name.Split("_");
                if (MeshNumber.Equals(splitName[2].TrimStart('0')) && TargetSecond.Equals(splitName[3].TrimStart('0')))
                {
                    string[] lines = File.ReadAllLines(Folder + @"\" + OutputFile.Name);
                    for (int l = 0; l < lines.Length; l++)
                    {
                        if (l > 1)
                        {
                            
                            string line = lines[l];
                            //Console.WriteLine(line);
                            string[] data = line.Split(',');
                            double x;
                            double.TryParse(data[0] , out x);
                            double y;
                            double.TryParse(data[1], out y);
                            double z;
                            double.TryParse(data[2], out z);
                            double visibility;
                            double.TryParse(data[3], out visibility);
                            double HRR;
                            double.TryParse(data[4], out HRR);
                            double Temperature;
                            double.TryParse(data[5], out Temperature);
                            double velocity;
                            double.TryParse(data[6], out velocity);
                            double unknow;
                            double.TryParse(data[7], out unknow);
                            if(Temperature > TemperatureThreshold)
                            {
                                Console.WriteLine(Temperature);
                                Console.WriteLine(String.Format("x , y , z = {0}, {1}, {2}", x , y , z ));
                            }
                        }
                    }
                }
            }
        }
    }
}
