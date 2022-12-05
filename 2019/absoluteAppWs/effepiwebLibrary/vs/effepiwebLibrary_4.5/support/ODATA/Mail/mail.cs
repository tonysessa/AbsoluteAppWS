// codice generato automaticamente da xmlbean
using System;
using System.Text;
using System.Collections;
using System.Xml;

namespace Support.ODATA.Mail
{
    public class mail : Support.xml.XMLSerialization.GenericObject
	{
		// attributi

		// nodi figli
		public mail_SMTP _SMTP = null;
		public mail_BCC[] _BCC = null;
		public mail_TYPE _TYPE = null;
		public mail_ATTACHF[] _ATTACHF = null;
		public mail_CC[] _CC = null;
		public mail_SUBJECT _SUBJECT = null;
		public mail_ATTACHU[] _ATTACHU = null;
		public mail_FROM _FROM = null;
		public mail_BODY _BODY = null;
		public mail_TO[] _TO = null;

		// costruttore
		public mail()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_SMTP=(mail_SMTP)_getXMLChild(node,"SMTP",typeof(mail_SMTP));
			_BCC=(mail_BCC[])_getXMLChilds(node,"BCC",typeof(mail_BCC));
			_TYPE=(mail_TYPE)_getXMLChild(node,"TYPE",typeof(mail_TYPE));
			_ATTACHF=(mail_ATTACHF[])_getXMLChilds(node,"ATTACHF",typeof(mail_ATTACHF));
			_CC=(mail_CC[])_getXMLChilds(node,"CC",typeof(mail_CC));
			_SUBJECT=(mail_SUBJECT)_getXMLChild(node,"SUBJECT",typeof(mail_SUBJECT));
			_ATTACHU=(mail_ATTACHU[])_getXMLChilds(node,"ATTACHU",typeof(mail_ATTACHU));
			_FROM=(mail_FROM)_getXMLChild(node,"FROM",typeof(mail_FROM));
			_BODY=(mail_BODY)_getXMLChild(node,"BODY",typeof(mail_BODY));
			_TO=(mail_TO[])_getXMLChilds(node,"TO",typeof(mail_TO));
		}

		// deserializzatore da uno stream xml
		public static mail deserializeFromXML(System.IO.Stream xml)
		{
			return (mail)deserializeFromXML(xml,typeof(mail),"mail");
		}
	}

    public class mail_SMTP : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_SMTP()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_BCC : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_BCC()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_TYPE : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_TYPE()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_ATTACHF : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_ATTACHF()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_CC : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_CC()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_SUBJECT : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_SUBJECT()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_ATTACHU : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_ATTACHU()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_FROM : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_FROM()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

    public class mail_BODY : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
        public string _txt = null;

		// nodi figli

		// costruttore
		public mail_BODY()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
            _txt = _getXMLText(node);
		}
	}

    public class mail_TO : Support.xml.XMLSerialization.GenericObject
	{
		// attributi
		public string _txt = null;

		// nodi figli

		// costruttore
		public mail_TO()
		{
		}

		// deserializzatore da un nodo XML
		protected override void deserialize(System.Xml.XmlElement node)
		{
			base.deserialize(node);
			_txt=_getXMLText(node);
		}
	}

}
