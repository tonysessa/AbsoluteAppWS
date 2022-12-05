using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Support.xml.XMLSerialization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Text;
//
using System.Web;
using System.Web.Caching;
using Support.Library;
using System.Net;

namespace Support.Web
{
    public class CacheSet
    {
        private String cSetName = "";
        private List<String> keys = new List<String>();


        public CacheSet(String cSetName)
        {
            this.cSetName = cSetName;
        }

        private String getCacheKey(String localKey)
        {
            return cSetName + "#" + localKey;
        }

        public void RemoveFromCache(String localKey)
        {
            System.Web.Caching.Cache cake = HttpRuntime.Cache;
            //
            lock (this)
            {
                cake.Remove(getCacheKey(localKey));
                int idx = keys.IndexOf(localKey);
                if (idx != -1)
                    keys.RemoveAt(idx);
            }
        }

        private static Random rnd = new Random();

        public void AddToCache(String localKey, Object obj, int tmoInMinuti = -1)
        {
            if (tmoInMinuti < 1)
            {
                tmoInMinuti = rnd.Next(9 * 60, 12 * 60);
            }
            System.Web.Caching.Cache cake = HttpRuntime.Cache;
            //
            lock (this)
            {
                cake.Insert(getCacheKey(localKey), obj, null, DateTime.UtcNow.Add(TimeSpan.FromMinutes(tmoInMinuti)), System.Web.Caching.Cache.NoSlidingExpiration);
                int idx = keys.IndexOf(localKey);
                if (idx == -1)
                    keys.Add(localKey);
            }
        }

        public void RemoveAllFromCache()
        {
            System.Web.Caching.Cache cake = HttpRuntime.Cache;
            //
            lock (this)
            {
                foreach (String localKey in keys)
                {
                    cake.Remove(getCacheKey(localKey));
                }
                keys = new List<String>();
            }
        }

        public Object GetFromCache(String localKey)
        {
            System.Web.Caching.Cache cake = HttpRuntime.Cache;
            return cake.Get(getCacheKey(localKey));
        }
    }

    public class CacheManager
    {
        private Dictionary<String, CacheSet> cSet = new Dictionary<String, CacheSet>();
        //
        public CacheManager()
        {
        }
        //
        public string[] cacheNodes
        {
            get
            {
                String nodes = ConfigurationManager.AppSettings["Cluster.Nodes"];
                if (nodes == null)
                    return new string[0];
                return nodes.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }
        //
        public void ResetAll()
        {
            lock (this)
            {
                foreach (CacheSet c in cSet.Values)
                    c.RemoveAllFromCache();
                cSet = new Dictionary<String, CacheSet>();
            }
        }
        //
        public void AddToCache(String nlsContext, String cSetName, String localKey, Object value)
        {
            try
            {
                if (!cSet.ContainsKey(nlsContext + "#" + cSetName))
                    cSet[nlsContext + "#" + cSetName] = new CacheSet(nlsContext + "#" + cSetName);
                if (value == null)
                    cSet[nlsContext + "#" + cSetName].RemoveFromCache(localKey);
                else
                    cSet[nlsContext + "#" + cSetName].AddToCache(localKey, value);
            }
            catch
            {
            }
        }
        //
        public void RemoveFromCache(String nlsContext, String cSetName, String localKey)
        {
            try
            {
                if (cSet.ContainsKey(nlsContext + "#" + cSetName))
                    cSet[nlsContext + "#" + cSetName].RemoveFromCache(localKey);
            }
            catch
            {
            }
        }
        //
        public void RemoveAllFromCache(String nlsContext, String cSetName)
        {
            try
            {
                if (cSet.ContainsKey(nlsContext + "#" + cSetName))
                    cSet[nlsContext + "#" + cSetName].RemoveAllFromCache();
            }
            catch
            {
            }
        }
        //
        public Object GetFromCache(String nlsContext, String cSetName, string localKey)
        {
            try
            {
                if (cSet.ContainsKey(nlsContext + "#" + cSetName))
                    return cSet[nlsContext + "#" + cSetName].GetFromCache(localKey);
            }
            catch
            {
            }
            return null;
        }

        // ****************************************************** //

        private static CacheManager _that = new CacheManager();

        public static CacheManager that
        {
            get
            {
                return _that;
            }
        }

        public static String CreateKey(Object obj2)
        {
            GenericObject myobj = new GenericObject();
            CriptUtil myobjCript = new CriptUtil();
            String str1 = myobj.xmlSerialize(obj2);
            return myobjCript.MD5(str1);
        }

        public void NetGet(string sourceUrl)
        {
            try
            {                
                HttpWebRequest hwrRequest = (HttpWebRequest)WebRequest.Create(sourceUrl);
                hwrRequest.Method = "GET";
                hwrRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), hwrRequest);               
            }
            catch
            {
                //
            }
        }

        void FinishWebRequest(IAsyncResult result)
        {
            HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
        }
    }

}
