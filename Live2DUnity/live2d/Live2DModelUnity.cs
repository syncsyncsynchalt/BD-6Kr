using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace live2d
{
	public class Live2DModelUnity : ALive2DModel
	{
		public void releaseModel()
		{
		}

		public override void deleteTextures()
		{
        }

		public void setMatrix(Matrix4x4 mat)
		{
		}

		public void setMatrix(float[] mat)
		{
		}

		public override void update()
		{
		}

		public override void draw()
		{
		}

		public void setTexture(int textureNo, Texture2D texture)
		{
		}

		public static Live2DModelUnity loadModel(string filepath)
		{
			return new Live2DModelUnity();
		}

		public static Live2DModelUnity loadModel(Stream bin)
		{
            return new Live2DModelUnity();
        }

        public static Live2DModelUnity loadModel(byte[] bin)
		{
            return new Live2DModelUnity();
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

		public bool getCulling()
		{
			return false;
		}

		public void setLayer(int layer)
		{
		}

		public int getLayer()
		{
			return -1;
		}

		public void setRenderMode(int value)
		{
		}

		public int getRenderMode()
		{
			return -1;
		}

		public void setTextureColor(int textureNo, float r, float g, float b)
		{
		}

		public void setConvertTexture(int colorGroupNo, Texture2D texture)
		{
		}

		public string toStringDrawCommand()
		{
            return "";
		}
	}
}
