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
    #region CmsNlsContext
    [DataContract]
    public class CmsNlsContextListOptions
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
		public String startingPage = null;

		[DataMember]
		public String isoCode = null;

		[DataMember]
		public String title = null;

		[DataMember]
		public String description = null;

		[DataMember]
		public Int32? ord = null;


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
    public class CmsNlsContextItemResponse
    {
        [DataMember]
        public CmsNlsContext item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsNlsContextListResponse
    {
        [DataMember]
        public List<CmsNlsContext> items = null;

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
        public CmsNlsContextListResponse CmsNlsContextList(MyHeader header, CmsNlsContextListOptions rq)
        {

            CmsNlsContextListResponse results = new CmsNlsContextListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsNlsContext> query = from t in dbCtx.Context.CmsNlsContext														  					  
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  (rq.startingPage == null || t.StartingPage == rq.startingPage) &&
														  (rq.isoCode == null || t.IsoCode == rq.isoCode) &&
														  (rq.title == null || t.Title == rq.title) &&
														  (rq.description == null || t.Description == rq.description) &&
														  (rq.ord == null || t.Ord == rq.ord) &&
														  (rq.searchText == null || t.StartingPage.Contains(rq.searchText) ||t.IsoCode.Contains(rq.searchText) ||t.Title.Contains(rq.searchText) ||t.Description.Contains(rq.searchText)) &&
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
		public CmsNlsContextListResponse CmsNlsContextListCms(MyHeader header, CmsNlsContextListOptions rq)
        {

            CmsNlsContextListResponse results = new CmsNlsContextListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsNlsContext> query = from t in dbCtx.Context.CmsNlsContext														  				  
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  (rq.startingPage == null || t.StartingPage == rq.startingPage) &&
														  (rq.isoCode == null || t.IsoCode == rq.isoCode) &&
														  (rq.title == null || t.Title == rq.title) &&
														  (rq.description == null || t.Description == rq.description) &&
														  (rq.ord == null || t.Ord == rq.ord) &&
														  (rq.searchText == null || t.StartingPage.Contains(rq.searchText) ||t.IsoCode.Contains(rq.searchText) ||t.Title.Contains(rq.searchText) ||t.Description.Contains(rq.searchText)) &&														  
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
        public CmsNlsContextItemResponse CmsNlsContextGet(MyHeader header, String Uid)
        {
			return CmsNlsContextGet(header, System.Convert.ToInt32(Uid));
		}
        public CmsNlsContextItemResponse CmsNlsContextGet(MyHeader header, Int32 Uid)
        {

            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsNlsContext
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  .Include("GlobalParameter")

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
        public CmsNlsContextItemResponse CmsNlsContextGetCms(MyHeader header, String Uid)
        {
			return CmsNlsContextGetCms(header, System.Convert.ToInt32(Uid));
		}
        public CmsNlsContextItemResponse CmsNlsContextGetCms(MyHeader header, Int32 Uid)
        {

            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsNlsContext
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")

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
        public CmsNlsContextItemResponse CmsNlsContextGetFrom(MyHeader header, String Uid_From)
        {

            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsNlsContext
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")

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
        public CmsNlsContextItemResponse CmsNlsContextGetFromCms(MyHeader header, String Uid_From)
        {

            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsNlsContext
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")

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
		public CmsNlsContextItemResponse CmsNlsContextGetForUpdate(MyHeader header, Int32 uid)
		{
			return CmsNlsContextGetForUpdate(header, uid, "");
		}
		public CmsNlsContextItemResponse CmsNlsContextGetForUpdate(MyHeader header, Int32 uid, Int32 cmsHistoryId )
        {
            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsNlsContext", true, true, false)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsNlsContext itm = (from t in dbCtx.db.CmsNlsContext
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsNlsContext_db2JSON(itm.Uid);
                        CmsNlsContext_AddToHistory(header, json, itm);
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
                        CmsNlsContext n0 = Int21h.jsonDecode<CmsNlsContext>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsNlsContext_Versioning_MoveFromDb(header, uid, n0);
                        return CmsNlsContextGet(header, uid);
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
        public CmsNlsContextItemResponse CmsNlsContextUpsert(MyHeader header, CmsNlsContext data)
        {
            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsNlsContext									  
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")

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
                            result.item = new CmsNlsContext();
							//result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser  = header.cmsUserUid;
							//result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

							
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsNlsContext.Max(m => m.Ord);
								mm++;
							}
							catch
							{
	                        
							}
							result.item.Ord = (Int32)mm;
							

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
                        result.item = new CmsNlsContext();
                        result.item.Uid_CreationUser  = header.cmsUserUid;
						//result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsNlsContext.Max(m => m.Ord));
                            mm++;
						}
						catch
						{
	                        
						}
						result.item.Ord = (Int32)mm;
						

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

						result.item.StartingPage = data.StartingPage;

						result.item.IsoCode = data.IsoCode;

						result.item.Title = data.Title;

						result.item.Description = data.Description;

						if (data.Ord != null)
						    result.item.Ord = data.Ord;

                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsNlsContext.Add(result.item);
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
        public CmsNlsContextItemResponse CmsNlsContextDelete(MyHeader header, CmsNlsContext data)
        {

            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsNlsContext
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
		
		public CmsNlsContextItemResponse CmsNlsContextSetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            //
            try
            {
                // CmsNlsContext
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsNlsContext that = dbCtx.Context.CmsNlsContext.Where(t => t.Uid == uid).FirstOrDefault();
                    CmsNlsContext pivot = dbCtx.Context.CmsNlsContext.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;
                    //decimal pivot_lOrd = pivot.lOrd;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsNlsContext set Ord=Ord+1 where Ord>=" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsNlsContext set Ord=Ord+2 where Ord>" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsNlsContextGet(header, uid);
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
		

		#endregion
		// ******************************************************************************* //
        // Pubblico una CmsNlsContext o rispristino un nuvo record
        // ******************************************************************************* //
		#region Publish Method
		/*
		private String CmsNlsContext_Versioning_MoveFromDb(MyHeader header, Int32 _uid, CmsNlsContext ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsNlsContext n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsNlsContext>(CmsNlsContext_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsNlsContext n1 = dbCtx.db.CmsNlsContext.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsNlsContext_VersionableObjectCopy(n0, n1 == null ? new CmsNlsContext() : n1) as CmsNlsContext;
                if (n1.ToAdd)
                    dbCtx.db.CmsNlsContext.Add(n1);
                // immagini


                //CmsNlsContext_PublishRelated<CmsNlsContext_Models>(ctx, n1, "CmsNlsContext_Models", "Uid_CmsNlsContext", n1.__CmsNlsContext_Models, "Model", "Uid_Model", newContext != null);
                //CmsNlsContext_PublishRelated<CmsNlsContext_Models>(dbCtx, n1, "CmsNlsContext_Models", "Uid_CmsNlsContext", n1.__CmsNlsContext_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsNlsContext set " +
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
                    dbCtx.SqlExecuteNonQuery("update CmsNlsContext set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsNlsContext set " +
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
        public CmsNlsContextItemResponse CmsNlsContextVersioning_Copy(MyHeader header, Int32 uid, NewContextInfo ctxInfo)
        {
            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsNlsContext", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                String newUid = CmsNlsContext_Versioning_MoveFromDb(header, uid, null, ctxInfo);
                return CmsNlsContextGet(header, newUid);
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
                //CacheManager.that.RemoveAllFromCache(header.cmsCmsNlsContext.Uid, "CmsNlsContext");
            }

			//
			return result;
        }		
		*/
		
        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
		/*
        public CmsNlsContextItemResponse CmsNlsContext_Publish(MyHeader header, Int32 uid)
        {
            CmsNlsContextItemResponse result = new CmsNlsContextItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsNlsContext", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                CmsNlsContext_Versioning_MoveFromDb(header, uid, null);
                return CmsNlsContextGet(header, uid);
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
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsNlsContext");
            }
        }        
        // per pubblicare le relazioni
        public void CmsNlsContext_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsNlsContext img1 = dbCtx.Context.CmsNlsContext.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsNlsContext();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsNlsContext)CmsNlsContext_VersionableObjectCopy(img0, img1);
                
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
                        CmsNlsContext o = dbCtx.Context.CmsNlsContext.Where("where Uid='" + refValue + "'").FirstOrDefault();
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
                                    CmsNlsContext o1 = dbCtx.Context.CmsNlsContext.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
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
                        dbCtx.Context.CmsNlsContext.Add(img1);
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
        private String CmsNlsContext_db2JSON(Int32 uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsNlsContext itm = (from t in dbCtx.Context.CmsNlsContext
														  //.Include("Pages")
														  //.Include("PagesParagraphs")
														  //.Include("SiteMenu")
														  //.Include("SiteSubMenu")
														  //.Include("Courses")
														  //.Include("CoursesModules")
														  //.Include("CoursesModulesCalendar")
														  //.Include("CmsUsers_Acl")
														  //.Include("ModulesAttach_Categories")
														  //.Include("CoursesModulesAttach")
														  //.Include("CmsRepository")
														  //.Include("Users")
														  //.Include("CmsFile")
														  //.Include("CmsLabels")
														  //.Include("CmsResources")
														  //.Include("CmsRouting")
														  //.Include("GlobalParameter")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
		*/
		/*
        private long CmsNlsContext_AddToHistory(MyHeader header, String json, ICmsContent content)
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
        private ICmsVersionable CmsNlsContext_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
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

