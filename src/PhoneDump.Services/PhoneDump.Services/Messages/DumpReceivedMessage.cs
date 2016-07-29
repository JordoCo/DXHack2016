using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace PhoneDump.Services.Messages
{
    /// <summary>
    /// Used when dumps are received to marshal from API to Services. Not for VM's
    /// </summary>
    public class DumpReceivedMessage : XMessage
    {
        private readonly DumpWireEntity _entity;

        public DumpReceivedMessage(DumpWireEntity entity)
        {
            _entity = entity;
        }

        public DumpWireEntity Entity => _entity;
    }
}
