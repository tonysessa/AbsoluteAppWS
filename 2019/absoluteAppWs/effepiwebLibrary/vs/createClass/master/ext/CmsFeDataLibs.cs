using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Routing;
using DbModel;

using Support.db;
using Support.Library;
using Support.Web;
using System.IO;
using System.Net;

namespace dataLibs
{
    public partial class CmsFeDataLibs : CmsDataBaseLibs
    {
        public CmsNlsContextItemResponse CmsNlsContextGetFromHostName(String hostName)
        {
            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsNlsContext
                                   .Include("CmsLabels")
                                   .Include("CmsRouting")
                                   where t.StartingPage.Contains(hostName)
                                   select t).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Error = ex.Message;
                result.Success = false;
            }
            //
            return result;
        }

        
        public void RegisterRoutes(object routes, CmsNlsContext item)
        {
            throw new NotImplementedException();
        }

        public void RegisterRoutes(RouteCollection routes, CmsNlsContext _currentCmsNlsContext)
        {
            MyHeader header = new MyHeader();

            CmsFeDataLibs objCmsFeDataLibs = new CmsFeDataLibs();

            routes.Clear();
            routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.asmx/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.aspq/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.svc/{*pathInfo}", new StopRoutingHandler()));

            routes.Ignore("mail/{*pathInfo}");
            routes.Ignore("images/{*pathInfo}");
            routes.Ignore("scripts/{*pathInfo}");
            routes.Ignore("styles/{*pathInfo}");


            CmsRoutingListOptions rq = new CmsRoutingListOptions();
            rq.uid_CmsNlsContext = _currentCmsNlsContext.Uid;
            rq.sortBy = "Uid";
            rq.sortAscending = true;
            rq.statusFlag = (Int32)EnumCmsFileStatus.Enabled;


            CmsRoutingListResponse listResponse = objCmsFeDataLibs.CmsRoutingList(header, rq);
            if (listResponse.Success && listResponse.items != null)
            {
                for (int i = 0; i < listResponse.items.Count; i++)
                {
                    CmsRouting item = listResponse.items[i];

                    routes.MapPageRoute(item.Uid_CmsNlsContext + "-" + item.NameRoute, item.UrlMapping, item.UrlPhysicalPage, false);
                }
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            MyHeader header = new MyHeader();

            CmsFeDataLibs objCmsFeDataLibs = new CmsFeDataLibs();

            routes.Clear();
            routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.asmx/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.aspq/{*pathInfo}", new StopRoutingHandler()));
            routes.Add(new Route("{resource}.svc/{*pathInfo}", new StopRoutingHandler()));

            routes.Ignore("mail/{*pathInfo}");
            routes.Ignore("images/{*pathInfo}");
            routes.Ignore("scripts/{*pathInfo}");
            routes.Ignore("styles/{*pathInfo}");


            CmsRoutingListOptions rq = new CmsRoutingListOptions();
            rq.sortBy = "Uid";
            rq.sortAscending = true;
            rq.statusFlag = (Int32)EnumCmsFileStatus.Enabled;


            CmsRoutingListResponse listResponse = objCmsFeDataLibs.CmsRoutingList(header, rq);
            if (listResponse.Success && listResponse.items != null)
            {
                for (int i = 0; i < listResponse.items.Count; i++)
                {
                    CmsRouting item = listResponse.items[i];

                    routes.MapPageRoute(item.Uid_CmsNlsContext + "-" + item.NameRoute, item.UrlMapping, item.UrlPhysicalPage, false);
                }
            }
        }

    }

}
