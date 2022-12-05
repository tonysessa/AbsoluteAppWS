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
    #region CmsUsers_Acl
    [DataContract]
    public class CmsUsers_AclListOptions
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
		public Int32? uid_CmsUsers = null;

		[DataMember]
		public Int32? uid_CmsNlsContext = null;

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
    public class CmsUsers_AclItemResponse
    {
        [DataMember]
        public CmsUsers_Acl item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsUsers_AclListResponse
    {
        [DataMember]
        public List<CmsUsers_Acl> items = null;

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
        public CmsUsers_AclListResponse CmsUsers_AclList(MyHeader header, CmsUsers_AclListOptions rq)
        {

            CmsUsers_AclListResponse results = new CmsUsers_AclListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsUsers_Acl> query = from t in dbCtx.Context.CmsUsers_Acl														  					  
														  .Include("CmsUsers")
														  .Include("CmsNlsContext")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  (rq.uid_CmsUsers == null || t.Uid_CmsUsers == rq.uid_CmsUsers) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.canRead == null || t.CanRead == rq.canRead) &&
														  (rq.canWrite == null || t.CanWrite == rq.canWrite)  &&
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
		public CmsUsers_AclListResponse CmsUsers_AclListCms(MyHeader header, CmsUsers_AclListOptions rq)
        {

            CmsUsers_AclListResponse results = new CmsUsers_AclListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsUsers_Acl> query = from t in dbCtx.Context.CmsUsers_Acl														  				  
														  .Include("CmsUsers")
														  .Include("CmsNlsContext")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  (rq.uid_CmsUsers == null || t.Uid_CmsUsers == rq.uid_CmsUsers) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.canRead == null || t.CanRead == rq.canRead) &&
														  (rq.canWrite == null || t.CanWrite == rq.canWrite)  &&														  
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
        public CmsUsers_AclItemResponse CmsUsers_AclGet(MyHeader header, String Uid)
        {
			return CmsUsers_AclGet(header, System.Convert.ToInt32(Uid));
		}
        public CmsUsers_AclItemResponse CmsUsers_AclGet(MyHeader header, Int32 Uid)
        {

            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers_Acl
														  .Include("CmsUsers")
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
        public CmsUsers_AclItemResponse CmsUsers_AclGetCms(MyHeader header, String Uid)
        {
			return CmsUsers_AclGetCms(header, System.Convert.ToInt32(Uid));
		}
        public CmsUsers_AclItemResponse CmsUsers_AclGetCms(MyHeader header, Int32 Uid)
        {

            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers_Acl
														  .Include("CmsUsers")
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
        public CmsUsers_AclItemResponse CmsUsers_AclGetFrom(MyHeader header, String Uid_From)
        {

            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers_Acl
														  .Include("CmsUsers")
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
        public CmsUsers_AclItemResponse CmsUsers_AclGetFromCms(MyHeader header, String Uid_From)
        {

            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers_Acl
														  .Include("CmsUsers")
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
		public CmsUsers_AclItemResponse CmsUsers_AclGetForUpdate(MyHeader header, Int32 uid)
		{
			return CmsUsers_AclGetForUpdate(header, uid, "");
		}
		public CmsUsers_AclItemResponse CmsUsers_AclGetForUpdate(MyHeader header, Int32 uid, Int32 cmsHistoryId )
        {
            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsUsers_Acl", true, true, false)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsUsers_Acl itm = (from t in dbCtx.db.CmsUsers_Acl
														  .Include("CmsUsers")
														  .Include("CmsNlsContext")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsUsers_Acl_db2JSON(itm.Uid);
                        CmsUsers_Acl_AddToHistory(header, json, itm);
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
                        CmsUsers_Acl n0 = Int21h.jsonDecode<CmsUsers_Acl>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsUsers_Acl_Versioning_MoveFromDb(header, uid, n0);
                        return CmsUsers_AclGet(header, uid);
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
        public CmsUsers_AclItemResponse CmsUsers_AclUpsert(MyHeader header, CmsUsers_Acl data)
        {
            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsUsers_Acl									  
														  .Include("CmsUsers")
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
                            result.item = new CmsUsers_Acl();
							//result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser  = header.cmsUserUid;
							result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

							/*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsUsers_Acl.Max(m => m.Ord);
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
                        result.item = new CmsUsers_Acl();
                        result.item.Uid_CreationUser  = header.cmsUserUid;
						result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						/*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsUsers_Acl.Max(m => m.Ord));
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

						if (data.Uid_CmsUsers != null)
						    result.item.Uid_CmsUsers = data.Uid_CmsUsers;
						else
						    result.item.Uid_CmsUsers = null;

						if (data.Uid_CmsNlsContext != null)
						    result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;
						else
						    result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

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
                        dbCtx.Context.CmsUsers_Acl.Add(result.item);
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
        public CmsUsers_AclItemResponse CmsUsers_AclDelete(MyHeader header, CmsUsers_Acl data)
        {

            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsUsers_Acl
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
		public CmsUsers_AclItemResponse CmsUsers_AclSetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                // CmsUsers_Acl
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsUsers_Acl that = dbCtx.Context.CmsUsers_Acl.Where(t => t.Uid_CmsNlsContext == header.cmsCmsNlsContext.Uid && t.Uid == uid).FirstOrDefault();
                    CmsUsers_Acl pivot = dbCtx.Context.CmsUsers_Acl.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;
                    //decimal pivot_lOrd = pivot.lOrd;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsUsers_Acl set Ord=Ord+1 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>=" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsUsers_Acl set Ord=Ord+2 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsUsers_AclGet(header, uid);
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
        // Pubblico una CmsUsers_Acl o rispristino un nuvo record
        // ******************************************************************************* //
		#region Publish Method
		/*
		private String CmsUsers_Acl_Versioning_MoveFromDb(MyHeader header, Int32 _uid, CmsUsers_Acl ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsUsers_Acl n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsUsers_Acl>(CmsUsers_Acl_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsUsers_Acl n1 = dbCtx.db.CmsUsers_Acl.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsUsers_Acl_VersionableObjectCopy(n0, n1 == null ? new CmsUsers_Acl() : n1) as CmsUsers_Acl;
                if (n1.ToAdd)
                    dbCtx.db.CmsUsers_Acl.Add(n1);
                // immagini


                //CmsUsers_Acl_PublishRelated<CmsUsers_Acl_Models>(ctx, n1, "CmsUsers_Acl_Models", "Uid_CmsUsers_Acl", n1.__CmsUsers_Acl_Models, "Model", "Uid_Model", newContext != null);
                //CmsUsers_Acl_PublishRelated<CmsUsers_Acl_Models>(dbCtx, n1, "CmsUsers_Acl_Models", "Uid_CmsUsers_Acl", n1.__CmsUsers_Acl_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsUsers_Acl set " +
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
                    dbCtx.SqlExecuteNonQuery("update CmsUsers_Acl set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsUsers_Acl set " +
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
        public CmsUsers_AclItemResponse CmsUsers_AclVersioning_Copy(MyHeader header, Int32 uid, NewContextInfo ctxInfo)
        {
            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsUsers_Acl", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                String newUid = CmsUsers_Acl_Versioning_MoveFromDb(header, uid, null, ctxInfo);
                return CmsUsers_AclGet(header, newUid);
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
                //CacheManager.that.RemoveAllFromCache(header.cmsCmsNlsContext.Uid, "CmsUsers_Acl");
            }

			//
			return result;
        }		
		*/
		
        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
		/*
        public CmsUsers_AclItemResponse CmsUsers_Acl_Publish(MyHeader header, Int32 uid)
        {
            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsUsers_Acl", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                CmsUsers_Acl_Versioning_MoveFromDb(header, uid, null);
                return CmsUsers_AclGet(header, uid);
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
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsUsers_Acl");
            }
        }        
        // per pubblicare le relazioni
        public void CmsUsers_Acl_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsUsers_Acl img1 = dbCtx.Context.CmsUsers_Acl.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsUsers_Acl();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsUsers_Acl)CmsUsers_Acl_VersionableObjectCopy(img0, img1);
                
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
                        CmsUsers_Acl o = dbCtx.Context.CmsUsers_Acl.Where("where Uid='" + refValue + "'").FirstOrDefault();
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
                                    CmsUsers_Acl o1 = dbCtx.Context.CmsUsers_Acl.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
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
                        dbCtx.Context.CmsUsers_Acl.Add(img1);
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
        private String CmsUsers_Acl_db2JSON(Int32 uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsUsers_Acl itm = (from t in dbCtx.Context.CmsUsers_Acl
														  .Include("CmsUsers")
														  .Include("CmsNlsContext")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
		*/
		/*
        private long CmsUsers_Acl_AddToHistory(MyHeader header, String json, ICmsContent content)
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
        private ICmsVersionable CmsUsers_Acl_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
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

