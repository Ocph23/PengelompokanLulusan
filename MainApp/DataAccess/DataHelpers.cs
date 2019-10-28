using MainApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess
{
    public class DataHelpers
    {
        public static List<Gelombang> DataGelombang
        {
            get
            {
                return new List<Gelombang> {
            new Gelombang{Nama="I",Nilai=1 },
           new Gelombang{Nama="II",Nilai=2 },
            new Gelombang{Nama="III",Nilai=3 },
        };
            }
        }


        public static List<Suku> DataSuku
        {
            get
            {
                return new List<Suku> {
            new Suku{Nama="Asli Papua",Nilai=1 },
           new Suku{Nama="Non Papua",Nilai=2 },
        };
            }
        }


        public static double[][] CentroidAwalSample(int numClusters)
        {
            // convenience matrix allocator for Cluster()
            /* double[][] result = new double[numClusters][];
             for (int k = 0; k < numClusters; ++k)
                 result[k] = new double[numColumns];*/
            double[][] rawData = new double[numClusters][];
            rawData[0] = new double[] { 2, 4, 3.44, 2, 5.3, 4 };
            rawData[1] = new double[] { 1, 2, 2.92, 2, 4.5, 4 };
            rawData[2] = new double[] { 2, 2, 3.30, 2, 5.0, 4 };
            rawData[3] = new double[] { 1, 1, 3.05, 1, 3.3, 4 };
            return rawData;
        }


        public static double[][] DataSample
        {
            get
            {
                double[][] rawData = new double[90][];
                rawData[0] = new double[] { 1, 1, 3.29, 1, 5.5, 4 };
                rawData[1] = new double[] { 2, 3, 3.26, 2, 3.8, 4 };
                rawData[2] = new double[] { 2, 3, 3.90, 1, 5.5, 3.5 };
                rawData[3] = new double[] { 1, 3, 3.13, 1, 4.0, 4 };
                rawData[4] = new double[] { 1, 2, 3.51, 1, 3.8, 3.5 };
                rawData[5] = new double[] { 1, 2, 2.72, 1, 4.3, 5.5 };
                rawData[6] = new double[] { 2, 2, 3.31, 1, 4.0, 4 };
                rawData[7] = new double[] { 2, 3, 3.78, 1, 4.8, 3.5 };
                rawData[8] = new double[] { 2, 3, 3.10, 1, 3.8, 5 };
                rawData[9] = new double[] { 1, 3, 3.19, 1, 5.3, 4.5 };
                rawData[10] = new double[] { 1, 2, 2.71, 1, 3.3, 4.5 };
                rawData[11] = new double[] { 2, 4, 3.44, 2, 5.3, 4 };
                rawData[12] = new double[] { 1, 1, 3.05, 1, 3.8, 4 };
                rawData[13] = new double[] { 1, 3, 3.24, 1, 4.5, 4 };
                rawData[14] = new double[] { 1, 2, 3.28, 2, 5.0, 5.5 };
                rawData[15] = new double[] { 2, 1, 3.89, 1, 5.0, 4 };
                rawData[16] = new double[] { 2, 3, 3.28, 1, 5.0, 3.5 };
                rawData[17] = new double[] { 2, 3, 3.34, 1, 4.3, 4 };
                rawData[18] = new double[] { 2, 3, 3.44, 1, 4.5, 4 };
                rawData[19] = new double[] { 2, 1, 3.28, 1, 5.0, 4.5 };
                rawData[20] = new double[] { 2, 3, 3.06, 1, 5.0, 4 };
                rawData[21] = new double[] { 2, 2, 3.14, 2, 4.8, 4 };
                rawData[22] = new double[] { 2, 3, 3.43, 1, 6.3, 3.5 };
                rawData[23] = new double[] { 1, 2, 2.71, 1, 2.5, 4.5 };
                rawData[24] = new double[] { 2, 5, 3.49, 1, 7.0, 4.5 };
                rawData[25] = new double[] { 2, 3, 3.41, 2, 4.5, 4 };
                rawData[26] = new double[] { 2, 1, 2.94, 2, 5.3, 5 };
                rawData[27] = new double[] { 2, 3, 3.17, 2, 4.8, 4 };
                rawData[28] = new double[] { 2, 2, 3.51, 1, 3.8, 3.5 };
                rawData[29] = new double[] { 2, 2, 3.07, 1, 3.3, 4.5 };
                rawData[30] = new double[] { 2, 3, 3.22, 1, 4.8, 3.5 };
                rawData[31] = new double[] { 2, 1, 3.69, 2, 4.0, 4 };
                rawData[32] = new double[] { 1, 2, 3.10, 1, 3.8, 4 };
                rawData[33] = new double[] { 2, 2, 3.06, 1, 4.0, 5 };
                rawData[34] = new double[] { 1, 2, 3.07, 1, 4.8, 5 };
                rawData[35] = new double[] { 1, 3, 3.09, 1, 5.0, 5.5 };
                rawData[36] = new double[] { 1, 2, 2.85, 1, 2.0, 4.5 };
                rawData[37] = new double[] { 1, 2, 2.54, 3, 4.8, 6.5 };
                rawData[38] = new double[] { 2, 2, 2.90, 1, 5.8, 6 };
                rawData[39] = new double[] { 2, 5, 2.58, 1, 7.0, 7 };
                rawData[40] = new double[] { 2, 2, 3.59, 2, 6.0, 4 };
                rawData[41] = new double[] { 2, 5, 3.03, 1, 4.8, 4 };
                rawData[42] = new double[] { 2, 3, 3.06, 1, 4.8, 4 };
                rawData[43] = new double[] { 2, 1, 3.04, 1, 5.0, 4 };
                rawData[44] = new double[] { 2, 3, 2.99, 1, 5.5, 4 };
                rawData[45] = new double[] { 2, 2, 3.10, 1, 3.3, 4 };
                rawData[46] = new double[] { 1, 3, 2.83, 2, 4.8, 4 };
                rawData[47] = new double[] { 2, 2, 2.94, 2, 5.0, 4 };
                rawData[48] = new double[] { 2, 2, 3.27, 2, 5.5, 4 };
                rawData[49] = new double[] { 2, 2, 3.69, 1, 7.0, 3.5 };
                rawData[50] = new double[] { 2, 1, 3.55, 2, 4.0, 3.5 };
                rawData[51] = new double[] { 2, 1, 3.24, 1, 4.8, 3.5 };
                rawData[52] = new double[] { 2, 2, 3.39, 1, 4.5, 3.5 };
                rawData[53] = new double[] { 2, 4, 3.28, 2, 4.0, 3.5 };
                rawData[54] = new double[] { 2, 3, 2.97, 1, 3.3, 4.5 };
                rawData[55] = new double[] { 2, 3, 2.99, 1, 4.0, 4.5 };
                rawData[56] = new double[] { 2, 3, 3.10, 1, 3.8, 4 };
                rawData[57] = new double[] { 2, 1, 3.23, 1, 4.5, 4 };
                rawData[58] = new double[] { 2, 2, 3.18, 1, 3.5, 4 };
                rawData[59] = new double[] { 2, 3, 3.08, 1, 3.3, 4 };
                rawData[60] = new double[] { 1, 2, 2.92, 2, 4.5, 4 };
                rawData[61] = new double[] { 1, 2, 2.67, 2, 4.3, 5 };
                rawData[62] = new double[] { 2, 1, 3.52, 2, 5.3, 3.5 };
                rawData[63] = new double[] { 2, 2, 3.40, 1, 4.5, 3.5 };
                rawData[64] = new double[] { 2, 3, 3.30, 1, 4.3, 3.5 };
                rawData[65] = new double[] { 2, 1, 2.66, 1, 5.0, 5.5 };
                rawData[66] = new double[] { 1, 4, 2.85, 1, 3.8, 5.5 };
                rawData[67] = new double[] { 1, 2, 2.73, 1, 2.8, 4.5 };
                rawData[68] = new double[] { 1, 2, 3.19, 1, 5.3, 4.5 };
                rawData[69] = new double[] { 2, 2, 3.30, 2, 5.0, 4 };
                rawData[70] = new double[] { 2, 4, 3.45, 1, 5.0, 4 };
                rawData[71] = new double[] { 2, 1, 3.48, 1, 4.8, 4 };
                rawData[72] = new double[] { 2, 2, 3.44, 1, 4.5, 4 };
                rawData[73] = new double[] { 2, 3, 3.51, 1, 4.0, 3.5 };
                rawData[74] = new double[] { 2, 3, 3.33, 2, 4.8, 3.5 };
                rawData[75] = new double[] { 2, 3, 3.56, 1, 4.5, 3.5 };
                rawData[76] = new double[] { 2, 2, 3.56, 1, 4.0, 3.5 };
                rawData[77] = new double[] { 2, 1, 3.85, 3, 3.3, 4 };
                rawData[78] = new double[] { 2, 1, 3.76, 3, 4.0, 4 };
                rawData[79] = new double[] { 2, 1, 3.69, 3, 3.8, 4 };
                rawData[80] = new double[] { 1, 2, 3.15, 2, 4.5, 4 };
                rawData[81] = new double[] { 2, 3, 3.33, 1, 3.5, 4 };
                rawData[82] = new double[] { 1, 1, 3.05, 1, 3.3, 4 };
                rawData[83] = new double[] { 2, 2, 3.3, 2, 4.5, 4 };
                rawData[84] = new double[] { 2, 5, 3.26, 1, 4.3, 4 };
                rawData[85] = new double[] { 2, 1, 3.24, 3, 5.3, 4 };
                rawData[86] = new double[] { 2, 2, 3.48, 2, 4.5, 4 };
                rawData[87] = new double[] { 2, 5, 3.69, 1, 4.3, 4 };
                rawData[88] = new double[] { 2, 3, 3.28, 1, 5.0, 4 };
                rawData[89] = new double[] { 1, 3, 3.66, 1, 3.8, 4 };
                return rawData;
            }
        }
    }
}
