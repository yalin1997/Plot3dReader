using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Plot3dReader
{
    class DataFunction
    {
        double lowerBoundX = -20;
        double upperBoundX = 6;
        double lowerBoundY = -4;
        double upperBoundY = 22;
        double lowerBoundZ = 0;
        double upperBoundZ = 3;
        double fireX = -18;
        double fireY = 0;
        double fireZ = 0;
        private static FileInfo[] ReadFile()
        {
            DirectoryInfo d = new DirectoryInfo(@"D:\PyrosimFile\importCAD_3\importCAD_3");
            FileInfo[] fileList = d.GetFiles("*.txt");
            return fileList;
        }
        public static void readType1(string Folder, Dictionary<int, List<dataModel>> smokePointDic)
        {
            //Folder = @"D:\PyrosimFile\importCAD_3\importCAD_3";
            double TemperatureThreshold = 70.0;
            foreach (var OutputFile in ReadFile())
            {
                string[] splitName = OutputFile.Name.Split("_");
                int fileMesh = int.Parse(splitName[2].TrimStart('0'));
                int fileSecond = int.Parse(splitName[3].TrimStart('0'));
                string[] lines = File.ReadAllLines(Folder + @"\" + OutputFile.Name);
                if (!smokePointDic.ContainsKey(fileSecond))
                {
                    smokePointDic.Add(fileSecond, new List<dataModel>());
                }
                //using (StreamWriter file = new StreamWriter(string.Format("output_M_{0}_S_{1}", fileMesh, fileSecond)))
                //{
                for (int l = 0; l < lines.Length; l++)
                {
                    if (l > 1)
                    {
                        string line = lines[l];
                        //Console.WriteLine(line);
                        string[] data = line.Split(',');
                        double x;
                        double.TryParse(data[0], out x);
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
                        if (Temperature > TemperatureThreshold)
                        {
                            Console.WriteLine(String.Format("x , y , z = {0}, {1}, {2}", x, y, z));
                            Console.WriteLine(Temperature);
                            dataModel tempData = new dataModel()
                            {
                                x = x,
                                y = y,
                                z = z,
                                visibility = visibility,
                                HRR = HRR,
                                Temperature = Temperature,
                                velocity = velocity,
                                unknow = unknow
                            };
                            smokePointDic[fileSecond].Add(tempData);
                            //file.WriteLine(JsonConvert.SerializeObject(tempData));
                        }
                    }
                }
            }
        }
        public static void readOtherPlot3D(string Folder, Dictionary<int, List<dataModel>> smokePointDic , int flag)
        {
            foreach (var OutputFile in ReadFile())
            {
                string[] splitName = OutputFile.Name.Split("_");
                int fileMesh = int.Parse(splitName[2].TrimStart('0'));
                int fileSecond = int.Parse(splitName[3].TrimStart('0'));
                string[] lines = File.ReadAllLines(Folder + @"\" + OutputFile.Name);
                if (!smokePointDic.ContainsKey(fileSecond))
                {
                    smokePointDic.Add(fileSecond, new List<dataModel>());
                }
                //using (StreamWriter file = new StreamWriter(string.Format("output_M_{0}_S_{1}", fileMesh, fileSecond)))
                //{
                for (int l = 0; l < lines.Length; l++)
                {
                    if (l > 1)
                    {
                        string line = lines[l];
                        //Console.WriteLine(line);
                        string[] data = line.Split(',');
                        double x = double.Parse(data[0]);
                        double y = double.Parse(data[1]);
                        double z = double.Parse(data[2]);
                        double q4 = double.Parse(data[3]);
                        double q5 = double.Parse(data[4]);
                        double q6 = double.Parse(data[5]);
                        double q7 = double.Parse(data[6]);
                        double q8 = double.Parse(data[7]);
                        dataModel tempData = new dataModel()
                        {
                            x = x,
                            y = y,
                            z = z,
                            type2QuantityList = new List<double>()
                            {
                                q4, q5, q6, q7, q8
                            }
                        };
                        findDataModel(smokePointDic[fileSecond], tempData, flag);
                    }
                }
            }
        }
        public static void findDataModel(List<dataModel> pointList , dataModel targetData, int flag)
        {
            foreach(dataModel item in pointList)
            {
                if(item.x == targetData.x && item.y == targetData.y && item.z == targetData.z)
                {
                    if (flag.Equals(2))
                    {
                        item.type2QuantityList = targetData.type2QuantityList;
                        break;
                    }
                    else
                    {
                        item.type3QuantityList = targetData.type2QuantityList;
                        break;
                    }
                }
            }
        }
        public static void writeForm1Data(string Folder , Dictionary<int, List<dataModel>> smokePointDic)
        {
            foreach(int key in smokePointDic.Keys)
            {
                using(StreamWriter file = new StreamWriter(string.Format("output_S_{0}.txt", key)))
                {
                    file.Write(JsonConvert.SerializeObject(smokePointDic[key]));
                }
            }
        }
        public static void writeCsvData(string Folder, Dictionary<int, List<dataModel>> smokePointDic)
        {
            foreach(int key in smokePointDic.Keys)
            {
                using (StreamWriter file = new StreamWriter(string.Format("fireData_output.csv", key)))
                {
                    List<dataModel> tempList = smokePointDic[key];
                    file.Write("time, v1,v2,v3,v4,v5,v6,v7,v8,velocity");
                    foreach (dataModel item in tempList){
                        string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", key, item.x, item.y, item.z, item.visibility,item.HRR, item.Temperature, item.unknow, item.velocity);
                    }
                }
            }
        }
    }
}
