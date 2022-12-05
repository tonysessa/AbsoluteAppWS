using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
// Support
using Support.db;
using Support.CmsFunction;
using Support.Library;
using Support.Web;
//
using dataLibs;
using DbModel;
using System.IO;

namespace backOffice.cmsRepository.Ws
{
    /// <summary>
    /// Descrizione di riepilogo per calendar
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    [System.Web.Script.Services.ScriptService]
    public class repository : System.Web.Services.WebService
    {
        #region objUtil
        protected Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        protected Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        protected Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        protected Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        protected Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        protected Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
        protected Support.Library.ImageUtil objLibImage = new Support.Library.ImageUtil();
        protected Support.Library.FileUtil objFileUtil = new Support.Library.FileUtil();
        protected Support.Library.LogUtil objLogUtil = new Support.Library.LogUtil();
        protected Support.Library.FileUtil objLibFileUtil = new Support.Library.FileUtil();
        //
        protected WebContext wctx = null;
        protected MyHeader header = new MyHeader();
        protected CmsBoDataLibs objCmsBoDataLibs = new CmsBoDataLibs();
        #endregion

        #region Parametri
        public String sCryptKey = WebContext.getConfig("%.cryptKey").ToString();
        public String sLogDir = WebContext.getConfig("%.LogDir").ToString();
        public String sLogFile = WebContext.getConfig("%.LogFile").ToString();
        public String sStorageRepositoryBaseUrl = WebContext.getConfig("%.storageRepositoryBaseUrl").ToString();
        public String sStorageRepositoryBasePath = WebContext.getConfig("%.storageRepositoryBasePath").ToString();
        public CmsUserSession currentCmsUserSession = null;
        #endregion

        #region WebMethod
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void List(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent)
        {
            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";


            # region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            //
            if ((String.IsNullOrEmpty(uid_Repository)) && (String.IsNullOrEmpty(uid_Parent)))
            {
                //
                GetRepositoryList(uid_CmsNlsContext, uid_CmsUsers);
            }
            else if ((!String.IsNullOrEmpty(uid_Repository)) && (String.IsNullOrEmpty(uid_Parent)))
            {
                //
                GetRepositoryList(uid_CmsNlsContext, uid_CmsUsers, uid_Repository);
            }
            else if ((!String.IsNullOrEmpty(uid_Repository)) && (!String.IsNullOrEmpty(uid_Parent)))
            {
                GetRepositoryList(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, uid_Parent);
            }
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void Delete(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent, String uid_Items)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";

            // 
            Boolean bOk = true;


            //            
            String[] vUid_Items = uid_Items.Split('|');
            foreach (String current in vUid_Items)
            {
                CmsFileItemResponse currentCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, current);
                if (currentCmsFileItemResponse.Success)
                {
                    CmsFile myCmsFile = currentCmsFileItemResponse.item;

                    if (myCmsFile.IsDirectory)
                    {
                        #region Folder
                        DirectoryInfo myDirectoryInfo = new DirectoryInfo(myCmsFile.AbsolutePath);
                        if (myDirectoryInfo.Exists)
                        {
                            String sTmpPath = myCmsFile.AbsolutePath.Substring(0, myCmsFile.AbsolutePath.Length - 1) + "." + myCmsFile.Uid + ".todelete" + "\\";
                            myDirectoryInfo.MoveTo(sTmpPath);
                        }

                        //
                        myCmsFile.StatusFlag = (Int32)EnumCmsFileStatus.Deleted;
                        myCmsFile.Name = myCmsFile.Name + "." + myCmsFile.Uid + ".todelete";
                        CmsFileItemResponse currentCmsFileItemUpsertResponse = objCmsBoDataLibs.CmsFileUpsert(header, myCmsFile);
                        if (!currentCmsFileItemUpsertResponse.Success)
                            bOk = false;
                        #endregion
                    }
                    else if (myCmsFile.IsStream)
                    {
                        #region File
                        FileInfo myFileInfo = new FileInfo(myCmsFile.AbsolutePath);
                        if (myFileInfo.Exists)
                            myFileInfo.MoveTo(myCmsFile.AbsolutePath + "." + myCmsFile.Uid + ".todelete");

                        // Elimino thumb e ico se esistono
                        FileInfo myFileInfoPrev = new FileInfo(myCmsFile.AbsolutePathPrev);
                        if (myFileInfoPrev.Exists)
                            myFileInfoPrev.MoveTo(myCmsFile.AbsolutePathPrev + "." + myCmsFile.Uid + ".todelete");

                        FileInfo myFileInfoIco = new FileInfo(myCmsFile.AbsolutePathIco);
                        if (myFileInfoIco.Exists)
                            myFileInfoIco.MoveTo(myCmsFile.AbsolutePathIco + "." + myCmsFile.Uid + ".todelete");

                        //
                        myCmsFile.StatusFlag = (Int32)EnumCmsFileStatus.Deleted;
                        myCmsFile.Name = myCmsFile.Name + "." + myCmsFile.Uid + ".todelete";
                        CmsFileItemResponse currentCmsFileItemUpsertResponse = objCmsBoDataLibs.CmsFileUpsert(header, myCmsFile);
                        if (!currentCmsFileItemUpsertResponse.Success)
                            bOk = false;
                        #endregion
                    }
                }
            }

            if (!String.IsNullOrEmpty(uid_Parent))
            {
                // Parent is Folder
                CmsFileItemResponse destinationCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, uid_Parent);
                if (destinationCmsFileItemResponse.Success)
                {

                }
            }

            //
            if (bOk)
                List(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, uid_Parent);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void Move(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent, String uid_Items)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";

            // 
            Boolean bOk = false;

            //
            if (!String.IsNullOrEmpty(uid_Parent))
            {
                #region Move Parent Folder
                // Determino la cartella di destinazione
                CmsFileItemResponse destinationCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, uid_Parent);
                if ((destinationCmsFileItemResponse.Success) && (destinationCmsFileItemResponse.item != null))
                {
                    //
                    CmsFile currentDestinationCmsItem = destinationCmsFileItemResponse.item;

                    // Recupero l'elenco degli item da spostare (file e/o cartelle)         
                    String[] vUid_Parent = uid_Items.Split('|');
                    foreach (String current in vUid_Parent)
                    {
                        // Recupero l'item corrente
                        CmsFileItemResponse currentCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, current);
                        if ((currentCmsFileItemResponse.Success) && (currentCmsFileItemResponse.item != null))
                        {
                            CmsFile currentCmsItem = currentCmsFileItemResponse.item;
                            if (currentCmsItem.IsDirectory)
                            {
                                #region Folder
                                // Verifico che esista la cartella da spostare
                                DirectoryInfo myDirectoryInfo = new DirectoryInfo(currentCmsItem.AbsolutePath);
                                if (myDirectoryInfo.Exists)
                                {
                                    // La cartella esiste quindi la posso spostare

                                    // Determino il path
                                    String sNewRelativePath = String.Empty;
                                    sNewRelativePath = currentDestinationCmsItem.AbsolutePath;

                                    // Verifico se esiste già la catella di destinazione
                                    Int32 indexRename = 0;
                                    String sNewFolderName = currentCmsItem.Name;
                                    DirectoryInfo myNewDirectoryInfo = new DirectoryInfo(sNewRelativePath + sNewFolderName + "\\");
                                    while (myNewDirectoryInfo.Exists)
                                    {
                                        indexRename++;
                                        sNewFolderName = objLibString.ClearUrlName(currentCmsItem.Name) + "_" + indexRename.ToString().PadLeft(3, '0');
                                        myNewDirectoryInfo = new DirectoryInfo(sNewRelativePath + sNewFolderName + "\\");
                                    }

                                    // Aggiorno il file system
                                    myDirectoryInfo.MoveTo(sNewRelativePath + sNewFolderName + "\\");
                                }
                                #endregion
                            }
                            else if (currentCmsItem.IsStream)
                            {
                                #region File
                                // Verifico che esista il file da spostare
                                FileInfo myFileInfo = new FileInfo(currentCmsItem.AbsolutePath);
                                if (myFileInfo.Exists)
                                {
                                    // La cartella esiste quindi la posso spostare
                                    String name = myFileInfo.Name;
                                    String currentExtention = objLibString.GetExtentionFromFileName(myFileInfo.Name);
                                    if (!String.IsNullOrEmpty(currentExtention))
                                        name = name.Replace(currentExtention, "");

                                    // Determino il path
                                    String sNewRelativePath = String.Empty;
                                    sNewRelativePath = currentDestinationCmsItem.AbsolutePath;

                                    // Verifico se esiste già la il file fi destinazione
                                    Int32 indexRename = 0;
                                    String sNewFileName = objLibString.ClearUrlName(name) + "." + currentExtention;
                                    FileInfo myNewFileInfo = new FileInfo(sNewRelativePath + sNewFileName);
                                    while (myNewFileInfo.Exists)
                                    {
                                        indexRename++;
                                        sNewFileName = objLibString.ClearUrlName(name) + "_" + indexRename.ToString().PadLeft(3, '0') + "." + currentExtention;
                                        myNewFileInfo = new FileInfo(sNewRelativePath + sNewFileName);
                                    }

                                    // Aggiorno il file system
                                    myFileInfo.MoveTo(sNewRelativePath + sNewFileName);

                                    // Sposto thumb e ico se esistono
                                    FileInfo myFileInfoth = new FileInfo(currentCmsItem.AbsolutePathPrev);
                                    if (myFileInfoth.Exists)
                                        myFileInfoth.MoveTo(sNewRelativePath + "th_" + sNewFileName);

                                    FileInfo myFileInfoPrev = new FileInfo(currentCmsItem.AbsolutePathPrev);
                                    if (myFileInfoPrev.Exists)
                                        myFileInfoPrev.MoveTo(sNewRelativePath + "prev_" + sNewFileName);

                                    FileInfo myFileInfoIco = new FileInfo(currentCmsItem.AbsolutePathIco);
                                    if (myFileInfoIco.Exists)
                                        myFileInfoIco.MoveTo(sNewRelativePath + "ico_" + sNewFileName);
                                }
                                #endregion
                            }

                            // Salvo i dati sul database
                            if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
                                currentCmsItem.Uid_CmsRepository = System.Convert.ToInt32(uid_Repository);
                            currentCmsItem.Uid_Parent = destinationCmsFileItemResponse.item.Uid;
                            CmsFileItemResponse currentCmsFileItemUpsertResponse = objCmsBoDataLibs.CmsFileUpsert(header, currentCmsItem);
                            if (currentCmsFileItemUpsertResponse.Success)
                                bOk = true;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Move Repository Folder
                //
                CmsRepositoryItemResponse destinationCmsFileItemResponse = objCmsBoDataLibs.CmsRepositoryGet(header, uid_Repository);
                if ((destinationCmsFileItemResponse.Success) && (destinationCmsFileItemResponse.item != null))
                {
                    //
                    CmsRepository currentDestinationCmsItem = destinationCmsFileItemResponse.item;

                    // Recupero l'elenco degli item da spostare (file e/o cartelle)         
                    String[] vUid_Parent = uid_Items.Split('|');
                    foreach (String current in vUid_Parent)
                    {
                        // Recupero l'item corrente
                        CmsFileItemResponse currentCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, current);
                        if ((currentCmsFileItemResponse.Success) && (currentCmsFileItemResponse.item != null))
                        {
                            CmsFile currentCmsItem = currentCmsFileItemResponse.item;
                            if (currentCmsItem.IsDirectory)
                            {
                                #region Folder
                                // Verifico che esista la cartella da spostare
                                DirectoryInfo myDirectoryInfo = new DirectoryInfo(currentCmsItem.AbsolutePath);
                                if (myDirectoryInfo.Exists)
                                {
                                    // La cartella esiste quindi la posso spostare

                                    // Determino il path
                                    String sNewRelativePath = String.Empty;
                                    sNewRelativePath = currentDestinationCmsItem.AbsolutePath;

                                    // Verifico se esiste già la catella di destinazione
                                    Int32 indexRename = 0;
                                    String sNewFolderName = currentCmsItem.Name;
                                    DirectoryInfo myNewDirectoryInfo = new DirectoryInfo(sNewRelativePath + sNewFolderName + "\\");
                                    while (myNewDirectoryInfo.Exists)
                                    {
                                        indexRename++;
                                        sNewFolderName = objLibString.ClearUrlName(currentCmsItem.Name) + "_" + indexRename.ToString().PadLeft(3, '0');
                                        myNewDirectoryInfo = new DirectoryInfo(sNewRelativePath + sNewFolderName + "\\");
                                    }

                                    // Aggiorno il file system
                                    myDirectoryInfo.MoveTo(sNewRelativePath + sNewFolderName + "\\");
                                }
                                #endregion
                            }
                            else if (currentCmsItem.IsStream)
                            {
                                #region File
                                // Verifico che esista il file da spostare
                                FileInfo myFileInfo = new FileInfo(currentCmsItem.AbsolutePath);
                                if (myFileInfo.Exists)
                                {
                                    // La cartella esiste quindi la posso spostare
                                    String name = myFileInfo.Name;
                                    String currentExtention = objLibString.GetExtentionFromFileName(myFileInfo.Name);
                                    if (!String.IsNullOrEmpty(currentExtention))
                                        name = name.Replace(currentExtention, "");

                                    // Determino il path
                                    String sNewRelativePath = String.Empty;
                                    sNewRelativePath = currentDestinationCmsItem.AbsolutePath;

                                    // Verifico se esiste già la il file fi destinazione
                                    Int32 indexRename = 0;
                                    String sNewFileName = objLibString.ClearUrlName(name) + "." + currentExtention;
                                    FileInfo myNewFileInfo = new FileInfo(sNewRelativePath + sNewFileName);
                                    while (myNewFileInfo.Exists)
                                    {
                                        indexRename++;
                                        sNewFileName = objLibString.ClearUrlName(name) + "_" + indexRename.ToString().PadLeft(3, '0') + "." + currentExtention;
                                        myNewFileInfo = new FileInfo(sNewRelativePath + sNewFileName);
                                    }

                                    // Aggiorno il file system
                                    myFileInfo.MoveTo(sNewRelativePath + sNewFileName + "\\");

                                    // Sposto thumb e ico se esistono
                                    FileInfo myFileInfoth = new FileInfo(currentCmsItem.AbsolutePathPrev);
                                    if (myFileInfoth.Exists)
                                        myFileInfoth.MoveTo(sNewRelativePath + "th_" + sNewFileName);

                                    FileInfo myFileInfoPrev = new FileInfo(currentCmsItem.AbsolutePathPrev);
                                    if (myFileInfoPrev.Exists)
                                        myFileInfoPrev.MoveTo(sNewRelativePath + "prev_" + sNewFileName);

                                    FileInfo myFileInfoIco = new FileInfo(currentCmsItem.AbsolutePathIco);
                                    if (myFileInfoIco.Exists)
                                        myFileInfoIco.MoveTo(sNewRelativePath + "ico_" + sNewFileName);
                                }
                                #endregion
                            }

                            // Salvo i datI sil database
                            currentCmsItem.Uid_Parent = null;
                            if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
                                currentCmsItem.Uid_CmsRepository = System.Convert.ToInt32(uid_Repository);
                            CmsFileItemResponse currentCmsFileItemUpsertResponse = objCmsBoDataLibs.CmsFileUpsert(header, currentCmsItem);
                            if (currentCmsFileItemUpsertResponse.Success)
                                bOk = true;
                        }
                    }
                }
                #endregion
            }

            //
            if (bOk)
                List(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, uid_Parent);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void Rename(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent, String uid_Item, String name)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";

            if (!objLibMath.isNumber(uid_Item) && (System.Convert.ToInt32(uid_Item) <= 0))
                uid_Item = "";

            //
            Boolean bOk = false;

            // 
            if (!String.IsNullOrEmpty(uid_Item))
            {
                CmsFileItemResponse myCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, uid_Item);
                if (myCmsFileItemResponse.Success)
                {
                    //
                    CmsFile myCmsItem = myCmsFileItemResponse.item;

                    if (!myCmsItem.Name.ToLower().Equals(name.ToLower()))
                    {
                        // Verifico il tipo
                        if (myCmsItem.IsDirectory)
                        {
                            #region Folder
                            // Verifico che esista la cartella di origine su file system
                            DirectoryInfo myDirectoryInfo = new DirectoryInfo(myCmsItem.AbsolutePath);
                            if (myDirectoryInfo.Exists)
                            {
                                // La cartella esiste quindi la posso spostare

                                // Determino il path
                                String sNewRelativePath = String.Empty;
                                if (myCmsItem.ParentItem != null)
                                    sNewRelativePath = myCmsItem.ParentItem.AbsolutePath;
                                else
                                    sNewRelativePath = sStorageRepositoryBasePath;

                                // Verifico se esiste già la catella di destinazione
                                Int32 indexRename = 0;
                                String sNewFolderName = objLibString.ClearUrlName(name);
                                DirectoryInfo myNewDirectoryInfo = new DirectoryInfo(sNewRelativePath + sNewFolderName + "\\");
                                while (myNewDirectoryInfo.Exists)
                                {
                                    indexRename++;
                                    sNewFolderName = objLibString.ClearUrlName(name) + "_" + indexRename.ToString().PadLeft(3, '0');
                                    myNewDirectoryInfo = new DirectoryInfo(sNewRelativePath + sNewFolderName + "\\");
                                }

                                // Aggiorno il file system
                                myDirectoryInfo.MoveTo(sNewRelativePath + sNewFolderName + "\\");

                                // Aggiorno i dati sul db
                                myCmsItem.Name = sNewFolderName;
                                CmsFileItemResponse responseCmsFileItemResponse = objCmsBoDataLibs.CmsFileUpsert(header, myCmsItem);
                                if (responseCmsFileItemResponse.Success)
                                    bOk = true;

                            }
                            else
                            {
                                // Non esiste la cartella di origine su file system - ERRORE IMPREVISTO
                            }
                            #endregion
                        }
                        else if (myCmsItem.IsStream)
                        {
                            #region File
                            // Verifico che esista la cartella di origine su file system
                            FileInfo myFileInfo = new FileInfo(myCmsItem.AbsolutePath);
                            if (myFileInfo.Exists)
                            {
                                // La cartella esiste quindi la posso spostare
                                String currentExtention = objLibString.GetExtentionFromFileName(myFileInfo.Name);
                                if (!String.IsNullOrEmpty(currentExtention))
                                    name = name.Replace(currentExtention, "");

                                // Determino il path
                                String sNewRelativePath = String.Empty;
                                if (myCmsItem.ParentItem != null)
                                    sNewRelativePath = myCmsItem.ParentItem.AbsolutePath;
                                else
                                    sNewRelativePath = sStorageRepositoryBasePath;

                                // Verifico se esiste già la il file fi destinazione
                                Int32 indexRename = 0;
                                String sNewFileName = objLibString.ClearUrlName(name) + "." + currentExtention;
                                FileInfo myNewFileInfo = new FileInfo(sNewRelativePath + sNewFileName);
                                while (myNewFileInfo.Exists)
                                {
                                    indexRename++;
                                    sNewFileName = objLibString.ClearUrlName(name) + "_" + indexRename.ToString().PadLeft(3, '0') + "." + currentExtention;
                                    myNewFileInfo = new FileInfo(sNewRelativePath + sNewFileName);
                                }

                                // Aggiorno il file system
                                myFileInfo.MoveTo(sNewRelativePath + sNewFileName);

                                // Rinomino thumb e ico se esistono
                                FileInfo myFileInfoth = new FileInfo(myCmsItem.AbsolutePathPrev);
                                if (myFileInfoth.Exists)
                                    myFileInfoth.MoveTo(sNewRelativePath + "th_" + sNewFileName);

                                FileInfo myFileInfoPrev = new FileInfo(myCmsItem.AbsolutePathPrev);
                                if (myFileInfoPrev.Exists)
                                    myFileInfoPrev.MoveTo(sNewRelativePath + "prev_" + sNewFileName);

                                FileInfo myFileInfoIco = new FileInfo(myCmsItem.AbsolutePathIco);
                                if (myFileInfoIco.Exists)
                                    myFileInfoIco.MoveTo(sNewRelativePath + "ico_" + sNewFileName);

                                // Aggiorno i dati sul db
                                myCmsItem.Name = sNewFileName;
                                CmsFileItemResponse responseCmsFileItemResponse = objCmsBoDataLibs.CmsFileUpsert(header, myCmsItem);
                                if (responseCmsFileItemResponse.Success)
                                    bOk = true;

                            }
                            else
                            {
                                // Non esiste la cartella di origine su file system - ERRORE IMPREVISTO
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        // Stesso nome non faccio niente
                    }
                }
            }

            //
            if (bOk)
                List(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, uid_Parent);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void CreateFolder(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent, String name)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";


            // 
            Boolean bOk = false;

            //
            String sRelativePath = String.Empty;

            if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
            {
                CmsRepositoryItemResponse myCmsRepositoryItemResponse = objCmsBoDataLibs.CmsRepositoryGet(header, uid_Repository);
                if (myCmsRepositoryItemResponse.Success)
                {
                    CmsRepository myCmsRepository = myCmsRepositoryItemResponse.item;
                    sRelativePath = myCmsRepository.RelativePath;
                }
            }

            if (objLibMath.isNumber(uid_Parent) && (System.Convert.ToInt32(uid_Parent) > 0))
            {
                CmsFileItemResponse myCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, uid_Parent);
                if (myCmsFileItemResponse.Success)
                {
                    CmsFile myCmsRepositoryFolder = myCmsFileItemResponse.item;
                    sRelativePath = myCmsRepositoryFolder.RelativePath;
                }
            }

            if (!String.IsNullOrEmpty(sRelativePath))
            {
                // Create Directory
                String sNewFolderName = objLibString.ClearUrlName(name);
                DirectoryInfo myDirectoryInfo = new DirectoryInfo(sStorageRepositoryBasePath + sRelativePath + sNewFolderName);
                Int32 indexRename = 0;
                while (myDirectoryInfo.Exists)
                {
                    indexRename++;
                    sNewFolderName = objLibString.ClearUrlName(name) + "_" + indexRename.ToString().PadLeft(3, '0');
                    myDirectoryInfo = new DirectoryInfo(sStorageRepositoryBasePath + sRelativePath + sNewFolderName);
                }
                myDirectoryInfo.Create();

                CmsFile myCmsFile = new CmsFile();
                if (objLibMath.isNumber(uid_CmsUsers))
                    myCmsFile.Uid_CreationUser = System.Convert.ToInt32(uid_CmsUsers);
                if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
                    myCmsFile.Uid_CmsRepository = System.Convert.ToInt32(uid_Repository);
                myCmsFile.Name = sNewFolderName;
                if (objLibMath.isNumber(uid_Parent) && (System.Convert.ToInt32(uid_Parent) > 0))
                    myCmsFile.Uid_Parent = System.Convert.ToInt32(uid_Parent);
                myCmsFile.FileType = EnumCmsFileType.Directory;

                CmsFileItemResponse responseFileItemResponse = objCmsBoDataLibs.CmsFileUpsert(header, myCmsFile);
                if (responseFileItemResponse.Success)
                    bOk = true;
            }

            //
            if (bOk)
                List(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, uid_Parent);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void Breadcumbs(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";

            //
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            //
            Boolean bEnd = false;
            List<RepositoryItemJs.items> myListFolderItems = new List<RepositoryItemJs.items>();

            // List Response
            if (objLibMath.isNumber(uid_Parent))
            {
                //
                String sCurrentUid_Parent = uid_Parent;
                while (!bEnd)
                {
                    if (!String.IsNullOrEmpty(sCurrentUid_Parent))
                    {
                        CmsFileItemResponse responseItem = objCmsBoDataLibs.CmsFileGet(header, sCurrentUid_Parent);
                        if ((responseItem.Success) && (responseItem.item != null))
                        {
                            // Parent Folder
                            RepositoryItemJs.items myCurrentParentItems = new RepositoryItemJs.items();
                            if (!uid_Repository.Equals(responseItem.item.Uid))
                                myCurrentParentItems.Uid = responseItem.item.Uid.ToString();
                            myCurrentParentItems.UidRepository = uid_Repository;
                            myCurrentParentItems.name = responseItem.item.Name;
                            myCurrentParentItems.typo = "folder";
                            myListFolderItems.Add(myCurrentParentItems);

                            if (responseItem.item.Uid_Parent != null)
                                bEnd = true;
                            else
                                sCurrentUid_Parent = responseItem.item.Uid_Parent.ToString();
                        }
                        else
                            bEnd = true;
                    }
                    else
                    {
                        bEnd = true;
                    }
                }
            }

            if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
            {
                CmsRepositoryItemResponse responseItem = objCmsBoDataLibs.CmsRepositoryGet(header, uid_Repository);
                if (responseItem.Success)
                {
                    // Parent Folder
                    RepositoryItemJs.items myCurrentParentItems = new RepositoryItemJs.items();
                    //if (!uid_Repository.Equals(responseItem.item.Uid))
                    //    myCurrentParentItems.Uid = responseItem.item.Uid.ToString();
                    myCurrentParentItems.UidRepository = uid_Repository;
                    myCurrentParentItems.name = responseItem.item.Folder;
                    myCurrentParentItems.typo = "folder";
                    myListFolderItems.Add(myCurrentParentItems);
                }
            }

            // Parent Folder
            RepositoryItemJs.items myCurrentRootItems = new RepositoryItemJs.items();
            myCurrentRootItems.Uid = "";
            myCurrentRootItems.UidRepository = "";
            myCurrentRootItems.name = "root";
            myCurrentRootItems.typo = "folder";
            myListFolderItems.Add(myCurrentRootItems);

            //
            Context.Response.Write(js.Serialize(myListFolderItems));
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void Search(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent, String text)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {

                List<RepositoryItemJs.items> myListFileItems = new List<RepositoryItemJs.items>();
                List<RepositoryItemJs.items> myListFolderItems = new List<RepositoryItemJs.items>();

                // Request List
                CmsFileListOptions rq = new CmsFileListOptions();
                //rq.uid_Repository = uid_Repository;
                //rq.uid_Parent = uid_Parent;
                rq.searchText = text;
                rq.sortBy = "Name";
                rq.sortAscending = true;

                // List Response
                CmsFileListResponse responseList = objCmsBoDataLibs.CmsFileList(header, rq);
                if (responseList.Success)
                {

                    if (!String.IsNullOrEmpty(uid_Parent))
                    {
                        CmsFileItemResponse responseGet = objCmsBoDataLibs.CmsFileGet(header, uid_Parent);
                        if (responseGet.Success)
                        {
                            // Parent Folder
                            RepositoryItemJs.items myCurrentParentItems = new RepositoryItemJs.items();
                            myCurrentParentItems.Uid = responseGet.item.Uid_Parent.ToString();
                            myCurrentParentItems.UidRepository = uid_Repository;
                            myCurrentParentItems.name = "../";
                            myCurrentParentItems.typo = "folder";
                            myListFolderItems.Add(myCurrentParentItems);
                        }
                    }
                    else
                    {
                        // Parent Folder
                        RepositoryItemJs.items myCurrentParentItems = new RepositoryItemJs.items();
                        myCurrentParentItems.Uid = "";
                        myCurrentParentItems.UidRepository = "";
                        myCurrentParentItems.name = "../";
                        myCurrentParentItems.typo = "folder";
                        myListFolderItems.Add(myCurrentParentItems);
                    }


                    for (int i = 0; i < responseList.items.Count; i++)
                    {
                        if (responseList.items[i].IsDirectory)
                        {
                            RepositoryItemJs.items myCurrentListItems = GetFolderItemJs(responseList.items[i]);
                            myListFolderItems.Add(myCurrentListItems);
                        }
                        else
                        {
                            RepositoryItemJs.items myCurrentListItems = GetFileItemJs(responseList.items[i]);
                            myListFileItems.Add(myCurrentListItems);
                        }
                    }
                }

                if (HttpContext.Current.Session["CmsUserSession"] != null)
                {
                    CmsUserSession currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
                    if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
                        currentCmsUserSession.LastUidCmsRepository = System.Convert.ToInt32(uid_Repository);
                    if (objLibMath.isNumber(uid_Parent) && (System.Convert.ToInt32(uid_Parent) > 0))
                        currentCmsUserSession.LastUidCmsRepositoryFolder = System.Convert.ToInt32(uid_Parent);
                }

                List<RepositoryItemJs.items> myListItems = myListFolderItems.Concat(myListFileItems).ToList();
                Context.Response.Write(js.Serialize(myListItems));

            }
            catch (Exception ex)
            {
                //
                RepositoryItemJs.Error err = new RepositoryItemJs.Error();
                err.msg = ex.Message;

                Context.Response.Write(js.Serialize(err));
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void Detail(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid)
        {
            #region Check Login
            if (Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            #region Header
            header.cmsUserUid = currentCmsUserSession.currentUid;
            header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
            #endregion

            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!String.IsNullOrEmpty(uid_Repository))
            {
                //
                GetRepositoryItem(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, uid);
            }
        }
        #endregion

        #region Private Method
        private void GetRepositoryList(String uid_CmsNlsContext, String uid_CmsUsers)
        {
            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            #region Check Login
            CmsUserSession currentCmsUserSession = null;
            if (HttpContext.Current.Session["CmsUserSession"] != null)
                currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
            #endregion

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {

                List<RepositoryItemJs.items> myListItems = new List<RepositoryItemJs.items>();

                // Request List
                CmsRepositoryListOptions rq = new CmsRepositoryListOptions();
                rq.sortBy = "Folder";
                rq.sortAscending = true;

                // List Response
                if (objLibMath.isNumber(uid_CmsUsers))
                    header.cmsUserUid = System.Convert.ToInt32(uid_CmsUsers);
                CmsRepositoryListResponse responseList = objCmsBoDataLibs.CmsRepositoryListCmsForAcl(currentCmsUserSession.currentEnableCmsNlsContext);
                if (responseList.Success)
                {

                    for (int i = 0; i < responseList.items.Count; i++)
                    {
                        RepositoryItemJs.items myCurrentListItems = GetRepositoryItemJs(responseList.items[i]);
                        myListItems.Add(myCurrentListItems);
                    }
                }

                currentCmsUserSession.LastUidCmsRepository = 0;
                currentCmsUserSession.LastUidCmsRepositoryFolder = 0;

                Context.Response.Write(js.Serialize(myListItems));

            }
            catch (Exception ex)
            {
                //
                RepositoryItemJs.Error err = new RepositoryItemJs.Error();
                err.msg = ex.Message;

                Context.Response.Write(js.Serialize(err));
            }
        }
        private void GetRepositoryList(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository)
        {
            GetRepositoryList(uid_CmsNlsContext, uid_CmsUsers, uid_Repository, "");
        }
        private void GetRepositoryList(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid_Parent)
        {
            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid_Parent) || (System.Convert.ToInt32(uid_Parent) <= 0))
                uid_Parent = "";

            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {

                List<RepositoryItemJs.items> myListFileItems = new List<RepositoryItemJs.items>();
                List<RepositoryItemJs.items> myListFolderItems = new List<RepositoryItemJs.items>();

                // Request List
                CmsFileListOptions rq = new CmsFileListOptions();

                if (!String.IsNullOrEmpty(uid_Repository))
                    rq.uid_CmsRepository = System.Convert.ToInt32(uid_Repository);
                if (!String.IsNullOrEmpty(uid_Parent))
                    rq.uid_Parent = System.Convert.ToInt32(uid_Parent);
                rq.sortBy = "Name";
                rq.sortAscending = true;

                // List Response
                CmsFileListResponse responseList = objCmsBoDataLibs.CmsFileList(header, rq);
                if (responseList.Success)
                {

                    if (!String.IsNullOrEmpty(uid_Parent))
                    {
                        CmsFileItemResponse responseGet = objCmsBoDataLibs.CmsFileGet(header, uid_Parent);
                        if (responseGet.Success)
                        {
                            // Parent Folder
                            RepositoryItemJs.items myCurrentParentItems = new RepositoryItemJs.items();
                            myCurrentParentItems.Uid = responseGet.item.Uid_Parent.ToString();
                            myCurrentParentItems.UidRepository = uid_Repository;
                            myCurrentParentItems.name = "../";
                            myCurrentParentItems.typo = "folder";
                            myListFolderItems.Add(myCurrentParentItems);
                        }
                    }
                    else
                    {
                        // Parent Folder
                        RepositoryItemJs.items myCurrentParentItems = new RepositoryItemJs.items();
                        myCurrentParentItems.Uid = "";
                        myCurrentParentItems.UidRepository = "";
                        myCurrentParentItems.name = "../";
                        myCurrentParentItems.typo = "folder";
                        myListFolderItems.Add(myCurrentParentItems);
                    }


                    for (int i = 0; i < responseList.items.Count; i++)
                    {
                        if (responseList.items[i].IsDirectory)
                        {
                            RepositoryItemJs.items myCurrentListItems = GetFolderItemJs(responseList.items[i]);
                            myListFolderItems.Add(myCurrentListItems);
                        }
                        else
                        {
                            RepositoryItemJs.items myCurrentListItems = GetFileItemJs(responseList.items[i]);
                            myListFileItems.Add(myCurrentListItems);
                        }
                    }
                }

                if (HttpContext.Current.Session["CmsUserSession"] != null)
                {
                    CmsUserSession currentCmsUserSession = (CmsUserSession)Session["CmsUserSession"];
                    if (objLibMath.isNumber(uid_Repository) && (System.Convert.ToInt32(uid_Repository) > 0))
                        currentCmsUserSession.LastUidCmsRepository = System.Convert.ToInt32(uid_Repository);
                    if (objLibMath.isNumber(uid_Parent) && (System.Convert.ToInt32(uid_Parent) > 0))
                        currentCmsUserSession.LastUidCmsRepositoryFolder = System.Convert.ToInt32(uid_Parent);
                }

                List<RepositoryItemJs.items> myListItems = myListFolderItems.Concat(myListFileItems).ToList();
                Context.Response.Write(js.Serialize(myListItems));

            }
            catch (Exception ex)
            {
                //
                RepositoryItemJs.Error err = new RepositoryItemJs.Error();
                err.msg = ex.Message;

                Context.Response.Write(js.Serialize(err));
            }
        }
        private void GetRepositoryItem(String uid_CmsNlsContext, String uid_CmsUsers, String uid_Repository, String uid)
        {
            // Default Value to String
            if (!objLibMath.isNumber(uid_CmsNlsContext) || (System.Convert.ToInt32(uid_CmsNlsContext) <= 0))
                uid_CmsNlsContext = "";

            if (!objLibMath.isNumber(uid_CmsUsers) || (System.Convert.ToInt32(uid_CmsUsers) <= 0))
                uid_CmsUsers = "";

            if (!objLibMath.isNumber(uid_Repository) || (System.Convert.ToInt32(uid_Repository) <= 0))
                uid_Repository = "";

            if (!objLibMath.isNumber(uid) || (System.Convert.ToInt32(uid) <= 0))
                uid = "";


            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            try
            {
                List<RepositoryItemJs.items> myListFileItems = new List<RepositoryItemJs.items>();
                List<RepositoryItemJs.items> myListFolderItems = new List<RepositoryItemJs.items>();

                if (!String.IsNullOrEmpty(uid))
                {
                    CmsFileItemResponse responseGet = objCmsBoDataLibs.CmsFileGet(header, uid);
                    if (responseGet.Success)
                    {
                        if (responseGet.item.IsDirectory)
                        {
                            RepositoryItemJs.items myCurrentListItems = GetFolderDetailItemJs(responseGet.item);
                            myListFolderItems.Add(myCurrentListItems);
                        }
                        else
                        {
                            RepositoryItemJs.items myCurrentListItems = GetFileItemDetailJs(responseGet.item);
                            myListFileItems.Add(myCurrentListItems);
                        }
                    }
                }
                else
                {
                    // List Response
                    CmsRepositoryItemResponse responseGet = objCmsBoDataLibs.CmsRepositoryGet(header, uid_Repository);
                    if (responseGet.Success)
                    {

                        RepositoryItemJs.items myCurrentListItems = GetRepositoryItemJs(responseGet.item);
                        myListFileItems.Add(myCurrentListItems);
                    }
                }

                List<RepositoryItemJs.items> myListItems = myListFolderItems.Concat(myListFileItems).ToList();
                Context.Response.Write(js.Serialize(myListItems));
            }
            catch (Exception ex)
            {
                //
                RepositoryItemJs.Error err = new RepositoryItemJs.Error();
                err.msg = ex.Message;

                Context.Response.Write(js.Serialize(err));
            }
        }
        private RepositoryItemJs.items GetRepositoryItemJs(CmsRepository _CmsRepository)
        {
            RepositoryItemJs.items myItems = new RepositoryItemJs.items();

            if (_CmsRepository != null)
            {
                try
                {
                    myItems.UidRepository = _CmsRepository.Uid.ToString();
                    myItems.Uid = "";
                    myItems.name = _CmsRepository.Folder;
                    myItems.typo = "folder";
                    myItems.path = _CmsRepository.AbsolutePath;

                    RepositoryItemJs.itemDetails myItemDetailsNome = new RepositoryItemJs.itemDetails();
                    myItemDetailsNome.label = "CREATED BY";
                    myItemDetailsNome.value = _CmsRepository.Folder;
                    myItems.details.Add(myItemDetailsNome);

                    RepositoryItemJs.itemDetails myItemDetailsAutor = new RepositoryItemJs.itemDetails();
                    myItemDetailsAutor.label = "UPLOADED BY";
                    myItemDetailsAutor.value = _CmsRepository.Uid_CreationUser.ToString();
                    myItems.details.Add(myItemDetailsAutor);

                }
                catch (Exception ex)
                {
                    String sErr = ex.Message.ToString();
                }
            }

            return myItems;
        }
        private RepositoryItemJs.items GetFolderItemJs(CmsFile _CmsFile)
        {
            RepositoryItemJs.items myItems = new RepositoryItemJs.items();

            if (_CmsFile != null)
            {
                try
                {
                    myItems.UidRepository = _CmsFile.Uid_CmsRepository.ToString();
                    myItems.Uid = _CmsFile.Uid.ToString();
                    myItems.name = _CmsFile.Name;
                    myItems.typo = "folder";
                    myItems.path = _CmsFile.AbsolutePath;
                    myItems.extension = "";

                    if (!String.IsNullOrEmpty(_CmsFile.ImagePrev))
                        myItems.thumb = _CmsFile.AbsoluteUrlPrev;

                    List<RepositoryItemJs.itemDetails> myListItemDetails = new List<RepositoryItemJs.itemDetails>();

                    RepositoryItemJs.itemDetails myItemDetailsNome = new RepositoryItemJs.itemDetails();
                    myItemDetailsNome.label = "nome";
                    myItemDetailsNome.value = _CmsFile.Name;
                    myItems.details.Add(myItemDetailsNome);

                    RepositoryItemJs.itemDetails myItemDetailsAutor = new RepositoryItemJs.itemDetails();
                    myItemDetailsAutor.label = "autor";
                    myItemDetailsAutor.value = _CmsFile.Uid_CreationUser.ToString();
                    myItems.details.Add(myItemDetailsAutor);
                }
                catch (Exception ex)
                {
                    String sErr = ex.Message.ToString();
                }
            }

            return myItems;
        }
        private RepositoryItemJs.items GetFileItemJs(CmsFile _CmsFile)
        {
            RepositoryItemJs.items myItems = new RepositoryItemJs.items();

            if (_CmsFile != null)
            {
                try
                {
                    myItems.Uid = _CmsFile.Uid.ToString();
                    myItems.name = _CmsFile.Name;
                    myItems.typo = "file";
                    myItems.path = _CmsFile.AbsolutePath;
                    myItems.extension = objLibString.GetExtentionFromFileName(_CmsFile.Name);

                    if (!String.IsNullOrEmpty(_CmsFile.ImagePrev))
                        myItems.thumb = _CmsFile.AbsoluteUrlPrev;

                    myItems.original = _CmsFile.AbsoluteUrl;

                    List<RepositoryItemJs.itemDetails> myListItemDetails = new List<RepositoryItemJs.itemDetails>();
                    RepositoryItemJs.itemDetails myItemDetails = new RepositoryItemJs.itemDetails();

                    RepositoryItemJs.itemDetails myItemDetailsNome = new RepositoryItemJs.itemDetails();
                    myItemDetailsNome.label = "FILENAME";
                    myItemDetailsNome.value = _CmsFile.Name;
                    myItems.details.Add(myItemDetailsNome);

                    RepositoryItemJs.itemDetails myItemDetailsAutor = new RepositoryItemJs.itemDetails();
                    myItemDetailsAutor.label = "UPLOADED BY";
                    myItemDetailsAutor.value = _CmsFile.Uid_CreationUser.ToString();
                    myItems.details.Add(myItemDetailsAutor);

                    String sDimension = String.Empty;
                    Int32 nDimension = 0;
                    if (_CmsFile.FileSize != null)
                        nDimension = (Int32)_CmsFile.FileSize;

                    if (nDimension > 0)
                    {
                        if (nDimension > 1024)
                        {
                            Double dDimension = nDimension / 1024;
                            if (dDimension > 1000)
                                sDimension = (dDimension / 1000).ToString() + " MB";
                            else
                                sDimension = dDimension.ToString() + " KB";
                        }
                        else
                        {
                            sDimension = nDimension.ToString() + " byte";
                        }
                    }

                    if (!String.IsNullOrEmpty(sDimension))
                    {
                        RepositoryItemJs.itemDetails myItemDetailDimension = new RepositoryItemJs.itemDetails();
                        myItemDetailDimension.label = "WEIGHT";
                        myItemDetailDimension.value = sDimension;
                        myItems.details.Add(myItemDetailDimension);
                    }
                }
                catch (Exception ex)
                {
                    String sErr = ex.Message.ToString();
                }
            }

            return myItems;
        }
        private RepositoryItemJs.items GetFolderDetailItemJs(CmsFile _CmsFile)
        {
            RepositoryItemJs.items myItems = new RepositoryItemJs.items();

            if (_CmsFile != null)
            {
                try
                {
                    myItems.UidRepository = _CmsFile.Uid_CmsRepository.ToString();
                    myItems.Uid = _CmsFile.Uid.ToString();
                    myItems.name = _CmsFile.Name;
                    myItems.typo = "folder";
                    myItems.path = _CmsFile.AbsolutePath;
                    myItems.extension = "";

                    if (!String.IsNullOrEmpty(_CmsFile.ImagePrev))
                        myItems.thumb = _CmsFile.AbsoluteUrlPrev;

                    List<RepositoryItemJs.itemDetails> myListItemDetails = new List<RepositoryItemJs.itemDetails>();

                    RepositoryItemJs.itemDetails myItemDetailsNome = new RepositoryItemJs.itemDetails();
                    myItemDetailsNome.label = "FOLDERNAME";
                    myItemDetailsNome.value = _CmsFile.Name;
                    myItems.details.Add(myItemDetailsNome);

                    RepositoryItemJs.itemDetails myItemDetailsAutor = new RepositoryItemJs.itemDetails();
                    myItemDetailsAutor.label = "CREATED BY";
                    myItemDetailsAutor.value = _CmsFile.Uid_CreationUser.ToString();
                    myItems.details.Add(myItemDetailsAutor);
                }
                catch (Exception ex)
                {
                    String sErr = ex.Message.ToString();
                }
            }

            return myItems;
        }
        private RepositoryItemJs.items GetFileItemDetailJs(CmsFile _CmsFile)
        {
            RepositoryItemJs.items myItems = new RepositoryItemJs.items();

            if (_CmsFile != null)
            {
                try
                {
                    myItems.Uid = _CmsFile.Uid.ToString();
                    myItems.name = _CmsFile.Name;
                    myItems.typo = "file";
                    myItems.path = _CmsFile.AbsolutePath;
                    myItems.extension = objLibString.GetExtentionFromFileName(_CmsFile.Name);

                    if (!String.IsNullOrEmpty(_CmsFile.ImagePrev))
                        myItems.thumb = _CmsFile.AbsoluteUrlPrev;

                    myItems.original = _CmsFile.AbsoluteUrl;

                    List<RepositoryItemJs.itemDetails> myListItemDetails = new List<RepositoryItemJs.itemDetails>();
                    RepositoryItemJs.itemDetails myItemDetails = new RepositoryItemJs.itemDetails();

                    RepositoryItemJs.itemDetails myItemDetailsNome = new RepositoryItemJs.itemDetails();
                    myItemDetailsNome.label = "FILENAME";
                    myItemDetailsNome.value = _CmsFile.Name;
                    myItems.details.Add(myItemDetailsNome);

                    RepositoryItemJs.itemDetails myItemDetailsAutor = new RepositoryItemJs.itemDetails();
                    myItemDetailsAutor.label = "UPLOADED BY";
                    myItemDetailsAutor.value = _CmsFile.Uid_CreationUser.ToString();
                    myItems.details.Add(myItemDetailsAutor);

                    String sDimension = String.Empty;
                    Int32 nDimension = 0;
                    if (_CmsFile.FileSize != null)
                        nDimension = (Int32)_CmsFile.FileSize;

                    if (nDimension > 0)
                    {
                        if (nDimension > 1024)
                        {
                            Double dDimension = nDimension / 1024;
                            if (dDimension > 1000)
                                sDimension = (dDimension / 1000).ToString() + " MB";
                            else
                                sDimension = dDimension.ToString() + " KB";
                        }
                        else
                        {
                            sDimension = nDimension.ToString() + " byte";
                        }
                    }

                    if (!String.IsNullOrEmpty(sDimension))
                    {
                        RepositoryItemJs.itemDetails myItemDetailDimension = new RepositoryItemJs.itemDetails();
                        myItemDetailDimension.label = "WEIGHT";
                        myItemDetailDimension.value = sDimension;
                        myItems.details.Add(myItemDetailDimension);
                    }


                    String sRisolution = String.Empty;
                    if (objLibImage.FileContentTypeImage(_CmsFile.FileContentType))
                    {

                        IBinaryObject objImageUrl = BinaryManager.ReadFromUri(new Uri(_CmsFile.AbsoluteUrl));
                        System.Drawing.Image image = System.Drawing.Image.FromStream(objImageUrl.DataStream);
                        sRisolution = image.Width + "px x " + image.Height + "px";
                    }

                    if (!String.IsNullOrEmpty(sRisolution))
                    {
                        RepositoryItemJs.itemDetails myItemDetailsRisolution = new RepositoryItemJs.itemDetails();
                        myItemDetailsRisolution.label = "DIMENSIONS";
                        myItemDetailsRisolution.value = sRisolution;
                        myItems.details.Add(myItemDetailsRisolution);
                    }

                }
                catch (Exception ex)
                {
                    String sErr = ex.Message.ToString();
                }
            }

            return myItems;
        }
        #endregion
    }
}
