using System.Collections.Generic;
using System.Threading;

namespace live2d
{
    public class BaseDataID : ID
    {
        public static BaseDataID DST_BASE_ID()
        {
            return new BaseDataID();
        }

        public BaseDataID createIDForSerialize()
        {
            return new BaseDataID();
        }

        public static BaseDataID getID(string tmp_idstr)
        {
            return new BaseDataID();
        }

        public static BaseDataID getIDAsync(string tmp_idstr)
        {
            return new BaseDataID();
        }
    }
}
