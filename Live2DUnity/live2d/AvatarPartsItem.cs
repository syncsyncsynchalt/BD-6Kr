using System.Collections.Generic;
using System.IO;

namespace live2d
{
    public class AvatarPartsItem : ISerializableV2
    {
        public class AvatarTextureInfo
        {
        }

        public AvatarPartsItem()
        {
        }

        public List<IBaseData> getBaseDataList()
        {
            return new List<IBaseData> { };
        }

        public List<IDrawData> getDrawDataList()
        {
            return new List<IDrawData> { };
        }

        public void readV2(BReader br)
        {
        }
        public void init(string partsID, ALive2DModel model)
        {
        }

        public static AvatarPartsItem loadAvatarParts(string filepath)
        {
            return new AvatarPartsItem();
        }

        public static AvatarPartsItem loadAvatarParts(byte[] bin)
        {
            return new AvatarPartsItem();
        }

        public static AvatarPartsItem loadAvatarParts(Stream input)
        {
            return new AvatarPartsItem();

        }

        public int getTextureCount()
        {
            return -1;
        }

        public void setTextureInfo(int partsTextureNo, float textureScaleW = 1f, float textureScaleH = 1f)
        {
        }

        public void setTextureIndex(int partsTextureNo, int textureIndex)
        {
        }

        public int getTextureIndex(int partsTextureNo)
        {
            return -1;
        }

        public int getColorGroupNo(int partsTextureNo)
        {
            return -1;
        }

        public void setPartsNo(int no)
        {
        }

        public int getPartsNo()
        {
            return -1;
        }
    }
}
