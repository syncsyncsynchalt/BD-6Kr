using System.Collections.Generic;
using System.Threading;

namespace live2d
{
    public class PartsDataID : ID
    {
        public PartsDataID createIDForSerialize()
        {
            return new PartsDataID();
        }

        public static PartsDataID getID(string id)
        {
            return new PartsDataID();
        }

        public static PartsDataID getIDAsync(string id)
        {
            return new PartsDataID();
        }
    }
}
