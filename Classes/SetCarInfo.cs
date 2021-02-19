using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboAz_App.Classes
{
    class SetCarInfo
    {
        GetCarInfo getCarInfo = new GetCarInfo();
        public void SetCarBrandData(LookUpEdit lkUpEdtCarBrand)
        {
            lkUpEdtCarBrand.Properties.DataSource = getCarInfo.GetCarBrand();
            lkUpEdtCarBrand.Properties.DisplayMember = "Brand_Name";
            lkUpEdtCarBrand.Properties.ValueMember = "ID";
        }
        public void SetCarModelData(LookUpEdit lkUpEdtCarModel, int brandID)
        {
            lkUpEdtCarModel.Properties.DataSource = getCarInfo.GetCarModel(brandID);
            lkUpEdtCarModel.Properties.DisplayMember = "Model_Name";
            lkUpEdtCarModel.Properties.ValueMember = "ID";
        }

        public void SetGeneralInfo(LookUpEdit lkUpEdtCarModel, int typeID)
        {
            lkUpEdtCarModel.Properties.DataSource = getCarInfo.GetCarGeneralData(typeID);
            lkUpEdtCarModel.Properties.DisplayMember = "Type_Name";
            lkUpEdtCarModel.Properties.ValueMember = "ID";
        }
        public void SetCarYearData(LookUpEdit lkUpEdtCarYear)
        {
            lkUpEdtCarYear.Properties.DataSource = getCarInfo.GetCarBrand();
            lkUpEdtCarYear.Properties.DisplayMember = "Brand_Name";
            lkUpEdtCarYear.Properties.ValueMember = "ID";
           
        }
        public void SetCarYear(LookUpEdit lkUpEdtCarYear)
        {
            List<int> yearlist = new List<int>();
            for (int i = DateTime.Now.Year; i >= 1960; i--)
            {
                yearlist.Add(i);
            }
            lkUpEdtCarYear.Properties.DataSource = yearlist;
        }
        public void SetCarEngineSize(LookUpEdit lkUpEdtEngineSize)
        {
            List<int> sizelist = new List<int>();
            for (int i = 50; i <= 160000; i+=50)
            {
                sizelist.Add(i);
            }
            lkUpEdtEngineSize.Properties.DataSource = sizelist;
        }
    }
}
