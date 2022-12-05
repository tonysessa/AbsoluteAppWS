// codice generato automaticamente da xmlbean 
using System;
using System.Text;
using System.Collections;
using System.Xml;

namespace Support.ODATA.Resources
{
	public class resources : Support.xml.XMLSerialization.GenericObject
	{
		// attributi

		// nodi figli
		public resources_resource[] _RESOURCE = null;

		// costruttore
		public resources()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_RESOURCE=(resources_resource[])_getXMLChilds(node,"resource",typeof(resources_resource));
		}

		// deserializzatore da uno stream xml
		public static resources deserializeFromXML(System.IO.Stream xml)
		{
			return (resources)deserializeFromXML(xml,typeof(resources),"resources");
		}
	}

    public class resources_resource : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _name = null;
		public string _value = null;

		// nodi figli

		// costruttore
		public resources_resource()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_name=_getXMLAttribute(node,"name");
			_value=_getXMLAttribute(node,"value");
            //
            if (_value == null)
                _value = _getXMLText(node);
		}
	}

}
