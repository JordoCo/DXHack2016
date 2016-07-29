using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace PhoneDump.Services.Messages
{
    public class NewDumpMessage : XMessage
    {
        private readonly DumpWireEntity _entity;

        public NewDumpMessage(DumpWireEntity entity)
        {
            _entity = entity;
        }
        
        public DumpWireEntity Entity => _entity;
    }
}
