using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace PhoneDump.Entity.Dumps
{
    public class DumpWireEntity : IEntity
    {
        public Guid Id { get; set; }

        public string EncodedData { get; set; }
        public string RawData { get; set; }
        public string MediaType { get; set; }
        public string Param { get; set; }
    }
}
