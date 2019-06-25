using System.Collections.Generic;
using System.Threading;

namespace live2d
{
    public class DrawDataID : ID
    {
        private DrawDataID()
        {
        }

        private DrawDataID(string _0020)
        {
        }

        public DrawDataID createIDForSerialize()
        {
            return new DrawDataID();
        }

        public static DrawDataID getID(string tmp_idstr)
        {
            return new DrawDataID();
        }

        public static DrawDataID getIDAsync(string tmp_idstr)
        {
            return new DrawDataID();
        }
    }
}
