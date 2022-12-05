using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data;
//
using DbModel;

namespace dataLibs
{
    #region CmsRouting
    [DataContract]
    public class CmsRoutingListOptions
    {
        [DataMember]
        public String searchText = null;

        [DataMember]
        public Boolean sortAscending = true;

        [DataMember]
        public String sortBy = null;

        [DataMember]
        public Boolean includeDeleted = false;

        [DataMember]
        public Int32? Uid_CreationUser  = null;

		[DataMember]
		public Int32? uid_CreationUser = null;

		[DataMember]
		public Int32? statusFlag = null;

		[DataMember]
		public Int32? uid_CmsNlsContext = null;

		[DataMember]
		public String nameRoute = null;

		[DataMember]
		public String urlMapping = null;

		[DataMember]
		public String urlPhysicalPage = null;

		[DataMember]
		public String metaTagTitle = null;

		[DataMember]
		public String metaTagDescription = null;

		[DataMember]
		public String metaTagKeywords = null;


        [DataMember]
        public Paging paging = null;

        public String sqlOrderByExpression
        {
            get
            {
                return (String.IsNullOrEmpty(sortBy) ? "Uid" : sortBy) + " " + (sortAscending ? "ascending" : "descending");
            }
        }
    }

    [DataContract]
    public class CmsRoutingItemResponse
    {
        [DataMember]
        public CmsRouting item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsRoutingListResponse
    {
        [DataMember]
        public List<CmsRouting> items = null;

        [DataMember]
        public Paging paging = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }
    #endregion

    public partial class CmsDataBaseLibs
    {
		#region Generic Method
        // ******************************************************************************* //
        // Prendo l'elenco dei record
        // ******************************************************************************* //
        public CmsRoutingListResponse CmsRoutingList(MyHeader header, CmsRoutingListOptions rq)
        {

            CmsRoutingListResponse results = new CmsRoutingListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRouting> query = from t in dbCtx.Context.CmsRouting														  					  
														  .Include("CmsNlsContext")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.nameRoute == null || t.NameRoute == rq.nameRoute) &&
														  (rq.urlMapping == null || t.UrlMapping == rq.urlMapping) &&
														  (rq.urlPhysicalPage == null || t.UrlPhysicalPage == rq.urlPhysicalPage) &&
														  (rq.metaTagTitle == null || t.MetaTagTitle == rq.metaTagTitle) &&
														  (rq.metaTagDescription == null || t.MetaTagDescription == rq.metaTagDescription) &&
														  (rq.metaTagKeywords == null || t.MetaTagKeywords == rq.metaTagKeywords) &&
														  (rq.searchText == null || t.NameRoute.Contains(rq.searchText) ||t.UrlMapping.Contains(rq.searchText) ||t.UrlPhysicalPage.Contains(rq.searchText) ||t.MetaTagTitle.Contains(rq.searchText) ||t.MetaTagDescription.Contains(rq.searchText) ||t.MetaTagKeywords.Contains(rq.searchText)) &&
														  (t.StatusFlag != (int)EnumCmsContentStatus.Deleted || (t.StatusFlag == (int)EnumCmsContentStatus.Deleted && rq.includeDeleted))
                                                          select t;
					
					// Paging
                    if (rq.paging == null)
                    {
                        results.items = query.OrderBy(rq.sqlOrderByExpression).ToList();
                    }
                    else
                    {
                        results.paging = new Paging();
                        results.paging.PageSize = rq.paging.PageSize <= 0 ? 20 : rq.paging.PageSize;
                        results.paging.PageNumber = rq.paging.PageNumber >= 0 ? rq.paging.PageNumber : 0;
                        results.paging.TotalRows = query.Count();
                        results.paging.TotalPages = (int)Math.Ceiling((double)results.paging.TotalRows / (double)results.paging.PageSize);
                        results.items = query.OrderBy(rq.sqlOrderByExpression).Skip(results.paging.PageNumber * results.paging.PageSize).Take(results.paging.PageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                results.Ex = ex;
                results.Error = ex.Message;
                results.Success = false;
            }
            //
            return results;
        }

        // ******************************************************************************* //
        // Prendo l'elenco dei record per il BackOffice
        // ******************************************************************************* //
		public CmsRoutingListResponse CmsRoutingListCms(MyHeader header, CmsRoutingListOptions rq)
        {

            CmsRoutingListResponse results = new CmsRoutingListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRouting> query = from t in dbCtx.Context.CmsRouting														  				  
														  .Include("CmsNlsContext")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.nameRoute == null || t.NameRoute == rq.nameRoute) &&
														  (rq.urlMapping == null || t.UrlMapping == rq.urlMapping) &&
														  (rq.urlPhysicalPage == null || t.UrlPhysicalPage == rq.urlPhysicalPage) &&
														  (rq.metaTagTitle == null || t.MetaTagTitle == rq.metaTagTitle) &&
														  (rq.metaTagDescription == null || t.MetaTagDescription == rq.metaTagDescription) &&
														  (rq.metaTagKeywords == null || t.MetaTagKeywords == rq.metaTagKeywords) &&
														  (rq.searchText == null || t.NameRoute.Contains(rq.searchText) ||t.UrlMapping.Contains(rq.searchText) ||t.UrlPhysicalPage.Contains(rq.searchText) ||t.MetaTagTitle.Contains(rq.searchText) ||t.MetaTagDescription.Contains(rq.searchText) ||t.MetaTagKeywords.Contains(rq.searchText)) &&														  
														  (t.StatusFlag != (int)EnumCmsContentStatus.Deleted || (t.StatusFlag == (int)EnumCmsContentStatus.Deleted && rq.includeDeleted))
                                                          select t;
					
					// Paging
                    if (rq.paging == null)
                    {
                        results.items = query.OrderBy(rq.sqlOrderByExpression).ToList();
                    }
                    else
                    {
                        results.paging = new Paging();
                        results.paging.PageSize = rq.paging.PageSize <= 0 ? 20 : rq.paging.PageSize;
                        results.paging.PageNumber = rq.paging.PageNumber >= 0 ? rq.paging.PageNumber : 0;
                        results.paging.TotalRows = query.Count();
                        results.paging.TotalPages = (int)Math.Ceiling((double)results.paging.TotalRows / (double)results.paging.PageSize);
                        results.items = query.OrderBy(rq.sqlOrderByExpression).Skip(results.paging.PageNumber * results.paging.PageSize).Take(results.paging.PageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                results.Ex = ex;
                results.Error = ex.Message;
                results.Success = false;
            }
            //
            return results;
        }

		// ******************************************************************************* //
        // Prendo un record
        // ******************************************************************************* //
        public CmsRoutingItemResponse CmsRoutingGet(MyHeader header, String Uid)
        {
			return CmsRoutingGet(header, System.Convert.ToInt32(Uid));
		}
        public CmsRoutingItemResponse CmsRoutingGet(MyHeader header, Int32 Uid)
        {

            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRouting
														  .Include("CmsNlsContext")

								where t.Uid == Uid &&
								(t.StatusFlag != (int)EnumCmsContent.Deleted)
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

        // ******************************************************************************* //
        // Prendo un record
        // ******************************************************************************* //
        public CmsRoutingItemResponse CmsRoutingGetCms(MyHeader header, String Uid)
        {
			return CmsRoutingGetCms(header, System.Convert.ToInt32(Uid));
		}
        public CmsRoutingItemResponse CmsRoutingGetCms(MyHeader header, Int32 Uid)
        {

            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRouting
														  .Include("CmsNlsContext")

								where t.Uid == Uid
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

		/*
		// ******************************************************************************* //
        // Prendo un record
        // ******************************************************************************* //
        public CmsRoutingItemResponse CmsRoutingGetFrom(MyHeader header, String Uid_From)
        {

            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRouting
														  .Include("CmsNlsContext")

								where t.Uid_From == Uid_From &&
								(t.StatusFlag != (int)EnumCmsContent.Deleted)
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

		// ******************************************************************************* //
        // Prendo un record
        // ******************************************************************************* //
        public CmsRoutingItemResponse CmsRoutingGetFromCms(MyHeader header, String Uid_From)
        {

            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRouting
														  .Include("CmsNlsContext")

								where t.Uid_From == Uid_From
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
		*/

        // ******************************************************************************* //
        // Prendo un record e salvo la versione precedente
        // ******************************************************************************* //
		/*
		public CmsRoutingItemResponse CmsRoutingGetForUpdate(MyHeader header, Int32 uid)
		{
			return CmsRoutingGetForUpdate(header, uid, "");
		}
		public CmsRoutingItemResponse CmsRoutingGetForUpdate(MyHeader header, Int32 uid, Int32 cmsHistoryId )
        {
            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRouting", true, true, false)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsRouting itm = (from t in dbCtx.db.CmsRouting
														  .Include("CmsNlsContext")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsRouting_db2JSON(itm.Uid);
                        CmsRouting_AddToHistory(header, json, itm);
                        itm.VersionCurrent++;
                        itm.Uid_PublishUser = null;
                        itm.PublishDate = null;
                        dbCtx.SaveChanges();
                    }
                    //
                    result.item = itm;

                    // 
                    if (!String.IsNullOrEmpty(cmsHistoryId))
                    {
                        long hId = Int21h.atol(cmsHistoryId);
                        CmsHistory h = dbCtx.Context.CmsHistory.Where(t => t.Id == hId).FirstOrDefault();
                        CmsRouting n0 = Int21h.jsonDecode<CmsRouting>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsRouting_Versioning_MoveFromDb(header, uid, n0);
                        return CmsRoutingGet(header, uid);
                    }
                }
                if (result.item != null)
                {
                    result.Success = true;
                }
                else {
                    result.Error = "ERROR_NOTFOUND";
                    result.Success = false;
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
		*/

        // ******************************************************************************* //
        // Inserisco o modifico un record
        // ******************************************************************************* //
        public CmsRoutingItemResponse CmsRoutingUpsert(MyHeader header, CmsRouting data)
        {
            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    bool isInsert = true;
                    if (data.Uid > 0)
                        isInsert = false;

                    if (data.Uid > 0)
                    {
                        result.item = (from t in dbCtx.Context.CmsRouting									  
														  .Include("CmsNlsContext")

                                       where t.Uid == data.Uid
                                       select t).FirstOrDefault();

                        if (result.item != null)
                        {
							isInsert = false;
                            result.item.UpdateDate = DateTime.Now;
                            result.item.Uid_UpdateUser = header.cmsUserUid;
							result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;
                        }
                        else
                        {
							isInsert = true;
                            result.item = new CmsRouting();
							//result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser  = header.cmsUserUid;
							result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

							/*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsRouting.Max(m => m.Ord);
								mm++;
							}
							catch
							{
	                        
							}
							result.item.Ord = (Int32)mm;
							*/

                            //
                            if (data.CreationDate.Equals(DateTime.MinValue))
                                result.item.CreationDate = DateTime.Now;
                            else
                                result.item.CreationDate = data.CreationDate;
                        }
                    }
                    else
                    {
						isInsert = true;
                        result.item = new CmsRouting();
                        result.item.Uid_CreationUser  = header.cmsUserUid;
						result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						/*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsRouting.Max(m => m.Ord));
                            mm++;
						}
						catch
						{
	                        
						}
						result.item.Ord = (Int32)mm;
						*/

                        //
                        if (data.CreationDate.Equals(DateTime.MinValue))
                            result.item.CreationDate = DateTime.Now;
                        else
                            result.item.CreationDate = data.CreationDate;

						//result.item.UpdateDate = DateTime.Now;
                    }

                    //
                    if (result != null)
                    {
                        //
						if (data.Uid_CreationUser != null)
						    result.item.Uid_CreationUser = data.Uid_CreationUser;
						else
						    result.item.Uid_CreationUser = null;

						if (data.StatusFlag != null)
						    result.item.StatusFlag = data.StatusFlag;
						else
						    result.item.StatusFlag = 0;

						if (data.Uid_CmsNlsContext != null)
						    result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;
						else
						    result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						result.item.NameRoute = data.NameRoute;

						result.item.UrlMapping = data.UrlMapping;

						result.item.UrlPhysicalPage = data.UrlPhysicalPage;

						result.item.MetaTagTitle = data.MetaTagTitle;

						result.item.MetaTagDescription = data.MetaTagDescription;

						result.item.MetaTagKeywords = data.MetaTagKeywords;


                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsRouting.Add(result.item);
                    //
                    dbCtx.Context.SaveChanges();
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

		// ******************************************************************************* //
        // Elimino il record
        // ******************************************************************************* //
        public CmsRoutingItemResponse CmsRoutingDelete(MyHeader header, CmsRouting data)
        {

            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsRouting
                                   where t.Uid == data.Uid
                                   select t).FirstOrDefault();

                    if (result != null)
                    {
                        result.item.UpdateDate = DateTime.Now;
                        result.item.Uid_UpdateUser = header.cmsUserUid;
                        result.item.StatusFlag = (int)EnumCmsContent.Deleted;

                        //
                        dbCtx.Context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //
                result.Ex = ex;
                result.Error = ex.Message;
                result.Success = false;
            }
            //
            return result;
        }       

		// ******************************************************************************* //
        // Ordino i record
        // ******************************************************************************* //
		// -1: mette uid prima di refToUid
        // +1: mette uid dopo di refToUid
		/*
		public CmsRoutingItemResponse CmsRoutingSetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                // CmsRouting
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsRouting that = dbCtx.Context.CmsRouting.Where(t => t.Uid_CmsNlsContext == header.cmsCmsNlsContext.Uid && t.Uid == uid).FirstOrDefault();
                    CmsRouting pivot = dbCtx.Context.CmsRouting.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;
                    //decimal pivot_lOrd = pivot.lOrd;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsRouting set Ord=Ord+1 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>=" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsRouting set Ord=Ord+2 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsRoutingGet(header, uid);
            }
            catch (Exception ex)
            {
                //
                result.Ex = ex;
                result.Error = ex.Message;
                result.Success = false;
            }
            //
            return result;
        }     
		*/

		#endregion
		// ******************************************************************************* //
        // Pubblico una CmsRouting o rispristino un nuvo record
        // ******************************************************************************* //
		#region Publish Method
		/*
		private String CmsRouting_Versioning_MoveFromDb(MyHeader header, Int32 _uid, CmsRouting ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsRouting n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsRouting>(CmsRouting_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsRouting n1 = dbCtx.db.CmsRouting.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsRouting_VersionableObjectCopy(n0, n1 == null ? new CmsRouting() : n1) as CmsRouting;
                if (n1.ToAdd)
                    dbCtx.db.CmsRouting.Add(n1);
                // immagini


                //CmsRouting_PublishRelated<CmsRouting_Models>(ctx, n1, "CmsRouting_Models", "Uid_CmsRouting", n1.__CmsRouting_Models, "Model", "Uid_Model", newContext != null);
                //CmsRouting_PublishRelated<CmsRouting_Models>(dbCtx, n1, "CmsRouting_Models", "Uid_CmsRouting", n1.__CmsRouting_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRouting set " +
                                            "StatusFlag = " + (int)EnumCmsContentStatus.Draft +
                                            ",Uid_PublishUser = NULL " +
                                            ",Uid_UpdateUser = '" + header.cmsUserUid + "' " +
                                            ",UpdateDate = getdate() " +
                                            ",PublishDate = NULL " +
                                            ",VersionPublished = 0 " +
                                            ",VersionCurrent = 1 " +
                                            //",PublishOnlineDate = NULL " +
                                            //",PublishOfflineDate = NULL " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else if (ripristina == null) // PUBBLICAZIONE
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRouting set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRouting set " +
                                            "StatusFlag = " + (int)EnumCmsContentStatus.Draft +
                                            ",Uid_PublishUser = NULL " +
                                            ",Uid_UpdateUser = '" + header.cmsUserUid + "' " +
                                            ",UpdateDate = getdate() " +
                                            ",PublishDate = NULL " +
                                            //",PublishOnlineDate = NULL " +
                                            //",PublishOfflineDate = NULL " +
                                            "where Uid='" + n0.Uid + "'");
                }
                dbCtx.SaveChanges();
                //
                return n0.Uid;
            }
        }
		*/
		
        // ******************************************************************************* //
        // Copia un record
        // ******************************************************************************* //
		/*
        public CmsRoutingItemResponse CmsRoutingVersioning_Copy(MyHeader header, Int32 uid, NewContextInfo ctxInfo)
        {
            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRouting", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                String newUid = CmsRouting_Versioning_MoveFromDb(header, uid, null, ctxInfo);
                return CmsRoutingGet(header, newUid);
            }
            catch (ConstraintException ex)
            {
                result.Ex = ex;
                result.Error = "ConstraintException";
                result.Success = false;
            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Error = ex.Message;
                result.Success = false;
            }
            finally
            {
                //CacheManager.that.RemoveAllFromCache(header.cmsCmsNlsContext.Uid, "CmsRouting");
            }

			//
			return result;
        }		
		*/
		
        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
		/*
        public CmsRoutingItemResponse CmsRouting_Publish(MyHeader header, Int32 uid)
        {
            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRouting", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                CmsRouting_Versioning_MoveFromDb(header, uid, null);
                return CmsRoutingGet(header, uid);
            }
            catch (ConstraintException ex)
            {
                result.Ex = ex;
                result.Error = "ConstraintException";
                result.Success = false;

                return result;
            }
            catch (Exception ex)
            {
                //
                result.Ex = ex;
                result.Error = ex.Message;
                result.Success = false;
                return result;
            }
            finally
            {
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsRouting");
            }
        }        
        // per pubblicare le relazioni
        public void CmsRouting_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsRouting img1 = dbCtx.Context.CmsRouting.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsRouting();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsRouting)CmsRouting_VersionableObjectCopy(img0, img1);
                
                // PATCH
                if (dbCtx._dummy.ContainsKey(childTableName + "/" + img0.Key))
                    img1.ToAdd = false;
                dbCtx._dummy[childTableName + "/" + img0.Key] = true;
                // END

                if (img1.ToAdd)
                {
                    // DEVO VERIFICARE VINCOLI?
                    if (constTableName != null && constFieldName != null)
                    {
                        String refValue = (String)(img1.GetType().GetProperty(constFieldName).GetGetMethod().Invoke(img1, null));
                        CmsRouting o = dbCtx.Context.CmsRouting.Where("where Uid='" + refValue + "'").FirstOrDefault();
                        if (o == null)
                        {
                            if (img1.SuperStatusFlag == (int)EnumCmsContentStatus.Deleted)
                            {
                                img1.ToAdd = false;
                            }
                            else if (dbCtx.isProd)
                            {
                                using (MyEntityContext ctx1 = new MyEntityContext(true, false))
                                {
                                    CmsRouting o1 = dbCtx.Context.CmsRouting.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
                                    img1.ToAdd = o1 != null;
                                }
                            }
                            if (img1.ToAdd)
                            {
                                if (ignoreConstraints)
                                    img1.ToAdd = false;
                                else
                                    throw new ConstraintException(constTableName);
                            }
                        }
                    }
                    if (img1.ToAdd)
                    {
                        dbCtx.Context.CmsRouting.Add(img1);
                        lst += ",'" + img1.Key + "'";
                    }
                }
                else
                {
                    lst += ",'" + img1.Key + "'";
                }
            }
            if (childFieldName != null)
                dbCtx.SqlExecuteNonQuery("update " + childTableName + " set StatusFlag=" + (int)(EnumCmsContentStatus.Deleted) + " where " + childFieldName + "='" + masterUid + "' and Uid NOT IN(" + lst + ")");
        }
		*/
        #endregion

        #region Add Method	
		/*
        private String CmsRouting_db2JSON(Int32 uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsRouting itm = (from t in dbCtx.Context.CmsRouting
														  .Include("CmsNlsContext")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
		*/
		/*
        private long CmsRouting_AddToHistory(MyHeader header, String json, ICmsContent content)
        {
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsHistory h = new CmsHistory();
                h.ContentName = content.ContentName;
                h.CreationDate = DateTime.Now;
                h.Uid_CreationUser  = header.cmsUserUid;
                h.SerializedObject = json;
                h.Version = content.VersionCurrent;
                h.PublishDate = content.PublishDate;
                h.Uid_PublishUser = content.Uid_PublishUser;
                h.Uid_CmsNlsContext = content.Uid_CmsNlsContext;
                h.ContentUid = content.Uid;
                dbCtx.db.CmsHistory.Add(h);
                dbCtx.SaveChanges();
                return (long)(h.Id);
            }
        }
        private ICmsVersionable CmsRouting_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
        {            //
            PropertyInfo[] sourceProps = source.GetType().GetProperties(BindingFlags.Public |
                                                            BindingFlags.IgnoreCase |
                                                            BindingFlags.Instance |
                                                            BindingFlags.NonPublic);
            foreach (PropertyInfo sfld in sourceProps)
            {
                object[] attrs = sfld.GetCustomAttributes(true);
                bool ok = false;
                for (int i = 0; ok == false && i < attrs.Length; i++)
                    ok = attrs[i] is Newtonsoft.Json.JsonPropertyAttribute;
                if (!ok)
                    continue;
                PropertyInfo dfld = destination.GetType().GetProperty(sfld.Name,
                                        BindingFlags.Public |
                                        BindingFlags.IgnoreCase |
                                        BindingFlags.Instance |
                                        BindingFlags.NonPublic);
                try
                {
                    if (dfld != null && sfld.Name.ToLower() != "ord")
                        dfld.SetValue(destination, sfld.GetValue(source, null), null);
                }
                catch
                {

                }
            }
            //
            return destination;
        }
		*/
        #endregion
    }
}

