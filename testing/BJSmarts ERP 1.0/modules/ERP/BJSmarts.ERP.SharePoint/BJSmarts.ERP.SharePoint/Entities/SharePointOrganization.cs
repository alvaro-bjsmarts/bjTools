using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;

namespace BJSmarts.ERP.SharePoint.Entities
{
    public class SharePointOrganization : IOrganization
    {
        public SharePointOrganization()
        {
            Settings = new SharePointSettings();
            Log = new SharePointLog();
        }

        public static ISettings Settings { get; set; }
        public static ILog Log { get; set; }
        

        public string ID
        {
            get
            {

                string OrgId = string.Empty;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    try
                    {

                        using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
                        {
                            connection.Open();

                            string sql = "select [OrganizationId] from organizations where SiteId = @SiteId";
                            
                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {                            
                                cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar, 50).Value = SPContext.Current.Site.ID.ToString();

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        OrgId = reader.GetInt32(0).ToString();
                                    }
                                }
                            }                            
                        }

                    }
                    catch (SqlException sqlEx)
                    {
                        Log.Error(sqlEx.Message, sqlEx.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex.StackTrace);
                    }

                });

                return OrgId;
            }
        }

        public string Name
        {
            get
            {

                string OrgName = string.Empty;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    try
                    {

                        using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
                        {
                            connection.Open();

                            string sql = "select [Name] from organizations where SiteId = @SiteId";

                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar, 50).Value = SPContext.Current.Site.ID.ToString();

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        OrgName = reader.GetString(0);
                                    }
                                }
                            }
                        }

                    }
                    catch (SqlException sqlEx)
                    {
                        Log.Error(sqlEx.Message, sqlEx.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex.StackTrace);
                    }

                });

                return OrgName;
            }
        }

        public string Industry 
        {
            get
            {
                string IndustryName = string.Empty;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    try
                    {

                        using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
                        {
                            connection.Open();

                            string sql = "select [Industry] from organizations where SiteId = @SiteId";

                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar, 50).Value = SPContext.Current.Site.ID.ToString();

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        IndustryName = reader.GetString(0);
                                    }
                                }
                            }
                        }

                    }
                    catch (SqlException sqlEx)
                    {
                        Log.Error(sqlEx.Message, sqlEx.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex.StackTrace);
                    }

                });

                return IndustryName;
            }
        }

        public string IndustryId 
        {
            get
            {
                string Id = string.Empty;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {

                    try
                    {

                        using (SqlConnection connection = new SqlConnection(Settings.DatabaseConnectionString))
                        {
                            connection.Open();

                            string sql = "select [IndustryId] from organizations where SiteId = @SiteId";

                            using (SqlCommand cmd = new SqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar, 50).Value = SPContext.Current.Site.ID.ToString();

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Id = reader.GetInt32(0).ToString();
                                    }
                                }
                            }
                        }

                    }
                    catch (SqlException sqlEx)
                    {
                        Log.Error(sqlEx.Message, sqlEx.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message, ex.StackTrace);
                    }

                });

                return Id;
            }
        }
    }
}
