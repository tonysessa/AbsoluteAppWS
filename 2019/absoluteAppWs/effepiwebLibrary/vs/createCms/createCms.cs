using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//
using Support.Library;
using Support.Web;
using Support.db;
using Support.Mail;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace KWWeb_ContextUtli
{
    class importCustomerProfiles
    {
        static Support.Library.DateUtil objLibDate = new Support.Library.DateUtil();
        static Support.Library.DbUtil objLibDB = new Support.Library.DbUtil();
        static Support.Library.StringUtil objLibString = new Support.Library.StringUtil();
        static Support.Library.StringSqlUtil objLibSqlString = new Support.Library.StringSqlUtil();
        static Support.Library.MathUtil objLibMath = new Support.Library.MathUtil();
        static Support.Library.CriptUtil objLibCript = new Support.Library.CriptUtil();
        static Support.Library.LogUtil objLibLog = new Support.Library.LogUtil();
        static Support.Library.ImageUtil objLibImage = new Support.Library.ImageUtil();

        //
        static void Main(string[] args)
        {
            mainCreateCms();
        }

        static void LogUtf8(String filename, String str)
        {
            using (var sw = new StreamWriter(new FileStream(filename, FileMode.Append), Encoding.UTF8))
            {
                sw.WriteLine(str.ToString());
                sw.Close();
            }
        }


        //
        static void mainCreateCms()
        {
            String sStrLog = String.Empty;
            //         
            String sSourcedir = WebContext.getConfig("%.sourcedir").ToString();

            IDataProvider dp = IDataProviderFactory.factory("DataProviderSqlServer");
            String sSql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
            SimpleDataSet ds = dp.executeQuery(sSql);

            //
            String sDestdir = WebContext.getConfig("%.destdir").ToString();
            String sColumnDataCode_SupportPage = String.Empty;
            String sFileNamePage_Support = "setOrder.aspx.cs";

            DirectoryInfo dr = new DirectoryInfo(sDestdir + "support");
            if (!dr.Exists)
                Directory.CreateDirectory(sDestdir + "support");

            for (int i = 0; i < ds.Table.Rows.Count; i++)
            {
                //                     
                String sContentTable = ds.Rows[i]["TABLE_NAME"].ToString();
                Boolean bCreate = true;

                if (sContentTable.Equals("CmsNlsContext")
                    || sContentTable.Equals("CmsRepository") || sContentTable.Equals("CmsFile")
                    //|| sContentTable.Equals("CmsRepositoryFolder") || sContentTable.Equals("CmsResources")
                    //|| sContentTable.Equals("CmsRoles") || sContentTable.Equals("CmsRouting") || sContentTable.Equals("CmsUsers")
                    //|| sContentTable.Equals("CmsShare") || sContentTable.Equals("CmsSections") || sContentTable.Equals("CmsSubSections")
                    )
                    bCreate = false;

                /*
                bCreate = false;
                if (sContentTable.Equals("Pages"))
                    bCreate = true;
                */

                if (bCreate)
                {
                    String sSqlTable = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + objLibString.sQuote(sContentTable) + " ORDER BY ORDINAL_POSITION";
                    SimpleDataSet dsTableColumns = dp.executeQuery(sSqlTable);

                    //
                    Console.WriteLine(sContentTable);

                    //
                    String sColumnData = String.Empty;
                    String sColumnHeader = String.Empty;
                    String sColumnHeaderCode_List = String.Empty;
                    String sColumnHeaderCode_Designer = String.Empty;

                    //
                    String sFileNamePage_List = "list.aspx";
                    String sFileNameClass_List = "list.aspx.cs";
                    String sFileNameDesigner_List = "list.aspx.designer.cs";
                    String sColumnDataCode_List = String.Empty;

                    String sFileNamePage_Deatils = "details.aspx";
                    String sFileNameClass_Deatils = "details.aspx.cs";
                    String sFileNameDesigner_Deatils = "details.aspx.designer.cs";

                    String sFileNamePage_Export = "export.aspx";
                    String sFileNameClass_Export = "export.aspx.cs";
                    String sFileNameDesigner_Export = "export.aspx.designer.cs";

                    String sFileNamePage_Action = "action.aspx";
                    String sFileNameClass_Action = "action.aspx.cs";
                    String sFileNameDesigner_Action = "action.aspx.designer.cs";

                    String sColumnDataCode_InputHtml = String.Empty;
                    String sColumnDataCode_InputHtmlPublish = String.Empty;
                    String sColumnDataCode_DeatilsInitUpdate = String.Empty;
                    String sColumnDataCode_DeatilsInitUpdate_LoadRelated = String.Empty;
                    String sTableRelatedDataCode_DeleteCommand = String.Empty;
                    String sColumnDataCode_DeatilsSave = String.Empty;
                    String sColumnDataCode_DeatilsInitPage = String.Empty;
                    String sColumnDataCode_Designer = String.Empty;

                    //
                    String sRqCmsNlsContextFilter = String.Empty;

                    // Directory
                    DirectoryInfo md = new DirectoryInfo(sDestdir + "\\" + sContentTable);
                    if (!md.Exists)
                        md.Create();

                    String sDestdirSection = sDestdir + "\\" + sContentTable + "\\";


                    // Files
                    FileInfo mf = new FileInfo(sDestdirSection + sFileNamePage_List);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameClass_List);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameDesigner_List);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNamePage_Deatils);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameClass_Deatils);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameDesigner_Deatils);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNamePage_Export);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameClass_Export);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameDesigner_Export);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNamePage_Action);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameClass_Action);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdirSection + sFileNameDesigner_Action);
                    if (mf.Exists)
                        mf.Delete();

                    mf = new FileInfo(sDestdir + "support\\" + sFileNamePage_Support);
                    if (mf.Exists)
                        mf.Delete();

                    //
                    String sExportFieldList = "String[] listColumns = new String[" + dsTableColumns.Rows.Count.ToString() + "];" + "\n";

                    //
                    String sTitleField = "Uid";

                    //
                    Boolean bFieldOrd = false;

                    //
                    Boolean bFieldPublish = false;
                    String sFieldPublishStartComment = "/*";
                    String sFieldPublishEndComment = "*/";
                    String sRqSectionUri = "";

                    // Verifico i campi che ci sono nella tabella
                    for (int j = 0; j < dsTableColumns.Table.Rows.Count; j++)
                    {
                        if (dsTableColumns.Rows[j]["COLUMN_NAME"].ToString().ToLower().Equals("ord"))
                        {
                            bFieldOrd = true;
                        }

                        if (dsTableColumns.Rows[j]["COLUMN_NAME"].ToString().ToLower().Equals("versionpublished"))
                        {
                            bFieldPublish = true;
                            sFieldPublishStartComment = "";
                            sFieldPublishEndComment = "";
                        }

                        if (dsTableColumns.Rows[j]["COLUMN_NAME"].ToString().ToLower().Equals("sectionuri"))
                        {
                            sRqSectionUri = "rq.sectionUri = sectionUri;";
                        }
                    }

                    //
                    for (int j = 0; j < dsTableColumns.Table.Rows.Count; j++)
                    {
                        //                        
                        String sTABLE_NAME = dsTableColumns.Rows[j]["TABLE_NAME"].ToString();
                        String sCOLUMN_NAME = dsTableColumns.Rows[j]["COLUMN_NAME"].ToString();
                        String sIS_NULLABLE = dsTableColumns.Rows[j]["IS_NULLABLE"].ToString();
                        String sDATA_TYPE = dsTableColumns.Rows[j]["DATA_TYPE"].ToString();
                        String sCHARACTER_MAXIMUN_LENGTH = dsTableColumns.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        Int32 nCHARACTER_MAXIMUN_LENGTH = 0;

                        String sSqlFK = "SELECT OBJECT_NAME(fk.parent_object_id) 'ParentTable', c1.name 'ParentColumn', OBJECT_NAME(fk.referenced_object_id) 'ReferencedTable', c2.name 'ReferencedColumn' FROM sys.foreign_keys fk INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id INNER JOIN sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id INNER JOIN sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id WHERE OBJECT_NAME(fk.parent_object_id) = '" + sTABLE_NAME + "' AND c1.name = '" + sCOLUMN_NAME + "' ";
                        SimpleDataSet dsTableFK = dp.executeQuery(sSqlFK);

                        String sREFERENCED_TABLE = String.Empty;
                        String sREFERENCED_COLUMN = String.Empty;
                        if (dsTableFK != null && (dsTableFK.Table.Rows.Count > 0))
                        {
                            sREFERENCED_TABLE = dsTableFK.Table.Rows[0]["ReferencedTable"].ToString();
                            sREFERENCED_COLUMN = dsTableFK.Table.Rows[0]["ReferencedColumn"].ToString();
                        }

                        //
                        String sCOLUMN_NAME_PARAMETER = sCOLUMN_NAME.Substring(0, 1).ToLower() + sCOLUMN_NAME.Substring(1);

                        //
                        sExportFieldList += "                listColumns[" + j.ToString() + "] = \"" + sCOLUMN_NAME + "\";" + "\n";

                        //
                        if ((!sCOLUMN_NAME.ToLower().Equals("uid")) && (!sCOLUMN_NAME.ToLower().Equals("creationdate")) && (!sCOLUMN_NAME.ToLower().Equals("uid_creationuser"))
                            && (!sCOLUMN_NAME.ToLower().Equals("updatedate")) && (!sCOLUMN_NAME.ToLower().Equals("uid_updateuser")) && (!sCOLUMN_NAME.ToLower().Equals("ord"))
                            && (!sCOLUMN_NAME.ToLower().Equals("uid_cmsnlscontext")) && (!sCOLUMN_NAME.ToLower().Equals("statusflag"))
                             && (!sCOLUMN_NAME.ToLower().Equals("creationdate")) && (!sCOLUMN_NAME.ToLower().Equals("uid_cmsusers")) && (!sCOLUMN_NAME.ToLower().Equals("updatedate"))
                             && (!sCOLUMN_NAME.ToLower().Equals("uid_cmsusers_mod")) && (!sCOLUMN_NAME.ToLower().Equals("lord")) && (!sCOLUMN_NAME.ToLower().Equals("rord"))
                                  && (!sCOLUMN_NAME.ToLower().Equals("versioncurrent")) && (!sCOLUMN_NAME.ToLower().Equals("versionpublished"))
                                  && (!sCOLUMN_NAME.ToLower().Equals("publishdate")) && (!sCOLUMN_NAME.ToLower().Equals("uid_publishuser"))
                                   //&& (!sCOLUMN_NAME.ToLower().Equals("uid_from")) && (!sCOLUMN_NAME.ToLower().Equals("import_id"))
                                   && (!sCOLUMN_NAME.ToLower().Equals("linktitle")) && (!sCOLUMN_NAME.ToLower().Equals("linktarget"))
                                  )
                        {
                            if (sDATA_TYPE.ToLower().Equals("nvarchar"))
                            {
                                #region nvarchar
                                if (sCOLUMN_NAME.ToLower().IndexOf("imageurl") >= 0)
                                {
                                    #region ImagePrev
                                    if (sCOLUMN_NAME.ToLower().IndexOf("_prev") >= 0)
                                    {
                                        //
                                        sColumnHeader += "							<th class=\"record-data-header img-data\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal></th>" + "\n";
                                        sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                        sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                        sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                        sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                        sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                        sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                        sColumnHeaderCode_List += "\n";

                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";


                                        //
                                        sColumnData += "								<td class=\"record-data img-data\"><asp:Image ID=\"Image_" + sCOLUMN_NAME + "\" runat=\"server\" Visible=\"false\" /></td>" + "\n";

                                        //
                                        sColumnDataCode_List += "                \n";
                                        sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                        sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = currentObj." + sCOLUMN_NAME + ";" + "\n";
                                        sColumnDataCode_List += "                if (!String.IsNullOrEmpty(s" + sCOLUMN_NAME + "))" + "\n";
                                        sColumnDataCode_List += "                {" + "\n";
                                        sColumnDataCode_List += "                    Image myImage_" + sCOLUMN_NAME + " = (Image)e.Item.FindControl(\"Image_" + sCOLUMN_NAME + "\");" + "\n";
                                        sColumnDataCode_List += "                    myImage_" + sCOLUMN_NAME + ".ImageUrl = sStoragePublicBaseUrl + s" + sCOLUMN_NAME + ";" + "\n";
                                        sColumnDataCode_List += "                    myImage_" + sCOLUMN_NAME + ".Visible = true;" + "\n";
                                        sColumnDataCode_List += "                }" + "\n";
                                    }
                                    #endregion

                                    #region Image
                                    if (sCOLUMN_NAME.ToLower().IndexOf("_prev") < 0)
                                    {

                                        // sColumnDataCode_DeatilsInitPage
                                        //sColumnDataCode_DeatilsInitPage += "            Boolean bCmsUploadImageCrop_" + sCOLUMN_NAME + "_HasCrop = System.Convert.ToBoolean(currentCmsUserSession.GetGlobalResources(\"" + sTABLE_NAME + ".CmsUploadImageCrop_" + sCOLUMN_NAME + ".HasCrop\", \"true\"));" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "            if (bCmsUploadImageCrop_" + sCOLUMN_NAME + "_HasCrop)" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "            {" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "                int nCmsUploadImageCrop_" + sCOLUMN_NAME + "_Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"" + sTABLE_NAME + ".CmsUploadImageCrop_" + sCOLUMN_NAME + ".Width\", \"1440\"));" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "                int nCmsUploadImageCrop_" + sCOLUMN_NAME + "_Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"" + sTABLE_NAME + ".CmsUploadImageCrop_" + sCOLUMN_NAME + ".Height\", \"260\"));" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "                CmsUploadImageCrop_" + sCOLUMN_NAME + ".SetCropArea(nCmsUploadImageCrop_" + sCOLUMN_NAME + "_Width, nCmsUploadImageCrop_" + sCOLUMN_NAME + "_Height);" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "            }" + "\n";

                                        // sColumnDataCode_DeatilsInitUpdate
                                        sColumnDataCode_DeatilsInitUpdate += "                \n";
                                        sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                if (!String.IsNullOrEmpty(updateObject." + sCOLUMN_NAME + "))" + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                {" + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                    CmsUploadImageCrop_" + sCOLUMN_NAME + ".UploadDir = sUploadDir;" + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                    CmsUploadImageCrop_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + "_Prev, updateObject." + sCOLUMN_NAME + ");" + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                }" + "\n";


                                        sColumnDataCode_DeatilsSave += "                #region " + sCOLUMN_NAME + "" + "\n";
                                        sColumnDataCode_DeatilsSave += "                if (CmsUploadImageCrop_" + sCOLUMN_NAME + ".HttpFileUrl.Equals(\"TODELETE\"))" + "\n";
                                        sColumnDataCode_DeatilsSave += "                {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = \"\";" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + "_Prev = \"\";" + "\n";
                                        sColumnDataCode_DeatilsSave += "                }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                else if (!String.IsNullOrEmpty(CmsUploadImageCrop_" + sCOLUMN_NAME + ".HttpFileUrl))" + "\n";
                                        sColumnDataCode_DeatilsSave += "                {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    // This code works, but for ASCII only" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    String url = CmsUploadImageCrop_" + sCOLUMN_NAME + ".HttpFileUrl;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                    if (CmsUploadImageCrop_" + sCOLUMN_NAME + ".isCrop)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        #region Crop" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        String[] parameter = CmsUploadImageCrop_" + sCOLUMN_NAME + ".Parameter.Split('|');" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                        if (parameter.Length.Equals(4))" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int coordx = Convert.ToInt16(parameter[0]);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int coordy = Convert.ToInt16(parameter[1]);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int coordW = Convert.ToInt16(parameter[2]);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int coordH = Convert.ToInt16(parameter[3]);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                            IBinaryObject obj" + sCOLUMN_NAME + " = BinaryManager.ReadFromUri(new Uri(url));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            obj" + sCOLUMN_NAME + ".cache();" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                            if (obj" + sCOLUMN_NAME + " != null)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                                IBinaryObject CropImage = objLibImage.Crop(obj" + sCOLUMN_NAME + ", coordW, coordH, coordx, coordy);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                                // " + sCOLUMN_NAME + "" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                int n" + sCOLUMN_NAME + "Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ".Width\", \"1024\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                int n" + sCOLUMN_NAME + "Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ".Height\", \"768\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                IBinaryObject bytes = objLibImage.ResizeImage(CropImage, n" + sCOLUMN_NAME + "Width, n" + sCOLUMN_NAME + "Height, false, false);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                CmsStorageFile objCmsStorageFile = new CmsStorageFile();" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                objCmsStorageFile.RelativePath = sUploadDir + obj" + sCOLUMN_NAME + ".FullName;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                StorageFileItemResponse myResp = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFile, bytes.Buffer);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                if (myResp.Success)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                    updateObject." + sCOLUMN_NAME + " = myResp.item.RelativePath;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                                // " + sCOLUMN_NAME + "_Prev" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                CropImage.DataStream.Position = 0;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                int n" + sCOLUMN_NAME + "_PrevWidth = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "_Prev.Width\", \"320\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                int n" + sCOLUMN_NAME + "_PrevHeight = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "_Prev.Height\", \"0\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                IBinaryObject bytesThumb = objLibImage.ResizeImage(CropImage, n" + sCOLUMN_NAME + "_PrevWidth, n" + sCOLUMN_NAME + "_PrevHeight, false, false);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                CmsStorageFile objCmsStorageFileThumb = new CmsStorageFile();" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                objCmsStorageFileThumb.RelativePath = sUploadDir + \"th_\" + obj" + sCOLUMN_NAME + ".FullName;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                StorageFileItemResponse myRespThumb = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFileThumb, bytesThumb.Buffer);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                if (myRespThumb.Success)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                    updateObject." + sCOLUMN_NAME + "_Prev = myRespThumb.item.RelativePath;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                            }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        #endregion" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    else" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        #region Resize" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        IBinaryObject obj" + sCOLUMN_NAME + " = BinaryManager.ReadFromUri(new Uri(url));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        obj" + sCOLUMN_NAME + ".cache();" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                        if (obj" + sCOLUMN_NAME + " != null)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            // " + sCOLUMN_NAME + "" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            //CropImage.DataStream.Position = 0;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int n" + sCOLUMN_NAME + "Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ".Width\", \"1024\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int n" + sCOLUMN_NAME + "Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ".Height\", \"768\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                            IBinaryObject bytes = objLibImage.ResizeImage(obj" + sCOLUMN_NAME + ", n" + sCOLUMN_NAME + "Width, n" + sCOLUMN_NAME + "Height, false, false);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            CmsStorageFile objCmsStorageFile = new CmsStorageFile();" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            objCmsStorageFile.RelativePath = sUploadDir + obj" + sCOLUMN_NAME + ".FullName;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            StorageFileItemResponse myResp = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFile, bytes.Buffer);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            if (myResp.Success)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                updateObject." + sCOLUMN_NAME + " = myResp.item.RelativePath;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                            // " + sCOLUMN_NAME + "_Prev" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            obj" + sCOLUMN_NAME + ".DataStream.Position = 0;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int n" + sCOLUMN_NAME + "_PrevWidth = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "_Prev.Width\", \"320\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            int n" + sCOLUMN_NAME + "_PrevHeight = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "_Prev.Height\", \"0\"));" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            IBinaryObject bytesThumb = objLibImage.ResizeImage(obj" + sCOLUMN_NAME + ", n" + sCOLUMN_NAME + "_PrevWidth, n" + sCOLUMN_NAME + "_PrevHeight, false, false);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            CmsStorageFile objCmsStorageFileThumb = new CmsStorageFile();" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            objCmsStorageFileThumb.RelativePath = sUploadDir + \"th_\" + obj" + sCOLUMN_NAME + ".FullName;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            StorageFileItemResponse myRespThumb = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFileThumb, bytesThumb.Buffer);" + "\n";
                                        sColumnDataCode_DeatilsSave += "                            if (myRespThumb.Success)" + "\n";
                                        sColumnDataCode_DeatilsSave += "                                updateObject." + sCOLUMN_NAME + "_Prev = myRespThumb.item.RelativePath;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                " + "\n";
                                        sColumnDataCode_DeatilsSave += "                        }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                        #endregion" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                else" + "\n";
                                        sColumnDataCode_DeatilsSave += "                {" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = CmsUploadImageCrop_" + sCOLUMN_NAME + ".ImageUrl;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + "_Prev = CmsUploadImageCrop_" + sCOLUMN_NAME + ".ImageUrl_Preview;" + "\n";
                                        sColumnDataCode_DeatilsSave += "                }" + "\n";
                                        sColumnDataCode_DeatilsSave += "                #endregion" + "\n";

                                        // sColumnDataCode_InputHtml Input
                                        sColumnDataCode_InputHtml += "                                                <uc11:CmsUploadImageCrop ID=\"CmsUploadImageCrop_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                        // sColumnDataCode_Designer
                                        sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                        sColumnDataCode_Designer += "        /// Controllo CmsUploadImageCrop_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                        sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                        sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsUploadImageCrop CmsUploadImageCrop_" + sCOLUMN_NAME + ";" + "\n";
                                    }
                                    #endregion
                                }
                                else if (sCOLUMN_NAME.ToLower().Equals("fileurl"))
                                {
                                    #region FileUrl
                                    // sColumnDataCode_DeatilsInitUpdate
                                    sColumnDataCode_DeatilsInitUpdate += "                \n";
                                    sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                if (!String.IsNullOrEmpty(updateObject." + sCOLUMN_NAME + "))" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                {" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                    CmsUploadFile_" + sCOLUMN_NAME + ".UploadDir = sUploadDir;" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                    CmsUploadFile_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ");" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                }" + "\n";

                                    // sColumnDataCode_DeatilsSave
                                    sColumnDataCode_DeatilsSave += "                \n";
                                    sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsSave += "                #region " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsUploadFile_" + sCOLUMN_NAME + ".HttpFileUrl))\n";
                                    sColumnDataCode_DeatilsSave += "                {\n";
                                    sColumnDataCode_DeatilsSave += "                    // This code works, but for ASCII only\n";
                                    sColumnDataCode_DeatilsSave += "                    String url = CmsUploadFile_" + sCOLUMN_NAME + ".HttpFileUrl;\n";
                                    sColumnDataCode_DeatilsSave += "                    IBinaryObject objFileUrl = BinaryManager.ReadFromUri(new Uri(url));\n";
                                    sColumnDataCode_DeatilsSave += "                \n";
                                    sColumnDataCode_DeatilsSave += "                    if (objFileUrl != null)\n";
                                    sColumnDataCode_DeatilsSave += "                    {\n";
                                    sColumnDataCode_DeatilsSave += "                        CmsStorageFile objCmsStorageFile = new CmsStorageFile();\n";
                                    sColumnDataCode_DeatilsSave += "                        objCmsStorageFile.RelativePath = sUploadDir + objFileUrl.FullName;\n";
                                    sColumnDataCode_DeatilsSave += "                        StorageFileItemResponse myResp = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFile, objFileUrl.Buffer);\n";
                                    sColumnDataCode_DeatilsSave += "                        if (myResp.Success)\n";
                                    sColumnDataCode_DeatilsSave += "                            updateObject." + sCOLUMN_NAME + " = myResp.item.RelativePath;\n";
                                    sColumnDataCode_DeatilsSave += "                    }\n";
                                    sColumnDataCode_DeatilsSave += "                }\n";
                                    sColumnDataCode_DeatilsSave += "                else\n";
                                    sColumnDataCode_DeatilsSave += "                {\n";
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = CmsUploadFile_" + sCOLUMN_NAME + "." + sCOLUMN_NAME + ";\n";
                                    sColumnDataCode_DeatilsSave += "                }\n";
                                    sColumnDataCode_DeatilsSave += "                #endregion  \n";

                                    // sColumnDataCode_InputHtml Input
                                    sColumnDataCode_InputHtml += "                                                <uc8:CmsUploadFile ID=\"CmsUploadFile_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                    // sColumnDataCode_Designer
                                    sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// Controllo CmsUploadFile_" + sCOLUMN_NAME + "." + "\n";
                                    sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                    sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                    sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsUploadFile CmsUploadFile_" + sCOLUMN_NAME + ";" + "\n";
                                    #endregion
                                }
                                else if (sCOLUMN_NAME.ToLower().Equals("linkurl"))
                                {
                                    #region linkurl
                                    // sColumnDataCode_DeatilsInitUpdate
                                    sColumnDataCode_DeatilsInitUpdate += "                \n";
                                    sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                if (!String.IsNullOrEmpty(updateObject." + sCOLUMN_NAME + "))" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                    CmsTextBoxLink_" + sCOLUMN_NAME + ".SetValue(updateObject.LinkTitle, updateObject.LinkUrl, updateObject.LinkTarget);" + "\n";

                                    // sColumnDataCode_DeatilsSave
                                    sColumnDataCode_DeatilsSave += "                \n";
                                    sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsSave += "                #region " + sCOLUMN_NAME + "\n";
                                    //sColumnDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsTextBoxLink_" + sCOLUMN_NAME + ".Value_Url))\n";
                                    sColumnDataCode_DeatilsSave += "                updateObject.LinkUrl = CmsTextBoxLink_" + sCOLUMN_NAME + ".Value_Url;" + "\n";
                                    //sColumnDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsTextBoxLink_" + sCOLUMN_NAME + ".Value_Target))\n";
                                    sColumnDataCode_DeatilsSave += "                updateObject.LinkTarget = CmsTextBoxLink_" + sCOLUMN_NAME + ".Value_Target;" + "\n";
                                    //sColumnDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsTextBoxLink_" + sCOLUMN_NAME + ".Value_Title))\n";
                                    sColumnDataCode_DeatilsSave += "                updateObject.LinkTitle = CmsTextBoxLink_" + sCOLUMN_NAME + ".Value_Title;" + "\n";
                                    sColumnDataCode_DeatilsSave += "                #endregion  \n";

                                    // sColumnDataCode_InputHtml Input
                                    sColumnDataCode_InputHtml += "                                                <uc6:CmsTextBoxLink ID=\"CmsTextBoxLink_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" ViewTitle=\"true\" ViewTarget=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                    // sColumnDataCode_Designer
                                    sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// Controllo CmsTextBoxLink_" + sCOLUMN_NAME + "." + "\n";
                                    sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                    sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                    sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsTextBoxLink CmsTextBoxLink_" + sCOLUMN_NAME + ";" + "\n";
                                    #endregion
                                }
                                else
                                {
                                    #region other column
                                    if (String.IsNullOrEmpty(sREFERENCED_TABLE))
                                    {
                                        if (!String.IsNullOrEmpty(sCHARACTER_MAXIMUN_LENGTH))
                                            nCHARACTER_MAXIMUN_LENGTH = System.Convert.ToInt32(sCHARACTER_MAXIMUN_LENGTH);

                                        sColumnHeader += "							<th class=\"record-data-header txt-middle\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal><asp:LinkButton ID=\"LinkButton_Column_Header_" + sCOLUMN_NAME + "\" OnClick=\"LinkButton_Column_Header_Click\" CausesValidation=\"false\" CommandArgument=\"" + sCOLUMN_NAME + "\" runat=\"server\"><i id=\"iLinkButton_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\" class=\"fa fa-sort - desc\"></i></asp:LinkButton></th>" + "\n";
                                        sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                        sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                        sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                        sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                        sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                        sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                        sColumnHeaderCode_List += "\n";

                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo LinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.LinkButton LinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo iLinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.HtmlControls.HtmlGenericControl iLinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                        sColumnData += "									<td class=\"record-data txt-middle\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>" + "\n";

                                        sColumnDataCode_List += "                \n";
                                        sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                        sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                        sColumnDataCode_List += "                if (currentObj." + sCOLUMN_NAME + " != null)" + "\n";
                                        sColumnDataCode_List += "                    s" + sCOLUMN_NAME + " = currentObj." + sCOLUMN_NAME + ".ToString();" + "\n";
                                        sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                        sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                        if (nCHARACTER_MAXIMUN_LENGTH > 400)
                                        {
                                            if (sIS_NULLABLE.Equals("NO"))
                                                sColumnDataCode_InputHtml += "                                                <uc4:CmsTextBox ID=\"CmsTextBox_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" TextMode=\"MultiLine\" Required=\"true\" runat=\"server\" showSpacer=\"true\"  cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                            else
                                                sColumnDataCode_InputHtml += "                                                <uc4:CmsTextBox ID=\"CmsTextBox_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" TextMode=\"MultiLine\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                        }
                                        else
                                        {
                                            if (sIS_NULLABLE.Equals("NO"))
                                                sColumnDataCode_InputHtml += "                                                <uc4:CmsTextBox ID=\"CmsTextBox_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                            else
                                                sColumnDataCode_InputHtml += "                                                <uc4:CmsTextBox ID=\"CmsTextBox_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                        }

                                        sColumnDataCode_DeatilsSave += "                \n";
                                        sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                        sColumnDataCode_DeatilsSave += "                updateObject." + sCOLUMN_NAME + " = CmsTextBox_" + sCOLUMN_NAME + ".Value;" + "\n";

                                        sColumnDataCode_DeatilsInitUpdate += "                \n";
                                        sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                      CmsTextBox_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ".ToString());" + "\n";

                                        sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                        sColumnDataCode_Designer += "        /// Controllo CmsTextBox_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                        sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                        sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsTextBox CmsTextBox_" + sCOLUMN_NAME + ";" + "\n";

                                    }
                                    else
                                    {
                                        sColumnHeader += "							<th class=\"record-data-header txt-middle\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal><asp:LinkButton ID=\"LinkButton_Column_Header_" + sCOLUMN_NAME + "\" OnClick=\"LinkButton_Column_Header_Click\" CausesValidation=\"false\" CommandArgument=\"" + sCOLUMN_NAME + "\" runat=\"server\"><i id=\"iLinkButton_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\" class=\"fa fa-sort - desc\"></i></asp:LinkButton></th>" + "\n";
                                        sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                        sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                        sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                        sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                        sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                        sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                        sColumnHeaderCode_List += "\n";

                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo LinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.LinkButton LinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                        sColumnHeaderCode_Designer += "\n";
                                        sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Controllo iLinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                        sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnHeaderCode_Designer += "        protected global::System.Web.UI.HtmlControls.HtmlGenericControl iLinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                        sColumnData += "									<td class=\"record-data txt-middle\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>" + "\n";

                                        sColumnDataCode_List += "                \n";
                                        sColumnDataCode_List += "                // " + sCOLUMN_NAME + " (Related " + sREFERENCED_TABLE + ")\n";
                                        sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                        sColumnDataCode_List += "                if (currentObj.i" + sREFERENCED_TABLE + " != null)" + "\n";
                                        sColumnDataCode_List += "                    s" + sCOLUMN_NAME + " = currentObj.i" + sREFERENCED_TABLE + ".Title;" + "\n";
                                        sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                        sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                        //                                        
                                        sColumnDataCode_DeatilsInitPage += "            \n";
                                        sColumnDataCode_DeatilsInitPage += "            // Binding CmsDropDownList_" + sREFERENCED_TABLE + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE + "ListOptions rq" + sREFERENCED_TABLE + "ListOptions = new " + sREFERENCED_TABLE + "ListOptions();" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.sortBy = \"Title\";" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.sortAscending = true;" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.uid_CmsNlsContext = currentCmsUserSession.currentCmsNlsContext.Uid;" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            \n";
                                        sColumnDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE + "ListResponse rs" + sREFERENCED_TABLE + "ListResponse = objCmsBoDataLibs." + sREFERENCED_TABLE + "ListCms(header, rq" + sREFERENCED_TABLE + "ListOptions);" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            if (rs" + sREFERENCED_TABLE + "ListResponse != null)" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            {" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataSource = rs" + sREFERENCED_TABLE + "ListResponse.items;" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataTextField = \"Title\";" + "\n";
                                        //sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataValueField = \"Uid\";" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".Bind();" + "\n";
                                        sColumnDataCode_DeatilsInitPage += "            }" + "\n";

                                        sColumnDataCode_DeatilsInitUpdate += "                \n";
                                        sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                        sColumnDataCode_DeatilsInitUpdate += "                      CmsDropDownList_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ".ToString());" + "\n";

                                        sColumnDataCode_DeatilsSave += "                \n";
                                        sColumnDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsDropDownList_" + sCOLUMN_NAME + ".Value))\n";
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = CmsDropDownList_" + sCOLUMN_NAME + ".Value;\n";

                                        // Form Input
                                        if (sIS_NULLABLE.Equals("NO"))
                                            sColumnDataCode_InputHtml += "                                                <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                        else
                                            sColumnDataCode_InputHtml += "                                                <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                        sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                        sColumnDataCode_Designer += "        /// Controllo CmsDropDownList_" + sCOLUMN_NAME + "." + "\n";
                                        sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                        sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                        sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                        sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                        sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsDropDownList CmsDropDownList_" + sCOLUMN_NAME + ";" + "\n";

                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else if ((sDATA_TYPE.ToLower().Equals("ntext")) || (sDATA_TYPE.ToLower().Equals("text")))
                            {
                                #region ntext                                
                                sColumnHeader += "							<!--<th class=\"record-data-header txt-long\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal></th>-->" + "\n";
                                sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                sColumnHeaderCode_List += "\n";

                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                sColumnData += "									<!--<td class=\"record-data txt-long\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>-->" + "\n";

                                // Repeater_List_Column_ItemDataBound
                                sColumnDataCode_List += "                \n";
                                sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = currentObj." + sCOLUMN_NAME + ";" + "\n";
                                sColumnDataCode_List += "                s" + sCOLUMN_NAME + " = objLibString.sStripHTML(s" + sCOLUMN_NAME + ");" + "\n";
                                sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = objLibString.TroncaTesto(s" + sCOLUMN_NAME + ", 100, true);" + "\n";

                                sColumnDataCode_DeatilsInitUpdate += "                \n";
                                sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                sColumnDataCode_DeatilsInitUpdate += "                  CmsHtmlEditor_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ");" + "\n";

                                sColumnDataCode_DeatilsSave += "                \n";
                                sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_DeatilsSave += "                updateObject." + sCOLUMN_NAME + " = CmsHtmlEditor_" + sCOLUMN_NAME + ".Value;" + "\n";

                                //
                                sColumnDataCode_InputHtml += "                                                <uc3:CmsHtmlEditor ID=\"CmsHtmlEditor_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" showSpacer=\"true\" runat=\"server\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                sColumnDataCode_Designer += "        /// Controllo CmsHtmlEditor_" + sCOLUMN_NAME + "." + "\n";
                                sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsHtmlEditor CmsHtmlEditor_" + sCOLUMN_NAME + ";" + "\n";

                                #endregion
                            }
                            else if ((sDATA_TYPE.ToLower().Equals("int")) || (sDATA_TYPE.ToLower().Equals("double")))
                            {
                                #region int
                                sColumnHeader += "							<th class=\"record-data-header\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal><asp:LinkButton ID=\"LinkButton_Column_Header_" + sCOLUMN_NAME + "\" OnClick=\"LinkButton_Column_Header_Click\" CausesValidation=\"false\" CommandArgument=\"" + sCOLUMN_NAME + "\" runat=\"server\"><i id=\"iLinkButton_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\" class=\"fa fa-sort - desc\"></i></asp:LinkButton></th>" + "\n";
                                sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                sColumnHeaderCode_List += "\n";

                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo LinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.LinkButton LinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo iLinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.HtmlControls.HtmlGenericControl iLinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                sColumnData += "									<td class=\"record-data\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>" + "\n";

                                sColumnDataCode_List += "                \n";
                                if (String.IsNullOrEmpty(sREFERENCED_TABLE))
                                {
                                    sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                    sColumnDataCode_List += "                if (currentObj." + sCOLUMN_NAME + " != null)" + "\n";
                                    sColumnDataCode_List += "                    s" + sCOLUMN_NAME + " = currentObj." + sCOLUMN_NAME + ".ToString();" + "\n";
                                    sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                    sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                    sColumnDataCode_DeatilsInitUpdate += "                \n";
                                    sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                      CmsTextBoxNumber_" + sCOLUMN_NAME + ".SetValue((Int32)updateObject." + sCOLUMN_NAME + ");" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                \n";

                                    sColumnDataCode_DeatilsSave += "                \n";
                                    sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsSave += "                if (CmsTextBoxNumber_" + sCOLUMN_NAME + ".Value != null)" + "\n";
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = System.Convert.ToInt32(CmsTextBoxNumber_" + sCOLUMN_NAME + ".Value);" + "\n";
                                    sColumnDataCode_DeatilsSave += "                else" + "\n";
                                    if (sIS_NULLABLE.Equals("NO"))
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = 0;" + "\n";
                                    else
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = null;" + "\n";

                                    // Form Input
                                    if (sIS_NULLABLE.Equals("NO"))
                                        sColumnDataCode_InputHtml += "                                                <uc7:CmsTextBoxNumber ID=\"CmsTextBoxNumber_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" Required=\"true\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                    else
                                        sColumnDataCode_InputHtml += "                                                <uc7:CmsTextBoxNumber ID=\"CmsTextBoxNumber_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                    sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// Controllo CmsTextBoxNumber_" + sCOLUMN_NAME + "." + "\n";
                                    sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                    sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                    sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsTextBoxNumber CmsTextBoxNumber_" + sCOLUMN_NAME + ";" + "\n";
                                }
                                else
                                {
                                    sColumnDataCode_List += "                // " + sCOLUMN_NAME + " (Related " + sREFERENCED_TABLE + ")\n";
                                    sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                    sColumnDataCode_List += "                if (currentObj.i" + sREFERENCED_TABLE + " != null)" + "\n";
                                    sColumnDataCode_List += "                    s" + sCOLUMN_NAME + " = currentObj.i" + sREFERENCED_TABLE + ".Title;" + "\n";
                                    sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                    sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                    //                                        
                                    sColumnDataCode_DeatilsInitPage += "            \n";
                                    sColumnDataCode_DeatilsInitPage += "            // Binding CmsDropDownList_" + sREFERENCED_TABLE + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE + "ListOptions rq" + sREFERENCED_TABLE + "ListOptions = new " + sREFERENCED_TABLE + "ListOptions();" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.sortBy = \"Title\";" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.sortAscending = true;" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            \n";
                                    sColumnDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE + "ListResponse rs" + sREFERENCED_TABLE + "ListResponse = objCmsBoDataLibs." + sREFERENCED_TABLE + "ListCms(header, rq" + sREFERENCED_TABLE + "ListOptions);" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            if (rs" + sREFERENCED_TABLE + "ListResponse != null)" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            {" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataSource = rs" + sREFERENCED_TABLE + "ListResponse.items;" + "\n";
                                    //sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataTextField = \"Descrizione\";" + "\n";
                                    //sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataValueField = \"Uid\";" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".Bind();" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            }" + "\n";

                                    sColumnDataCode_DeatilsInitUpdate += "                \n";
                                    sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                      CmsDropDownList_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ".ToString());" + "\n";

                                    sColumnDataCode_DeatilsSave += "                \n";
                                    sColumnDataCode_DeatilsSave += "                if (objLibMath.isNumber(CmsDropDownList_" + sCOLUMN_NAME + ".Value))\n";
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = System.Convert.ToInt32(CmsDropDownList_" + sCOLUMN_NAME + ".Value);\n";
                                    sColumnDataCode_DeatilsSave += "                else \n";
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = null;\n";
                                    // Form Input
                                    if (sIS_NULLABLE.Equals("NO"))
                                        sColumnDataCode_InputHtml += "                                                <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                    else
                                        sColumnDataCode_InputHtml += "                                                <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                    sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// Controllo CmsDropDownList_" + sCOLUMN_NAME + "." + "\n";
                                    sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                    sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                    sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsDropDownList CmsDropDownList_" + sCOLUMN_NAME + ";" + "\n";
                                }
                                #endregion
                            }
                            else if (sDATA_TYPE.ToLower().Equals("smallint"))
                            {
                                #region smallint
                                sColumnHeader += "							<th class=\"record-data-header\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal><asp:LinkButton ID=\"LinkButton_Column_Header_" + sCOLUMN_NAME + "\" OnClick=\"LinkButton_Column_Header_Click\" CausesValidation=\"false\" CommandArgument=\"" + sCOLUMN_NAME + "\" runat=\"server\"><i id=\"iLinkButton_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\" class=\"fa fa-sort - desc\"></i></asp:LinkButton></th>" + "\n";
                                sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                sColumnHeaderCode_List += "\n";

                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo LinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.LinkButton LinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo iLinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.HtmlControls.HtmlGenericControl iLinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                sColumnData += "									<td class=\"record-data\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>" + "\n";

                                sColumnDataCode_List += "                \n";
                                if (String.IsNullOrEmpty(sREFERENCED_TABLE))
                                {
                                    sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                    sColumnDataCode_List += "                if (currentObj." + sCOLUMN_NAME + " != null)" + "\n";
                                    sColumnDataCode_List += "                   if (currentObj." + sCOLUMN_NAME + ".Equals(0))" + "\n";
                                    sColumnDataCode_List += "                       s" + sCOLUMN_NAME + " = \"No\";" + "\n";
                                    sColumnDataCode_List += "                   else" + "\n";
                                    sColumnDataCode_List += "                       s" + sCOLUMN_NAME + " = \"Sì\";" + "\n";
                                    sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                    sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                    sColumnDataCode_DeatilsInitPage += "                \n";
                                    sColumnDataCode_DeatilsInitPage += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsRadioButtonList_" + sCOLUMN_NAME + ".DataSourceValue = new String[] {\"1\", \"0\"};" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsRadioButtonList_" + sCOLUMN_NAME + ".DataSourceText = new String[] {\"Sì\", \"No\"};" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsRadioButtonList_" + sCOLUMN_NAME + ".Bind();" + "\n";

                                    sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                      CmsRadioButtonList_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ".ToString());" + "\n";

                                    sColumnDataCode_DeatilsSave += "                \n";
                                    sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsSave += "                if (objLibMath.isNumber(CmsRadioButtonList_" + sCOLUMN_NAME + ".Value))" + "\n";
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = short.Parse(CmsRadioButtonList_" + sCOLUMN_NAME + ".Value);" + "\n";
                                    sColumnDataCode_DeatilsSave += "                else" + "\n";
                                    if (sIS_NULLABLE.Equals("NO"))
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = 0;" + "\n";
                                    else
                                        sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = null;" + "\n";

                                    // Form Input
                                    if (sIS_NULLABLE.Equals("NO"))
                                        sColumnDataCode_InputHtml += "                                                <uc14:CmsRadioButtonList ID=\"CmsRadioButtonList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" Required=\"true\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                    else
                                        sColumnDataCode_InputHtml += "                                                <uc14:CmsRadioButtonList ID=\"CmsRadioButtonList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                    sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// Controllo CmsRadioButtonList_" + sCOLUMN_NAME + "." + "\n";
                                    sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                    sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                    sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsRadioButtonList CmsRadioButtonList_" + sCOLUMN_NAME + ";" + "\n";
                                }
                                else
                                {
                                    sColumnDataCode_List += "                // " + sCOLUMN_NAME + " (Related " + sREFERENCED_TABLE + ")\n";
                                    sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                    sColumnDataCode_List += "                if (currentObj." + sREFERENCED_TABLE + " != null)" + "\n";
                                    sColumnDataCode_List += "                    s" + sCOLUMN_NAME + " = currentObj." + sREFERENCED_TABLE + ".Title;" + "\n";
                                    sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                    sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                    //
                                    sColumnDataCode_DeatilsInitPage += "            \n";
                                    sColumnDataCode_DeatilsInitPage += "            // Binding CmsDropDownList_" + sREFERENCED_TABLE + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE + "ListOptions rq" + sREFERENCED_TABLE + "ListOptions = new " + sREFERENCED_TABLE + "ListOptions();" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.sortBy = \"Title\";" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.sortAscending = true;" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE + "ListOptions.uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            \n";
                                    sColumnDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE + "ListResponse rs" + sREFERENCED_TABLE + "ListResponse = objCmsBoDataLibs." + sREFERENCED_TABLE + "ListCms(header, rq" + sREFERENCED_TABLE + "ListOptions);" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            if (rs" + sREFERENCED_TABLE + "ListResponse != null)" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            {" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataSource = rs" + sREFERENCED_TABLE + "ListResponse.items;" + "\n";
                                    //sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataTextField = \"Descrizione\";" + "\n";
                                    //sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".DataValueField = \"Uid\";" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "                CmsDropDownList_" + sCOLUMN_NAME + ".Bind();" + "\n";
                                    sColumnDataCode_DeatilsInitPage += "            }" + "\n";

                                    sColumnDataCode_DeatilsInitUpdate += "                \n";
                                    sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                    sColumnDataCode_DeatilsInitUpdate += "                  CmsDropDownList_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ".ToString());" + "\n";

                                    // Form Input
                                    if (sIS_NULLABLE.Equals("NO"))
                                        sColumnDataCode_InputHtml += "                                                <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                    else
                                        sColumnDataCode_InputHtml += "                                                <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                    sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// Controllo CmsDropDownList_" + sCOLUMN_NAME + "." + "\n";
                                    sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                    sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                    sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                    sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsDropDownList CmsDropDownList_" + sCOLUMN_NAME + ";" + "\n";
                                }

                                #endregion
                            }
                            else if (sDATA_TYPE.ToLower().Equals("datetime"))
                            {
                                #region datetime
                                sColumnHeader += "							<!--<th class=\"record-data-header\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal><asp:LinkButton ID=\"LinkButton_Column_Header_" + sCOLUMN_NAME + "\" OnClick=\"LinkButton_Column_Header_Click\" CausesValidation=\"false\" CommandArgument=\"" + sCOLUMN_NAME + "\" runat=\"server\"><i id=\"iLinkButton_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\" class=\"fa fa-sort - desc\"></i></asp:LinkButton></th>-->" + "\n";
                                sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                sColumnHeaderCode_List += "\n";

                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo LinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.LinkButton LinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";
                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo iLinkButton_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.HtmlControls.HtmlGenericControl iLinkButton_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                sColumnData += "									<!--<td class=\"record-data\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>-->" + "\n";

                                sColumnDataCode_List += "                \n";
                                sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_List += "                DateTime d" + sCOLUMN_NAME + " = DateTime.MinValue;" + "\n";
                                sColumnDataCode_List += "                if (currentObj." + sCOLUMN_NAME + " != null)" + "\n";
                                sColumnDataCode_List += "                    d" + sCOLUMN_NAME + " = (DateTime)currentObj." + sCOLUMN_NAME + ";" + "\n";
                                sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                sColumnDataCode_List += "                if (d" + sCOLUMN_NAME + " != DateTime.MinValue)" + "\n";
                                sColumnDataCode_List += "                   myLiteral_Column_" + sCOLUMN_NAME + ".Text = objLibDate.DateToString(d" + sCOLUMN_NAME + ", \"GMA\", \"/\");" + "\n";

                                sColumnDataCode_DeatilsInitUpdate += "                \n";
                                sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                sColumnDataCode_DeatilsInitUpdate += "                  CmsTextBoxData_" + sCOLUMN_NAME + ".SetValue((DateTime)updateObject." + sCOLUMN_NAME + ");" + "\n";

                                sColumnDataCode_DeatilsSave += "                \n";
                                sColumnDataCode_DeatilsSave += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_DeatilsSave += "                if (!CmsTextBoxData_" + sCOLUMN_NAME + ".Value.Equals(DateTime.MinValue))" + "\n";
                                sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = CmsTextBoxData_" + sCOLUMN_NAME + ".Value;" + "\n";
                                sColumnDataCode_DeatilsSave += "                else" + "\n";
                                if (sIS_NULLABLE.Equals("NO"))
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = DateTime.MinValue;" + "\n";
                                else
                                    sColumnDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME + " = null;" + "\n";

                                // Form Input
                                if (sIS_NULLABLE.Equals("NO"))
                                    sColumnDataCode_InputHtml += "                                                <uc5:CmsTextBoxData ID=\"CmsTextBoxData_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" Required=\"true\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                else
                                    sColumnDataCode_InputHtml += "                                                <uc5:CmsTextBoxData ID=\"CmsTextBoxData_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                sColumnDataCode_Designer += "        /// Controllo CmsTextBoxData_" + sCOLUMN_NAME + "." + "\n";
                                sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsTextBoxData CmsTextBoxData_" + sCOLUMN_NAME + ";" + "\n";
                                #endregion
                            }
                            else
                            {
                                #region other type
                                sColumnHeader += "							<!--<th class=\"record-data-header\"><asp:Literal ID=\"Literal_Column_Header_" + sCOLUMN_NAME + "\" runat=\"server\">" + sCOLUMN_NAME + "</asp:Literal></th>-->" + "\n";
                                sColumnHeaderCode_List += "            //Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + ";\n";
                                sColumnHeaderCode_List += "            cmsLabelKey = \"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\";\n";
                                sColumnHeaderCode_List += "            headerTitle = currentCmsUserSession.GetGlobalLabel(cmsLabelKey);\n";
                                sColumnHeaderCode_List += "            if (String.IsNullOrEmpty(headerTitle))\n";
                                sColumnHeaderCode_List += "                headerTitle = cmsLabelKey.Substring(cmsLabelKey.LastIndexOf(\".\") + 1);\n";
                                sColumnHeaderCode_List += "            Literal_Column_Header_" + sCOLUMN_NAME + ".Text = headerTitle;\n";
                                sColumnHeaderCode_List += "\n";

                                sColumnHeaderCode_Designer += "\n";
                                sColumnHeaderCode_Designer += "        /// <summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Controllo Literal_Column_Header_" + sCOLUMN_NAME + "." + "\n";
                                sColumnHeaderCode_Designer += "        /// </summary>" + "\n";
                                sColumnHeaderCode_Designer += "        /// <remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        /// Campo generato automaticamente." + "\n";
                                sColumnHeaderCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnHeaderCode_Designer += "        /// </remarks>" + "\n";
                                sColumnHeaderCode_Designer += "        protected global::System.Web.UI.WebControls.Literal Literal_Column_Header_" + sCOLUMN_NAME + ";" + "\n";

                                sColumnData += "									<!--<td class=\"record-data\"><asp:Literal ID=\"Literal_Column_" + sCOLUMN_NAME + "\" runat=\"server\"></asp:Literal></td>-->" + "\n";

                                sColumnDataCode_List += "                \n";
                                sColumnDataCode_List += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_List += "                String s" + sCOLUMN_NAME + " = String.Empty;" + "\n";
                                sColumnDataCode_List += "                if (currentObj." + sCOLUMN_NAME + " != null)" + "\n";
                                sColumnDataCode_List += "                    s" + sCOLUMN_NAME + " = currentObj." + sCOLUMN_NAME + ".ToString();" + "\n";
                                sColumnDataCode_List += "                Literal myLiteral_Column_" + sCOLUMN_NAME + " = (Literal)e.Item.FindControl(\"Literal_Column_" + sCOLUMN_NAME + "\");" + "\n";
                                sColumnDataCode_List += "                myLiteral_Column_" + sCOLUMN_NAME + ".Text = s" + sCOLUMN_NAME + ";" + "\n";

                                sColumnDataCode_DeatilsInitUpdate += "                \n";
                                sColumnDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME + "\n";
                                sColumnDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME + " != null)" + "\n";
                                sColumnDataCode_DeatilsInitUpdate += "                  CmsTextBox_" + sCOLUMN_NAME + ".SetValue(updateObject." + sCOLUMN_NAME + ".ToString());" + "\n";

                                // Form Input
                                if (sIS_NULLABLE.Equals("NO"))
                                    sColumnDataCode_InputHtml += "                                                <uc4:CmsTextBox ID=\"CmsTextBox_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" Required=\"true\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";
                                else
                                    sColumnDataCode_InputHtml += "                                                <uc4:CmsTextBox ID=\"CmsTextBox_" + sCOLUMN_NAME + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME)) + "\" runat=\"server\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH.ToString() + "\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME + "." + sCOLUMN_NAME + "\" />" + "\n";

                                sColumnDataCode_Designer += "        /// <summary>" + "\n";
                                sColumnDataCode_Designer += "        /// Controllo CmsTextBox_" + sCOLUMN_NAME + "." + "\n";
                                sColumnDataCode_Designer += "        /// </summary>" + "\n";
                                sColumnDataCode_Designer += "        /// <remarks>" + "\n";
                                sColumnDataCode_Designer += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                sColumnDataCode_Designer += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                sColumnDataCode_Designer += "        /// </remarks>" + "\n";
                                sColumnDataCode_Designer += "        protected global::backOffice.ucControls.CmsTextBox CmsTextBox_" + sCOLUMN_NAME + ";" + "\n";
                                #endregion
                            }
                        }
                        else
                        {
                            if (sCOLUMN_NAME.ToLower().Equals("ord"))
                            {
                                sColumnDataCode_SupportPage += "                        if (Section.Equals(\"" + sTABLE_NAME + "\"))" + "\n";
                                sColumnDataCode_SupportPage += "                        {" + "\n";
                                sColumnDataCode_SupportPage += "                            " + sTABLE_NAME + "ItemResponse rsp = objCmsBoDataLibs." + sTABLE_NAME + "SetOrder(header, Uid, RefUid, nAction);" + "\n";
                                sColumnDataCode_SupportPage += "                            bReturn = rsp.Success;" + "\n";
                                sColumnDataCode_SupportPage += "                        }" + "\n";
                            }
                        }
                    }

                    //
                    String sTableRelatedDataCode_Js = "";
                    String sTableRelatedDataCode_BindJs = "";
                    String sTableRelatedDataCode_Tmp = "";
                    String sTableRelatedDataCode_ListPage = "";
                    String sTableRelatedDataCode_DeatilsPage = "";
                    String sTableRelatedDataCode_DeatilsDesigner = "";
                    String sTableRelatedDataCode_DeatilsInitPage = "";
                    String sTableRelatedDataCode_DeatilsInitUpdate_All = "";
                    String sTableRelatedDataCode_DeatilsSave_All = "";
                    String sTableRelatedDataCode_DeatilsHideAllRelated = "";
                    String sTableRelatedDataCode_DeatilsInitUpdateRelated = "";

                    String sTableRelatedDataCode_ActionPage = "";
                    String sTableRelatedDataCode_ActionDesigner = "";
                    String sTableRelatedDataCode_ActionInitPage = "";
                    String sColumnDataCode_ActionInitUpdate_LoadRelated = "";
                    String sColumnDataCode_ActionInitUpdate_DeleteRelated = "";

                    // Controllo le tabelle correlate
                    String sSqlFKRel = "SELECT OBJECT_NAME(fk.parent_object_id) 'ParentTable', c1.name 'ParentColumn', OBJECT_NAME(fk.referenced_object_id) 'ReferencedTable', c2.name 'ReferencedColumn' FROM sys.foreign_keys fk INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id INNER JOIN sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id INNER JOIN sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id WHERE OBJECT_NAME(fk.referenced_object_id) = '" + sContentTable + "' ";
                    SimpleDataSet dsTableFKRel = dp.executeQuery(sSqlFKRel);
                    for (int y = 0; y < dsTableFKRel.Table.Rows.Count; y++)
                    {
                        //
                        String sTableRelatedDataCode_DeatilsInitUpdate = "";
                        String sTableRelatedDataCode_DeatilsSave = "";
                        String sContentTableRelated = dsTableFKRel.Table.Rows[y]["ParentTable"].ToString();


                        //
                        Boolean bFieldOrdRelated = false;
                        Boolean bFieldImageRelated = false;

                        // Elenco Campi della tabella Correlata
                        String sSqlTableRelated = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + objLibString.sQuote(sContentTableRelated) + " ORDER BY ORDINAL_POSITION";
                        SimpleDataSet dsTableRelatedColumns = dp.executeQuery(sSqlTableRelated);

                        // Verifico i campi che ci sono nella tabella
                        for (int z = 0; z < dsTableRelatedColumns.Table.Rows.Count; z++)
                        {
                            if (dsTableRelatedColumns.Rows[z]["COLUMN_NAME"].ToString().ToLower().Equals("imageurl"))
                            {
                                bFieldImageRelated = true;
                            }

                            if (dsTableRelatedColumns.Rows[z]["COLUMN_NAME"].ToString().ToLower().Equals("ord"))
                            {
                                bFieldOrdRelated = true;
                            }
                        }


                        // Html Related
                        sTableRelatedDataCode_DeatilsPage += "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                        		    <!-- " + sContentTableRelated + " List -->" + "\n";

                        sTableRelatedDataCode_BindJs += "        function Panel_Related_" + sContentTableRelated + "_List_Bind() {" + "\n";
                        sTableRelatedDataCode_BindJs += "            var sData = '<%=getDataActionUrl(Uid, \"" + sContentTableRelated + "\") %>';" + "\n";
                        sTableRelatedDataCode_BindJs += "            $.ajax({" + "\n";
                        sTableRelatedDataCode_BindJs += "                url: \"action.aspx\"," + "\n";
                        sTableRelatedDataCode_BindJs += "                type: 'POST'," + "\n";
                        sTableRelatedDataCode_BindJs += "                data: sData," + "\n";
                        sTableRelatedDataCode_BindJs += "                success: function (data) {" + "\n";
                        sTableRelatedDataCode_BindJs += "                    $('#Panel_Related_" + sContentTableRelated + "_List').html(data);" + "\n";
                        sTableRelatedDataCode_BindJs += "                    InitAction('Panel_Related_" + sContentTableRelated + "_List');" + "\n";
                        sTableRelatedDataCode_BindJs += "                }" + "\n";
                        sTableRelatedDataCode_BindJs += "            });" + "\n";
                        sTableRelatedDataCode_BindJs += "        }" + "\n";

                        sTableRelatedDataCode_Js += "                setTimeout(\"Panel_Related_" + sContentTableRelated + "_List_Bind()\", 100);" + "\n";

                        sTableRelatedDataCode_Tmp = "                                                    <asp:Panel ID=\"Panel_Related_" + sContentTableRelated + "_List\" CssClass=\"widget\" runat=\"server\" Visible=\"false\" EnableViewState=\"false\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                      <div class=\"widget-header bordered-bottom bordered-themesecondary\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                          <span class=\"widget-caption\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                              " + sContentTableRelated + "</span>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                      </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                      <div class=\"widget-body\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                          <div class=\"with-footer\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                              <div class=\"btn-group\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                  <div class=\"dropdown\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                      <asp:HyperLink ID=\"HyperLink_" + sContentTableRelated + "_Action\" NavigateUrl=\"javascript:;\" runat=\"server\" CssClass=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">Action Tools<i class=\"fa fa-sort-desc\"></i></asp:HyperLink>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                      <ul class=\"dropdown-menu dropdown-default\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                          <li>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                              <asp:HyperLink ID=\"HyperLink_" + sContentTableRelated + "_Action_Add\" runat=\"server\"><i class=\"dropdown-icon fa fa-plus\"></i>Add " + sContentTableRelated + "</asp:HyperLink></li>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                          <li class=\"divider\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                              </li>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                          <li>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                              <asp:HyperLink ID=\"HyperLink_" + sContentTableRelated + "_Action_Delete\" CssClass=\"lnkActionList\" data-action=\"deleterelated\" data-url=\"action.aspx\"  runat=\"server\"><i class=\"dropdown-icon fa fa-trash-o\"></i>Delete</asp:HyperLink></li>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                      </ul>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                  </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                              </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                              <div class=\"btn-group pull-right\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                <asp:HyperLink ID=\"HyperLink_" + sContentTableRelated + "_ReorderRecord\" runat=\"server\" CssClass=\"btn btn-default lnkReorder\" CausesValidation=\"false\" Visible=\"{{bOrderRelated}}\" NavigateUrl=\"javascript:enableSortable('Panel_Related_" + sContentTableRelated + "_List');\" ><i class='fa fa-sort'></i> Reorder record</asp:HyperLink>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                <asp:HyperLink ID=\"HyperLink_" + sContentTableRelated + "_ApplyReorder\" runat=\"server\" CssClass=\"btn btn-default lnkApply\" CausesValidation=\"false\" Visible=\"{{bOrderRelated}}\" NavigateUrl=\"javascript:disableSortable('Panel_Related_" + sContentTableRelated + "_List');\" ><i class='fa fa-check'></i> Apply order</asp:HyperLink>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                              </div>" + "\n";
                        if (bFieldImageRelated)
                            sTableRelatedDataCode_Tmp += "                                                              <div class=\"card-list tmp-card-stripe listsortable\" contenttable=\"" + sContentTableRelated + "\">" + "\n";
                        else
                            sTableRelatedDataCode_Tmp += "                                                              <div class=\"card-list card-stripe listsortable\" contenttable=\"" + sContentTableRelated + "\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                  <asp:Repeater ID=\"Repeater_" + sContentTableRelated + "\" runat=\"server\" EnableViewState=\"false\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                      <ItemTemplate>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                          <asp:Panel ID=\"Panel_Card\" runat=\"server\" CssClass=\"card card-inverse\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                              <div class=\"card-block\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                  <div class=\"checkbox\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                      <label>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                          <input type=\"checkbox\" ID=\"" + sContentTableRelated + "_checkboxSel\" Visible=\"true\" runat=\"server\" class=\"lnkCheckbox\" />" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                              <span class=\"text\"></span></label>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                  </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                  <h3 class=\"card-title\"><asp:Literal ID=\"Literal_Title\" runat=\"server\"></asp:Literal></h3>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                  <div class=\"btn-group\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                      <asp:HyperLink ID=\"HyperLink_Update\" CssClass=\"lnkAction btn btn-default btn-sm shiny icon-only\" runat=\"server\" ToolTip=\"Update\" NavigateUrl=\"javascript:;\"><i class=\"fa fa-edit\"></i></asp:HyperLink>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                      <asp:HyperLink ID=\"HyperLink_Delete\" CssClass=\"lnkAction btn btn-default btn-sm shiny icon-only\" runat=\"server\" ToolTip=\"Delete\" NavigateUrl=\"javascript:;\"><i class=\"fa fa-trash-o\"></i></asp:HyperLink>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                                  </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                              </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                          </asp:Panel>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                      </ItemTemplate>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                                  </asp:Repeater>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                              </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                          </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                          <div class=\"footer\">" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                          </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                      </div>" + "\n";
                        sTableRelatedDataCode_Tmp += "                                                  </asp:Panel>" + "\n";

                        sTableRelatedDataCode_ActionPage += sTableRelatedDataCode_Tmp;

                        sTableRelatedDataCode_Tmp = "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo Repeater_" + sContentTableRelated + "." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.Repeater Repeater_" + sContentTableRelated + ";" + "\n";

                        sTableRelatedDataCode_Tmp += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo HyperLink_" + sContentTableRelated + "_Action." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.HyperLink HyperLink_" + sContentTableRelated + "_Action;" + "\n";

                        sTableRelatedDataCode_Tmp += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo HyperLink_" + sContentTableRelated + "_Action_Add." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.HyperLink HyperLink_" + sContentTableRelated + "_Action_Add;" + "\n";

                        sTableRelatedDataCode_Tmp += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo HyperLink_" + sContentTableRelated + "_Action_Delete." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.HyperLink HyperLink_" + sContentTableRelated + "_Action_Delete;" + "\n";

                        sTableRelatedDataCode_Tmp += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo Panel_Related_" + sContentTableRelated + "_List." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.Panel Panel_Related_" + sContentTableRelated + "_List;" + "\n";

                        sTableRelatedDataCode_Tmp += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo HyperLink_" + sContentTableRelated + "_ReorderRecord." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.HyperLink HyperLink_" + sContentTableRelated + "_ReorderRecord ;" + "\n";

                        sTableRelatedDataCode_Tmp += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Controllo HyperLink_" + sContentTableRelated + "_ApplyReorder." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_Tmp += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_Tmp += "        protected global::System.Web.UI.WebControls.HyperLink HyperLink_" + sContentTableRelated + "_ApplyReorder;" + "\n";

                        sTableRelatedDataCode_ActionDesigner += sTableRelatedDataCode_Tmp;

                        sTableRelatedDataCode_DeatilsPage += "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                    <asp:Panel ID=\"Panel_Related_" + sContentTableRelated + "_Form\" CssClass=\"widget\" runat=\"server\" DefaultButton=\"Button_Related_" + sContentTableRelated + "_Save\" Visible=\"false\">" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <div class=\"widget-header bordered-bottom bordered-themesecondary\">" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                          <span class=\"widget-caption\">" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                            " + sContentTableRelated + "</span>" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                          <span class=\"widget-caption-right\">";
                        sTableRelatedDataCode_DeatilsPage += "                                                              <asp:Literal ID=\"Literal_Cms_" + sContentTableRelated + "_LastCorrection\" runat=\"server\" Visible=\"false\">Last Update</asp:Literal>:&nbsp;<asp:Literal ID=\"Literal_" + sContentTableRelated + "_LastModify\" runat=\"server\"></asp:Literal> | <i runat=\"server\" id=\"iconStatusFlag_" + sContentTableRelated + "\"></i></span>";
                        sTableRelatedDataCode_DeatilsPage += "                                                        </div>" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <asp:Panel ID=\"Panel_Field_" + sContentTableRelated + "\" runat=\"server\" CssClass=\"widget-body\" EnableViewState=\"false\">" + "\n";

                        for (int z = 0; z < dsTableRelatedColumns.Table.Rows.Count; z++)
                        {
                            //
                            String sTABLE_NAME_RELATED = dsTableRelatedColumns.Rows[z]["TABLE_NAME"].ToString();
                            String sCOLUMN_NAME_RELATED = dsTableRelatedColumns.Rows[z]["COLUMN_NAME"].ToString();
                            String sIS_NULLABLE_RELATED = dsTableRelatedColumns.Rows[z]["IS_NULLABLE"].ToString();
                            String sDATA_TYPE_RELATED = dsTableRelatedColumns.Rows[z]["DATA_TYPE"].ToString();
                            String sCHARACTER_MAXIMUN_LENGTH_RELATED = dsTableRelatedColumns.Rows[z]["CHARACTER_MAXIMUM_LENGTH"].ToString();
                            Int32 nCHARACTER_MAXIMUN_LENGTH_RELATED = 0;

                            String sSqlFK = "SELECT OBJECT_NAME(fk.parent_object_id) 'ParentTable', c1.name 'ParentColumn', OBJECT_NAME(fk.referenced_object_id) 'ReferencedTable', c2.name 'ReferencedColumn' FROM sys.foreign_keys fk INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id INNER JOIN sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id INNER JOIN sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id WHERE OBJECT_NAME(fk.parent_object_id) = '" + sTABLE_NAME_RELATED + "' AND c1.name = '" + sCOLUMN_NAME_RELATED + "' ";
                            SimpleDataSet dsTableFK = dp.executeQuery(sSqlFK);

                            String sREFERENCED_TABLE_RELATED = String.Empty;
                            String sREFERENCED_COLUMN_RELATED = String.Empty;
                            if (dsTableFK != null && (dsTableFK.Table.Rows.Count > 0))
                            {
                                sREFERENCED_TABLE_RELATED = dsTableFK.Table.Rows[0]["ReferencedTable"].ToString();
                                sREFERENCED_COLUMN_RELATED = dsTableFK.Table.Rows[0]["ReferencedColumn"].ToString();
                            }

                            if ((!sCOLUMN_NAME_RELATED.ToLower().Equals("uid")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("creationdate")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_creationuser"))
                                && (!sCOLUMN_NAME_RELATED.ToLower().Equals("updatedate")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_updateuser")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("ord"))
                                && (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_cmsnlscontext")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("statusflag"))
                                 && (!sCOLUMN_NAME_RELATED.ToLower().Equals("creationdate")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_cmsusers"))
                                 && (!sCOLUMN_NAME_RELATED.ToLower().Equals("updatedate")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_cmsusers_mod"))
                                 && (!sCOLUMN_NAME_RELATED.ToLower().Equals("lord")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("rord"))
                                  && (!sCOLUMN_NAME_RELATED.ToLower().Equals("versioncurrent")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("versionpublished"))
                                  && (!sCOLUMN_NAME_RELATED.ToLower().Equals("publishdate")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_publishuser"))
                                  //&& (!sCOLUMN_NAME_RELATED.ToLower().Equals("uid_from")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("import_id"))
                                  && (!sCOLUMN_NAME_RELATED.ToLower().Equals("linktitle")) && (!sCOLUMN_NAME_RELATED.ToLower().Equals("linktarget"))
                                  )
                            {
                                if (sDATA_TYPE_RELATED.ToLower().Equals("nvarchar"))
                                {
                                    #region nvarchar
                                    if (sCOLUMN_NAME_RELATED.ToLower().IndexOf("imageurl") >= 0)
                                    {
                                        #region ImgUrl
                                        if (sCOLUMN_NAME_RELATED.ToLower().IndexOf("prev") >= 0)
                                        {
                                            //
                                        }

                                        if (sCOLUMN_NAME_RELATED.ToLower().IndexOf("prev") < 0)
                                        {
                                            //
                                            //sColumnDataCode_DeatilsInitPage += "            " + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "            Boolean bCmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_HasCrop = System.Convert.ToBoolean(currentCmsUserSession.GetGlobalResources(\"" + sTABLE_NAME_RELATED + ".CmsUploadImageCrop_" + sCOLUMN_NAME_RELATED + ".HasCrop\", \"true\"));" + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "            if (bCmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_HasCrop)" + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "            {" + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "                int nCmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"" + sTABLE_NAME_RELATED + ".CmsUploadImageCrop_" + sCOLUMN_NAME_RELATED + ".Width\", \"1440\"));" + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "                int nCmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"" + sTABLE_NAME_RELATED + ".CmsUploadImageCrop_" + sCOLUMN_NAME_RELATED + ".Height\", \"260\"));" + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "                CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetCropArea(nCmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_Width, nCmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_Height);" + "\n";
                                            //sColumnDataCode_DeatilsInitPage += "            }" + "\n";

                                            //
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                // Image \n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                if (!String.IsNullOrEmpty(updateObject." + sCOLUMN_NAME_RELATED + "))" + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                {" + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".UploadDir = sUploadDir;" + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + "_Prev, updateObject." + sCOLUMN_NAME_RELATED + ");" + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                }" + "\n";


                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                #region " + sCOLUMN_NAME_RELATED + "" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                if (CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".HttpFileUrl.Equals(\"TODELETE\"))" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + " = \"\";" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + "_Prev = \"\";" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                else if (!String.IsNullOrEmpty(CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".HttpFileUrl))" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    // This code works, but for ASCII only" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    String url = CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".HttpFileUrl;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    if (CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".isCrop)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        #region Crop" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        String[] parameter = CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Parameter.Split('|');" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        if (parameter.Length.Equals(4))" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int coordx = Convert.ToInt16(parameter[0]);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int coordy = Convert.ToInt16(parameter[1]);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int coordW = Convert.ToInt16(parameter[2]);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int coordH = Convert.ToInt16(parameter[3]);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            IBinaryObject obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + " = BinaryManager.ReadFromUri(new Uri(url));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".cache();" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            if (obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                IBinaryObject CropImage = objLibImage.Crop(obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ", coordW, coordH, coordx, coordy);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                // " + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + ".Width\", \"1024\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + ".Height\", \"768\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                IBinaryObject bytes = objLibImage.ResizeImage(CropImage, n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Width, n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Height, false, false);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                CmsStorageFile objCmsStorageFile = new CmsStorageFile();" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                objCmsStorageFile.RelativePath = sUploadDir + obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".FullName;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                StorageFileItemResponse myResp = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFile, bytes.Buffer);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                if (myResp.Success)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                    updateObject." + sCOLUMN_NAME_RELATED + " = myResp.item.RelativePath;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                // " + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_Prev" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                CropImage.DataStream.Position = 0;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevWidth = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "_Prev.Width\", \"320\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevHeight = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "_Prev.Height\", \"0\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                IBinaryObject bytesThumb = objLibImage.ResizeImage(CropImage, n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevWidth, n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevHeight, false, false);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                CmsStorageFile objCmsStorageFileThumb = new CmsStorageFile();" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                objCmsStorageFileThumb.RelativePath = sUploadDir + \"th_\" + obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".FullName;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                StorageFileItemResponse myRespThumb = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFileThumb, bytesThumb.Buffer);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                if (myRespThumb.Success)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                    updateObject." + sCOLUMN_NAME_RELATED + "_Prev = myRespThumb.item.RelativePath;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        #endregion" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    else" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        #region Resize" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        IBinaryObject obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + " = BinaryManager.ReadFromUri(new Uri(url));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".cache();" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        if (obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            // " + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            //CropImage.DataStream.Position = 0;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Width = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + ".Width\", \"1024\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Height = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + ".Height\", \"768\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            IBinaryObject bytes = objLibImage.ResizeImage(obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ", n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Width, n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "Height, false, false);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            CmsStorageFile objCmsStorageFile = new CmsStorageFile();" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            objCmsStorageFile.RelativePath = sUploadDir + obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".FullName;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            StorageFileItemResponse myResp = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFile, bytes.Buffer);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            if (myResp.Success)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                updateObject." + sCOLUMN_NAME_RELATED + " = myResp.item.RelativePath;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            // " + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_Prev" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".DataStream.Position = 0;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevWidth = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "_Prev.Width\", \"320\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            int n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevHeight = System.Convert.ToInt32(currentCmsUserSession.GetGlobalResources(\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "_Prev.Height\", \"0\"));" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            IBinaryObject bytesThumb = objLibImage.ResizeImage(obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ", n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevWidth, n" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "_PrevHeight, false, false);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            CmsStorageFile objCmsStorageFileThumb = new CmsStorageFile();" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            objCmsStorageFileThumb.RelativePath = sUploadDir + \"th_\" + obj" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".FullName;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            StorageFileItemResponse myRespThumb = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFileThumb, bytesThumb.Buffer);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                            if (myRespThumb.Success)" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                                updateObject." + sCOLUMN_NAME_RELATED + "_Prev = myRespThumb.item.RelativePath;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                " + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        #endregion" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                else" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                {" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + " = CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".ImageUrl;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + "_Prev = CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".ImageUrl_Preview;" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                }" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "                #endregion" + "\n";

                                            // Form Input
                                            sTableRelatedDataCode_DeatilsPage += "                    <uc11:CmsUploadImageCrop ID=\"CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" runat=\"server\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                            sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsUploadImageCrop CmsUploadImageCrop_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ";" + "\n";

                                        }
                                        #endregion                                        
                                    }
                                    else if (sCOLUMN_NAME_RELATED.ToLower().Equals("linkurl"))
                                    {
                                        #region linkurl

                                        sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                if (!String.IsNullOrEmpty(updateObject." + sCOLUMN_NAME_RELATED + "))" + "\n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue(updateObject.LinkTitle, updateObject.LinkUrl, updateObject.LinkTarget);" + "\n";

                                        sTableRelatedDataCode_DeatilsSave += "                \n";
                                        sTableRelatedDataCode_DeatilsSave += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                        sTableRelatedDataCode_DeatilsSave += "                #region " + sCOLUMN_NAME_RELATED + "\n";
                                        sTableRelatedDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value_Url))\n";
                                        sTableRelatedDataCode_DeatilsSave += "                    updateObject.LinkUrl = CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value_Url;" + "\n";
                                        sTableRelatedDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value_Target))\n";
                                        sTableRelatedDataCode_DeatilsSave += "                    updateObject.LinkTarget = CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value_Target;" + "\n";
                                        sTableRelatedDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value_Title))\n";
                                        sTableRelatedDataCode_DeatilsSave += "                    updateObject.LinkTitle = CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value_Title;" + "\n";
                                        sTableRelatedDataCode_DeatilsSave += "                #endregion  \n";

                                        // sColumnDataCode_InputHtml Input
                                        sTableRelatedDataCode_DeatilsPage += "                    <uc6:CmsTextBoxLink ID=\"CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" runat=\"server\" ViewTitle=\"true\" ViewTarget=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                        // sColumnDataCode_Designer
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsTextBoxLink CmsTextBoxLink_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "; " + "\n";
                                        #endregion
                                    }
                                    else if (sCOLUMN_NAME_RELATED.ToLower().Equals("fileurl"))
                                    {
                                        #region FileUrl
                                        //
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                // Image \n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                if (!String.IsNullOrEmpty(updateObject." + sCOLUMN_NAME_RELATED + "))" + "\n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                {" + "\n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".UploadDir = sUploadDir;" + "\n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ");" + "\n";
                                        sTableRelatedDataCode_DeatilsInitUpdate += "                }" + "\n";

                                        sTableRelatedDataCode_DeatilsSave += "            #region " + sCOLUMN_NAME_RELATED + "\n";
                                        sTableRelatedDataCode_DeatilsSave += "            if (!String.IsNullOrEmpty(CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".HttpFileUrl))\n";
                                        sTableRelatedDataCode_DeatilsSave += "            {\n";
                                        sTableRelatedDataCode_DeatilsSave += "                // This code works, but for ASCII only\n";
                                        sTableRelatedDataCode_DeatilsSave += "                String url = CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".HttpFileUrl;\n";
                                        sTableRelatedDataCode_DeatilsSave += "                IBinaryObject objFileUrl = BinaryManager.ReadFromUri(new Uri(url));\n";
                                        sTableRelatedDataCode_DeatilsSave += "            \n";
                                        sTableRelatedDataCode_DeatilsSave += "                    if (objFileUrl != null)\n";
                                        sTableRelatedDataCode_DeatilsSave += "                    {\n";
                                        sTableRelatedDataCode_DeatilsSave += "                        CmsStorageFile objCmsStorageFile = new CmsStorageFile();\n";
                                        sTableRelatedDataCode_DeatilsSave += "                        objCmsStorageFile.RelativePath = sUploadDir + objFileUrl.FullName;\n";
                                        sTableRelatedDataCode_DeatilsSave += "                        StorageFileItemResponse myResp = objCmsBoDataLibs.StorageFileUpsert(header, objCmsStorageFile, objFileUrl.Buffer);\n";
                                        sTableRelatedDataCode_DeatilsSave += "                        if (myResp.Success)\n";
                                        sTableRelatedDataCode_DeatilsSave += "                            updateObject." + sCOLUMN_NAME_RELATED + " = myResp.item.RelativePath;\n";
                                        sTableRelatedDataCode_DeatilsSave += "                    }\n";
                                        sTableRelatedDataCode_DeatilsSave += "            }\n";
                                        sTableRelatedDataCode_DeatilsSave += "            else\n";
                                        sTableRelatedDataCode_DeatilsSave += "            {\n";
                                        sTableRelatedDataCode_DeatilsSave += "                updateObject.FileUrl = CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".FileUrl;\n";
                                        sTableRelatedDataCode_DeatilsSave += "            }\n";
                                        sTableRelatedDataCode_DeatilsSave += "            #endregion  \n";

                                        // Form Input
                                        sTableRelatedDataCode_DeatilsPage += "                    <uc8:CmsUploadFile ID=\"CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" runat=\"server\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsUploadFile CmsUploadFile_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ";" + "\n";
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Other Field
                                        //if ((!sCOLUMN_NAME_RELATED.Equals("Uid_CmsNlsContext")) && (!sCOLUMN_NAME_RELATED.Equals("StatusFlag")) && (!sCOLUMN_NAME_RELATED.Equals("Uid_" + sContentTable)))
                                        if ((!sCOLUMN_NAME_RELATED.Equals("StatusFlag")) && (!sCOLUMN_NAME_RELATED.Equals("Uid_" + sContentTable)))
                                        {
                                            if (String.IsNullOrEmpty(sREFERENCED_TABLE_RELATED))
                                            {
                                                if (!String.IsNullOrEmpty(sCHARACTER_MAXIMUN_LENGTH_RELATED))
                                                    nCHARACTER_MAXIMUN_LENGTH_RELATED = System.Convert.ToInt32(sCHARACTER_MAXIMUN_LENGTH_RELATED);

                                                sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                                sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                                sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                                sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ".ToString());" + "\n";

                                                sTableRelatedDataCode_DeatilsSave += "            \n";
                                                sTableRelatedDataCode_DeatilsSave += "            // " + sCOLUMN_NAME_RELATED + "\n";
                                                sTableRelatedDataCode_DeatilsSave += "            updateObject." + sCOLUMN_NAME_RELATED + " = CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value;" + "\n";

                                                if (nCHARACTER_MAXIMUN_LENGTH_RELATED > 255)
                                                {
                                                    if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                        sTableRelatedDataCode_DeatilsPage += "                    <uc4:CmsTextBox ID=\"CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" Required=\"true\" TextMode=\"MultiLine\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                                    else
                                                        sTableRelatedDataCode_DeatilsPage += "                    <uc4:CmsTextBox ID=\"CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" runat=\"server\" TextMode=\"MultiLine\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                                }
                                                else
                                                {
                                                    if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                        sTableRelatedDataCode_DeatilsPage += "                    <uc4:CmsTextBox ID=\"CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                                    else
                                                        sTableRelatedDataCode_DeatilsPage += "                    <uc4:CmsTextBox ID=\"CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                                }

                                                sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                                sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsTextBox CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ";" + "\n";
                                            }
                                            else
                                            {
                                                if (sREFERENCED_TABLE_RELATED.ToLower().Equals("model"))
                                                {
                                                    //
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sREFERENCED_TABLE_RELATED + "\n";
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ".ToString());" + "\n";

                                                    sTableRelatedDataCode_DeatilsSave += "                \n";
                                                    sTableRelatedDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Value))\n";
                                                    sTableRelatedDataCode_DeatilsSave += "                    updateObject.Uid_" + sREFERENCED_TABLE_RELATED + " = CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Value;\n";

                                                    // Form Input
                                                    if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                        sTableRelatedDataCode_DeatilsPage += "                    <ucCmsDropDownListModel:CmsDropDownListModel ID=\"CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                                    else
                                                        sTableRelatedDataCode_DeatilsPage += "                    <ucCmsDropDownListModel:CmsDropDownListModel ID=\"CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "." + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsDropDownListModel CmsDropDownListModel_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ";" + "\n";
                                                }
                                                else
                                                {
                                                    //
                                                    sColumnDataCode_DeatilsInitUpdate_LoadRelated += "            CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "_Bind();" + "\n";

                                                    //
                                                    sTableRelatedDataCode_DeatilsInitPage += "        protected void CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "_Bind()" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "        {" + "\n";

                                                    sTableRelatedDataCode_DeatilsInitPage += "            \n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            // Binding CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE_RELATED + "ListOptions rq" + sREFERENCED_TABLE_RELATED + "ListOptions = new " + sREFERENCED_TABLE_RELATED + "ListOptions();" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE_RELATED + "ListOptions.sortBy = \"Title\";" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE_RELATED + "ListOptions.sortAscending = true;" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE_RELATED + "ListOptions.uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            \n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE_RELATED + "ListResponse rs" + sREFERENCED_TABLE_RELATED + "ListResponse = objCmsBoDataLibs." + sREFERENCED_TABLE_RELATED + "ListCms(header, rq" + sREFERENCED_TABLE_RELATED + "ListOptions);" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            if (rs" + sREFERENCED_TABLE_RELATED + "ListResponse != null)" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            {" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".DataSource = rs" + sREFERENCED_TABLE_RELATED + "ListResponse.items;" + "\n";
                                                    //sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".DataTextField = \"Descrizione\";" + "\n";
                                                    //sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".DataValueField = \"Uid\";" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Bind();" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "            }" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitPage += "        }" + "\n";

                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sREFERENCED_TABLE_RELATED + "\n";
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                                    sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ".ToString());" + "\n";

                                                    sTableRelatedDataCode_DeatilsSave += "                \n";
                                                    sTableRelatedDataCode_DeatilsSave += "                if (!String.IsNullOrEmpty(CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Value))\n";
                                                    sTableRelatedDataCode_DeatilsSave += "                    updateObject.Uid_" + sREFERENCED_TABLE_RELATED + " = CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Value;\n";

                                                    // Form Input
                                                    if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                        sTableRelatedDataCode_DeatilsPage += "                    <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE_RELATED)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                                    else
                                                        sTableRelatedDataCode_DeatilsPage += "                    <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE_RELATED)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "." + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                                    sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsDropDownList CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ";" + "\n";
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else if (sDATA_TYPE_RELATED.ToLower().Equals("ntext"))
                                {
                                    #region ntext                                    
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsHtmlEditor_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ");" + "\n";

                                    sTableRelatedDataCode_DeatilsSave += "            \n";
                                    sTableRelatedDataCode_DeatilsSave += "            // " + sCOLUMN_NAME_RELATED + "\n";
                                    sTableRelatedDataCode_DeatilsSave += "            updateObject." + sCOLUMN_NAME_RELATED + " = CmsHtmlEditor_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value;" + "\n";

                                    sTableRelatedDataCode_DeatilsPage += "                    <uc3:CmsHtmlEditor ID=\"CmsHtmlEditor_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" showSpacer=\"true\" runat=\"server\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsHtmlEditor_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsHtmlEditor CmsHtmlEditor_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ";" + "\n";
                                    #endregion
                                }
                                else if ((sDATA_TYPE_RELATED.ToLower().Equals("int")) || (sDATA_TYPE_RELATED.ToLower().Equals("double")))
                                {
                                    #region int
                                    if ((!sCOLUMN_NAME_RELATED.Equals("Uid_CmsNlsContext")) && (!sCOLUMN_NAME_RELATED.Equals("StatusFlag")) && (!sCOLUMN_NAME_RELATED.Equals("Uid_" + sContentTable)))
                                    {
                                        if (String.IsNullOrEmpty(sREFERENCED_TABLE_RELATED))
                                        {

                                            sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                    CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ".ToString());" + "\n";

                                            sTableRelatedDataCode_DeatilsSave += "            \n";
                                            sTableRelatedDataCode_DeatilsSave += "            // " + sCOLUMN_NAME_RELATED + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "            if (objLibMath.isNumber(CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value))" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "              updateObject." + sCOLUMN_NAME_RELATED + " = System.Convert.ToInt32(CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value);" + "\n";
                                            sTableRelatedDataCode_DeatilsSave += "            else" + "\n";

                                            if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                sTableRelatedDataCode_DeatilsSave += "              updateObject." + sCOLUMN_NAME_RELATED + " = 0;" + "\n";
                                            else
                                                sTableRelatedDataCode_DeatilsSave += "              updateObject." + sCOLUMN_NAME_RELATED + " = null;" + "\n";


                                            // Form Input
                                            if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                sTableRelatedDataCode_DeatilsPage += "                    <uc4:CmsTextBox ID=\"CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" Required=\"true\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                            else
                                                sTableRelatedDataCode_DeatilsPage += "                    <uc4:CmsTextBox ID=\"CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" runat=\"server\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                            sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsTextBox CmsTextBox_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ";" + "\n";
                                        }
                                        else
                                        {
                                            //
                                            sColumnDataCode_DeatilsInitUpdate_LoadRelated += "            CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "_Bind();" + "\n";

                                            //
                                            sTableRelatedDataCode_DeatilsInitPage += "        protected void CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "_Bind()" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "        {" + "\n";

                                            sTableRelatedDataCode_DeatilsInitPage += "            \n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            // Binding CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE_RELATED + "ListOptions rq" + sREFERENCED_TABLE_RELATED + "ListOptions = new " + sREFERENCED_TABLE_RELATED + "ListOptions();" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE_RELATED + "ListOptions.sortBy = \"Title\";" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE_RELATED + "ListOptions.sortAscending = true;" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            rq" + sREFERENCED_TABLE_RELATED + "ListOptions.uid_CmsNlsContext = header.cmsCmsNlsContext.Uid;" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            \n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            " + sREFERENCED_TABLE_RELATED + "ListResponse rs" + sREFERENCED_TABLE_RELATED + "ListResponse = objCmsBoDataLibs." + sREFERENCED_TABLE_RELATED + "ListCms(header, rq" + sREFERENCED_TABLE_RELATED + "ListOptions);" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            if (rs" + sREFERENCED_TABLE_RELATED + "ListResponse != null)" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            {" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".DataSource = rs" + sREFERENCED_TABLE_RELATED + "ListResponse.items;" + "\n";
                                            //sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".DataTextField = \"Descrizione\";" + "\n";
                                            //sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".DataValueField = \"Uid\";" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Bind();" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "            }" + "\n";
                                            sTableRelatedDataCode_DeatilsInitPage += "        }" + "\n";

                                            sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                            sTableRelatedDataCode_DeatilsInitUpdate += "                CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".SetValue(updateObject." + sCOLUMN_NAME_RELATED + ".ToString());" + "\n";

                                            sTableRelatedDataCode_DeatilsSave += "                \n";
                                            sTableRelatedDataCode_DeatilsSave += "                if (objLibMath.isNumber(CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Value))\n";
                                            sTableRelatedDataCode_DeatilsSave += "                    updateObject.Uid_" + sREFERENCED_TABLE_RELATED + " = System.Convert.ToInt32(CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ".Value);\n";
                                            sTableRelatedDataCode_DeatilsSave += "                        else \n";
                                            sTableRelatedDataCode_DeatilsSave += "                    updateObject.Uid_" + sREFERENCED_TABLE_RELATED + " = null;\n";

                                            // Form Input
                                            if (sIS_NULLABLE_RELATED.Equals("NO"))
                                                sTableRelatedDataCode_DeatilsPage += "                    <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE_RELATED)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                            else
                                                sTableRelatedDataCode_DeatilsPage += "                    <uc2:CmsDropDownList ID=\"CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sREFERENCED_TABLE_RELATED)) + "\" DataTextField=\"Title\" DataValueField=\"Uid\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";

                                            sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + "." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                            sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsDropDownList CmsDropDownList_" + sTABLE_NAME_RELATED + "_" + sREFERENCED_TABLE_RELATED + "_" + sREFERENCED_COLUMN_RELATED + ";" + "\n";
                                        }
                                    }
                                    #endregion
                                }
                                else if (sDATA_TYPE_RELATED.ToLower().Equals("datetime"))
                                {
                                    #region datetime
                                    if (!String.IsNullOrEmpty(sCHARACTER_MAXIMUN_LENGTH_RELATED))
                                        nCHARACTER_MAXIMUN_LENGTH_RELATED = System.Convert.ToInt32(sCHARACTER_MAXIMUN_LENGTH_RELATED);

                                    sTableRelatedDataCode_DeatilsInitUpdate += "                \n";
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                if (updateObject." + sCOLUMN_NAME_RELATED + " != null)" + "\n";
                                    sTableRelatedDataCode_DeatilsInitUpdate += "                  CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".SetValue((DateTime)updateObject." + sCOLUMN_NAME_RELATED + ");" + "\n";

                                    sTableRelatedDataCode_DeatilsSave += "                \n";
                                    sTableRelatedDataCode_DeatilsSave += "                // " + sCOLUMN_NAME_RELATED + "\n";
                                    sTableRelatedDataCode_DeatilsSave += "                if (!CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value.Equals(DateTime.MinValue))" + "\n";
                                    sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + " = CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ".Value;" + "\n";
                                    sTableRelatedDataCode_DeatilsSave += "                else" + "\n";
                                    if (sIS_NULLABLE_RELATED.Equals("NO"))
                                        sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + " = DateTime.MinValue;" + "\n";
                                    else
                                        sTableRelatedDataCode_DeatilsSave += "                    updateObject." + sCOLUMN_NAME_RELATED + " = null;" + "\n";


                                    if (nCHARACTER_MAXIMUN_LENGTH_RELATED > 255)
                                    {
                                        if (sIS_NULLABLE_RELATED.Equals("NO"))
                                            sTableRelatedDataCode_DeatilsPage += "                    <uc5:CmsTextBoxData ID=\"CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" Required=\"true\" TextMode=\"MultiLine\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                        else
                                            sTableRelatedDataCode_DeatilsPage += "                    <uc5:CmsTextBoxData ID=\"CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" runat=\"server\" TextMode=\"MultiLine\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                    }
                                    else
                                    {
                                        if (sIS_NULLABLE_RELATED.Equals("NO"))
                                            sTableRelatedDataCode_DeatilsPage += "                    <uc5:CmsTextBoxData ID=\"CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" Required=\"true\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                        else
                                            sTableRelatedDataCode_DeatilsPage += "                    <uc5:CmsTextBoxData ID=\"CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "\" Title=\"" + ClearColumnName(objLibString.ProperCase(sCOLUMN_NAME_RELATED)) + "\" MaxLength=\"" + nCHARACTER_MAXIMUN_LENGTH_RELATED.ToString() + "\" runat=\"server\" showSpacer=\"true\" cmsLabelKey=\"Cms." + sTABLE_NAME_RELATED + "." + sCOLUMN_NAME_RELATED + "\" />" + "\n";
                                    }

                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + "." + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                                    sTableRelatedDataCode_DeatilsDesigner += "        protected global::backOffice.ucControls.CmsTextBoxData CmsTextBoxData_" + sTABLE_NAME_RELATED + "_" + sCOLUMN_NAME_RELATED + ";" + "\n";

                                    #endregion
                                }
                            }
                        }

                        sTableRelatedDataCode_DeatilsPage += "                                                        <asp:HiddenField ID=\"HiddenField_" + sContentTableRelated + "_NextUid\" ClientIDMode=\"Static\" runat=\"server\" />" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <br />" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <br />" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <asp:Button CssClass=\"btn btn-success\" ID=\"Button_Related_" + sContentTableRelated + "_Save\" Text=\"Save\" ValidationGroup=\"" + sContentTableRelated + "\" runat=\"server\" OnClick=\"Button_Related_" + sContentTableRelated + "_Save_Click\" />" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <asp:Button CssClass=\"btn btn-success disabled\" ID=\"Button_Related_" + sContentTableRelated + "_SaveAndNext\" Text=\"Save and continue\" ValidationGroup=\"" + sContentTableRelated + "\" runat=\"server\" OnClick=\"Button_Related_" + sContentTableRelated + "_SaveAndNext_Click\" Visible=\"false\" />" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                        <asp:Button CssClass=\"btn btn-danger\" ID=\"Button_Related_" + sContentTableRelated + "_Cancel\" Text=\"Cancel\" CausesValidation=\"false\" runat=\"server\" OnClick=\"Button_Related_" + sContentTableRelated + "_Cancel_Click\" />" + "\n";

                        sTableRelatedDataCode_DeatilsPage += "                                                    </asp:Panel>" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "                                                </asp:Panel>" + "\n";
                        sTableRelatedDataCode_DeatilsPage += "\n";

                        sTableRelatedDataCode_ListPage += "                                                   <asp:Panel ID=\"Panel_Related_" + sContentTableRelated + "_List\" CssClass=\"widget\" runat=\"server\" Visible=\"true\" EnableViewState=\"false\">" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                     <div class=\"widget-header bordered-bottom bordered-themesecondary\">" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                         <span class=\"widget-caption\">" + sContentTableRelated + "</span>" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                     </div>" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                     <div class=\"widget-body\">" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                             <div class=\"related-loading-container\">" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                                 <div class=\"loader\"></div> " + "\n";
                        sTableRelatedDataCode_ListPage += "                                                             </div>" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                     </div>" + "\n";
                        sTableRelatedDataCode_ListPage += "                                                   </asp:Panel>" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Panel" + sContentTableRelated + "_List." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Panel Panel_Related_" + sContentTableRelated + "_List;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Panel_Related_" + sContentTableRelated + "_Form." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Panel Panel_Related_" + sContentTableRelated + "_Form;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Panel_Field_" + sContentTableRelated + "." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Panel Panel_Field_" + sContentTableRelated + ";" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Button_Related_" + sContentTableRelated + "_Cancel." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Button Button_Related_" + sContentTableRelated + "_Cancel;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Button_Related_" + sContentTableRelated + "_Save." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Button Button_Related_" + sContentTableRelated + "_Save;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Button_Related_" + sContentTableRelated + "_SaveAndNext." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Button Button_Related_" + sContentTableRelated + "_SaveAndNext;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo HiddenField_" + sContentTableRelated + "_NextUid." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.HiddenField HiddenField_" + sContentTableRelated + "_NextUid;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Literal_" + sContentTableRelated + "_LastModify." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Literal Literal_" + sContentTableRelated + "_LastModify;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo Literal_Cms_" + sContentTableRelated + "_LastCorrection." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente dal cmsGenerator." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.WebControls.Literal Literal_Cms_" + sContentTableRelated + "_LastCorrection;" + "\n";

                        sTableRelatedDataCode_DeatilsDesigner += "        /// <summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Controllo iconStatusFlag_" + sContentTableRelated + "." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </summary>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// <remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Campo generato automaticamente." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind." + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        /// </remarks>" + "\n";
                        sTableRelatedDataCode_DeatilsDesigner += "        protected global::System.Web.UI.HtmlControls.HtmlGenericControl iconStatusFlag_" + sContentTableRelated + "; " + "\n";


                        //
                        sTableRelatedDataCode_DeatilsHideAllRelated += "            Panel_Related_" + sContentTableRelated + "_List.Visible = false;\n";
                        sTableRelatedDataCode_DeatilsHideAllRelated += "            Panel_Related_" + sContentTableRelated + "_Form.Visible = false;\n";

                        //
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            //\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            if (contentTableRelated.Equals(\"" + sContentTableRelated + "\"))\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            {\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                 //\n";

                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.Button_Related_Salva.Text\")))" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                Button_Related_" + sContentTableRelated + "_Save.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.Button_Related_Salva.Text\");" + "\n";

                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.Button_Related_SalvaContinua.Text\")))" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                Button_Related_" + sContentTableRelated + "_SaveAndNext.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.Button_Related_SalvaContinua.Text\");" + "\n";

                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.Button_Related_Annulla.Text\")))" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                Button_Related_" + sContentTableRelated + "_Cancel.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.Button_Related_Annulla.Text\");" + "\n";


                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                 Panel_Related_" + sContentTableRelated + "_Form.Visible = true;\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                 Panel_Related_" + sContentTableRelated + "_List.Visible = false;\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                 if (!String.IsNullOrEmpty(UidRelated))\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "                     InitUpdate_" + sContentTableRelated + "(UidRelated);\n";
                        sTableRelatedDataCode_DeatilsInitUpdateRelated += "            }\n";

                        //
                        sTableRelatedDataCode_ActionInitPage += "        protected void Repeater_" + sContentTableRelated + "_Bind()" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "        {" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            Panel_Related_" + sContentTableRelated + "_List.Visible = true;" + "\n";

                        sTableRelatedDataCode_ActionInitPage += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Action.Text\")))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "               HyperLink_" + sContentTableRelated + "_Action.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Action.Text\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            HyperLink_" + sContentTableRelated + "_Action_Add.NavigateUrl = getCurrentDetailUrl(Uid.ToString()) + \"&ContentTableRelated=" + sContentTableRelated + "\";" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Related_Action_Add.Text\")))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "               HyperLink_" + sContentTableRelated + "_Action_Add.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Related_Action_Add.Text\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Related_Action_Delete.Text\")))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "               HyperLink_" + sContentTableRelated + "_Action_Delete.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Related_Action_Delete.Text\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.Button_ReorderRecord.Text\")))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "               HyperLink_" + sContentTableRelated + "_ReorderRecord.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.Button_ReorderRecord.Text\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.Button_ApplyReorder.Text\")))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "               HyperLink_" + sContentTableRelated + "_ApplyReorder.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.Button_ApplyReorder.Text\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            // Filtri per elenco" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            " + sContentTableRelated + "ListOptions rq = new " + sContentTableRelated + "ListOptions();" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            rq.uid_" + sContentTable + " = System.Convert.ToInt32(Uid);" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            Boolean bOrderRelated =  {{bOrderRelated}};" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (bOrderRelated)" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            {" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                rq.sortBy = \"Ord\";" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                rq.sortAscending = true;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                // Enable Sort" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                Panel_Related_" + sContentTableRelated + "_List.Attributes[\"class\"] = \"listSortable\";" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                HyperLink_" + sContentTableRelated + "_ReorderRecord.Visible = true;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                HyperLink_" + sContentTableRelated + "_ApplyReorder.Visible = true;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            }" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            rq.statusFlag = (int)EnumCmsContent.Enabled;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "\n";
                        sTableRelatedDataCode_ActionInitPage += "            //" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            " + sContentTableRelated + "ListResponse responseList = objCmsBoDataLibs." + sContentTableRelated + "ListCms(header, rq);" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (responseList.Success)" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            {" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                if (responseList != null)" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                {" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                    Repeater_" + sContentTableRelated + ".DataSource = responseList.items;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                    Repeater_" + sContentTableRelated + ".ItemDataBound += new RepeaterItemEventHandler(Repeater_" + sContentTableRelated + "_ItemDataBound);" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                    Repeater_" + sContentTableRelated + ".DataBind();" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                }" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            }" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "        }" + "\n";

                        sTableRelatedDataCode_ActionInitPage += "        protected void Repeater_" + sContentTableRelated + "_ItemDataBound(object sender, RepeaterItemEventArgs e)" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "        {" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            RepeaterItem item = e.Item;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            {" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                String nUid_Related = ((" + sContentTableRelated + ")e.Item.DataItem).Uid.ToString();" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                String sTitolo = ((" + sContentTableRelated + ")e.Item.DataItem).Title.ToString();" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                //" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                Panel myPanel_Card = (Panel)e.Item.FindControl(\"Panel_Card\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myPanel_Card.Attributes.Add(\"data-uid\", nUid_Related.ToString());" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                if (UidRelated.Equals(nUid_Related))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                   myPanel_Card.CssClass += \" relatedActive active\";" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                // CheckBox" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                HtmlControl mycheckboxSel = (HtmlControl)e.Item.FindControl(\"" + sContentTableRelated + "_checkboxSel\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                mycheckboxSel.Attributes.Add(\"data-uid\", nUid_Related.ToString());" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "" + "\n";
                        if (!bFieldImageRelated)
                            sTableRelatedDataCode_ActionInitPage += "                /*" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                String sImageUrl_Prev = ((" + sContentTableRelated + ")e.Item.DataItem).ImageUrl_Prev;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                if (!String.IsNullOrEmpty(sImageUrl_Prev))" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                    myPanel_Card.Attributes.Add(\"style\", \"background-image: url('\" + sStoragePublicBaseUrl + sImageUrl_Prev + \"')\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                else" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                    myPanel_Card.Attributes.Add(\"style\", \"background-image: url('\" + sCmsStartingpage + \"img/nofoto_related.png')\");" + "\n";
                        if (!bFieldImageRelated)
                            sTableRelatedDataCode_ActionInitPage += "                */" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                Literal myLiteral_Title = (Literal)e.Item.FindControl(\"Literal_Title\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myLiteral_Title.Text = sTitolo;" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                HyperLink myHyperLink_Update = (HyperLink)e.Item.FindControl(\"HyperLink_Update\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Update.NavigateUrl = getCurrentDetailUrl(Uid.ToString()) + \"&ContentTableRelated=" + sContentTableRelated + "&UidRelated=\" + nUid_Related.ToString();" + "\n";

                        sTableRelatedDataCode_ActionInitPage += "                if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Edit.ToolTip\"))) " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                   myHyperLink_Update.ToolTip = currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Edit.ToolTip\"); " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                HyperLink myHyperLink_Delete = (HyperLink)e.Item.FindControl(\"HyperLink_Delete\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Delete.Attributes.Add(\"data-url\", \"action.aspx\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Delete.Attributes.Add(\"data-content\", \"" + sContentTableRelated + "\");" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Delete.Attributes.Add(\"data-returnUrl\", getCurrentDetailUrl(Uid.ToString()));" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Delete.Attributes.Add(\"data-action\", \"deleteRelated\"); " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Delete.Attributes.Add(\"data-uid\", Uid.ToString()); " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                myHyperLink_Delete.Attributes.Add(\"data-uidRelated\", nUid_Related.ToString()); " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Delete.ToolTip\"))) " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "                   myHyperLink_Delete.ToolTip = currentCmsUserSession.GetGlobalLabel(\"Cms.HyperLink_Delete.ToolTip\"); " + "\n";
                        sTableRelatedDataCode_ActionInitPage += "" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "            }" + "\n";
                        sTableRelatedDataCode_ActionInitPage += "        }" + "\n";

                        // sColumnDataCode_DeatilsInitPage += "            Repeater_" + sContentTableRelated + "_Bind();" + "\n";

                        sColumnDataCode_ActionInitUpdate_LoadRelated += "                    if (sDataContent.Equals(\"" + sContentTableRelated + "\")) " + "\n";
                        sColumnDataCode_ActionInitUpdate_LoadRelated += "                    {" + "\n";
                        sColumnDataCode_ActionInitUpdate_LoadRelated += "                       Repeater_" + sContentTableRelated + "_Bind();" + "\n";
                        sColumnDataCode_ActionInitUpdate_LoadRelated += "                    }" + "\n";

                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                         if (sDataContent.Equals(\"" + sContentTableRelated + "\"))" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                         {" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              // Delete Status" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              " + sContentTableRelated + " updateObject = null;" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              if (!String.IsNullOrEmpty(sDataUidRelated))" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              {" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                  " + sContentTableRelated + "ItemResponse responseUserGet = objCmsBoDataLibs." + sContentTableRelated + "GetCms(header, sDataUidRelated);" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                  updateObject = responseUserGet.item;" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              " + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                  if (updateObject != null)" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                  {" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                      //" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                      updateObject.StatusFlag = (int)EnumCmsContent.Deleted;" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              " + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                      " + sContentTableRelated + "ItemResponse responseUpsert = objCmsBoDataLibs." + sContentTableRelated + "Upsert(header, updateObject);" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                      if (responseUpsert.Success)" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                          Repeater_" + sContentTableRelated + "_Bind();" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                                  }" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                              }" + "\n";
                        sColumnDataCode_ActionInitUpdate_DeleteRelated += "                         }" + "\n";

                        //
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "        protected void InitUpdate_" + sContentTableRelated + "(String nUid)" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "        {" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "\n";
                        //
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            if (!String.IsNullOrEmpty(currentCmsUserSession.GetGlobalLabel(\"Cms.Literal_Cms_LastCorrection.Text\")))" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                Literal_Cms_" + sContentTableRelated + "_LastCorrection.Text = currentCmsUserSession.GetGlobalLabel(\"Cms.Literal_Cms_LastCorrection.Text\");" + "\n";



                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            // Show Button Save" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            Button_Related_" + sContentTableRelated + "_Save.Visible = true;" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            " + sContentTableRelated + "ItemResponse responseGet = objCmsBoDataLibs." + sContentTableRelated + "GetCms(header, nUid);" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            if ((responseGet.Success) && (responseGet.item != null))" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            {" + "\n";
                        //
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                // Get Object " + sContentTableRelated + " \n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                " + sContentTableRelated + " updateObject = responseGet.item;\n";

                        // LastModify
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                #region Icon Status Flag\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                if (updateObject.StatusFlag.Equals((int)EnumCmsContent.Enabled))\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                    iconStatusFlag_" + sContentTableRelated + ".Attributes.Add(\"class\", \"fa fa-power-off success\");\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                else\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                    iconStatusFlag_" + sContentTableRelated + ".Attributes.Add(\"class\", \"fa fa-power-off danger\");\n";

                        sTableRelatedDataCode_DeatilsInitUpdate_All += "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                Literal_" + sContentTableRelated + "_LastModify.Text = objLibDate.DateTimeToString(updateObject.CreationDate, \"GMA\", \"/\", \":\");\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                if (updateObject.UpdateDate != null)\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                {\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                    DateTime dUpdate = (DateTime)updateObject.UpdateDate;\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                    Literal_" + sContentTableRelated + "_LastModify.Text = objLibDate.DateTimeToString(dUpdate, \"GMA\", \"/\", \":\");\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                }\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                Literal_" + sContentTableRelated + "_LastModify.Visible = true;\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                Literal_Cms_" + sContentTableRelated + "_LastCorrection.Visible = true;\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "                #endregion\n";

                        sTableRelatedDataCode_DeatilsInitUpdate_All += "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += sTableRelatedDataCode_DeatilsInitUpdate;
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "            }" + "\n";
                        sTableRelatedDataCode_DeatilsInitUpdate_All += "        }" + "\n";

                        //
                        sTableRelatedDataCode_DeatilsSave_All += "        protected Boolean Save_" + sContentTableRelated + "(EnumCmsContent _statusFlag)" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            // Object " + sContentTableRelated + " \n";
                        sTableRelatedDataCode_DeatilsSave_All += "            " + sContentTableRelated + " updateObject = new " + sContentTableRelated + "();" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            if (!String.IsNullOrEmpty(UidRelated))" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                updateObject = objCmsBoDataLibs." + sContentTableRelated + "GetCms(header, UidRelated).item;" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            // Uid " + sContentTable + " \n";
                        sTableRelatedDataCode_DeatilsSave_All += "            if (objLibMath.isNumber(Uid)) \n";
                        sTableRelatedDataCode_DeatilsSave_All += "                updateObject.Uid_" + sContentTable + " = System.Convert.ToInt32(Uid);\n";
                        sTableRelatedDataCode_DeatilsSave_All += sTableRelatedDataCode_DeatilsSave;
                        sTableRelatedDataCode_DeatilsSave_All += "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            // StatusFlag \n";
                        sTableRelatedDataCode_DeatilsSave_All += "            updateObject.StatusFlag = (int)EnumCmsContent.Enabled;\n";
                        sTableRelatedDataCode_DeatilsSave_All += "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            " + sContentTableRelated + "ItemResponse responseUpsert = objCmsBoDataLibs." + sContentTableRelated + "Upsert(header, updateObject);\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            if (responseUpsert.Success)\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            {\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                updateObject.Uid = responseUpsert.item.Uid;\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                UidRelated = updateObject.Uid.ToString();\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            }\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            return responseUpsert.Success;";
                        sTableRelatedDataCode_DeatilsSave_All += "        }" + "\n";

                        sTableRelatedDataCode_DeatilsSave_All += "        protected void Button_Related_" + sContentTableRelated + "_Save_Click(object sender, EventArgs e)" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            //nn avevo questa Associazione, quindi procedo con inserimento" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            if (Save_" + sContentTableRelated + "(EnumCmsContent.Enabled))" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                //" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                Response.Redirect(getCurrentDetailUrl(Uid.ToString()));" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            }" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            else" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                // Error" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            }" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        }" + "\n";

                        sTableRelatedDataCode_DeatilsSave_All += "        protected void Button_Related_" + sContentTableRelated + "_SaveAndNext_Click(object sender, EventArgs e)" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            //nn avevo questa Associazione, quindi procedo con inserimento" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            if (Save_" + sContentTableRelated + "(EnumCmsContent.Enabled))" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                //" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                Response.Redirect(getCurrentDetailUrl(Uid.ToString() + \"&ContentTableRelated=" + sContentTableRelated + "&UidRelated=\" + HiddenField_" + sContentTableRelated + "_NextUid.Value));" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            }" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            else" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "                // Error" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            }" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        }" + "\n";

                        sTableRelatedDataCode_DeatilsSave_All += "        protected void Button_Related_" + sContentTableRelated + "_Cancel_Click(object sender, EventArgs e)" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        {" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            //" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "            Response.Redirect(getCurrentDetailUrl(Uid.ToString()));" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "        }" + "\n";
                        sTableRelatedDataCode_DeatilsSave_All += "\n";

                        sTableRelatedDataCode_DeleteCommand += "                if (vParameter[1].Equals(\"" + sContentTableRelated + "\"))" + "\n";
                        sTableRelatedDataCode_DeleteCommand += "                    if (Delete_" + sContentTableRelated + "(vParameter[2]))" + "\n";
                        sTableRelatedDataCode_DeleteCommand += "                        Response.Redirect(getCurrentDetailUrl(Uid.ToString()));" + "\n";

                        //sTableRelatedDataCode_DeatilsInitUpdate_All += sTableRelatedDataCode_DeatilsSave;

                        //
                        sTableRelatedDataCode_ActionInitPage = sTableRelatedDataCode_ActionInitPage.Replace("{{bOrderRelated}}", bFieldOrdRelated.ToString().ToLower());
                        sTableRelatedDataCode_ActionPage = sTableRelatedDataCode_ActionPage.Replace("{{bOrderRelated}}", bFieldOrdRelated.ToString().ToLower());
                        sTableRelatedDataCode_DeatilsInitPage = sTableRelatedDataCode_DeatilsInitPage.Replace("{{bOrderRelated}}", bFieldOrdRelated.ToString().ToLower());
                    }

                    //
                    if (sContentTable.Equals("CmsNlsContext") || sContentTable.Equals("CmsUsers") || sContentTable.Equals("CmsRoles") || sContentTable.Equals("CmsSections") || sContentTable.Equals("CmsSubSections"))
                        sRqCmsNlsContextFilter = "";
                    else
                        sRqCmsNlsContextFilter = "rq.uid_CmsNlsContext = currentCmsUserSession.currentCmsNlsContext.Uid;";


                    // Title Field
                    sColumnDataCode_List = sColumnDataCode_List.Replace("{{TitleField}}", sTitleField);
                    sColumnDataCode_DeatilsInitPage = sColumnDataCode_DeatilsInitPage.Replace("{{TitleField}}", sTitleField);


                    string[] lines = System.IO.File.ReadAllLines(sSourcedir + "list.aspx.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{RqSectionUri}}", sRqSectionUri);
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnHeader}}", sColumnHeader);
                        sStrLog = sStrLog.Replace("{{ColumnHeaderCode_List}}", sColumnHeaderCode_List);
                        sStrLog = sStrLog.Replace("{{ColumnData}}", sColumnData);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode}}", sColumnDataCode_List);
                        sStrLog = sStrLog.Replace("{{rqCmsNlsContextFilter}}", sRqCmsNlsContextFilter);

                        //
                        LogUtf8(sDestdirSection + sFileNameClass_List, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "list.aspx.designer.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{RqSectionUri}}", sRqSectionUri);
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnHeader}}", sColumnHeader);
                        sStrLog = sStrLog.Replace("{{ColumnHeaderCode_List}}", sColumnHeaderCode_List);
                        sStrLog = sStrLog.Replace("{{ColumnHeaderCode_Designer}}", sColumnHeaderCode_Designer);
                        sStrLog = sStrLog.Replace("{{ColumnData}}", sColumnData);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode}}", sColumnDataCode_List);
                        sStrLog = sStrLog.Replace("{{rqCmsNlsContextFilter}}", sRqCmsNlsContextFilter);

                        //
                        LogUtf8(sDestdirSection + sFileNameDesigner_List, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "list.aspx.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{RqSectionUri}}", sRqSectionUri);
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnHeader}}", sColumnHeader);
                        sStrLog = sStrLog.Replace("{{ColumnHeaderCode_List}}", sColumnHeaderCode_List);
                        sStrLog = sStrLog.Replace("{{ColumnData}}", sColumnData);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode}}", sColumnDataCode_List);
                        sStrLog = sStrLog.Replace("{{rqCmsNlsContextFilter}}", sRqCmsNlsContextFilter);

                        //
                        LogUtf8(sDestdirSection + sFileNamePage_List, sStrLog);
                    }

                    // Details Page
                    lines = System.IO.File.ReadAllLines(sSourcedir + "details.aspx.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate}}", sColumnDataCode_DeatilsInitUpdate);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate_LoadRelated}}", sColumnDataCode_DeatilsInitUpdate_LoadRelated);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsSave}}", sColumnDataCode_DeatilsSave);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtml}}", sColumnDataCode_InputHtml);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitPage}}", sColumnDataCode_DeatilsInitPage);

                        //
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_Js}}", sTableRelatedDataCode_Js);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_BindJs}}", sTableRelatedDataCode_BindJs);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitPage}}", sTableRelatedDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdate}}", sTableRelatedDataCode_DeatilsInitUpdate_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsSave}}", sTableRelatedDataCode_DeatilsSave_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ListPage}}", sTableRelatedDataCode_ListPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsPage}}", sTableRelatedDataCode_DeatilsPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsDesigner}}", sTableRelatedDataCode_DeatilsDesigner);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsHideAllRelated}}", sTableRelatedDataCode_DeatilsHideAllRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdateRelated}}", sTableRelatedDataCode_DeatilsInitUpdateRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeleteCommand}}", sTableRelatedDataCode_DeleteCommand);

                        //
                        LogUtf8(sDestdirSection + sFileNameClass_Deatils, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "details.aspx.designer.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate}}", sColumnDataCode_DeatilsInitUpdate);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsSave}}", sColumnDataCode_DeatilsSave);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtml}}", sColumnDataCode_InputHtml);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitPage}}", sColumnDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_Designer}}", sColumnDataCode_Designer);
                        //
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_Js}}", sTableRelatedDataCode_Js);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_BindJs}}", sTableRelatedDataCode_BindJs);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitPage}}", sTableRelatedDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdate}}", sTableRelatedDataCode_DeatilsInitUpdate_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ListPage}}", sTableRelatedDataCode_ListPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsSave}}", sTableRelatedDataCode_DeatilsSave_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsPage}}", sTableRelatedDataCode_DeatilsPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsDesigner}}", sTableRelatedDataCode_DeatilsDesigner);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeleteCommand}}", sTableRelatedDataCode_DeleteCommand);
                        //
                        LogUtf8(sDestdirSection + sFileNameDesigner_Deatils, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "details.aspx.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate}}", sColumnDataCode_DeatilsInitUpdate);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsSave}}", sColumnDataCode_DeatilsSave);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtml}}", sColumnDataCode_InputHtml);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtmlPublish}}", sColumnDataCode_InputHtmlPublish);
                        //
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_Js}}", sTableRelatedDataCode_Js);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_BindJs}}", sTableRelatedDataCode_BindJs);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitPage}}", sTableRelatedDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdate}}", sTableRelatedDataCode_DeatilsInitUpdate_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsSave}}", sTableRelatedDataCode_DeatilsSave_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsPage}}", sTableRelatedDataCode_DeatilsPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ListPage}}", sTableRelatedDataCode_ListPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsDesigner}}", sTableRelatedDataCode_DeatilsDesigner);
                        //
                        LogUtf8(sDestdirSection + sFileNamePage_Deatils, sStrLog);
                    }

                    // Action Page
                    lines = System.IO.File.ReadAllLines(sSourcedir + "action.aspx.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate}}", sColumnDataCode_DeatilsInitUpdate);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate_LoadRelated}}", sColumnDataCode_DeatilsInitUpdate_LoadRelated);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsSave}}", sColumnDataCode_DeatilsSave);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtml}}", sColumnDataCode_InputHtml);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitPage}}", sColumnDataCode_DeatilsInitPage);

                        //
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitPage}}", sTableRelatedDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdate}}", sTableRelatedDataCode_DeatilsInitUpdate_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsSave}}", sTableRelatedDataCode_DeatilsSave_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionPage}}", sTableRelatedDataCode_ActionPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionInitPage}}", sTableRelatedDataCode_ActionInitPage);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_ActionInitUpdate_LoadRelated}}", sColumnDataCode_ActionInitUpdate_LoadRelated);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_ActionInitUpdate_DeleteRelated}}", sColumnDataCode_ActionInitUpdate_DeleteRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionDesigner}}", sTableRelatedDataCode_ActionDesigner);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsHideAllRelated}}", sTableRelatedDataCode_DeatilsHideAllRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdateRelated}}", sTableRelatedDataCode_DeatilsInitUpdateRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeleteCommand}}", sTableRelatedDataCode_DeleteCommand);

                        //
                        LogUtf8(sDestdirSection + sFileNameClass_Action, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "action.aspx.designer.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate}}", sColumnDataCode_DeatilsInitUpdate);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsSave}}", sColumnDataCode_DeatilsSave);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtml}}", sColumnDataCode_InputHtml);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitPage}}", sColumnDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_Designer}}", sColumnDataCode_Designer);
                        //
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitPage}}", sTableRelatedDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdate}}", sTableRelatedDataCode_DeatilsInitUpdate_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsSave}}", sTableRelatedDataCode_DeatilsSave_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionPage}}", sTableRelatedDataCode_ActionPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionInitPage}}", sTableRelatedDataCode_ActionInitPage);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_ActionInitUpdate_LoadRelated}}", sColumnDataCode_ActionInitUpdate_LoadRelated);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_ActionInitUpdate_DeleteRelated}}", sColumnDataCode_ActionInitUpdate_DeleteRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionDesigner}}", sTableRelatedDataCode_ActionDesigner);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeleteCommand}}", sTableRelatedDataCode_DeleteCommand);
                        //
                        LogUtf8(sDestdirSection + sFileNameDesigner_Action, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "action.aspx.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsInitUpdate}}", sColumnDataCode_DeatilsInitUpdate);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_DeatilsSave}}", sColumnDataCode_DeatilsSave);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_InputHtml}}", sColumnDataCode_InputHtml);
                        //
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitPage}}", sTableRelatedDataCode_DeatilsInitPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsInitUpdate}}", sTableRelatedDataCode_DeatilsInitUpdate_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_DeatilsSave}}", sTableRelatedDataCode_DeatilsSave_All);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionPage}}", sTableRelatedDataCode_ActionPage);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionInitPage}}", sTableRelatedDataCode_ActionInitPage);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_ActionInitUpdate_LoadRelated}}", sColumnDataCode_ActionInitUpdate_LoadRelated);
                        sStrLog = sStrLog.Replace("{{ColumnDataCode_ActionInitUpdate_DeleteRelated}}", sColumnDataCode_ActionInitUpdate_DeleteRelated);
                        sStrLog = sStrLog.Replace("{{TableRelatedDataCode_ActionDesigner}}", sTableRelatedDataCode_ActionDesigner);
                        //
                        LogUtf8(sDestdirSection + sFileNamePage_Action, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "export.aspx.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ExportFieldList}}", sExportFieldList);
                        //
                        LogUtf8(sDestdirSection + sFileNamePage_Export, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "export.aspx.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ExportFieldList}}", sExportFieldList);
                        //
                        LogUtf8(sDestdirSection + sFileNameClass_Export, sStrLog);
                    }

                    lines = System.IO.File.ReadAllLines(sSourcedir + "export.aspx.designer.cs.txt");
                    foreach (string line in lines)
                    {
                        //
                        sStrLog = line;
                        sStrLog = sStrLog.Replace("{{bOrder}}", bFieldOrd.ToString().ToLower().ToLower());
                        sStrLog = sStrLog.Replace("{{bFieldPublish}}", bFieldPublish.ToString().ToLower().ToLower());
                        sStrLog = sStrLog.Replace("{{PublishStartComment}}", sFieldPublishStartComment.ToString());
                        sStrLog = sStrLog.Replace("{{PublishEndComment}}", sFieldPublishEndComment.ToString());
                        sStrLog = sStrLog.Replace("{{TitleField}}", sTitleField);
                        sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                        sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                        sStrLog = sStrLog.Replace("{{ExportFieldList}}", sExportFieldList);
                        //
                        LogUtf8(sDestdirSection + sFileNameDesigner_Export, sStrLog);
                    }
                }
            }

            String[] linesSupport = System.IO.File.ReadAllLines(sSourcedir + "setOrder.aspx.cs.txt");
            foreach (string line in linesSupport)
            {
                //
                sStrLog = line;
                sStrLog = sStrLog.Replace("{{ColumnDataCode_SupportPage}}", sColumnDataCode_SupportPage);
                //
                LogUtf8(sDestdir + "support\\" + sFileNamePage_Support, sStrLog);
            }

            dp.close();
        }

        public static String ClearContentTable(String _contentTable, String _contentTableRelated)
        {
            return _contentTableRelated.Replace(_contentTable + "_", "").Replace("_", " ");
        }
        public static String ClearColumnName(String _fieldName)
        {
            return _fieldName.Replace("Uid_", "").Replace("_", " ");
        }
    }
}
