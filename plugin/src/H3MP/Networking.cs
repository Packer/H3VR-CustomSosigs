using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using H3MP;
using H3MP.Tracking;

namespace CustomSosigLoader
{
    internal class Networking
    {
        public void thing()
        {
            TrackedSosigData.OnCollectAdditionalData += IncludeData;
        }

        private void IncludeData(ref bool collected, TrackedSosigData trackedSosigData)
        {
            throw new NotImplementedException();
        }
    }
}
