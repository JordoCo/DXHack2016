using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureFunctionsToolkit.Standard.Extensions;

namespace BaseEncoder
{
    class Program
    {
        static void Main(string[] args)
        {

            var f = @"C:\Temp\16118j.jpg";

            var data = File.ReadAllBytes(f);

            var b64 = Convert.ToBase64String(data);

            var m = new MoreComplexData
            {
                Text = b64
            };

            File.WriteAllText(@"c:\temp\output.txt",m.Serialise());
        }
    }

    public sealed class MoreComplexData
    {
        public string Text { get; set; }
    }
}
