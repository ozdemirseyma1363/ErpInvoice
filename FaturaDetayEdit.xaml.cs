using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace ErpFatura
{
    /// <summary>
    /// Interaction logic for FaturaEdit.xaml
    /// </summary>
    public partial class FaturaDetayEdit : Window
    {
        int no = 0;
        public FaturaDetayEdit(int No)
        {
            no = No;
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            /*           
             *                       FaturaEdit edit = new FaturaEdit(0);
            edit.TxtNo.Text =edit.lstGrid.SelectedIndex.ToString();
            int k = Convert.ToInt32(edit.TxtNo.Text);
             *           if (edit.TxtNo.Text != "-1")
            {

                txtBirimFiyat.Text = Convert.ToDecimal(DataTemp.TempFaturaDetay[k].BirimFiyat).ToString();
                cmbStokNo.Text = Convert.ToDecimal(DataTemp.TempFaturaDetay[k].Stok.Ad).ToString();
                txtMiktar.Text = Convert.ToDecimal(DataTemp.TempFaturaDetay[k].Miktar).ToString();
                txtKdvOrani.Text = Convert.ToDecimal(DataTemp.TempFaturaDetay[k].KdvOrani).ToString();
*/

            FaturaEdit edit = new FaturaEdit(0);
            DataModel2 dm = new DataModel2();
            edit.TxtNo.Text = edit.lstGrid.SelectedIndex.ToString();
            int k = Convert.ToInt32(edit.TxtNo.Text);

            cmbStokNo.ItemsSource = dm.Stoks.Select(s => new { s.No, s.Ad }).ToList();
            if (no != 0)
            {
                var f = DataTemp.TempFaturaDetay.Where(q => q.No == no).FirstOrDefault();
                cmbStokNo.SelectedValue = f.StokNo.ToString();
                lblNo.Content = f.No.ToString();
                txtBirimFiyat.Text = f.BirimFiyat.Value.ToString();
                txtKdvOrani.Text = f.KdvOrani.Value.ToString();
                txtMiktar.Text = f.Miktar.ToString();           
            }
            else
            {
                lblNo.Content = "0";
            }
            }       
        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {

            DataModel2 dm = new DataModel2();
            int stokNo = Convert.ToInt16(cmbStokNo.SelectedValue.ToString());
            if (no!=0)
            {               
                var fd = DataTemp.TempFaturaDetay.Where(q => q.No == no).FirstOrDefault();
                fd.KdvOrani = Convert.ToDecimal(txtKdvOrani.Text);
                fd.BirimFiyat= Convert.ToDecimal(txtBirimFiyat.Text);
                fd.Miktar = Convert.ToDecimal(txtMiktar.Text);
                fd.StokNo = stokNo;
                fd.Tutar = fd.BirimFiyat * fd.Miktar;
                fd.KdvTutar = fd.Tutar * (fd.KdvOrani) / 100;
                fd.Stok.Ad = cmbStokNo.Text;        
            }
            else 
            {
                FaturaDetay fd = new FaturaDetay();
                fd.No = -(DataTemp.TempFaturaDetay.Count+1);
                fd.KdvOrani = Convert.ToDecimal(txtKdvOrani.Text);
                fd.BirimFiyat = Convert.ToDecimal(txtBirimFiyat.Text);
                fd.Miktar = Convert.ToDecimal(txtMiktar.Text);
                fd.StokNo = stokNo;
                fd.Tutar = fd.BirimFiyat * fd.Miktar;
                fd.KdvTutar = fd.Tutar * (fd.KdvOrani) / 100;
                fd.Stok = dm.Stoks.Where(q => q.No == stokNo).FirstOrDefault();
                DataTemp.TempFaturaDetay.Add(fd);
            }
       


            this.Close();
        }
        private void cmbStokNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbStokNo.SelectedValue.ToString()))
            {
                DataModel2 dm = new DataModel2();
                int stokNo = Convert.ToInt16(cmbStokNo.SelectedValue.ToString());
                var st = dm.Stoks.Where(q => q.No == stokNo).FirstOrDefault();
                txtBirimFiyat.Text = st.BirimFiyat.ToString();
                txtKdvOrani.Text = "18";
                txtMiktar.Text = "1";
            }
            
            
        }

        private void txtBirimFiyat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9.]"))
            {
                e.Handled = true;
            }
        }

        private void txtMiktar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9.]"))
            {
                e.Handled = true;
            }
        }

        private void txtKdvOrani_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9.]"))
            {
                e.Handled = true;
            }
        }
    }
}
