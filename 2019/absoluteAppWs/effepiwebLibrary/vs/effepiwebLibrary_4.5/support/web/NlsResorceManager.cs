// by wlf

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Support;

namespace Support.Web
{
    public class NlsResorceManager
    {
        private WebContext wctx = null;
        private string lng = null;
        private System.Collections.Hashtable nlsTable = new System.Collections.Hashtable();
        //private ODATA.Resources.resources = 

        public NlsResorceManager(WebContext wctx, string lng)
        {
            this.wctx = wctx;
            this.lng = lng;
            // tiro su il file
            ODATA.Resources.resources res = ODATA.Resources.resources.deserializeFromXML(wctx.getXMLResourceFromRepository("resources",lng));
            for (int i = 0; i < res._RESOURCE.Length; i++)
                nlsTable.Add(res._RESOURCE[i]._name,res._RESOURCE[i]._value);
        }

        public string find(string name)
        {
            return (string)nlsTable[name];
        }

        public string find(string name, string defValue )
        {
            string r = find(name);
            return String.IsNullOrEmpty(r) ? defValue : r;
        }

        public SortedList<string, string> FindBlock(string prefix)
        {
            SortedList<string, string> sortList = new SortedList<string, string>();
            
            try
            {
                foreach (DictionaryEntry de in nlsTable)
                {
                    if (((string)de.Key).StartsWith(prefix) == true)
                    {
                        sortList.Add(de.Key.ToString().Substring(prefix.Length, de.Key.ToString().Length - prefix.Length), de.Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                sortList.Clear();
                sortList.Add(e.Message.ToString(),"Errore");
            }

            return sortList;
            
        }

        public string formatEuroOta(double val)
        {
            return string.Format("{0:0.00}", val).Replace(',','.');
        }

        public string formatEuroView(double val)
        {
            return string.Format("{0:0.00}", val);
        }
    }
}
