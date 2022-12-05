using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Support.xml.XMLSerialization
{
    public class GenericObject
    {
        public GenericObject()
        {
        }

        protected virtual void deserialize(System.Xml.XmlElement node)
        {
            System.Console.WriteLine(node.Name);
            // Nothing to do
        }

        protected string _getXMLAttribute(System.Xml.XmlElement node, string attName)
        {
            XmlAttribute att = node.Attributes[attName];
            if (att == null)
                return null;
            return att.Value;
        }

        protected string _getXMLText(System.Xml.XmlElement node)
        {
            XmlNode n = node.FirstChild;
            if (n!=null && (n.NodeType == XmlNodeType.Text || n.NodeType == XmlNodeType.CDATA))
                return n.Value;
            return null;
        }

        protected GenericObject _getXMLChild(System.Xml.XmlElement node,string tagName,Type factory)
        {
            XmlNode n = node.SelectSingleNode("./*[local-name() = '" + tagName + "']");
            if (n == null)
                return null;
            // ok
            GenericObject obj = (GenericObject)Activator.CreateInstance(factory);
            obj.deserialize((XmlElement)n);
            //
            return obj;
        }

        protected GenericObject[] _getXMLChilds(System.Xml.XmlElement node, string tagName, Type factory)
        {
            // cerco tutti i figli
            XmlNodeList lista = node.SelectNodes("./*[local-name() = '" + tagName + "']");
            if (lista == null)
                return null;
            // ok
            GenericObject[] ret = (GenericObject[])Array.CreateInstance(factory, lista.Count);
            //new GenericObject[lista.Count];
            for (int i = 0; i < lista.Count; i++)
            {
                ret[i] = (GenericObject)Activator.CreateInstance(factory);
                ret[i].deserialize((XmlElement)lista[i]);
            }
            return ret;
        }

        // loader
        public static GenericObject deserializeFromXML(System.IO.Stream xml, Type factory, String tagName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xml);
                // becco root
                XmlNodeList fiddi = doc.ChildNodes;
                XmlNode root = null;
                int i = 0;
                while (fiddi != null && root == null && i < fiddi.Count)
                {
                    XmlNode nd = fiddi[i++];
                    if (nd.NodeType == XmlNodeType.Element)
                        root = nd;
                }
                // NUOVO
                // cerco il nodo che mi interessa
                while (root != null && !root.Name.Equals(tagName))
                {
                    fiddi = root.ChildNodes;
                    root = null;
                    i = 0;
                    while (fiddi != null && root == null && i < fiddi.Count)
                    {
                        XmlNode nd = fiddi[i++];
                        if (nd.NodeType == XmlNodeType.Element)
                            root = nd;
                    }
                }
                if (root == null)
                    return null;
                // FINE NUOVO

                // verifico che il nodo radice sia quello cercato
                //if (root == null || !root.Name.Equals(tagName))
                //    return null;
                // a questo punto ok
                GenericObject obj = (GenericObject)Activator.CreateInstance(factory);
                obj.deserialize((XmlElement)root);
                //
                return obj;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
        }

        public string xmlSerialize(Object obj)
        {
            return xmlSerialize(obj, false);
        }

        public string xmlSerialize(Object obj, bool ignoreNameSpace)
        {
            if (obj == null)
                return "";
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            if (ignoreNameSpace)
                ns.Add("", "");
            StringWriter sw = new StringWriter();
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            XmlWriter writer = new XmlTextWriterFormattedNoDeclaration(sw);
            xs.Serialize(writer, obj, ns);
            return sw.ToString();
        }

        public class XmlTextWriterFormattedNoDeclaration : System.Xml.XmlTextWriter
        {
            public XmlTextWriterFormattedNoDeclaration(System.IO.TextWriter w)
                : base(w)
            {
                Formatting = System.Xml.Formatting.Indented;
            }

            public override void WriteStartDocument() { } // suppress
        }
    }
}
