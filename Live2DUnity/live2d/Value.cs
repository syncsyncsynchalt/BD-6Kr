using System.Collections.Generic;

namespace live2d
{
	public class Value
	{
		private object o;

		public Value(object o)
		{
			this.o = o;
		}

		public string toString()
		{
			return toString("");
		}

		public string toString(string indent)
		{
			if (o is string)
			{
				return (string)o;
			}
			if (o is List<Value>)
			{
				string text = indent + "[\n";
				foreach (Value item in (List<Value>)o)
				{
					string text2 = text;
					text = text2 + indent + "\t" + item.toString(indent + "\t") + "\n";
				}
				return text + indent + "]\n";
			}
			if (o is Dictionary<string, Value>)
			{
				string text3 = indent + "{\n";
				Dictionary<string, Value> dictionary = (Dictionary<string, Value>)o;
				foreach (KeyValuePair<string, Value> item2 in dictionary)
				{
					Value value = item2.Value;
					string text4 = text3;
					text3 = text4 + indent + "\t" + item2.Key + " : " + value.toString(indent + "\t") + "\n";
				}
				return text3 + indent + "}\n";
			}
			return string.Concat(o);
		}

		public int toInt()
		{
			return toInt(0);
		}

		public int toInt(int defaultV)
		{
			if (o is double)
			{
				return (int)(double)o;
			}
			return defaultV;
		}

		public float toFloat()
		{
			return toFloat(0f);
		}

		public float toFloat(float defaultV)
		{
			if (o is double)
			{
				return (float)(double)o;
			}
			return defaultV;
		}

		public double toDouble()
		{
			return toDouble(0.0);
		}

		public double toDouble(double defaultV)
		{
			if (o is double)
			{
				return (double)o;
			}
			return defaultV;
		}

		public List<Value> getVector(List<Value> defalutV)
		{
			if (o is List<Value>)
			{
				return (List<Value>)o;
			}
			return defalutV;
		}

		public Value get(int index)
		{
			if (o is List<Value>)
			{
				return ((List<Value>)o)[index];
			}
			return null;
		}

		public Dictionary<string, Value> getMap(Dictionary<string, Value> defalutV)
		{
			if (o is Dictionary<string, Value>)
			{
				return (Dictionary<string, Value>)o;
			}
			return defalutV;
		}

		public Value get(string key)
		{
			if (o is Dictionary<string, Value>)
			{
				return ((Dictionary<string, Value>)o)[key];
			}
			return null;
		}

		public List<string> keySet()
		{
			if (o is Dictionary<string, Value>)
			{
				return new List<string>(((Dictionary<string, Value>)o).Keys);
			}
			return null;
		}

		public Dictionary<string, Value> toMap()
		{
			if (o is Dictionary<string, Value>)
			{
				return (Dictionary<string, Value>)o;
			}
			return null;
		}

		public bool isNull()
		{
			return o == null;
		}

		public bool isBoolean()
		{
			return o is bool;
		}

		public bool isDouble()
		{
			return o is double;
		}

		public bool isstring()
		{
			return o is string;
		}

		public bool isArray()
		{
			return o is List<Value>;
		}

		public bool isMap()
		{
			return o is Dictionary<string, Value>;
		}
	}
}
