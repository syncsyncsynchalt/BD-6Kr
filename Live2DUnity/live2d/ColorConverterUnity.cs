using UnityEngine;

namespace live2d
{
    public class ColorConverterUnity : ColorConverter
    {
        public void setTexture(Texture2D tex)
        {
        }

        public Texture2D getTexture()
        {
            return new Texture2D(0, 0);
        }

        public Matrix4x4 getConvertMatrix()
        {
            return new Matrix4x4();
        }

        public override void clearCache()
        {
        }
    }
}
