using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Server_Lib
{
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (!isEmptyElement)
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
				XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					reader.ReadStartElement("KeyValuePair");
					reader.ReadStartElement("Key");
					TKey key = (TKey)xmlSerializer.Deserialize(reader);
					reader.ReadEndElement();
					reader.ReadStartElement("Value");
					TValue value = (TValue)xmlSerializer2.Deserialize(reader);
					reader.ReadEndElement();
					reader.ReadEndElement();
					Add(key, value);
					reader.MoveToContent();
				}
				reader.ReadEndElement();
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			foreach (TKey key in base.Keys)
			{
				writer.WriteStartElement("KeyValuePair");
				writer.WriteStartElement("Key");
				xmlSerializer.Serialize(writer, key);
				writer.WriteEndElement();
				writer.WriteStartElement("Value");
				xmlSerializer2.Serialize(writer, this[key]);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
}
