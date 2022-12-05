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
    #region CmsFile
    [DataContract]
    public class CmsFileListOptions
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
		public Int32? uid_CmsRepository = null;

		[DataMember]
		public Int32? uid_Parent = null;

		[DataMember]
		public String name = null;

		[DataMember]
		public Int32? fileTypeFlag = null;

		[DataMember]
		public Int32? fileSize = null;

		[DataMember]
		public String fileExtension = null;

		[DataMember]
		public String fileContentType = null;

		[DataMember]
		public String fileMetaData = null;

		[DataMember]
		public String imageIco = null;


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
    public class CmsFileItemResponse
    {
        [DataMember]
        public CmsFile item = null;

        [DataMember]
        public Boolean Success = true;

        [DataMember]
        public Exception Ex = null;

        [DataMember]
        public String Error = null;
    }

    [DataContract]
    public class CmsFileListResponse
    {
        [DataMember]
        public List<CmsFile> items = null;

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
        public CmsFileListResponse CmsFileList(MyHeader header, CmsFileListOptions rq)
        {

            CmsFileListResponse results = new CmsFileListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsFile> query = from t in dbCtx.Context.CmsFile														  					  
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  //(rq.uid_CmsRepository == null || t.Uid_CmsRepository == rq.uid_CmsRepository) &&
                                                          //(rq.uid_Parent == null || t.Uid_Parent == rq.uid_Parent) &&
                                                          (t.Uid_Parent == rq.uid_Parent) &&
                                                          (rq.name == null || t.Name == rq.name) &&
														  (rq.fileTypeFlag == null || t.FileTypeFlag == rq.fileTypeFlag) &&
														  (rq.fileSize == null || t.FileSize == rq.fileSize) &&
														  (rq.fileExtension == null || t.FileExtension == rq.fileExtension) &&
														  (rq.fileContentType == null || t.FileContentType == rq.fileContentType) &&
														  (rq.fileMetaData == null || t.FileMetaData == rq.fileMetaData) &&
														  (rq.imageIco == null || t.ImageIco == rq.imageIco) &&
														  (rq.searchText == null || t.ImagePrev.Contains(rq.searchText) ||t.Note.Contains(rq.searchText)) &&
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
		public CmsFileListResponse CmsFileListCms(MyHeader header, CmsFileListOptions rq)
        {

            CmsFileListResponse results = new CmsFileListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsFile> query = from t in dbCtx.Context.CmsFile														  				  
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")
														  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
														  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
														  ((rq.uid_CmsNlsContext == null || t.Uid_CmsNlsContext == rq.uid_CmsNlsContext) || (t.Uid_CmsNlsContext == null)) &&
														  (rq.uid_CmsRepository == null || t.Uid_CmsRepository == rq.uid_CmsRepository) &&
														  (rq.uid_Parent == null || t.Uid_Parent == rq.uid_Parent) &&
														  (rq.name == null || t.Name == rq.name) &&
														  (rq.fileTypeFlag == null || t.FileTypeFlag == rq.fileTypeFlag) &&
														  (rq.fileSize == null || t.FileSize == rq.fileSize) &&
														  (rq.fileExtension == null || t.FileExtension == rq.fileExtension) &&
														  (rq.fileContentType == null || t.FileContentType == rq.fileContentType) &&
														  (rq.fileMetaData == null || t.FileMetaData == rq.fileMetaData) &&
														  (rq.imageIco == null || t.ImageIco == rq.imageIco) &&
														  (rq.searchText == null || t.ImagePrev.Contains(rq.searchText) ||t.Note.Contains(rq.searchText)) &&														  
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
        public CmsFileItemResponse CmsFileGet(MyHeader header, String Uid)
        {
            return CmsFileGet(header, System.Convert.ToInt32(Uid));
        }
        public CmsFileItemResponse CmsFileGet(MyHeader header, Int32 Uid)
        {

            CmsFileItemResponse result = new CmsFileItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsFile
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")

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
        public CmsFileItemResponse CmsFileGetCms(MyHeader header, Int32 Uid)
        {

            CmsFileItemResponse result = new CmsFileItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsFile
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")

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
		public CmsFileItemResponse CmsFileGetForUpdate(MyHeader header, Int32 uid)
		{
			return CmsFileGetForUpdate(header, uid, "");
		}
		public CmsFileItemResponse CmsFileGetForUpdate(MyHeader header, Int32 uid, Int32 cmsHistoryId )
        {
            CmsFileItemResponse result = new CmsFileItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsFile", true, true, false)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    CmsFile itm = (from t in dbCtx.db.CmsFile
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")

                                 where t.Uid == uid
                                 select t).FirstOrDefault();

                    // se versione committata
                    if (itm != null && itm.IsPublished)
                    {
                        String json = CmsFile_db2JSON(itm.Uid);
                        CmsFile_AddToHistory(header, json, itm);
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
                        CmsFile n0 = Int21h.jsonDecode<CmsFile>(h.SerializedObject);
                        n0.isStage = true;
                        n0.VersionCurrent = itm.VersionCurrent;
                        n0.VersionPublished = itm.VersionPublished;
                        CmsFile_Versioning_MoveFromDb(header, uid, n0);
                        return CmsFileGet(header, uid);
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
        public CmsFileItemResponse CmsFileUpsert(MyHeader header, CmsFile data)
        {
            CmsFileItemResponse result = new CmsFileItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsFile									  
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")

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
                            result.item = new CmsFile();
							//result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser  = header.cmsUserUid;
							result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

							/*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsFile.Max(m => m.Ord);
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
                        result.item = new CmsFile();
                        result.item.Uid_CreationUser  = header.cmsUserUid;
						result.item.Uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;

						/*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsFile.Max(m => m.Ord));
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

						if (data.Uid_CmsRepository != null)
						    result.item.Uid_CmsRepository = data.Uid_CmsRepository;

						if (data.Uid_Parent != null)
						    result.item.Uid_Parent = data.Uid_Parent;
						else
						    result.item.Uid_Parent = null;

						result.item.Name = data.Name;

						if (data.FileTypeFlag != null)
						    result.item.FileTypeFlag = data.FileTypeFlag;

						if (data.FileSize != null)
						    result.item.FileSize = data.FileSize;
						else
						    result.item.FileSize = 0;

						result.item.FileExtension = data.FileExtension;

						result.item.FileContentType = data.FileContentType;

						result.item.FileMetaData = data.FileMetaData;

						result.item.ImageIco = data.ImageIco;

						result.item.ImagePrev = data.ImagePrev;

						result.item.Note = data.Note;

                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsFile.Add(result.item);
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
        public CmsFileItemResponse CmsFileDelete(MyHeader header, CmsFile data)
        {

            CmsFileItemResponse result = new CmsFileItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

                    result.item = (from t in dbCtx.Context.CmsFile
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
		public CmsFileItemResponse CmsFileSetOrder(MyHeader header, Int32 uid, Int32 refToUid, Int32 action)
        {
            CmsFileItemResponse result = new CmsFileItemResponse();
            //
            try
            {
                // CmsFile
                using (MyEntityContext dbCtx = new MyEntityContext())
                {

					CmsFile that = dbCtx.Context.CmsFile.Where(t => t.Uid_CmsNlsContext == header.cmsCmsNlsContext.Uid && t.Uid == uid).FirstOrDefault();
                    CmsFile pivot = dbCtx.Context.CmsFile.Where(t => t.Uid == refToUid).FirstOrDefault();

                    decimal pivot_Ord = pivot.Ord.Value;
                    //decimal pivot_lOrd = pivot.lOrd;

                    if (action == -1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsFile set Ord=Ord+1 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>=" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)pivot_Ord;
                    }
                    else if (action == 1)
                    {
                        dbCtx.Context.Database.ExecuteSqlCommand("update CmsFile set Ord=Ord+2 where Uid_CmsNlsContext='" + that.Uid_CmsNlsContext + "' and Ord>" + (long)pivot_Ord + " and Uid!='" + uid + "'");
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
					else if (action == 0)
                    {
                        that.Ord = (Int32)(pivot_Ord + 1);
                    }
                   
					//
					dbCtx.SaveChanges();
                }
                return CmsFileGet(header, uid);
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
        // Pubblico una CmsFile o rispristino un nuvo record
        // ******************************************************************************* //
		#region Publish Method
		/*
		private String CmsFile_Versioning_MoveFromDb(MyHeader header, Int32 _uid, CmsFile ripristina = null, NewContextInfo newContext = null)
        {
            // serializzo/Deserializzo
            CmsFile n0 = ripristina != null ? ripristina : Int21h.jsonDecode<CmsFile>(CmsFile_db2JSON(_uid));
            if (newContext != null)
                n0.PrepareForCopy(newContext);
            // vado sul db di produzione se devo pubbicare, altrimenti su stage
            using (MyEntityContext dbCtx = new MyEntityContext(ripristina != null || newContext != null, false))
            {
                CmsFile n1 = dbCtx.db.CmsFile.Where(t => t.Uid == n0.Uid).FirstOrDefault();
                n1 = CmsFile_VersionableObjectCopy(n0, n1 == null ? new CmsFile() : n1) as CmsFile;
                if (n1.ToAdd)
                    dbCtx.db.CmsFile.Add(n1);
                // immagini


                //CmsFile_PublishRelated<CmsFile_Models>(ctx, n1, "CmsFile_Models", "Uid_CmsFile", n1.__CmsFile_Models, "Model", "Uid_Model", newContext != null);
                //CmsFile_PublishRelated<CmsFile_Models>(dbCtx, n1, "CmsFile_Models", "Uid_CmsFile", n1.__CmsFile_Models, "Model", "Uid_Model", true); // Ingore constraint

                //
                dbCtx.db.SaveChanges();
            }
            // aggiorno numero di versione corrente nel db di stage
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                // copia
                if (newContext != null)
                {
                    dbCtx.SqlExecuteNonQuery("update CmsFile set " +
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
                    dbCtx.SqlExecuteNonQuery("update CmsFile set " +
                                            "VersionPublished = VersionCurrent " +
                                            ",Uid_PublishUser = '" + header.cmsUserUid + "' " +
                                            ",PublishDate = getdate() " +
                                            "where Uid='" + n0.Uid + "'");
                }
                else // RECOVER 
                {
                    dbCtx.SqlExecuteNonQuery("update CmsFile set " +
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
        public CmsFileItemResponse CmsFileVersioning_Copy(MyHeader header, Int32 uid, NewContextInfo ctxInfo)
        {
            CmsFileItemResponse result = new CmsFileItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsFile", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                String newUid = CmsFile_Versioning_MoveFromDb(header, uid, null, ctxInfo);
                return CmsFileGet(header, newUid);
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
                //CacheManager.that.RemoveAllFromCache(header.cmsCmsNlsContext.Uid, "CmsFile");
            }

			//
			return result;
        }		
		*/
		
        // ******************************************************************************* //
        // Pubblica una versione
        // ******************************************************************************* //
		/*
        public CmsFileItemResponse CmsFile_Publish(MyHeader header, Int32 uid)
        {
            CmsFileItemResponse result = new CmsFileItemResponse();
            // securit
            //if (CheckBoAccessToken(new AclPermission("CmsSection", "CmsFile", true, false, true)) == null)
            //{
            //    result.Error = ServiceResponse.ERROR_AUTHORIZATION;
            //    return result;
            //}
            try
            {
                CmsFile_Versioning_MoveFromDb(header, uid, null);
                return CmsFileGet(header, uid);
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
                CacheManager.that.RemoveAllFromCacheBO(header.cmsCmsNlsContext, "CmsFile");
            }
        }        
        // per pubblicare le relazioni
        public void CmsFile_PublishRelated<T>(MyEntityContext dbCtx, ICmsVersionable master, String childTableName, String childFieldName, List<T> elements, String constTableName = null, String constFieldName = null, bool ignoreConstraints = false) where T : DbModel.ICmsVersionable, new()
        {
            String lst = "'dummy'";
            String masterUid = master.Key;
            foreach (T img0 in elements)
            {
                CmsFile img1 = dbCtx.Context.CmsFile.Where("Uid='" + img0.Key + "'").FirstOrDefault();
                if (img1 == null)
                {
                    img1 = new CmsFile();
                    img1.ToAdd = true;
                }
                //
                img1 = (CmsFile)CmsFile_VersionableObjectCopy(img0, img1);
                
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
                        CmsFile o = dbCtx.Context.CmsFile.Where("where Uid='" + refValue + "'").FirstOrDefault();
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
                                    CmsFile o1 = dbCtx.Context.CmsFile.Where("where StatusFlag!=" + (int)(EnumCmsContentStatus.Deleted) + " AND Uid='" + refValue + "'").FirstOrDefault();
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
                        dbCtx.Context.CmsFile.Add(img1);
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
        private String CmsFile_db2JSON(Int32 uid)
        {
            String json;
            using (MyEntityContext dbCtx = new MyEntityContext())
            {
                CmsFile itm = (from t in dbCtx.Context.CmsFile
														  .Include("CmsNlsContext")
														  .Include("CmsRepository")

                                       where t.Uid == uid
                                       select t).FirstOrDefault();
                json = Int21h.jsonEncode(itm);
            }
            return json;
        }
		*/
		/*
        private long CmsFile_AddToHistory(MyHeader header, String json, ICmsContent content)
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
        private ICmsVersionable CmsFile_VersionableObjectCopy(ICmsVersionable source, ICmsVersionable destination)
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

