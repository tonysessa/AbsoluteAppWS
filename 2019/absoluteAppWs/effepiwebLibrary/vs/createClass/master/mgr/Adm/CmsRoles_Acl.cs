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
    #region CmsRoles_Acl
    [DataContract]
    public class CmsRoles_AclListOptions
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
        public Int32? uid_CreationUser = null;

        [DataMember]
        public Int32? statusFlag = null;

        [DataMember]
        public Int32? uid_CmsRoles = null;

        [DataMember]
        public Int32? uid_CmsSubSections = null;

        [DataMember]
        public Int32? canRead = null;

        [DataMember]
        public Int32? canWrite = null;


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
    public class CmsRoles_AclItemResponse
    {
        [DataMember]
        public CmsRoles_Acl item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsRoles_AclListResponse
    {
        [DataMember]
        public List<CmsRoles_Acl> items = null;

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
        public CmsRoles_AclListResponse CmsRoles_AclList(MyHeader header, CmsRoles_AclListOptions rq)
        {

            CmsRoles_AclListResponse results = new CmsRoles_AclListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRoles_Acl> query = from t in dbCtx.Context.CmsRoles_Acl
                                                          .Include("CmsRoles")
                                                          .Include("CmsSubSections")
                                                     where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
                                                     (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
                                                     (rq.uid_CmsRoles == null || t.Uid_CmsRoles == rq.uid_CmsRoles) &&
                                                     (rq.uid_CmsSubSections == null || t.Uid_CmsSubSections == rq.uid_CmsSubSections) &&
                                                     (rq.canRead == null || t.CanRead == rq.canRead) &&
                                                     (rq.canWrite == null || t.CanWrite == rq.canWrite) &&
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
        public CmsRoles_AclListResponse CmsRoles_AclListCms(MyHeader header, CmsRoles_AclListOptions rq)
        {

            CmsRoles_AclListResponse results = new CmsRoles_AclListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRoles_Acl> query = from t in dbCtx.Context.CmsRoles_Acl
                                                          .Include("CmsRoles")
                                                          .Include("CmsSubSections")
                                                          .Include("CmsSubSections.CmsSections")
                                                     where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
                                                          (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
                                                          (rq.uid_CmsRoles == null || t.Uid_CmsRoles == rq.uid_CmsRoles) &&
                                                          (rq.uid_CmsSubSections == null || t.Uid_CmsSubSections == rq.uid_CmsSubSections) &&
                                                          (rq.canRead == null || t.CanRead == rq.canRead) &&
                                                          (rq.canWrite == null || t.CanWrite == rq.canWrite) &&
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
        public CmsRoles_AclItemResponse CmsRoles_AclGet(MyHeader header, String Uid)
        {
            return CmsRoles_AclGet(header, System.Convert.ToInt32(Uid));
        }
        public CmsRoles_AclItemResponse CmsRoles_AclGet(MyHeader header, Int32 Uid)
        {

            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRoles_Acl
                                                          .Include("CmsRoles")
                                                          .Include("CmsSubSections")

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
        public CmsRoles_AclItemResponse CmsRoles_AclGetCms(MyHeader header, String Uid)
        {
            return CmsRoles_AclGetCms(header, System.Convert.ToInt32(Uid));
        }
        public CmsRoles_AclItemResponse CmsRoles_AclGetCms(MyHeader header, Int32 Uid)
        {

            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRoles_Acl
                                                          .Include("CmsRoles")
                                                          .Include("CmsSubSections")

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
        public CmsRoles_AclItemResponse CmsRoles_AclGetFrom(MyHeader header, String Uid_From)
        {

            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRoles_Acl
														  .Include("CmsRoles")
														  .Include("CmsSubSections")

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
        public CmsRoles_AclItemResponse CmsRoles_AclGetFromCms(MyHeader header, String Uid_From)
        {

            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRoles_Acl
														  .Include("CmsRoles")
														  .Include("CmsSubSections")

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
		public CmsRoles_AclItemResponse CmsRoles_AclGetForUpdate(MyHeader header, Int32 uid)
		{
			return CmsRoles_AclGetForUpdate(header, uid, "");
		}
		public CmsRoles_AclItemResponse CmsRoles_AclGetForUpdate(MyHeader header, Int32 uid, Int32 cmsHistoryId )
        {
            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRoles_Acl", true, true, false)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsRoles_Acl itm = (from t in dbCtx.db.CmsRoles_Acl
														  .Include("CmsRoles")
														  .Include("CmsSubSections")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsRoles_Acl_db2JSON(itm.Uid);
                        CmsRoles_Acl_AddToHistory(header, json, itm);
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
                        CmsRoles_Acl n0 = Int21h.jsonDecode<CmsRoles_Acl>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsRoles_Acl_Versioning_MoveFromDb(header, uid, n0);
                        return CmsRoles_AclGet(header, uid);
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
        public CmsRoles_AclItemResponse CmsRoles_AclUpsert(MyHeader header, CmsRoles_Acl data)
        {
            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsRoles_Acl
                                                          .Include("CmsRoles")
                                                          .Include("CmsSubSections")

                                       where t.Uid == data.Uid
                                       select t).FirstOrDefault();

                        if (result.item != null)
                        {
                            isInsert = false;
                            result.item.UpdateDate = DateTime.Now;
                            result.item.Uid_UpdateUser = header.cmsUserUid;
                            //result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;
                        }
                        else
                        {
                            isInsert = true;
                            result.item = new CmsRoles_Acl();
                            //result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser = header.cmsUserUid;
                            //result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

                            /*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsRoles_Acl.Max(m => m.Ord);
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
                        result.item = new CmsRoles_Acl();
                        result.item.Uid_CreationUser = header.cmsUserUid;
                        //result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

                        /*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsRoles_Acl.Max(m => m.Ord));
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

                        if (data.Uid_CmsRoles != null)
                            result.item.Uid_CmsRoles = data.Uid_CmsRoles;
                        else
                            result.item.Uid_CmsRoles = null;

                        if (data.Uid_CmsSubSections != null)
                            result.item.Uid_CmsSubSections = data.Uid_CmsSubSections;
                        else
                            result.item.Uid_CmsSubSections = null;

                        if (data.CanRead != null)
                            result.item.CanRead = data.CanRead;
                        else
                            result.item.CanRead = 1;

                        if (data.CanWrite != null)
                            result.item.CanWrite = data.CanWrite;
                        else
                            result.item.CanWrite = 1;


                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsRoles_Acl.Add(result.item);
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
        public CmsRoles_AclItemResponse CmsRoles_AclDelete(MyHeader header, CmsRoles_Acl data)
        {

            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsRoles_Acl
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
		public CmsRoles_AclItemResponse CmsRoles_AclSetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            //
            try
            {
                // CmsRoles_Acl
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsRoles_Acl that = dbCtx.Context.CmsRoles_Acl.Where(t => t.Uid_CmsNlsContext == header.cmsCmsNlsContext.Uid && t.Uid == uid).FirstOrDefault();
                    CmsRoles_Acl pivot = dbCtx.Context.CmsRoles_Acl.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;
                    //decimal pivot_lOrd = pivot.lOrd;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsRoles_Acl set Ord=Ord+1 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>=" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsRoles_Acl set Ord=Ord+2 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsRoles_AclGet(header, uid);
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
        // Pubblico una CmsRoles_Acl o rispristino un nuvo record
        // ******************************************************************************* //
        #region Publish Method
        /*
		private String CmsRoles_Acl_Versioning_MoveFromDb(MyHeader header, Int32 _uid, CmsRoles_Acl ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsRoles_Acl n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsRoles_Acl>(CmsRoles_Acl_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsRoles_Acl n1 = dbCtx.db.CmsRoles_Acl.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsRoles_Acl_VersionableObjectCopy(n0, n1 == null ? new CmsRoles_Acl() : n1) as CmsRoles_Acl;
                if (n1.ToAdd)
                    dbCtx.db.CmsRoles_Acl.Add(n1);
                // immagini


                //CmsRoles_Acl_PublishRelated<CmsRoles_Acl_Models>(ctx, n1, "CmsRoles_Acl_Models", "Uid_CmsRoles_Acl", n1.__CmsRoles_Acl_Models, "Model", "Uid_Model", newContext != null);
                //CmsRoles_Acl_PublishRelated<CmsRoles_Acl_Models>(dbCtx, n1, "CmsRoles_Acl_Models", "Uid_CmsRoles_Acl", n1.__CmsRoles_Acl_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRoles_Acl set " +
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
                    dbCtx.SqlExecuteNonQuery("update CmsRoles_Acl set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsRoles_Acl set " +
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
        public CmsRoles_AclItemResponse CmsRoles_AclVersioning_Copy(MyHeader header, Int32 uid, NewContextInfo ctxInfo)
        {
            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRoles_Acl", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                String newUid = CmsRoles_Acl_Versioning_MoveFromDb(header, uid, null, ctxInfo);
                return CmsRoles_AclGet(header, newUid);
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
                //CacheManager.that.RemoveAllFromCache(header.cmsCmsNlsContext.Uid, "CmsRoles_Acl");
            }

			//
			return result;
        }		
		*/

        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
        /*
        public CmsRoles_AclItemResponse CmsRoles_Acl_Publish(MyHeader header, Int32 uid)
        {
            CmsRoles_AclItemResponse result = new CmsRoles_AclItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsRoles_Acl", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                CmsRoles_Acl_Versioning_MoveFromDb(header, uid, null);
                return CmsRoles_AclGet(header, uid);
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
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsRoles_Acl");
            }
        }        
        // per pubblicare le relazioni
        public void CmsRoles_Acl_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsRoles_Acl img1 = dbCtx.Context.CmsRoles_Acl.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsRoles_Acl();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsRoles_Acl)CmsRoles_Acl_VersionableObjectCopy(img0, img1);
                
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
                        CmsRoles_Acl o = dbCtx.Context.CmsRoles_Acl.Where("where Uid='" + refValue + "'").FirstOrDefault();
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
                                    CmsRoles_Acl o1 = dbCtx.Context.CmsRoles_Acl.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
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
                        dbCtx.Context.CmsRoles_Acl.Add(img1);
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
        private String CmsRoles_Acl_db2JSON(Int32 uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsRoles_Acl itm = (from t in dbCtx.Context.CmsRoles_Acl
														  .Include("CmsRoles")
														  .Include("CmsSubSections")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
		*/
        /*
        private long CmsRoles_Acl_AddToHistory(MyHeader header, String json, ICmsContent content)
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
        private ICmsVersionable CmsRoles_Acl_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
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

