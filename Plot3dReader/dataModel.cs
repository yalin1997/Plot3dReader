using System;
using System.Collections.Generic;
using System.Text;


namespace Plot3dReader
{
    public class dataModel
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double visibility { get; set; }
        public double HRR { get; set; }
        public double Temperature { get; set; }
        public double velocity { get; set; }
        public double unknow { get; set; }
        public List<double> type2QuantityList { get; set; }
        public List<double> type3QuantityList { get; set; }
    }
}
