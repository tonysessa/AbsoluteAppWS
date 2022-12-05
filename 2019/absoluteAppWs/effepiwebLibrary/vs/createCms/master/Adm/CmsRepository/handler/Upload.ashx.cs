using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Web.Script.Serialization;
// Support
using Support.db;
using Support.Library;
using Support.CmsFunction;
using Support.Web;
using Support.jQueryFileUpload;
//
using DbModel;
using dataLibs;

namespace backOffice.cmsRepository.handler
{
    /// <summary>
    /// Descrizione di riepilogo per Upload
    /// </summary>
    public class Upload : IHttpHandler, IReadOnlySessionState
    {
        #region objUtil
        protected Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        protected Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        protected Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        protected Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        protected Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        protected Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
        protected Support.Library.ImageUtil objLibImage = new Support.Library.ImageUtil();
        protected Support.Library.FileUtil objLibFileUtil = new Support.Library.FileUtil();
        //
        protected WebContext wctx = null;
        protected MyHeader header = new MyHeader();
        protected CmsFunction objCmsFunction = new CmsFunction();
        protected CmsBoDataLibs objCmsBoDataLibs = new CmsBoDataLibs();
        #endregion

        #region Parametri
        public CmsUserSession currentCmsUserSession;
        public String sCmsStartingpage = WebContext.getConfig("%.cmsStartingpage").ToString();
        public String sStorageRepositoryBaseUrl = WebContext.getConfig("%.storageRepositoryBaseUrl").ToString();
        public String sStorageRepositoryBasePath = WebContext.getConfig("%.storageRepositoryBasePath").ToString();
        #endregion

        #region Variabili
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }  
        private readonly JavaScriptSerializer js = new JavaScriptSerializer();
        #endregion

        #region Main
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["CmsUserSession"] != null)
            {
                currentCmsUserSession = (CmsUserSession)context.Session["CmsUserSession"];

                context.Response.AddHeader("Pragma", "no-cache");
                context.Response.AddHeader("Cache-Control", "private, no-cache");

                HandleMethod(context);
            }
            else
            {
                context.Response.Write("NOSESSION");
            }
        }
        #endregion

        // Handle request based on method
        private void HandleMethod(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "HEAD":
                case "GET":
                    break;
                case "POST":
                    UploadFile(context);
                    break;
                case "PUT":
                    UploadFile(context);
                    break;
                case "DELETE":
                    //DeleteFile(context);
                    //break;
                case "OPTIONS":
                    //ReturnOptions(context);
                    //break;
                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;
            var UidCmsRepository = context.Request["UidCmsRepository"];
            var UidCmsRepositoryFolder = context.Request["UidCmsRepositoryFolder"];

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses, UidCmsRepository, UidCmsRepositoryFolder);
            }
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses, String _UidCmsRepository, String _UidCmsRepositoryFolder)
        {
            List<RepositoryItemJs.items> myListFileItems = new List<RepositoryItemJs.items>();
            String sRelativePath = String.Empty;
            if (!String.IsNullOrEmpty(_UidCmsRepository))
            {
                CmsRepositoryItemResponse myCmsRepositoryItemResponse = objCmsBoDataLibs.CmsRepositoryGet(header, _UidCmsRepository);
                if (myCmsRepositoryItemResponse.Success)
                {
                    CmsRepository myCmsRepository = myCmsRepositoryItemResponse.item;
                    sRelativePath = myCmsRepository.RelativePath;
                }
            }

            if (objLibMath.isNumber(_UidCmsRepositoryFolder))
            {
                CmsFileItemResponse myCmsFileItemResponse = objCmsBoDataLibs.CmsFileGet(header, _UidCmsRepositoryFolder);
                if ((myCmsFileItemResponse.Success) && (myCmsFileItemResponse.item != null))
                {
                    CmsFile myCmsRepositoryFolder = myCmsFileItemResponse.item;
                    sRelativePath = myCmsRepositoryFolder.RelativePath;
                }
            }

            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                            
                //
                var file = context.Request.Files[i];

                DirectoryInfo dr = new DirectoryInfo(sStorageRepositoryBasePath + sRelativePath);
                if (!dr.Exists)
                    Directory.CreateDirectory(sStorageRepositoryBasePath + sRelativePath);

                String sNewFileName = objLibFileUtil.ClearFileName(file.FileName);
                String sFileNamePreview = String.Empty;
                String sNewFileNameExtention = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1, file.FileName.Length - (file.FileName.LastIndexOf('.') + 1));
                FileInfo myFileInfo = new FileInfo(sStorageRepositoryBasePath + sRelativePath + sNewFileName);
                Int32 indexRename = 0;
                while (myFileInfo.Exists)
                {
                    indexRename++;
                    sNewFileName = file.FileName.Replace(sNewFileNameExtention, "") + indexRename.ToString().PadLeft(3, '0') + "." + sNewFileNameExtention;
                    myFileInfo = new FileInfo(sStorageRepositoryBasePath + sRelativePath + sNewFileName);
                }
                file.SaveAs(sStorageRepositoryBasePath + sRelativePath + sNewFileName);
                IBinaryObject myByte = new BinaryObject(file.InputStream);

                //ImagePrev
                if (objLibImage.FileContentTypeImage(file.ContentType))
                {
                    String sUrlUri = sStorageRepositoryBaseUrl + sRelativePath + sNewFileName;
                    int nImageUrl_PrevWidth = 320;
                    int nImageUrl_PrevHeight = 0;
                    if (objLibImage.ResizeImage(sStorageRepositoryBasePath + sRelativePath + sNewFileName, sStorageRepositoryBasePath + sRelativePath + "th_" + sNewFileName, nImageUrl_PrevWidth, nImageUrl_PrevHeight, false, false))
                        sFileNamePreview = "th_" + sNewFileName;

                }

                CmsFile myCmsFile = new CmsFile();
                if (objLibMath.isNumber(_UidCmsRepository))
                    myCmsFile.Uid_CmsRepository = System.Convert.ToInt32(_UidCmsRepository);
                myCmsFile.Name = sNewFileName;
                if (!String.IsNullOrEmpty(sFileNamePreview))
                {
                    myCmsFile.ImagePrev = sFileNamePreview;
                    myCmsFile.ImageIco = sFileNamePreview;
                }
                myCmsFile.FileContentType = file.ContentType;
                if (objLibMath.isNumber(_UidCmsRepositoryFolder))
                    myCmsFile.Uid_Parent = System.Convert.ToInt32(_UidCmsRepositoryFolder);
                myCmsFile.FileType = EnumCmsFileType.Stream;

                //
                header.cmsUserUid = currentCmsUserSession.currentUid;
                header.cmsCmsNlsContext = currentCmsUserSession.currentCmsNlsContext;
                
                CmsFileItemResponse responseFileItemResponse = objCmsBoDataLibs.CmsFileUpsert(header, myCmsFile);
                if (responseFileItemResponse.Success)
                {                    
                    RepositoryItemJs.items myCurrentListItems = GetFileItemJs(responseFileItemResponse.item, sRelativePath);
                    myListFileItems.Add(myCurrentListItems);                    
                }
            }

            context.Response.Write(js.Serialize(myListFileItems));
        }

        private RepositoryItemJs.items GetFileItemJs(CmsFile _CmsFile, String _relativePath)
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

                    myItems.original = sStorageRepositoryBaseUrl + _CmsFile.RelativePath;

                    List<RepositoryItemJs.itemDetails> myListItemDetails = new List<RepositoryItemJs.itemDetails>();
                    RepositoryItemJs.itemDetails myItemDetails = new RepositoryItemJs.itemDetails();

                    RepositoryItemJs.itemDetails myItemDetailsNome = new RepositoryItemJs.itemDetails();
                    myItemDetailsNome.label = "nome";
                    myItemDetailsNome.value = _CmsFile.Name;
                    myItems.details.Add(myItemDetailsNome);

                    RepositoryItemJs.itemDetails myItemDetailsAutor = new RepositoryItemJs.itemDetails();
                    myItemDetailsAutor.label = "autor";
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
                        myItemDetailDimension.label = "weigth";
                        myItemDetailDimension.value = sDimension;
                        myItems.details.Add(myItemDetailDimension);
                    }

                    /*
                    String sRisolution = String.Empty;
                    if (objLibImage.FileContentTypeImage(_CmsFile.FileContentType))
                    {

                        IBinaryObject objImageUrl = BinaryManager.ReadFromUri(new Uri(_CmsFile.AbsoluteUrl));
                        System.Drawing.Image image = System.Drawing.Image.FromStream(objImageUrl.DataStream);
                        sRisolution = image.Width + "px x " + image.Height + "px";
                    }

                    if (!String.IsNullOrEmpty(sRisolution))
                    {
                        myItemDetails.label = "weigth";
                        myItemDetails.value = sRisolution;
                        myListItemDetails.Add(myItemDetails);
                    }
                    */
                }
                catch (Exception ex)
                {
                    String sErr = ex.Message.ToString();
                }
            }

            return myItems;
        }
    }
}