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
            mainCreateClass();
        }




        //
        static void mainCreateClass()
        {
            // Defaul Field Database

            String sField_CreationDate = "DataCreazione";
            String sField_UpdateDate = "DataUltimaModifica";
            String sField_DefaultTitle = "Titolo";
            //
            String sField_CreationUser = "Uid_CmsUsers";
            String sField_UpdateUser = "Uid_CmsUsers_Mod";

            // Defaul Field Database            
            sField_CreationDate = "CreationDate";
            sField_UpdateDate = "UpdateDate";
            sField_DefaultTitle = "Title";
            sField_CreationUser = "Uid_CreationUser ";
            sField_UpdateUser = "Uid_UpdateUser";



            String sUseKW = WebContext.getConfig("%.useKW").ToString();
            String sFilesTemplate = String.Empty;
            String sFilesTemplateView = String.Empty;


            if (sUseKW.Equals("true"))
            {
                sFilesTemplate = "ClassTemplateKW";
                sFilesTemplateView = "ClassTemplateForViewKW";
            }
            else
            {
                sFilesTemplate = "ClassTemplate"; //non kawasaki
                sFilesTemplateView = "ClassTemplateForView"; //non kawasaki
            }



            /*
            sFilesTemplate = "ClassTemplateKW";
            sFilesTemplateView = "ClassTemplateForViewKW";
            */

            String sStrLog = String.Empty;
            //
            String sDestdir = WebContext.getConfig("%.destdir").ToString();
            String sSourcedir = WebContext.getConfig("%.sourcedir").ToString();

            DirectoryInfo dr = new DirectoryInfo(sDestdir);
            if (!dr.Exists)
                Directory.CreateDirectory(sDestdir);

            dr = new DirectoryInfo(sDestdir + "partial");
            if (!dr.Exists)
                Directory.CreateDirectory(sDestdir + "partial");

            IDataProvider dp = IDataProviderFactory.factory("DataProviderSqlServer");
            String sSql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE (TABLE_TYPE = 'BASE TABLE') AND TABLE_NAME = 'HpEmotionWindow' ORDER BY TABLE_NAME";
            sSql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE (TABLE_TYPE = 'BASE TABLE') ORDER BY TABLE_NAME";

            SimpleDataSet ds = dp.executeQuery(sSql);

            for (int i = 0; i < ds.Table.Rows.Count; i++)
            {
                String sContentTable = ds.Rows[i]["TABLE_NAME"].ToString();
                String sContentTableType = ds.Rows[i]["TABLE_TYPE"].ToString();
                String sSqlTable = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + objLibString.sQuote(sContentTable) + " ORDER BY ORDINAL_POSITION";
                SimpleDataSet dsTable = dp.executeQuery(sSqlTable);

                //
                Console.WriteLine(sContentTable);

                //
                Boolean bTitle = false;
                Boolean bOrder = false;
                Boolean bFrom = false;
                Boolean bPublish = false;
                String sFieldList_Upsert = String.Empty;
                String sFieldList_Where = String.Empty;
                String sFieldList_ListOptions = String.Empty;
                String sFileNameClass = sContentTable + ".cs";
                String sFileNamePartialClass = sContentTable + ".cs";
                String sFileClassInclude = String.Empty;
                String sFieldList_WhereTextSearch = String.Empty;
                String sPartialRelated = String.Empty;

                FileInfo mf = new FileInfo(sDestdir + sFileNameClass);
                if (mf.Exists)
                    mf.Delete();

                mf = new FileInfo(sDestdir + "partial\\" + sFileNamePartialClass);
                if (mf.Exists)
                    mf.Delete();

                //
                String sSqlFKRel = "SELECT OBJECT_NAME(fk.parent_object_id) 'ParentTable', c1.name 'ParentColumn', OBJECT_NAME(fk.referenced_object_id) 'ReferencedTable', c2.name 'ReferencedColumn' FROM sys.foreign_keys fk INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id INNER JOIN sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id INNER JOIN sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id WHERE OBJECT_NAME(fk.referenced_object_id) = '" + sContentTable + "'";
                SimpleDataSet dsTableFKRel = dp.executeQuery(sSqlFKRel);
                for (int g = 0; g < dsTableFKRel.Table.Rows.Count; g++)
                {
                    String sREFERENCED_TABLE = dsTableFKRel.Table.Rows[g]["ParentTable"].ToString();
                    sFileClassInclude += "														  //.Include(\"" + sREFERENCED_TABLE + "\")\n";

                    sPartialRelated += "\n";
                    sPartialRelated += "        public List<" + sREFERENCED_TABLE + "> i" + sREFERENCED_TABLE + "\n";
                    sPartialRelated += "        {\n";
                    sPartialRelated += "            get\n";
                    sPartialRelated += "            {\n";
                    sPartialRelated += "                if (this." + sREFERENCED_TABLE + " != null)\n";
                    sPartialRelated += "                    return this." + sREFERENCED_TABLE + ".Where(t => t.StatusFlag == (Int32)EnumCmsContent.Enabled).ToList();\n";
                    sPartialRelated += "                else\n";
                    sPartialRelated += "                    return null;\n";
                    sPartialRelated += "            }\n";
                    sPartialRelated += "            set\n";
                    sPartialRelated += "            {\n";
                    sPartialRelated += "                //\n";
                    sPartialRelated += "            }\n";
                    sPartialRelated += "        }\n";


                }

                for (int j = 0; j < dsTable.Table.Rows.Count; j++)
                {
                    //
                    String sTABLE_NAME = dsTable.Rows[j]["TABLE_NAME"].ToString();
                    String sCOLUMN_NAME = dsTable.Rows[j]["COLUMN_NAME"].ToString();
                    String sCOLUMN_DEFAULT = dsTable.Rows[j]["COLUMN_DEFAULT"].ToString();
                    String sIS_NULLABLE = dsTable.Rows[j]["IS_NULLABLE"].ToString().ToLower();
                    String sDATA_TYPE = dsTable.Rows[j]["DATA_TYPE"].ToString();
                    String sCHARACTER_MAXIMUN_LENGTH = dsTable.Rows[j]["CHARACTER_MAXIMUM_LENGTH"].ToString();

                    if (sCOLUMN_NAME.ToLower().Equals("versioncurrent"))
                        bPublish = true;

                    if (sCOLUMN_NAME.ToLower().Equals(sField_DefaultTitle.ToLower()))
                        bTitle = true;

                    //
                    String sSqlFK = "SELECT OBJECT_NAME(fk.parent_object_id) 'ParentTable', c1.name 'ParentColumn', OBJECT_NAME(fk.referenced_object_id) 'ReferencedTable', c2.name 'ReferencedColumn' FROM sys.foreign_keys fk INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id INNER JOIN sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id INNER JOIN sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id WHERE OBJECT_NAME(fk.parent_object_id) = '" + sTABLE_NAME + "' AND c1.name = '" + sCOLUMN_NAME + "' ";
                    SimpleDataSet dsTableFK = dp.executeQuery(sSqlFK);

                    String sREFERENCED_TABLE = String.Empty;
                    String sREFERENCED_COLUMN = String.Empty;
                    for (int g = 0; g < dsTableFK.Table.Rows.Count; g++)
                    {
                        sREFERENCED_TABLE = dsTableFK.Table.Rows[g]["ReferencedTable"].ToString();
                        sREFERENCED_COLUMN = dsTableFK.Table.Rows[g]["ReferencedColumn"].ToString();

                        sFileClassInclude += "														  .Include(\"" + sREFERENCED_TABLE + "\")\n";

                        sPartialRelated += "        \n";
                        sPartialRelated += "        public " + sREFERENCED_TABLE + " i" + sREFERENCED_TABLE + "\n";
                        sPartialRelated += "        {\n";
                        sPartialRelated += "            get\n";
                        sPartialRelated += "            {\n";
                        sPartialRelated += "                return this." + sREFERENCED_TABLE + ";\n";
                        sPartialRelated += "            }\n";
                        sPartialRelated += "            set\n";
                        sPartialRelated += "            {\n";
                        sPartialRelated += "                //\n";
                        sPartialRelated += "            }\n";
                        sPartialRelated += "        }\n";
                    }

                    //
                    String sCOLUMN_NAME_PARAMETER = sCOLUMN_NAME.Substring(0, 1).ToLower() + sCOLUMN_NAME.Substring(1);

                    if (sCOLUMN_NAME_PARAMETER.Equals("class"))
                        sCOLUMN_NAME_PARAMETER = "Class";

                    if (
                        (!sCOLUMN_NAME.ToLower().Equals("uid")) &&
                        (!sCOLUMN_NAME.ToLower().Equals(sField_CreationDate.ToLower())) &&
                        (!sCOLUMN_NAME.ToLower().Equals(sField_UpdateDate.ToLower())) &&
                        (!sCOLUMN_NAME.ToLower().Equals(sField_CreationUser.ToLower())) &&
                        (!sCOLUMN_NAME.ToLower().Equals(sField_UpdateUser.ToLower()))
                        )
                    {
                        if (sDATA_TYPE.ToLower().Equals("nvarchar"))
                        {
                            //
                            if ((!sCOLUMN_NAME.ToLower().Equals("image")) && (!sCOLUMN_NAME.ToLower().Equals("imageprev")) && (!sCOLUMN_NAME.ToLower().Equals("imagedetails")) && (!sCOLUMN_NAME.ToLower().Equals("imagedetailsprev")) && (!sCOLUMN_NAME.ToLower().Equals("uid_cmsnlscontext")) && (!sCOLUMN_NAME.ToLower().Equals("imageurl")) && (!sCOLUMN_NAME.ToLower().Equals("imageurl_prev")) && (!sCOLUMN_NAME.ToLower().Equals("fileurl")))
                            {
                                sFieldList_Upsert += "						result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";
                                sFieldList_Upsert += "\n";

                                sFieldList_ListOptions += "		[DataMember]\n";
                                sFieldList_ListOptions += "		public String " + sCOLUMN_NAME_PARAMETER + " = null;\n\n";

                                if (sFieldList_Where.Length.Equals(0))
                                    sFieldList_Where = "														  where (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";
                                else
                                    sFieldList_Where += "														  (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";

                                // Search Text
                                sFieldList_WhereTextSearch += "t." + sCOLUMN_NAME + ".Contains(rq.searchText) ||";
                            }
                            else if (sCOLUMN_NAME.ToLower().Equals("uid_cmsnlscontext"))
                            {
                                sFieldList_Upsert += "						//result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";
                                sFieldList_Upsert += "\n";


                                sFieldList_ListOptions += "		[DataMember]\n";
                                sFieldList_ListOptions += "		public String " + sCOLUMN_NAME_PARAMETER + " = null;\n\n";

                                if (sFieldList_Where.Length.Equals(0))
                                    sFieldList_Where = "														  where (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";
                                else
                                    sFieldList_Where += "														  //(rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";

                            }
                            else
                            {

                                sFieldList_Upsert += "						result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";
                                sFieldList_Upsert += "\n";

                                // Search Text
                                //sFieldList_WhereTextSearch += "t." + sCOLUMN_NAME + ".Contains(rq.searchText) ||";
                            }
                        }
                        else if ((sDATA_TYPE.ToLower().Equals("ntext")) || (sDATA_TYPE.ToLower().Equals("text")))
                        {
                            //
                            sFieldList_Upsert += "						result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";

                            // Text Search
                            sFieldList_WhereTextSearch += "t." + sCOLUMN_NAME + ".Contains(rq.searchText) ||";
                        }
                        else if (sDATA_TYPE.ToLower().Equals("int"))
                        {
                            //
                            sFieldList_Upsert += "						if (data." + sCOLUMN_NAME + " != null)\n";
                            sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";

                            if (sCOLUMN_NAME.ToLower().Equals("uid_cmsnlscontext"))
                            {

                                sFieldList_Upsert += "						else\n";
                                sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = header.cmsCmsNlsContext.Uid;\n";

                                sFieldList_Upsert += "\n";
                            }
                            else if (sCOLUMN_NAME.ToLower().Equals("ord"))
                            {
                                //
                            }
                            else
                            {
                                if (sCOLUMN_DEFAULT.Length > 0)
                                {
                                    String sCOLUMN_DEFAULT_VALUE = sCOLUMN_DEFAULT.Replace("(", "");
                                    sCOLUMN_DEFAULT_VALUE = sCOLUMN_DEFAULT_VALUE.Replace(")", "");
                                    sFieldList_Upsert += "						else\n";
                                    sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = " + sCOLUMN_DEFAULT_VALUE + ";\n";
                                }
                                else
                                {
                                    if (sIS_NULLABLE.Equals("yes"))
                                    {
                                        sFieldList_Upsert += "						else\n";
                                        sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = null;\n";
                                    }
                                }
                                sFieldList_Upsert += "\n";
                            }

                            //
                            sFieldList_ListOptions += "		[DataMember]\n";
                            //if (sCOLUMN_NAME_PARAMETER.ToLower().Equals("uid_cmsnlscontext"))
                            //    sFieldList_ListOptions += "		public Int32? " + sCOLUMN_NAME_PARAMETER + " = 1;\n\n";
                            //else
                            sFieldList_ListOptions += "		public Int32? " + sCOLUMN_NAME_PARAMETER + " = null;\n\n";

                            if (sCOLUMN_NAME.ToLower().Equals("statusflag"))
                            {
                                if (sFieldList_Where.Length.Equals(0))
                                    sFieldList_Where = "														  where (rq.statusFlag == null || t." + sCOLUMN_NAME + " == rq.statusFlag) &&\n";
                                else
                                    sFieldList_Where += "														  (rq.statusFlag == null || t." + sCOLUMN_NAME + " == rq.statusFlag) &&\n";
                            }
                            else if (sCOLUMN_NAME.ToLower().Equals("uid_cmsnlscontext"))
                            {
                                if (sFieldList_Where.Length.Equals(0))
                                    sFieldList_Where = "														  where ((rq.uid_CmsNlsContext == null || t." + sCOLUMN_NAME + " == rq.uid_CmsNlsContext) || (t.uid_CmsNlsContext == null)) &&\n";
                                else
                                    sFieldList_Where += "														  ((rq.uid_CmsNlsContext == null || t." + sCOLUMN_NAME + " == rq.uid_CmsNlsContext) || (t." + sCOLUMN_NAME + " == null)) &&\n";
                            }
                            else
                            {
                                if (sFieldList_Where.Length.Equals(0))
                                    sFieldList_Where = "														  where (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";
                                else
                                    sFieldList_Where += "														  (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";

                                // Text Search
                                //sFieldList_WhereTextSearch += " t." + sCOLUMN_NAME_PARAMETER + ".Contains(rq.searchText) ||";
                            }
                        }
                        else if (sDATA_TYPE.ToLower().Equals("smallint"))
                        {
                            //
                            sFieldList_Upsert += "						if (data." + sCOLUMN_NAME + " != null)\n";
                            sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";

                            if (sIS_NULLABLE.Equals("yes"))
                            {
                                sFieldList_Upsert += "						else\n";
                                sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = null;\n";
                            }
                            sFieldList_Upsert += "\n";

                            //
                            sFieldList_ListOptions += "		[DataMember]\n";
                            sFieldList_ListOptions += "		public short? " + sCOLUMN_NAME_PARAMETER + " = null;\n\n";

                            if (sFieldList_Where.Length.Equals(0))
                                sFieldList_Where = "														  where (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";
                            else
                                sFieldList_Where += "														  (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";

                        }
                        else if (sDATA_TYPE.ToLower().Equals("datetime"))
                        {

                            //
                            sFieldList_Upsert += "						if (data." + sCOLUMN_NAME + " != null)\n";
                            sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = data." + sCOLUMN_NAME + ";\n";
                            if (sIS_NULLABLE.Equals("yes"))
                            {
                                sFieldList_Upsert += "						else\n";
                                sFieldList_Upsert += "						    result.item." + sCOLUMN_NAME + " = null;\n";
                            }
                            sFieldList_Upsert += "\n";

                            //
                            sFieldList_ListOptions += "		[DataMember]\n";
                            sFieldList_ListOptions += "		public DateTime? " + sCOLUMN_NAME_PARAMETER + " = null;\n\n";

                            if (sFieldList_Where.Length.Equals(0))
                                sFieldList_Where = "														  where (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";
                            else
                                sFieldList_Where += "														  (rq." + sCOLUMN_NAME_PARAMETER + " == null || t." + sCOLUMN_NAME + " == rq." + sCOLUMN_NAME_PARAMETER + ") &&\n";
                        }
                    }

                    if (sCOLUMN_NAME.ToLower().Equals("ord"))
                        bOrder = true;

                    if (sCOLUMN_NAME.ToLower().Equals("uid_from"))
                        bFrom = true;
                }

                //
                if (sFieldList_WhereTextSearch.Length > 0)
                    sFieldList_WhereTextSearch = "														  (rq.searchText == null || " + sFieldList_WhereTextSearch.Substring(0, sFieldList_WhereTextSearch.Length - 3) + ")";
                else
                {
                    //
                    if (sFieldList_Where.Length > 0)
                        sFieldList_Where = sFieldList_Where.Substring(0, sFieldList_Where.Length - 3);
                }

                string[] lines = null;
                lines = System.IO.File.ReadAllLines(sSourcedir + sFilesTemplate + ".txt");
                if (sContentTableType.Equals("VIEW"))
                    lines = System.IO.File.ReadAllLines(sSourcedir + sFilesTemplateView + ".txt");

                foreach (string line in lines)
                {
                    //
                    sStrLog = line;
                    sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                    sStrLog = sStrLog.Replace("{{FieldList_Upsert}}", sFieldList_Upsert);
                    sStrLog = sStrLog.Replace("{{FieldList_Where}}", sFieldList_Where);
                    sStrLog = sStrLog.Replace("{{FieldList_WhereTextSearch}}", sFieldList_WhereTextSearch);
                    sStrLog = sStrLog.Replace("{{FieldList_ListOptions}}", sFieldList_ListOptions);
                    sStrLog = sStrLog.Replace("{{FileClassInclude}}", sFileClassInclude);

                    sStrLog = sStrLog.Replace("{{Field_CreationDate}}", sField_CreationDate);
                    sStrLog = sStrLog.Replace("{{Field_UpdateDate}}", sField_UpdateDate);
                    sStrLog = sStrLog.Replace("{{Field_DefaultTitle}}", sField_DefaultTitle);
                    sStrLog = sStrLog.Replace("{{Field_CreationUser}}", sField_CreationUser);
                    sStrLog = sStrLog.Replace("{{Field_UpdateUser}}", sField_UpdateUser);

                    if (!bFrom)
                    {
                        sStrLog = sStrLog.Replace("{{startCommentFrom}}", "/*");
                        sStrLog = sStrLog.Replace("{{endCommentFrom}}", "*/");
                    }
                    else
                    {
                        sStrLog = sStrLog.Replace("{{startCommentFrom}}", "");
                        sStrLog = sStrLog.Replace("{{endCommentFrom}}", "");
                    }

                    if (!bOrder)
                    {
                        sStrLog = sStrLog.Replace("{{startCommentOrd}}", "/*");
                        sStrLog = sStrLog.Replace("{{endCommentOrd}}", "*/");
                    }
                    else
                    {
                        sStrLog = sStrLog.Replace("{{startCommentOrd}}", "");
                        sStrLog = sStrLog.Replace("{{endCommentOrd}}", "");
                    }

                    if (!bPublish)
                    {
                        sStrLog = sStrLog.Replace("{{startCommentPublish}}", "/*");
                        sStrLog = sStrLog.Replace("{{endCommentPublish}}", "*/");
                    }
                    else
                    {
                        sStrLog = sStrLog.Replace("{{startCommentPublish}}", "");
                        sStrLog = sStrLog.Replace("{{endCommentPublish}}", "");
                    }

                    //
                    objLibLog.Log(sDestdir + sFileNameClass, sStrLog);

                }

                String sPartialTitle = String.Empty;
                if (!bTitle)
                {
                    sPartialTitle += "        public String Title\n";
                    sPartialTitle += "        {\n";
                    sPartialTitle += "            get\n";
                    sPartialTitle += "            {\n";
                    sPartialTitle += "                return this.Uid.ToString();\n";
                    sPartialTitle += "            }\n";
                    sPartialTitle += "            set\n";
                    sPartialTitle += "            {\n";
                    sPartialTitle += "                //\n";
                    sPartialTitle += "            }\n";
                    sPartialTitle += "        }\n";
                }

                lines = System.IO.File.ReadAllLines(sSourcedir + "ClassPartial.txt");
                foreach (string line in lines)
                {
                    //
                    sStrLog = line;
                    sStrLog = sStrLog.Replace("{{PartialTitle}}", sPartialTitle);
                    sStrLog = sStrLog.Replace("{{PartialRelated}}", sPartialRelated);
                    sStrLog = sStrLog.Replace("{{Field_CreationDate}}", sField_CreationDate);
                    sStrLog = sStrLog.Replace("{{Field_UpdateDate}}", sField_UpdateDate);
                    sStrLog = sStrLog.Replace("{{ContentTable}}", sContentTable);
                    sStrLog = sStrLog.Replace("{{lContentTable}}", sContentTable.ToLower());
                    //
                    objLibLog.Log(sDestdir + "partial\\" + sFileNamePartialClass, sStrLog);
                }
            }
            dp.close();
        }
    }
}
