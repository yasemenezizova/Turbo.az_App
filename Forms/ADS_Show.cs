using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Card.ViewInfo;
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
using TurboAz_App.Forms;
using TurboAz_App.Utils;

namespace TurboAz_App
{
    public partial class ADS_Show : Form
    {
        public ADS_Show()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAds_Click(object sender, EventArgs e)
        {
            ADS_Add aDS_Add = new ADS_Add();
            aDS_Add.ShowDialog();
            GetCars();
        }
        private void ADS_Show_Load(object sender, EventArgs e)
        {
            setCarInfo.SetCarBrandData(lkUpEdtBrand);
            setCarInfo.SetGeneralInfo(lkUpEdtCurrency, 3);
            lkUpEdtCurrency.EditValue = 2;
            setCarInfo.SetGeneralInfo(lkUpEdtCity, 7);
            GetCars();
        }

        DataTable dataTableCars = new DataTable();
        private void GetCars()
        {
            string query =  $@"SELECT ADS.ID,
                                      Price,
                                      (BRD.Brand_Name+' '+MDL.Model_Name) Brand, 
                                      (SELECT TOP(1) IMG.Car_Image from Car_Images IMG) Car_Image, 
                                      GI.Type_Name City_ID, 
                                      ADS.Year, 
                                      ADS.Walk from Car_ADS ADS 
                                      join Car_Models MDL on MDL.ID=Model_ID 
                                      join Car_Brands BRD on MDL.Brand_ID=BRD.ID 
                                      join General_Info GI on GI.ID=ADS.City_ID 
                                      where Currency_ID= {lkUpEdtCurrency.EditValue}";


            if (lkUpEdtBrand.EditValue != null)
            {
                query = query + $" AND MDL.[Brand_ID]={lkUpEdtBrand.EditValue}";
            }

            if (lkUpEdtModel.EditValue != null)
            {
                query = query + $" AND ADS.[Model_ID]={lkUpEdtModel.EditValue}";
            }
            if ((int)lkUpEdtCurrency.EditValue != 2)
            {
                query = query + $" AND ADS.[Currency_ID]={lkUpEdtCurrency.EditValue}";
            }


            if (minPrice.EditValue != "")
            {
                query = query + $" AND ADS.[Price]>={minPrice.Text}";
            }

            if (maxPrice.EditValue != "")
            {
                query = query + $" AND ADS.[Price]<={maxPrice.Text}";
            }

            if (txtMinYear.EditValue != "")
            {
                query = query + $" AND ADS.[Year]>={txtMinYear.Text}";
            }

            if (maxYear.EditValue != "")
            {
                query = query + $" AND ADS.[Year]<={maxYear.Text}";
            }

            if (lkUpEdtCity.EditValue != null)
            {
                query = query + $" AND ADS.[City_ID]={lkUpEdtCity.EditValue}";
            }


            if (chckCredit.Checked)
            {
                query = query + $" AND ADS.[Credit]={chckCredit.Checked}";
            }


            if (chchkBarter.Checked)
            {
                query = query + $" AND ADS.[Barter]={chchkBarter.Checked}";
            }
            SqlConnection sqlConnection = new SqlConnection(SqlUtils.conString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

            sqlDataAdapter.Fill(dataTableCars);
            grdCntrlImages.DataSource = dataTableCars;
        }


        SetCarInfo setCarInfo = new SetCarInfo();


        private void lkUpEdtBrands_EditValueChanged(object sender, EventArgs e)
        {
            setCarInfo.SetCarModelData(lkUpEdtModel, (int)lkUpEdtBrand.EditValue);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetCars();
            MessageBox.Show("Axtaris bitdi", "Xəbərdarlıq bitdi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        private void cardVwImages_MouseDown(object sender, MouseEventArgs e)
        {

            CardView cardView = (CardView)sender;
            CardHitInfo hInfo = cardView.CalcHitInfo(e.X, e.Y);
            if (hInfo.HitTest == CardHitTest.FieldValue || hInfo.HitTest == CardHitTest.FieldCaption || hInfo.InCard)
            {
                int rowHandle = hInfo.RowHandle;
                int id = (int)cardVwImages.GetRowCellValue(rowHandle, "ID");
                ADS_Info carInfos = new ADS_Info(id);
                carInfos.ShowDialog();
            }
        }

        private void grdCntrlImages_Click(object sender, EventArgs e)
        {

        }
    }
}
