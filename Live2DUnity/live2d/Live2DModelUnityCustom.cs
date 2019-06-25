using System;
using System.IO;
using UnityEngine;

namespace live2d
{
    public class Live2DModelUnityCustom : ALive2DModel
    {
        public Live2DModelUnityCustom()
        {
        }

        public override void deleteTextures()
        {
        }

        public void releaseModel()
        {
        }

        public void setMatrix(Matrix4x4 mat)
        {
        }

        public void setMatrix(float[] mat)
        {
        }

        public override void draw()
        {
        }

        public void setTexture(int textureNo, Texture2D texture)
        {
        }

        public static Live2DModelUnityCustom loadModel(string filepath)
        {
            return new Live2DModelUnityCustom();
        }

        public static Live2DModelUnityCustom loadModel(Stream bin)
        {
            return new Live2DModelUnityCustom();
        }

        public static Live2DModelUnityCustom loadModel(byte[] bin)
        {
            return new Live2DModelUnityCustom();
        }

        public override DrawParam getDrawParam()
        {
            return null;
        }

        public void enableCullingSetting(bool b)
        {
        }

        public void setCulling(bool b)
        {
        }

        public void setTextureColor(int textureNo, float r, float g, float b)
        {
        }
    }
}
