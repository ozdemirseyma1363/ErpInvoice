using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
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
    public partial class FaturaEdit : Window
    {
        int no = 0;
        public FaturaEdit(int No)
        {
            
            no = No;
            InitializeComponent();            
        }
        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCariNo.Text == "")
            {
                MessageBox.Show("Lütfen Cariyi Seçiniz");
                return;
            }
                FaturaEdit fe = new FaturaEdit(no);
                FaturaDetayEdit fd = new FaturaDetayEdit(0);
                fd.ShowDialog();
                //cmbCariNo.IsEnabled = false;
                txtFaturaNumara.IsEnabled = false;     
            Bind();
        }
        private void Bind()
        {
            var lst = DataTemp.TempFaturaDetay.Select(s=> new {s.No,Stok=s.Stok.Ad,s.BirimFiyat,s.Miktar,s.KdvOrani,s.Tutar,Toplam=s.KdvTutar+s.Tutar ,KdvTutar=s.KdvTutar}).ToList();
            lstGrid.ItemsSource = lst;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string barkod = "000000000000";
            txtFaturaNumara.Text ="FT"+ barkod;          
            DataModel2 dm = new DataModel2();
            cmbCariNo.ItemsSource = dm.Caris.Select(s => new { s.No, s.Ad }).OrderBy(f=>f.Ad).ToList();
            DataTemp.TempFaturaDetay = new List<FaturaDetay>();
            DataTemp.TempFaturaDetay.Clear();
            if (no > 0)
            {              
                var f = dm.Faturas.Where(q => q.No == no).FirstOrDefault();            
                txtFaturaNumara.Text = "FT" + barkod.Insert(barkod.Length, f.No.ToString());           
                f.FaturaNumarasi = txtFaturaNumara.Text;//
                cmbCariNo.SelectedValue = f.CariNo.ToString();
                txtFaturaNumara.Text = f.FaturaNumarasi.ToString();
                lblNo.Content = f.No.ToString();
                DataTemp.TempFaturaDetay =f.FaturaDetays.ToList();
            }
            else
            {
                txtFaturaNumara.Text = "FT" + barkod;
                lblNo.Content = "0";
                int fn = dm.Faturas.Any() ? dm.Faturas.Max(m => m.No) : 0;
                txtFaturaNumara.Text ="FT"+ barkod.Insert(barkod.Length, (fn+1).ToString());
            }
            Bind();
        }
        private void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            if(cmbCariNo.SelectedIndex==-1)
            {
                MessageBox.Show("Cari Seçiniz!");
                return;
            }
            if (txtFaturaNumara.Text.Length<1)
            {
                MessageBox.Show("Fatura Numarası Hatalı!");
                return;
            }
            if (DataTemp.TempFaturaDetay==null || DataTemp.TempFaturaDetay.Count==0)
            {
                MessageBox.Show("Ürün Ekleyiniz!");
                return;
            }
            DataModel2 dm = new DataModel2();
            int cariNo = Convert.ToInt16(cmbCariNo.SelectedValue.ToString());
            if (no > 0)
            {
                var f = dm.Faturas.Where(q => q.No == no).FirstOrDefault();
                f.FaturaNumarasi = (txtFaturaNumara.Text);
                f.Tarih = DateTime.Now;
                f.CariNo = cariNo;              
                f.Kdv = DataTemp.TempFaturaDetay.Sum(sm => sm.KdvTutar);
                f.Toplam = DataTemp.TempFaturaDetay.Sum(sm => sm.Tutar+sm.KdvTutar);
                foreach (var item in DataTemp.TempFaturaDetay.Where(q=>q.No<0).ToList())
                {
                    FaturaDetay fd = new FaturaDetay();
                    fd.Fatura = f;
                    fd.Tutar = item.Tutar;
                    fd.BirimFiyat = item.BirimFiyat;
                    fd.KdvTutar = item.KdvTutar;
                    fd.KdvOrani = item.KdvOrani;
                    fd.Miktar = item.Miktar;
                    fd.StokNo = item.StokNo;
                    dm.FaturaDetays.Add(fd);
                    dm.SaveChanges();
                    this.Close();
                }
                foreach (var item in DataTemp.TempFaturaDetay.Where(q => q.No > 0).ToList()){
                    var fd = dm.FaturaDetays.Where(q => q.No == item.No).FirstOrDefault();
                    fd.Tutar = item.Tutar;
                    fd.BirimFiyat = item.BirimFiyat;
                    fd.KdvTutar = item.KdvTutar;
                    fd.KdvOrani = item.KdvOrani;
                    fd.Miktar = item.Miktar;
                    fd.StokNo = item.StokNo;
                    dm.SaveChanges();
                    this.Close();
                }                
            }
            else
            {
                Fatura f = new Fatura();
                f.FaturaNumarasi =(txtFaturaNumara.Text);
                f.Tarih = DateTime.Now;
                f.CariNo = cariNo;
                f.Kdv = DataTemp.TempFaturaDetay.Sum(sm => sm.KdvTutar);
                f.Toplam = DataTemp.TempFaturaDetay.Sum(sm => sm.Tutar+sm.KdvTutar);
                dm.Faturas.Add(f);
                foreach (var item in DataTemp.TempFaturaDetay.ToList())
                {
                    FaturaDetay fd = new FaturaDetay();                  
                    fd.Fatura = f;
                    fd.Tutar = item.Tutar;
                    fd.BirimFiyat = item.BirimFiyat;
                    fd.KdvTutar = item.KdvTutar;
                    fd.KdvOrani = item.KdvOrani;
                    fd.Miktar = item.Miktar;
                    fd.StokNo = item.StokNo;
                    dm.FaturaDetays.Add(fd);
                    dm.SaveChanges();
                    this.Close();
                }
            }
        }        
        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            DataModel2 dm = new DataModel2();

            if (lstGrid.Items.Count == 0)
            {
                MessageBox.Show("Silnecek kayıt bulunamamıştır");
                return;
            }    
            if (lstGrid.SelectedIndex ==-1)
            {
                MessageBox.Show("Lütfen silinecek kaydı seçiniz");
                return;
            }        
            if (no > 0)
            {
                int l = 0;
                var p = int.TryParse(noTxt.Text, out l);
                if (l < 0)
                {
                    TxtNo.Text = lstGrid.SelectedIndex.ToString();
                    int index = Convert.ToInt32(TxtNo.Text);
                    var dt = DataTemp.TempFaturaDetay.Remove(DataTemp.TempFaturaDetay[index]);
                    Bind();
                }
                if (l > 0)
                {
                    var k = dm.FaturaDetays.Where(q => q.No == l).FirstOrDefault();
                    dm.FaturaDetays.Remove(k);
                    var s = DataTemp.TempFaturaDetay.Where(q => q.No == l).FirstOrDefault();
                    DataTemp.TempFaturaDetay.Remove(s);
                    dm.SaveChanges();
                    Bind();
                }
            }
            else
            {
                TxtNo.Text = lstGrid.SelectedIndex.ToString();
                int index = Convert.ToInt32(TxtNo.Text);
                var dt = DataTemp.TempFaturaDetay.Remove(DataTemp.TempFaturaDetay[index]);
                Bind();
            }
                //var u = DataTemp.TempFaturaDetay.Where(q => q.No == l).FirstOrDefault();
                //DataTemp.TempFaturaDetay.Remove(u);
                //if (l < 0)
                //{
                //    TxtNo.Text = lstGrid.SelectedIndex.ToString();
                //    int index = Convert.ToInt32(TxtNo.Text);
                //    var dt = DataTemp.TempFaturaDetay.Remove(DataTemp.TempFaturaDetay[index]);
                //    Bind();
                //}




                ////dm.SaveChanges();

                ////Bind();       
            }
        

        private void lstGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (lstGrid.SelectedIndex != -1)
            {
                TxtNo.Text = lstGrid.SelectedIndex.ToString();
                noTxt.Text = lstGrid.SelectedValue.ToString();
            }

       
           
            //if (lstGrid.SelectedIndex != -1)
            //{
            //    int index = Convert.ToInt32(TxtNo.Text);
              
            //}

            //DataModel2 dm = new DataModel2();
            //int No = 0;
            //if (lstGrid.SelectedItem != null)
            //{
            //    int.TryParse(lstGrid.SelectedValue.ToString(), out No);
            //}
            //if (No != 0)
            //{
            //    var u = dm.FaturaDetays.Where(q => q.No == No).FirstOrDefault();
            //    TxtNo.Text = u.No.ToString();
            //}




        }
       
        private void btnDuzenle_Click(object sender, RoutedEventArgs e)
        {
            if (lstGrid.Items.Count == 0)
            {
                MessageBox.Show("Düzenlenecek kayıt bulunamamıştır");
                return;
            }
            DataModel2 dm = new DataModel2();
            if (lstGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen düzenlenecek kaydı seçiniz");
                return;
            }
            if (lstGrid.SelectedIndex != -1)
            {
                int no = (int)lstGrid.SelectedValue;
                FaturaDetayEdit fe = new FaturaDetayEdit(no);
                var x = fe.ShowDialog();
                Bind();
            }
          
        }
        private void txtFaturaNumara_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }



  
    }
}
