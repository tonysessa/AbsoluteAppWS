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
    #region CmsUsers
    [DataContract]
    public class CmsUsersListOptions
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
		public String name = null;

		[DataMember]
		public String surname = null;

		[DataMember]
		public String email = null;

		[DataMember]
		public String username = null;

		[DataMember]
		public String password = null;

		[DataMember]
		public DateTime? dateLastLogin = null;

		[DataMember]
		public Int32? numLogin = null;

		[DataMember]
		public Int32? uid_CmsRoles = null;


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
    public class CmsUsersItemResponse
    {
        [DataMember]
        public CmsUsers item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsUsersListResponse
    {
        [DataMember]
        public List<CmsUsers> items = null;

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
        public CmsUsersListResponse CmsUsersList(MyHeader header, CmsUsersListOptions rq)
        {

            CmsUsersListResponse results = new CmsUsersListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsUsers> query = from t in dbCtx.Context.CmsUsers														  					  
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  (rq.name == null || t.Name == rq.name) &&
														  (rq.surname == null || t.Surname == rq.surname) &&
														  (rq.email == null || t.Email == rq.email) &&
														  (rq.username == null || t.Username == rq.username) &&
														  (rq.password == null || t.Password == rq.password) &&
														  (rq.dateLastLogin == null || t.DateLastLogin == rq.dateLastLogin) &&
														  (rq.numLogin == null || t.NumLogin == rq.numLogin) &&
														  (rq.uid_CmsRoles == null || t.Uid_CmsRoles == rq.uid_CmsRoles) &&
														  (rq.searchText == null || t.Name.Contains(rq.searchText) ||t.Surname.Contains(rq.searchText) ||t.Email.Contains(rq.searchText) ||t.Username.Contains(rq.searchText) ||t.Password.Contains(rq.searchText) ||t.Note.Contains(rq.searchText)) &&
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
		public CmsUsersListResponse CmsUsersListCms(MyHeader header, CmsUsersListOptions rq)
        {

            CmsUsersListResponse results = new CmsUsersListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsUsers> query = from t in dbCtx.Context.CmsUsers														  				  
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  (rq.name == null || t.Name == rq.name) &&
														  (rq.surname == null || t.Surname == rq.surname) &&
														  (rq.email == null || t.Email == rq.email) &&
														  (rq.username == null || t.Username == rq.username) &&
														  (rq.password == null || t.Password == rq.password) &&
														  (rq.dateLastLogin == null || t.DateLastLogin == rq.dateLastLogin) &&
														  (rq.numLogin == null || t.NumLogin == rq.numLogin) &&
														  (rq.uid_CmsRoles == null || t.Uid_CmsRoles == rq.uid_CmsRoles) &&
														  (rq.searchText == null || t.Name.Contains(rq.searchText) ||t.Surname.Contains(rq.searchText) ||t.Email.Contains(rq.searchText) ||t.Username.Contains(rq.searchText) ||t.Password.Contains(rq.searchText) ||t.Note.Contains(rq.searchText)) &&														  
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
        public CmsUsersItemResponse CmsUsersGet(MyHeader header, String Uid)
        {
			return CmsUsersGet(header, System.Convert.ToInt32(Uid));
		}
        public CmsUsersItemResponse CmsUsersGet(MyHeader header, Int32 Uid)
        {

            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

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
        public CmsUsersItemResponse CmsUsersGetCms(MyHeader header, String Uid)
        {
			return CmsUsersGetCms(header, System.Convert.ToInt32(Uid));
		}
        public CmsUsersItemResponse CmsUsersGetCms(MyHeader header, Int32 Uid)
        {

            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

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
        public CmsUsersItemResponse CmsUsersGetFrom(MyHeader header, String Uid_From)
        {

            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

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
        public CmsUsersItemResponse CmsUsersGetFromCms(MyHeader header, String Uid_From)
        {

            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

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
		public CmsUsersItemResponse CmsUsersGetForUpdate(MyHeader header, Int32 uid)
		{
			return CmsUsersGetForUpdate(header, uid, "");
		}
		public CmsUsersItemResponse CmsUsersGetForUpdate(MyHeader header, Int32 uid, Int32 cmsHistoryId )
        {
            CmsUsersItemResponse result = new CmsUsersItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsUsers", true, true, false)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsUsers itm = (from t in dbCtx.db.CmsUsers
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsUsers_db2JSON(itm.Uid);
                        CmsUsers_AddToHistory(header, json, itm);
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
                        CmsUsers n0 = Int21h.jsonDecode<CmsUsers>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsUsers_Versioning_MoveFromDb(header, uid, n0);
                        return CmsUsersGet(header, uid);
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
        public CmsUsersItemResponse CmsUsersUpsert(MyHeader header, CmsUsers data)
        {
            CmsUsersItemResponse result = new CmsUsersItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsUsers									  
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

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
                            result.item = new CmsUsers();
							//result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser  = header.cmsUserUid;
							//result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

							/*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsUsers.Max(m => m.Ord);
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
                        result.item = new CmsUsers();
                        result.item.Uid_CreationUser  = header.cmsUserUid;
						//result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						/*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsUsers.Max(m => m.Ord));
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

						result.item.Name = data.Name;

						result.item.Surname = data.Surname;

						result.item.Email = data.Email;

						result.item.Username = data.Username;

						result.item.Password = data.Password;

						if (data.DateLastLogin != null)
						    result.item.DateLastLogin = data.DateLastLogin;
						else
						    result.item.DateLastLogin = null;

						if (data.NumLogin != null)
						    result.item.NumLogin = data.NumLogin;
						else
						    result.item.NumLogin = 0;

						if (data.Uid_CmsRoles != null)
						    result.item.Uid_CmsRoles = data.Uid_CmsRoles;
						else
						    result.item.Uid_CmsRoles = null;

						result.item.Note = data.Note;

                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsUsers.Add(result.item);
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
        public CmsUsersItemResponse CmsUsersDelete(MyHeader header, CmsUsers data)
        {

            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsUsers
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
		public CmsUsersItemResponse CmsUsersSetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                // CmsUsers
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsUsers that = dbCtx.Context.CmsUsers.Where(t => t.Uid_CmsNlsContext == header.cmsCmsNlsContext.Uid && t.Uid == uid).FirstOrDefault();
                    CmsUsers pivot = dbCtx.Context.CmsUsers.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;
                    //decimal pivot_lOrd = pivot.lOrd;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsUsers set Ord=Ord+1 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>=" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsUsers set Ord=Ord+2 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsUsersGet(header, uid);
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
        // Pubblico una CmsUsers o rispristino un nuvo record
        // ******************************************************************************* //
		#region Publish Method
		/*
		private String CmsUsers_Versioning_MoveFromDb(MyHeader header, Int32 _uid, CmsUsers ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsUsers n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsUsers>(CmsUsers_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsUsers n1 = dbCtx.db.CmsUsers.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsUsers_VersionableObjectCopy(n0, n1 == null ? new CmsUsers() : n1) as CmsUsers;
                if (n1.ToAdd)
                    dbCtx.db.CmsUsers.Add(n1);
                // immagini


                //CmsUsers_PublishRelated<CmsUsers_Models>(ctx, n1, "CmsUsers_Models", "Uid_CmsUsers", n1.__CmsUsers_Models, "Model", "Uid_Model", newContext != null);
                //CmsUsers_PublishRelated<CmsUsers_Models>(dbCtx, n1, "CmsUsers_Models", "Uid_CmsUsers", n1.__CmsUsers_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsUsers set " +
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
                    dbCtx.SqlExecuteNonQuery("update CmsUsers set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsUsers set " +
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
        public CmsUsersItemResponse CmsUsersVersioning_Copy(MyHeader header, Int32 uid, NewContextInfo ctxInfo)
        {
            CmsUsersItemResponse result = new CmsUsersItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsUsers", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                String newUid = CmsUsers_Versioning_MoveFromDb(header, uid, null, ctxInfo);
                return CmsUsersGet(header, newUid);
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
                //CacheManager.that.RemoveAllFromCache(header.cmsCmsNlsContext.Uid, "CmsUsers");
            }

			//
			return result;
        }		
		*/
		
        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
		/*
        public CmsUsersItemResponse CmsUsers_Publish(MyHeader header, Int32 uid)
        {
            CmsUsersItemResponse result = new CmsUsersItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsUsers", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                CmsUsers_Versioning_MoveFromDb(header, uid, null);
                return CmsUsersGet(header, uid);
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
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsUsers");
            }
        }        
        // per pubblicare le relazioni
        public void CmsUsers_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsUsers img1 = dbCtx.Context.CmsUsers.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsUsers();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsUsers)CmsUsers_VersionableObjectCopy(img0, img1);
                
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
                        CmsUsers o = dbCtx.Context.CmsUsers.Where("where Uid='" + refValue + "'").FirstOrDefault();
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
                                    CmsUsers o1 = dbCtx.Context.CmsUsers.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
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
                        dbCtx.Context.CmsUsers.Add(img1);
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
        private String CmsUsers_db2JSON(Int32 uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsUsers itm = (from t in dbCtx.Context.CmsUsers
														  //.Include("CmsUsers_Acl")
														  .Include("CmsRoles")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
		*/
		/*
        private long CmsUsers_AddToHistory(MyHeader header, String json, ICmsContent content)
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
        private ICmsVersionable CmsUsers_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
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

