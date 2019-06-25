using System.Collections.Generic;
using System.Threading;

namespace live2d
{
    public class ParamID : ID
    {
        public ParamID createIDForSerialize()
        {
            return new ParamID();
        }

        public static ParamID getID(string tmp_idstr)
        {
            return new ParamID();
        }

        public static ParamID getIDAsync(string tmp_idstr)
        {
            return new ParamID();
        }
    }
}
