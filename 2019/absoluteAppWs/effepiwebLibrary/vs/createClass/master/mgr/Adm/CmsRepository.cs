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
    #region CmsRepository
    [DataContract]
    public class CmsRepositoryListOptions
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
        public Int32? Uid_CreationUser = null;

		[DataMember]
		public Int32? statusFlag = null;

		[DataMember]
		public Int32? uid_CmsNlsContext = null;

		[DataMember]
		public Int32? sharedFlag = null;


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
    public class CmsRepositoryItemResponse
    {
        [DataMember]
        public CmsRepository item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsRepositoryListResponse
    {
        [DataMember]
        public List<CmsRepository> items = null;

        [DataMember]
        public Paging paging = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class FileItemResponse
    {
        [DataMember]
        public CmsFile item = null;
    }


    [DataContract]
    public class StorageFileItemResponse
    {
        [DataMember]
        public CmsStorageFile item = null;

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
        public CmsRepositoryListResponse CmsRepositoryList(MyHeader header, CmsRepositoryListOptions rq)
        {

            CmsRepositoryListResponse results = new CmsRepositoryListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRepository> query = from t in dbCtx.Context.CmsRepository														  					  
														  //.Include("CmsFile")
														  .Include("CmsNlsContext")
														  where (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.sharedFlag == null || t.SharedFlag == rq.sharedFlag) &&
														  (rq.searchText == null || t.Note.Contains(rq.searchText)) &&
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
		public CmsRepositoryListResponse CmsRepositoryListCms(MyHeader header, CmsRepositoryListOptions rq)
        {

            CmsRepositoryListResponse results = new CmsRepositoryListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRepository> query = from t in dbCtx.Context.CmsRepository														  				  
														  //.Include("CmsFile")
														  .Include("CmsNlsContext")
														  where (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.sharedFlag == null || t.SharedFlag == rq.sharedFlag) &&
														  (rq.searchText == null || t.Note.Contains(rq.searchText)) &&														  
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
        public CmsRepositoryItemResponse CmsRepositoryGet(MyHeader header, String Uid)
        {
			CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                result = CmsRepositoryGet(header, System.Convert.ToInt32(Uid));
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
        public CmsRepositoryItemResponse CmsRepositoryGet(MyHeader header, Int32 Uid)
        {

            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRepository
														  //.Include("CmsFile")
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
        public CmsRepositoryItemResponse CmsRepositoryGetCms(MyHeader header, String Uid)
        {
			CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                result = CmsRepositoryGetCms(header, System.Convert.ToInt32(Uid));
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
        public CmsRepositoryItemResponse CmsRepositoryGetCms(MyHeader header, Int32 Uid)
        {

            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRepository
														  //.Include("CmsFile")
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

        // ******************************************************************************* //
        // Prendo un record e salvo la versione precedente
        // ******************************************************************************* //
		/*
		public CmsRepositoryItemResponse CmsRepositoryGetForUpdate(MyHeader header, String uid)
		{
			return CmsRepositoryGetForUpdate(header, uid, "");
		}
		public CmsRepositoryItemResponse CmsRepositoryGetForUpdate(MyHeader header, String uid, String cmsHistoryId )
        {
            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRepository", true, true, false)) == null)
            //{
            //    rs.ErrorCode = ServiceResponse.ERROR_AUTHORIZATION;
            //    return rs;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsRepository itm = (from t in dbCtx.db.CmsRepository
														  //.Include("CmsFile")
														  .Include("CmsNlsContext")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsRepository_db2JSON(itm.Uid);
                        CmsRepository_AddToHistory(header, json, itm);
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
                        CmsRepository n0 = Int21h.jsonDecode<CmsRepository>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsRepository_Versioning_MoveFromDb(header, uid, n0);
                        return CmsRepositoryGet(header, uid);
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
        public CmsRepositoryItemResponse CmsRepositoryUpsert(MyHeader header, CmsRepository data)
        {
            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    bool isInsert = true;
                    if (!String.IsNullOrEmpty("" + data.Uid))
                        isInsert = false;

                    if (!String.IsNullOrEmpty("" + data.Uid))
                    {
                        result.item = (from t in dbCtx.Context.CmsRepository									  
														  //.Include("CmsFile")
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
                            result.item = new CmsRepository();
							result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser = header.cmsUserUid;
							result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

							/*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsRepository.Max(m => m.Ord);
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
                        result.item = new CmsRepository();
						
						// Uid
						Int32? mmUid = 1;
						mmUid = (Int32)dbCtx.Context.CmsRepository.Max(m => m.Uid);
						mmUid++;
						result.item.Uid = (Int32)mmUid;
						
						//
                        result.item.Uid_CreationUser = header.cmsUserUid;
						result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						/*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsRepository.Max(m => m.Ord));
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
						result.item.UpdateDate = DateTime.Now;
                    }

                    //
                    if (result != null)
                    {
                        //
						if (data.StatusFlag != null)
						    result.item.StatusFlag = data.StatusFlag;
						else
						    result.item.StatusFlag = 0;

						if (data.Uid_CmsNlsContext != null)
						    result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;
						else
						    result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						if (data.SharedFlag != null)
						    result.item.SharedFlag = data.SharedFlag;
						else
						    result.item.SharedFlag = 0;

						result.item.Note = data.Note;

                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsRepository.Add(result.item);
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
        public CmsRepositoryItemResponse CmsRepositoryDelete(MyHeader header, CmsRepository data)
        {

            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsRepository
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
		public CmsRepositoryItemResponse CmsRepositorySetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            //
            try
            {
                // CmsRepository
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsRepository that = dbCtx.Context.CmsRepository.Where(t => t.Uid_CmsNlsContext == header.cmsCmsNlsContext.Uid && t.Uid == uid).FirstOrDefault();
                    CmsRepository pivot = dbCtx.Context.CmsRepository.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsRepository set Ord = Ord+1 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord >= " + (Int32)pivot_Ord + " and Uid != '" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsRepository set Ord = Ord+2 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord > " + (Int32)pivot_Ord + " and Uid != '" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsRepositoryGet(header, uid);
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

		#region Publish Method
        /*
		public OrderPublishStatusResponse Versioning_GetOrderPublishStatus()
        {
            OrderPublishStatusResponse rs = new OrderPublishStatusResponse();
            //
            // security
            if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRepository", true, false, false)) == null)
            {
                rs.ErrorCode = ServiceResponse.ERROR_AUTHORIZATION;
                return rs;
            }
            //
            try
            {
                //
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsOrderStatus os = ctx.db.CmsOrderStatus.Where(t => t.ContentName == "CmsRepository" && t.Uid_CmsNlsContext == header.cmsContext.Uid).FirstOrDefault();
                    if (os != null)
                        rs.RequirePublish = os.RequirePublish != 0;
                    rs.Success = true;
                }
            }
            catch (Exception ex)
            {
                rs.ErrorCode = ServiceResponse.ERROR_RUNTIME;
                rs.ErrorDescription = ex.Message;
            }
            //
            return rs;
        }		
		public ServiceResponse Versioning_PublishOrder()
        {
            ServiceResponse rs = new ServiceResponse();
            // securit
            if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRepository", true, false, true)) == null)
            {
                rs.ErrorCode = ServiceResponse.ERROR_AUTHORIZATION;
                return rs;
            }
            try
            {
                Dictionary<String, decimal> items = new Dictionary<String, decimal>();
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    foreach (var itm in ctx.db.CmsRepository.Where(t => t.Uid_CmsNlsContext == header.cmsContext.Uid))
                        items[itm.Uid] = itm.Ord;
                }
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    foreach (String uid in items.Keys)
                    {
                        var dbItem = ctx.db.CmsRepository.Where(t => t.Uid == uid).FirstOrDefault();
                        if (dbItem != null)
                            dbItem.Ord = (long)items[uid];
                    }

                    //
                    ctx.SaveChanges();
                }
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsOrderStatus os = ctx.db.CmsOrderStatus.Where(t => t.ContentName == "CmsRepository" && t.Uid_CmsNlsContext == header.cmsContext.Uid).FirstOrDefault();
                    if (os != null)
                        os.RequirePublish = 0;
                    ctx.SaveChanges();
                    rs.Success = true;
                    return rs;
                }
            }
            catch (Exception ex)
            {
                rs.ErrorCode = ServiceResponse.ERROR_RUNTIME;
                rs.ErrorDescription = ex.Message;
                return rs;
            }
            finally
            {
                CacheManager.that.RemoveAllFromCacheBO(header.cmsContext, "CmsRepository");
            }
        }
		*/
		/*
        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
        public CmsRepositoryItemResponse CmsRepository_Publish(MyHeader header, String uid)
        {
            CmsRepositoryItemResponse result = new CmsRepositoryItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRepository", true, false, true)) == null)
            //{
            //    rs.ErrorCode = ServiceResponse.ERROR_AUTHORIZATION;
            //    return rs;
            //}
            try
            {
                CmsRepository_Versioning_MoveFromDb(header, uid, null);
                return CmsRepositoryGet(header, uid);
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
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsRepository");
            }
        }
        // ******************************************************************************* //
        // Pubblico una CmsRepository o rispristino un nuvo record
        // ******************************************************************************* //
        private String CmsRepository_Versioning_MoveFromDb(MyHeader header, String _uid, CmsRepository ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsRepository n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsRepository>(CmsRepository_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsRepository n1 = dbCtx.db.CmsRepository.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsRepository_VersionableObjectCopy(n0, n1 == null ? new CmsRepository() : n1) as CmsRepository;
                if (n1.ToAdd)
                    dbCtx.db.CmsRepository.Add(n1);
                // immagini


                //CmsRepository_PublishRelated<CmsRepository_Models>(ctx, n1, "CmsRepository_Models", "Uid_CmsRepository", n1.__CmsRepository_Models, "Model", "Uid_Model", newContext != null);
                //CmsRepository_PublishRelated<CmsRepository_Models>(dbCtx, n1, "CmsRepository_Models", "Uid_CmsRepository", n1.__CmsRepository_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRepository set " +
                                            "StatusFlag = " + (int)EnumCmsContentStatus.Draft +
                                            ",Uid_PublishUser = NULL " +
                                            ",Uid_CmsUsers_Mod = '" + header.cmsUserUid + "' " +
                                            ",DataUltimaModifica = getdate() " +
                                            ",PublishDate = NULL " +
                                            ",VersionPublished = 0 " +
                                            ",VersionCurrent = 1 " +
                                            //",PublishOnlineDate = NULL " +
                                            //",PublishOfflineDate = NULL " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else if (ripristina == null) // PUBBLICAZIONE
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRepository set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRepository set " +
                                            "StatusFlag = " + (int)EnumCmsContentStatus.Draft +
                                            ",Uid_PublishUser = NULL " +
                                            ",Uid_CmsUsers_Mod = '" + header.cmsUserUid + "' " +
                                            ",DataUltimaModifica = getdate() " +
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
        // per pubblicare le relazioni
        public void CmsRepository_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsRepository img1 = dbCtx.Context.CmsRepository.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsRepository();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsRepository)CmsRepository_VersionableObjectCopy(img0, img1);
                
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
                        CmsRepository o = dbCtx.Context.CmsRepository.Where("where Uid='" + refValue + "'").FirstOrDefault();
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
                                    CmsRepository o1 = dbCtx.Context.CmsRepository.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
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
                        dbCtx.Context.CmsRepository.Add(img1);
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
        private String CmsRepository_db2JSON(String uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsRepository itm = (from t in dbCtx.db.CmsRepository
														  //.Include("CmsFile")
														  .Include("CmsNlsContext")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
        private long CmsRepository_AddToHistory(MyHeader header, String json, ICmsContent content)
        {
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsHistory h = new CmsHistory();
                h.ContentName = content.ContentName;
                h.DataCreazione = DateTime.Now;
                h.Uid_CreationUser = header.cmsUserUid;
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
        private ICmsVersionable CmsRepository_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
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

