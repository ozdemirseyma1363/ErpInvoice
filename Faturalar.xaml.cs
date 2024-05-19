using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErpFatura
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Faturalar : Window
    {
        public Faturalar()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Bind();//sayfa yüklenirken sırala
        }
        private void Bind()
        {
            DataModel2 dm = new DataModel2();
            lstGrid.ItemsSource = dm.Faturas.Select(s => new { s.No, FaturaNumara=s.FaturaNumarasi, Cari = s.Cari.Ad, s.Tarih,Tutar=s.Toplam }).ToList();//data mdelden belirli şeyleri getir ve listele
        }
        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {

            FaturaEdit fe = new FaturaEdit(0);
            var x = fe.ShowDialog();
            Bind();
        }
        private void lstGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataModel2 dm = new DataModel2();
            int No = 0;
            if (lstGrid.SelectedItem != null)
            {
                int.TryParse(lstGrid.SelectedValue.ToString(), out No);
            }
            if (No != 0)
            {
                var u = dm.FaturaDetays.Where(q => q.No == No).FirstOrDefault();
                //TxtNo.Text = u.No.ToString();
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DataModel2 dm = new DataModel2();
            if(lstGrid.SelectedItem ==null)
            {
                MessageBox.Show("Lütfen düzenlenecek kaydı seçiniz");
                return;
            }
            if (dm.Faturas.Count() == 0)
            {
                MessageBox.Show("Düzenlenecek Kayıt Bulunamamıştır.");
                return;
            }
            if (lstGrid.SelectedValue != null)
            {
                int no = (int)lstGrid.SelectedValue;
                FaturaEdit fe = new FaturaEdit(no);
                if (no == 0)
                {
                    fe.cmbCariNo.IsEnabled = true;

                }
                else
                {
                    //fe.cmbCariNo.IsEnabled = false;
                }
                var x = fe.ShowDialog();
                Bind();
            }
            
   
        }
        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (lstGrid.Items.Count == 0)
            {
                MessageBox.Show("Silinecek Kayıt Bulunamamıştır.");
                return;
            }
            if (lstGrid.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Silinecek Kaydı Seçiniz");
                return;
            }
            int no = (int)lstGrid.SelectedValue;
            DataModel2 dm = new DataModel2();
            if (lstGrid.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Silinecek Kaydı Seçiniz");
                return;
            }
            if (dm.Faturas.Count() == 0)
            {
                MessageBox.Show("Silinecek Kayıt Bulunamamıştır.");
                return;
            }
    
            var dr = MessageBox.Show("Silmek istediğinizden emin misiniz?", "Sor!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dr == MessageBoxResult.Yes)
            {
                var ftr = dm.Faturas.Where(q => q.No == no).FirstOrDefault();//
              
                foreach (var item in ftr.FaturaDetays.ToList())
                {
                    dm.FaturaDetays.Remove(item);
                }
            
                dm.Faturas.Remove(ftr);

                try
                {
                    dm.SaveChanges();
                    
                    Bind();
                    MessageBox.Show("Silindi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
              
            }
        }
        private void btnBul_Click(object sender, RoutedEventArgs e)
        {
            string find = txtBul.Text.ToLower();
            DataModel2 dm =new DataModel2();
            
            var lst = dm.Faturas.Where(q => q.Cari.Ad.ToLower().Contains(find) || q.FaturaNumarasi.ToLower().Contains(find)||q.FaturaDetays.Any(m=>m.Stok.Ad.Contains(find))).OrderBy(o => o.Cari.Soyad).ThenBy(od => od.FaturaNumarasi).Select(s => new { s.No, FaturaNumara = s.FaturaNumarasi, Cari = s.Cari.Ad, s.Tarih, Tutar = s.Toplam }).ToList();                      
            lstGrid.ItemsSource = lst;
        }
        private void txtBul_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                btnBul_Click(btnBul, e);
            }
        }

        private void btnSirala_Click(object sender, RoutedEventArgs e)
        {
            Bind();
        }
    }
}
