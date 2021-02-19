using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurboAz_App.Classes;
using TurboAz_App.Utils;

namespace TurboAz_App.Forms
{
    public partial class ADS_Add : Form
    {
        public ADS_Add()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        SetCarInfo setCarInfo = new SetCarInfo();
        GetCarInfo getCarInfo = new GetCarInfo();
        private void ADS_Add_Load(object sender, EventArgs e)
        {

            setCarInfo.SetCarBrandData(lkUpEdtBrand);
            setCarInfo.SetCarEngineSize(lkUpEdtEngineCapacity);
            setCarInfo.SetGeneralInfo(lkUpEdtBanType, 1);
            setCarInfo.SetGeneralInfo(lkUpEdtColor, 2);
            setCarInfo.SetGeneralInfo(lkUpEdtFuelType, 4);
            setCarInfo.SetGeneralInfo(lkUpEdtTransmission, 5);
            setCarInfo.SetGeneralInfo(lkUpEdtGearbox, 6);
            setCarInfo.SetGeneralInfo(lkUpEdtCity, 7);
            setCarInfo.SetCarYear(lkUpEdtYear);
             grdCntrlImage.DataSource = getCarInfo.GetImages("-1");
        }

        public bool EmptyControl()
        {
            bool control = true;
            if (lkUpEdtBrand.EditValue == null)
            {
                lkUpEdtBrand.ErrorText = "Markanı daxil edin!";
                control = false;
            }
            if (lkUpEdtModel.EditValue == null)
            {
                lkUpEdtModel.ErrorText = "Modeli daxil edin!";
                control = false;
            }
            if (lkUpEdtBanType.EditValue == null)
            {
                lkUpEdtBanType.ErrorText = "Ban növünü daxil edin!";
                control = false;
            }
            if ((decimal)spnEdtWalk.EditValue == 0)
            {
                spnEdtWalk.ErrorText = "Yürüşü daxil edin!";
                control = false;
            }
            if (lkUpEdtColor.EditValue == null)
            {
                lkUpEdtColor.ErrorText = "Rəngini daxil edin!";
                control = false;
            }
            if ((decimal)spnEdtPrice.EditValue == 0)
            {
                spnEdtPrice.ErrorText = "Qiyməti daxil edin!";
                control = false;
            }
            if (lkUpEdtFuelType.EditValue == null)
            {
                lkUpEdtFuelType.ErrorText = "Yanacaq növünü daxil edin!";
                control = false;
            }
            if (lkUpEdtTransmission.EditValue == null)
            {
                lkUpEdtTransmission.ErrorText = "Ötürücünü daxil edin!";
                control = false;
            }
            if (lkUpEdtGearbox.EditValue == null)
            {
                lkUpEdtGearbox.ErrorText = "Sürətlər qutusunu daxil edin!";
                control = false;
            }
            if (lkUpEdtYear.EditValue == null)
            {
                lkUpEdtYear.ErrorText = "Buraxılış ilini daxil edin!";
                control = false;
            }
            if (lkUpEdtEngineCapacity.EditValue == null)
            {
                lkUpEdtEngineCapacity.ErrorText = "Mühərrikin həcmini daxil edin!";
                control = false;
            }
            if ((decimal)spnEdtEnginePower.EditValue == 0)
            {
                spnEdtEnginePower.ErrorText = "Mühərrikin gücünü daxil edin!";
                control = false;
            }
            if (txtName.Text.Trim() == "")
            {
                txtName.ErrorText = "Adınızı daxil edin!";
                control = false;
            }
            if (lkUpEdtCity.EditValue == null)
            {
                lkUpEdtCity.ErrorText = "Şəhəri daxil edin!";
                control = false;
            }
            if (txtEdtEmail.Text.Trim() == "")
            {
                txtEdtEmail.ErrorText = "E-maili daxil edin!";
                control = false;
            }
            if (crdVwImages.DataRowCount < 3)
            {
                MessageBox.Show("Minimum üç şəkil əlavə edilməlidir!");
                control = false;
            }
            return control;
        }

        private void lkUpEdtBrands_EditValueChanged(object sender, EventArgs e)
        {
            setCarInfo.SetCarModelData(lkUpEdtModel, (int)lkUpEdtBrand.EditValue);
        }

        private byte[] GetByteImage(string filename)
        {
            byte[] imgByteArray = null;
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            imgByteArray = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            return imgByteArray;
        }

       
        private void InsertADSImage(SqlTransaction sqlTransaction, string adsID)
        {
            DataTable dataTableimg = (DataTable)grdCntrlImage.DataSource;
            for (int i = 0; i < dataTableimg.Rows.Count; i++)
            {
                DataRow dataRowimg = dataTableimg.Rows[i];
                string query = @"INSERT INTO [dbo].[Car_Images]
           ([Car_Image]
           ,[Ads_ID])
     VALUES
           (@Car_Image
           ,@Ads_ID)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlTransaction.Connection);
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.Parameters.Add("Car_Image", SqlDbType.VarBinary).Value = dataRowimg["CAR_IMAGE"];
                sqlCommand.Parameters.Add("Ads_ID", SqlDbType.Int).Value = adsID;
                sqlCommand.ExecuteNonQuery();
            }
        }
            

        private void InsertAllInfo()
        {
            SqlTransaction sqlTransaction = null;
            try
            {

                SqlConnection sqlConnection = new SqlConnection(SqlUtils.conString);
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                string insertID = InsertInfo(sqlTransaction);
                InsertADSImage(sqlTransaction, insertID);
                sqlTransaction.Commit();
                sqlConnection.Close();
                MessageBox.Show("Melumat yadda saxlanildi!!!!");
                this.Close();

            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                MessageBox.Show("Melumat yadda saxlanilan zaman xeta bas verdi!!!" + ex);
            }
        }

        private string InsertInfo(SqlTransaction sqlTransaction)
        {
            string query = @"INSERT INTO [dbo].[Car_ADS]
           ([Brand_ID]
           ,[Model_ID]
           ,[Ban_Type_ID]
           ,[Walk]
           ,[Color_ID]
           ,[Price]
           ,[Currency_ID]
           ,[Fuel_Type_ID]
           ,[Transmission_ID]
           ,[Gearbox_ID]
           ,[Year]
           ,[Engine_Capacity]
           ,[Engine_Power]
           ,[Credit]
           ,[Barter]
           ,[Note]
           ,[Alloy_wheels]
           ,[Central_closure]
           ,[Leather_salon]
           ,[Seat_ventilation]
           ,[ABS]
           ,[Parking_radar]
           ,[Conditioners]
           ,[Xenon_lamps]
           ,[Luke]
           ,[Rear_view_camera]
           ,[Rain_sensor]
           ,[Seat_heating]
           ,[Side_curtains]
           ,[Name]
           ,[City_ID]
           ,[E_mail])
     VALUES
           (@Brand_ID
           , @Model_ID
           , @Ban_Type_ID
           , @Walk
           , @Color_ID
           , @Price
           , @Currency_ID
           , @Fuel_Type_ID
           , @Transmission_ID
           , @Gearbox_ID
           , @Year
           , @Engine_Capacity
           , @Engine_Power
           , @Credit
           , @Barter
           , @Note
           , @Alloy_wheels
           , @Central_closure
           , @Leather_salon
           , @Seat_ventilation
           , @ABS
           , @Parking_radar
           , @Conditioners
           , @Xenon_lamps
           , @Luke
           , @Rear_view_camera
           , @Rain_sensor
           , @Seat_heating
           , @Side_curtains
           , @Name
           , @City_ID
           , @E_mail)SELECT SCOPE_IDENTITY();";
            SqlCommand sqlCommand = new SqlCommand(query, sqlTransaction.Connection);
            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.Parameters.Add("Brand_ID", SqlDbType.Int).Value = lkUpEdtBrand.EditValue;
            sqlCommand.Parameters.Add("Model_ID", SqlDbType.Int).Value = lkUpEdtModel.EditValue;
            sqlCommand.Parameters.Add("Ban_Type_ID", SqlDbType.Int).Value = lkUpEdtBanType.EditValue;
            sqlCommand.Parameters.Add("Walk", SqlDbType.Int).Value = spnEdtWalk.EditValue;
            sqlCommand.Parameters.Add("Color_ID", SqlDbType.Int).Value = lkUpEdtColor.EditValue;
            sqlCommand.Parameters.Add("Price", SqlDbType.Int).Value = spnEdtPrice.EditValue;
            sqlCommand.Parameters.Add("Currency_ID", SqlDbType.Int).Value = rdGrpCurrency.EditValue;
            sqlCommand.Parameters.Add("Fuel_Type_ID", SqlDbType.Int).Value = lkUpEdtFuelType.EditValue;
            sqlCommand.Parameters.Add("Transmission_ID", SqlDbType.Int).Value = lkUpEdtTransmission.EditValue;
            sqlCommand.Parameters.Add("Gearbox_ID", SqlDbType.Int).Value = lkUpEdtGearbox.EditValue;
            sqlCommand.Parameters.Add("Year", SqlDbType.Int).Value = lkUpEdtYear.EditValue;
            sqlCommand.Parameters.Add("Engine_Capacity", SqlDbType.Int).Value = lkUpEdtEngineCapacity.EditValue;
            sqlCommand.Parameters.Add("Engine_Power", SqlDbType.Int).Value = spnEdtEnginePower.EditValue;
            sqlCommand.Parameters.Add("Credit", SqlDbType.Bit).Value = chckEdtCredit.EditValue;
            sqlCommand.Parameters.Add("Barter", SqlDbType.Bit).Value = chckEdtBarter.EditValue;
            sqlCommand.Parameters.Add("Note", SqlDbType.NVarChar).Value = memoEdtNote.EditValue;
            sqlCommand.Parameters.Add("Alloy_wheels", SqlDbType.Bit).Value = chckEdtAlloyWhells.EditValue;
            sqlCommand.Parameters.Add("Central_closure", SqlDbType.Bit).Value = chckEdtCentralClosure.EditValue;
            sqlCommand.Parameters.Add("Leather_salon", SqlDbType.Bit).Value = chckEdtLeatherSalon.EditValue;
            sqlCommand.Parameters.Add("Seat_ventilation", SqlDbType.Bit).Value = chckEdtSeatVentilation.EditValue;
            sqlCommand.Parameters.Add("ABS", SqlDbType.Bit).Value = chckEdtABS.EditValue;
            sqlCommand.Parameters.Add("Parking_radar", SqlDbType.Bit).Value = chckedtParkingRadar.EditValue;
            sqlCommand.Parameters.Add("Conditioners", SqlDbType.Bit).Value = chckEdtConditioners.EditValue;
            sqlCommand.Parameters.Add("Xenon_lamps", SqlDbType.Bit).Value = chckEdtXenonLamps.EditValue;
            sqlCommand.Parameters.Add("Luke", SqlDbType.Bit).Value = chckEdtLuke.EditValue;
            sqlCommand.Parameters.Add("Rear_view_camera", SqlDbType.Bit).Value = chckEdtRearViewCamera.EditValue;
            sqlCommand.Parameters.Add("Rain_sensor", SqlDbType.Bit).Value = chckEdtRainSensor.EditValue;
            sqlCommand.Parameters.Add("Seat_heating", SqlDbType.Bit).Value = chckedtSeatHeating.EditValue;
            sqlCommand.Parameters.Add("Side_curtains", SqlDbType.Bit).Value = chckEdtSideCurtains.EditValue;
            sqlCommand.Parameters.Add("Name", SqlDbType.NVarChar).Value = txtName.EditValue;
            sqlCommand.Parameters.Add("City_ID", SqlDbType.Int).Value = lkUpEdtCity.EditValue;
            sqlCommand.Parameters.Add("E_mail", SqlDbType.NVarChar).Value = txtEdtEmail.EditValue;
            return sqlCommand.ExecuteScalar().ToString();
        }

        private void btnAddAds_Click(object sender, EventArgs e)
        {
            if (EmptyControl())
            {
                InsertAllInfo();
            }
        }

        private void btnAddImage_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files | *.jpg";
            DataTable dataTableimage = (DataTable)grdCntrlImage.DataSource;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    dataTableimage.Rows.Add(0, GetByteImage(filename));
                    GetByteImage(filename);
                }
            }

        }
    }
}