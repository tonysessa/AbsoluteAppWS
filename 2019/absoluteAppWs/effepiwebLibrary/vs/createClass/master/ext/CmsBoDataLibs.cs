using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Runtime.Serialization;
using DbModel;
using System.IO;
using Support.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dataLibs
{
    public partial class CmsBoDataLibs : CmsDataBaseLibs
    {
        Support.Library.StringUtil objStringUtil = new Support.Library.StringUtil();
        Support.Library.FileUtil objFileUtil = new Support.Library.FileUtil();

        //Cms
        #region Cms
        public CmsUsersItemResponse CmsUsersCheckLogin(String username, String password)
        {

            CmsUsersItemResponse result = new CmsUsersItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers
                                                          .Include("CmsRoles")
                                                          .Include("CmsUsers_Acl")
                                                          .Include("CmsUsers_Acl.CmsNlsContext")
                                                          .Include("CmsRoles.CmsRoles_Acl.CmsSubSections")
                                                          .Include("CmsRoles.CmsRoles_Acl.CmsSubSections.CmsSections")
                                   where t.Username == username && t.Password == password
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

        public CmsUsersListResponse CmsUsersListContextCms(CmsUsersListOptions rq, Int32 _uid_CmsNlsContext)
        {

            CmsUsersListResponse results = new CmsUsersListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsUsers> query = from t in dbCtx.Context.CmsUsers
                                                          .Include("CmsUsers_Acl")
                                                          .Include("CmsRoles")
                                                 where (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
                                                 (rq.name == null || t.Name == rq.name) &&
                                                 (rq.surname == null || t.Surname == rq.surname) &&
                                                 (rq.email == null || t.Email == rq.email) &&
                                                 (rq.username == null || t.Username == rq.username) &&
                                                 (rq.password == null || t.Password == rq.password) &&
                                                 (rq.dateLastLogin == null || t.DateLastLogin == rq.dateLastLogin) &&
                                                 (rq.numLogin == null || t.NumLogin == rq.numLogin) &&
                                                 (rq.uid_CmsRoles == null || t.Uid_CmsRoles == rq.uid_CmsRoles) &&
                                                 (rq.searchText == null || t.Name.Contains(rq.searchText) || t.Surname.Contains(rq.searchText) || t.Email.Contains(rq.searchText) || t.Username.Contains(rq.searchText) || t.Password.Contains(rq.searchText) || t.Note.Contains(rq.searchText)) &&
                                                 (t.StatusFlag != (int)EnumCmsContent.Deleted || (t.StatusFlag == (int)EnumCmsContent.Deleted && rq.includeDeleted)) &&
                                                 (t.CmsUsers_Acl.FirstOrDefault().Uid_CmsNlsContext == _uid_CmsNlsContext)
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
        public CmsRolesItemResponse CmsRolesGetFromUri(String _uri)
        {

            CmsRolesItemResponse result = new CmsRolesItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRoles

                                   where t.Uriname.ToUpper() == _uri.ToUpper()
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
        public CmsUsers_AclItemResponse CmsUsers_AclGet(Int32 _uidUser, Int32 _uid_CmsNlsContext)
        {

            CmsUsers_AclItemResponse result = new CmsUsers_AclItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsUsers_Acl

                                   where t.Uid_CmsUsers == _uidUser && t.Uid_CmsNlsContext == _uid_CmsNlsContext
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
        //
        // Repository
        public CmsRepositoryListResponse CmsRepositoryForUsers(List<CmsNlsContext> CmsNlsContextEnable)
        {

            CmsRepositoryListResponse results = new CmsRepositoryListResponse();
            //
            try
            {
                String uids = "|";
                for (int i = 0; i < CmsNlsContextEnable.Count; i++)
                    uids += CmsNlsContextEnable[i].Uid.ToString() + "|";

                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRepository> query = from t in dbCtx.Context.CmsRepository
                                                      where (t.StatusFlag == 0 || t.StatusFlag == 1) &&
                                                      (uids.Contains("|" + t.Uid_CmsNlsContext + "|"))
                                                      select t;

                    // Paging
                    results.items = query.OrderBy("Descrizione").ToList();
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

        /*public CmsRoutingItemResponse GetMetaTagRoute(String _nameroute)
        {

            CmsRoutingItemResponse result = new CmsRoutingItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsRouting
                                   where t.NameRoute == _nameroute && t.StatusFlag == (int)EnumCmsContent.Enabled
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
        }*/
        public CmsFileListResponse CmsFileListCms_BO(MyHeader header, CmsFileListOptions rq)
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
                                                (t.Uid_Parent == rq.uid_Parent) &&
                                                (rq.name == null || t.Name == rq.name) &&
                                                (rq.fileTypeFlag == null || t.FileTypeFlag == rq.fileTypeFlag) &&
                                                (rq.fileSize == null || t.FileSize == rq.fileSize) &&
                                                (rq.fileExtension == null || t.FileExtension == rq.fileExtension) &&
                                                (rq.fileContentType == null || t.FileContentType == rq.fileContentType) &&
                                                (rq.fileMetaData == null || t.FileMetaData == rq.fileMetaData) &&
                                                (rq.imageIco == null || t.ImageIco == rq.imageIco) &&
                                                (rq.searchText == null || t.ImagePrev.Contains(rq.searchText) || t.Note.Contains(rq.searchText)) &&
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
        #endregion

        #region Storage method
        private String getUniqueFileName(String folder, String name, Boolean isDirectory)
        {
            if (!isDirectory)
            {
                String ret = name;
                int index = name.LastIndexOf('.');
                String fileName = index != -1 ? name.Substring(0, index) : name;
                String fileExtension = index != -1 ? name.Substring(index) : "";
                //
                int cnt = 0;
                while (File.Exists(objStringUtil.CombinePath(folder, ret)))
                {
                    cnt++;
                    ret = fileName + "_" + cnt.ToString().PadLeft(3, '0') + fileExtension;
                }
                return ret;
            }
            else
            {
                String ret = name;
                String fileName = name;
                int cnt = 0;
                while (Directory.Exists(objStringUtil.CombinePath(folder, ret) + "\\"))
                {
                    cnt++;
                    ret = fileName + "_" + cnt.ToString().PadLeft(3, '0');
                }
                return ret;
            }
        }
        public StorageFileItemResponse StorageFileUpsert(MyHeader header, CmsStorageFile obj, byte[] fileContent)
        {
            StorageFileItemResponse res = new StorageFileItemResponse();
            // security
            //if (!CheckBoAccessToken())
            //{
            //    res.Error = "ERROR_AUTHORIZATION";
            //    return res;
            //}
            //
            try
            {
                res.item = obj;
                // vedo se è una cancellazione
                if (fileContent == null || fileContent.Length == 0)
                {
                    if (File.Exists(obj.AbsolutePath))
                        File.Delete(obj.AbsolutePath);
                    res.Success = true;
                    return res;
                }
                String basePath = objStringUtil.CombinePath(WebContext.getConfig("%.storagePublicBasePath").ToString(), objStringUtil.ClearUrlName(header.cmsCmsNlsContext.Title)).Replace("\\", "/");
                String dirAbsPath = "";
                // splitto in folders e name
                int idx = obj.RelativePath.LastIndexOfAny("/\\".ToCharArray());
                String fileDirs = idx <= 0 ? "" : obj.RelativePath.Substring(0, idx);
                String fileName = idx == -1 ? obj.RelativePath : obj.RelativePath.Substring(idx + 1);
                dirAbsPath = objStringUtil.CombinePath(basePath, fileDirs);
                fileName = getUniqueFileName(dirAbsPath, fileName, false);
                fileName = fileName.Replace("%20", "");
                // creo la directory
                Directory.CreateDirectory(dirAbsPath);
                //
                obj.RelativePath = objStringUtil.CombinePath(objStringUtil.CombinePath(objStringUtil.ClearUrlName(header.cmsCmsNlsContext.Title), fileDirs), fileName);
                // Stream
                {
                    MemoryStream inStream = new MemoryStream(fileContent);
                    inStream.Position = 0;
                    System.IO.FileStream outStream = System.IO.File.OpenWrite(res.item.AbsolutePath);
                    objFileUtil.CopyStream(inStream, outStream);
                    outStream.Close();
                }
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Ex = ex;
                res.Error = ex.Message;
            }
            //
            return res;
        }
        #endregion

        public CmsLabelsItemResponse CmsLabelsGetCmsFromKey(MyHeader header, String key)
        {

            CmsLabelsItemResponse result = new CmsLabelsItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsLabels
                                                          .Include("CmsNlsContext")

                                   where t.Key == key && t.Uid_CmsNlsContext == null
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
        public CmsResourcesItemResponse CmsResourcesGetCmsFromKey(MyHeader header, String key)
        {

            CmsResourcesItemResponse result = new CmsResourcesItemResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    result.item = (from t in dbCtx.Context.CmsResources
                                                          .Include("CmsNlsContext")

                                   where t.Key == key && t.Uid_CmsNlsContext == null
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
        public CmsResourcesItemResponse CmsResourcesUpsertForCms(MyHeader header, CmsResources data)
        {
            CmsResourcesItemResponse result = new CmsResourcesItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsResources
                                                          .Include("CmsNlsContext")

                                       where t.Uid == data.Uid
                                       select t).FirstOrDefault();

                        if (result.item != null)
                        {
                            isInsert = false;
                            result.item.UpdateDate = DateTime.Now;
                            result.item.Uid_UpdateUser = header.cmsUserUid;
                            result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;
                        }
                        else
                        {
                            isInsert = true;
                            result.item = new CmsResources();
                            //result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser = header.cmsUserUid;
                            result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;

                            /*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsResources.Max(m => m.Ord);
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
                        result.item = new CmsResources();
                        result.item.Uid_CreationUser = header.cmsUserUid;
                        result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;

                        /*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsResources.Max(m => m.Ord));
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

                        result.item.Key = data.Key;

                        result.item.Description = data.Description;

                        result.item.Note = data.Note;

                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsResources.Add(result.item);
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
        public CmsLabelsItemResponse CmsLabelsUpsertForCms(MyHeader header, CmsLabels data)
        {
            CmsLabelsItemResponse result = new CmsLabelsItemResponse();
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
                        result.item = (from t in dbCtx.Context.CmsLabels
                                                          .Include("CmsNlsContext")

                                       where t.Uid == data.Uid
                                       select t).FirstOrDefault();

                        if (result.item != null)
                        {
                            isInsert = false;
                            result.item.UpdateDate = DateTime.Now;
                            result.item.Uid_UpdateUser = header.cmsUserUid;
                            result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;
                        }
                        else
                        {
                            isInsert = true;
                            result.item = new CmsLabels();
                            //result.item.Uid = data.Uid;
                            result.item.Uid_CreationUser = header.cmsUserUid;
                            result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;

                            /*
							Int32? mm = 1;
							try
							{
								mm = (Int32)dbCtx.Context.CmsLabels.Max(m => m.Ord);
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
                        result.item = new CmsLabels();
                        result.item.Uid_CreationUser = header.cmsUserUid;
                        result.item.Uid_CmsNlsContext = data.Uid_CmsNlsContext;

                        /*
						Int32? mm = 1;
						try
						{
                            mm = (Int32)(dbCtx.Context.CmsLabels.Max(m => m.Ord));
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

                        result.item.Key = data.Key;

                        result.item.Description = data.Description;
                        result.item.Note = data.Note;

                    }

                    // save data
                    if (isInsert)
                        dbCtx.Context.CmsLabels.Add(result.item);
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
        public CmsRepositoryListResponse CmsRepositoryListCmsForAcl(List<CmsNlsContext> CmsNlsContextEnable)
        {

            CmsRepositoryListResponse results = new CmsRepositoryListResponse();
            //
            try
            {
                String uids = "|";
                for (int i = 0; i < CmsNlsContextEnable.Count; i++)
                    uids += CmsNlsContextEnable[i].Uid.ToString() + "|";

                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsRepository> query = from t in dbCtx.Context.CmsRepository
                                                      where (t.StatusFlag == 0 || t.StatusFlag == 1) &&
                                                      (uids.Contains("|" + t.Uid_CmsNlsContext.ToString() + "|")) &&
                                                      (t.Uid != 1) // Escludo il default repository
                                                      select t;

                    // Paging
                    results.items = query.OrderBy("Folder").ToList();
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

        public CmsLabelsListResponse CmsLabelsListForCms(MyHeader header, CmsLabelsListOptions rq)
        {

            CmsLabelsListResponse results = new CmsLabelsListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsLabels> query = from t in dbCtx.Context.CmsLabels
                                                          .Include("CmsNlsContext")
                                                  where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
                                                  (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
                                                  (t.Uid_CmsNlsContext == null) &&
                                                  (rq.key == null || t.Key == rq.key) &&
                                                  (rq.searchText == null || t.Key.Contains(rq.searchText) || t.Description.Contains(rq.searchText) || t.Note.Contains(rq.searchText)) &&
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
        public CmsResourcesListResponse CmsResourcesListForCms(MyHeader header, CmsResourcesListOptions rq)
        {

            CmsResourcesListResponse results = new CmsResourcesListResponse();
            //
            try
            {
                using (MyEntityContext dbCtx = new MyEntityContext())
                {
                    IQueryable<CmsResources> query = from t in dbCtx.Context.CmsResources
                                                          .Include("CmsNlsContext")
                                                     where (rq.uid_CreationUser == null || t.Uid_CreationUser == rq.uid_CreationUser) &&
                                                     (rq.statusFlag == null || t.StatusFlag == rq.statusFlag) &&
                                                     (t.Uid_CmsNlsContext == null) &&
                                                     (rq.key == null || t.Key == rq.key) &&
                                                     (rq.description == null || t.Description == rq.description) &&
                                                     (rq.searchText == null || t.Key.Contains(rq.searchText) || t.Description.Contains(rq.searchText) || t.Note.Contains(rq.searchText)) &&
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

        // FileMetaDataDecode
        public static Dictionary<String, String> FileMetaDataDecode(String str)
        {
            Dictionary<String, String> ret = new Dictionary<String, String>();
            try
            {
                JObject obj = jsonDecode<JObject>(str);
                foreach (JProperty p in obj.Properties())
                {
                    String var_name = p.Name;
                    String var_value = Safe((String)(p.Value));
                    ret[var_name] = var_value;
                }
            }
            catch
            {
            }
            return ret;
        }
        // FileMetaDataEncode
        public static String FileMetaDataEncode(Dictionary<String, String> value)
        {
            JObject obj = new JObject();
            if (value != null)
            {
                foreach (String var_name in value.Keys)
                    obj[var_name] = value[var_name];
            }
            return obj.ToString();
        }

        public static T jsonDecode<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        //
        public static string Safe(String str)
        {
            return (str == null) ? "" : str;
        }

        public static JObject jsonDecodeObject(string json)
        {
            return jsonDecode<JObject>(json);
        }
        //
        public static JArray jsonDecodeArray(string json)
        {
            return jsonDecode<JArray>(json);
        }
        //
        public static JContainer jsonDecode(string json)
        {
            try
            {
                return jsonDecodeObject(json);
            }
            catch
            {
            }
            try
            {
                return jsonDecodeArray(json);
            }
            catch
            {
            }
            return null;
        }
        //
        public static Object jsonPopulateObject(string json, Object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.Converters.Add(new DefaultDateTimeConverter());
            JsonConvert.PopulateObject(json, obj, settings);
            return obj;
        }
    }
}
