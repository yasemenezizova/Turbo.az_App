using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurboAz_App.Classes;
using TurboAz_App.Utils;

namespace TurboAz_App.Forms
{
    public partial class ADS_Info : Form
    {
        GetCarInfo getCar = new GetCarInfo();
        public int id { get; set; }
        public ADS_Info(int id)
        {
            InitializeComponent();
            this.id = id;
        }


        private void ADS_Info_Load(object sender, EventArgs e)
        {
            GetCarData();
        }


        private void GetCarData()
        {
            string query = $@"SELECT GN.Type_Name City_ID
                ,BRD.Brand_Name
                ,MDL.Model_Name 
                ,ADS.Engine_Power
				,ADS.Engine_Capacity
                ,ADS.Year
                ,GN2.Type_Name Ban_Type_ID
                ,GN3.Type_Name Color_ID
                ,GN5.Type_Name Fuel_Type_ID
                ,ADS.WALK
                ,GN7.Type_Name Gearbox_ID
                ,GN6.Type_Name Transmission_ID
                ,ADS.Price
                ,ADS.Currency_ID, 
				case when ADS.Currency_ID='3' Then 'EUR'
				when ADS.Currency_ID='2' Then 'USD'
				when ADS.Currency_ID='1' Then 'AZN'
				end as Currency
                FROM[dbo].[Car_ADS] ADS 
                JOIN General_Info GN ON GN.ID = ADS.City_ID
                JOIN Car_Models MDL ON MDL.ID = ADS.Model_ID	 
                JOIN Car_Brands BRD ON MDL.BRAND_ID = BRD.ID
                JOIN General_Info GN2 ON GN2.ID = ADS.Ban_Type_ID
                JOIN General_Info GN3 ON GN3.ID = ADS.Color_ID
                JOIN General_Info GN4 ON GN4.ID = ADS.Currency_ID
                JOIN General_Info GN5 ON GN5.ID = ADS.Fuel_Type_ID
                JOIN General_Info GN6 ON GN6.ID = ADS.Transmission_ID
                JOIN General_Info GN7 ON GN7.ID = ADS.Gearbox_ID
                JOIN General_Info GN8 ON GN8.ID = ADS.City_ID
                WHERE ADS.ID = {id}";

            DataTable dtTableCarInfo = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(SqlUtils.conString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            sqlDataAdapter.Fill(dtTableCarInfo);

            txtCity.Text = dtTableCarInfo.Rows[0]["City_ID"].ToString();
            txtBrand.Text = dtTableCarInfo.Rows[0]["Brand_Name"].ToString();
            txtModel.Text = dtTableCarInfo.Rows[0]["Model_Name"].ToString();
            txtYear.Text = dtTableCarInfo.Rows[0]["Year"].ToString();
            txtBanType.Text = dtTableCarInfo.Rows[0]["Ban_Type_ID"].ToString();
            txtColor.Text = dtTableCarInfo.Rows[0]["Color_ID"].ToString();
            txtEnginePower.Text = dtTableCarInfo.Rows[0]["Engine_Capacity"].ToString();
            txtFuelType.Text = dtTableCarInfo.Rows[0]["Fuel_Type_ID"].ToString();
            txtWalk.Text = dtTableCarInfo.Rows[0]["WALK"].ToString();
            txtGearbox.Text = dtTableCarInfo.Rows[0]["Gearbox_ID"].ToString();
            txtTransmission.Text = dtTableCarInfo.Rows[0]["Transmission_ID"].ToString();
            txtPrice.Text = dtTableCarInfo.Rows[0]["Price"].ToString() + " " + dtTableCarInfo.Rows[0]["Currency"].ToString();
            lblPrice.Text = dtTableCarInfo.Rows[0]["Price"].ToString() + " " + dtTableCarInfo.Rows[0]["Currency"].ToString();
            lblInfo.Text = dtTableCarInfo.Rows[0]["Brand_Name"].ToString() + " " + dtTableCarInfo.Rows[0]["Model_Name"].ToString() + ", " + dtTableCarInfo.Rows[0]["Engine_Capacity"].ToString() + " l, " + dtTableCarInfo.Rows[0]["WALK"].ToString() + " km";
            string queryImage = $@"SELECT IMG.Car_Image, IMG.ID FROM Car_Images IMG 
                                   JOIN Car_ADS ADS ON ADS.ID = IMG.Ads_ID
                                   WHERE ADS.ID={id}";


            SqlDataAdapter sqlDataAdapterImage = new SqlDataAdapter(queryImage, sqlConnection);
            DataTable dataTableImage = new DataTable();
            sqlDataAdapterImage.Fill(dataTableImage);
            grdControlInfo.DataSource = dataTableImage;

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblPrice_Click(object sender, EventArgs e)
        {

        }
    }
}
