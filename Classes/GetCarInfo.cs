using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboAz_App.Forms;
using TurboAz_App.Utils;

namespace TurboAz_App.Classes
{
    class GetCarInfo
    {


        public DataTable GetInfo(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(SqlUtils.conString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DataTable GetCarBrand()
        {
            string query = "select ID, Brand_Name from Car_Brands";
            return GetInfo(query);
        }
        public DataTable GetCarModel(int brandID)
        {
            string query = $"select ID, Model_Name from Car_Models where Brand_ID={brandID}";
            return GetInfo(query);
        }
        public DataTable GetCarGeneralData(int typeID)
        {
            string query = $"select ID,Type_Name from General_Info where Type_ID={typeID}";
            return GetInfo(query);
        }
        public DataTable GetImages(string adsID)
        {
            string query = $"select ID, Car_Image from Car_Images where Ads_ID={adsID} ";
            return GetInfo(query);

        }
    }
}