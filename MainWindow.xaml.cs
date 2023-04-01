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

using System.IO;

using Microsoft.Win32;
using System.Security.Cryptography;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;

namespace nhom7_RSA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rd_tdRSA.IsChecked = true;
            rd_tcRSA.IsChecked = false;
            rsa_maHoaBanRoMoi.IsEnabled = false;
            btChonFileKy_RSA.IsEnabled = btChonFileKy_RSA.IsEnabled = btChonFileXacThuc_RSA.IsEnabled = false;

        }


        public int danhDau = 0;

        private bool nguyenToCungNhau(int ai, int bi)// "Hàm kiểm tra hai số nguyên tố cùng nhau"
        {
            bool ktx_;
            // giải thuật Euclid;
            int temp;
            while (bi != 0)
            {
                temp = ai % bi;
                ai = bi;
                bi = temp;
            }
            if (ai == 1) { ktx_ = true; }
            else ktx_ = false;
            return ktx_;
        }

        #region Code mã hóa RSA
        private void reset_rsa()
        {
            rsa_soP.Text = rsa_soQ.Text = rsa_soPhiN.Text = rsa_soN.Text = rsa_soE.Text = rsa_soD.Text = string.Empty;

        }

        int RSA_soP, RSA_soQ, RSA_soN, RSA_soE, RSA_soD, RSA_soPhi_n;
        public int RSA_d_dau = 0;
        private int RSA_ChonSoNgauNhien()
        {
            Random rd = new Random();
            return rd.Next(11, 101);// tốc độ chậm nên chọn số bé
        }
        //"Hàm kiểm tra nguyên tố"
        private bool RSA_kiemTraNguyenTo(int xi)
        {
            bool kiemtra = true;
            if (xi == 2 || xi == 3)
            {
                // kiemtra = true;
                return kiemtra;
            }
            else
            {
                if (xi == 1 || xi % 2 == 0 || xi % 3 == 0)
                {
                    kiemtra = false;
                }
                else
                {
                    for (int i = 5; i <= Math.Sqrt(xi); i = i + 6)
                        if (xi % i == 0 || xi % (i + 2) == 0)
                        {
                            kiemtra = false;
                            break;
                        }
                }
            }
            return kiemtra;
        }
        // "Hàm kiểm tra hai số nguyên tố cùng nhau"
        private bool RSA_nguyenToCungNhau(int ai, int bi)
        {
            bool ktx_;
            // giải thuật Euclid;
            int temp;
            while (bi != 0)
            {
                temp = ai % bi;
                ai = bi;
                bi = temp;
            }
            if (ai == 1) { ktx_ = true; }
            else ktx_ = false;
            return ktx_;
        }
        // "Hàm lấy mod"
        public int RSA_mod(int mx, int ex, int nx)
        {

            //Sử dụng thuật toán "bình phương nhân"
            //Chuyển e sang hệ nhị phân
            int[] a = new int[100];
            int k = 0;
            do
            {
                a[k] = ex % 2;
                k++;
                ex = ex / 2;
            }
            while (ex != 0);
            //Quá trình lấy dư
            int kq = 1;
            for (int i = k - 1; i >= 0; i--)
            {
                kq = (kq * kq) % nx;
                if (a[i] == 1)
                    kq = (kq * mx) % nx;
            }
            return kq;
        }

        private void RSA_taoKhoa()
        {
            //Tinh n=p*q
            RSA_soN = RSA_soP * RSA_soQ;
            rsa_soN.Text = RSA_soN.ToString();
            //Tính Phi(n)=(p-1)*(q-1)
            RSA_soPhi_n = (RSA_soP - 1) * (RSA_soQ - 1);
            rsa_soPhiN.Text = RSA_soPhi_n.ToString();
            //Tính e là một số ngẫu nhiên có giá trị 0< e <phi(n) và là số nguyên tố cùng nhau với Phi(n)
            do
            {
                Random RSA_rd = new Random();
                RSA_soE = RSA_rd.Next(2, RSA_soPhi_n);
            }
            while (!nguyenToCungNhau(RSA_soE, RSA_soPhi_n));
            rsa_soE.Text = RSA_soE.ToString();

            //Tính d là nghịch đảo modular của e
            RSA_soD = 0;
            int i = 2;
            while (((1 + i * RSA_soPhi_n) % RSA_soE) != 0 || RSA_soD <= 0)
            {
                i++;
                RSA_soD = (1 + i * RSA_soPhi_n) / RSA_soE;
            }
            rsa_soD.Text = RSA_soD.ToString();
        }
        public void RSA_MaHoa(string ChuoiVao)
        {
            // taoKhoa();
            // Chuyen xau thanh ma Unicode
            byte[] mh_temp1 = Encoding.Unicode.GetBytes(ChuoiVao);
            string base64 = Convert.ToBase64String(mh_temp1);

            // Chuyen xau thanh ma Unicode
            int[] mh_temp2 = new int[base64.Length];
            for (int i = 0; i < base64.Length; i++)
            {
                mh_temp2[i] = (int)base64[i];
            }

            //Mảng a chứa các kí tự đã mã hóa
            int[] mh_temp3 = new int[mh_temp2.Length];
            for (int i = 0; i < mh_temp2.Length; i++)
            {
                mh_temp3[i] = RSA_mod(mh_temp2[i], RSA_soE, RSA_soN); // mã hóa
            }

            //Chuyển sang kiểu kí tự trong bảng mã Unicode
            string str = "";
            for (int i = 0; i < mh_temp3.Length; i++)
            {
                str = str + (char)mh_temp3[i];
            }
            byte[] data = Encoding.Unicode.GetBytes(str);
            rsa_BanMaHoa.Text = Convert.ToBase64String(data);
            rsa_banMaHoaGuiDen.Text = Convert.ToBase64String(data);

        }
        // hàm giải mã
        public void RSA_GiaiMa(string ChuoiVao)
        {
            byte[] temp2 = Convert.FromBase64String(ChuoiVao);
            string giaima = Encoding.Unicode.GetString(temp2);

            int[] b = new int[giaima.Length];
            for (int i = 0; i < giaima.Length; i++)
            {
                b[i] = (int)giaima[i];
            }
            //Giải mã
            int[] c = new int[b.Length];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = RSA_mod(b[i], RSA_soD, RSA_soN);// giải mã
            }

            string str = "";
            for (int i = 0; i < c.Length; i++)
            {
                str = str + (char)c[i];
            }
            byte[] data2 = Convert.FromBase64String(str);
            rsa_banGiaiMa.Text = Encoding.Unicode.GetString(data2);

        }
        private void rsa_TaoKhoa_Click(object sender, RoutedEventArgs e)
        {

            if (rd_tdRSA.IsChecked == true && rd_tcRSA.IsChecked == false)
            {
                reset_rsa();
                RSA_soP = RSA_soQ = 0;
                do
                {
                    RSA_soP = RSA_ChonSoNgauNhien();
                    RSA_soQ = RSA_ChonSoNgauNhien();
                }
                while (RSA_soP == RSA_soQ || !RSA_kiemTraNguyenTo(RSA_soP) || !RSA_kiemTraNguyenTo(RSA_soQ));
                rsa_soP.Text = RSA_soP.ToString();
                rsa_soQ.Text = RSA_soQ.ToString();
                RSA_taoKhoa();
                RSA_d_dau = 1;
                rsa_TaoKhoa.Content = "Tạo lại khóa mới";
                rsa_TaoKhoa.IsEnabled = false;
                rd_tcRSA.IsEnabled = false;
                rd_tdRSA.IsEnabled = false;
                rsa_btMaHoa.IsEnabled = true;
            }
            else
            {
                if (rd_tdRSA.IsChecked == false && rd_tcRSA.IsChecked == true)
                {
                    if (rsa_soP.Text == "" || rsa_soQ.Text == "")
                        MessageBox.Show("Phải nhập đủ 2 số p, q", "Thông Báo ", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        RSA_soP = int.Parse(rsa_soP.Text);
                        RSA_soQ = int.Parse(rsa_soQ.Text);
                        if (RSA_soP == RSA_soQ)
                        {
                            MessageBox.Show("Nhập 2 số nguyên tố khác nhau ", " Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            rsa_soQ.Focus();
                        }
                        else
                        {
                            if (!RSA_kiemTraNguyenTo(RSA_soP) || RSA_soP <= 1)
                            {
                                MessageBox.Show("[p] phải là số nguyên tố lớn hơn 1. Xin nhập lại ", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                                rsa_soP.Focus();
                            }
                            else
                            {
                                if (!RSA_kiemTraNguyenTo(RSA_soQ) || RSA_soQ <= 1)
                                {
                                    MessageBox.Show("[q] phải là số nguyên tố lớn hơn 1. Xin nhập lại ", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                                    rsa_soQ.Focus();
                                }
                                else
                                {
                                    RSA_taoKhoa();
                                    rsa_soP.Text = RSA_soP.ToString();
                                    rsa_soQ.Text = RSA_soQ.ToString();
                                    RSA_d_dau = 1;
                                    //bt_taokhoaTuychonMoi.Visible = true;
                                    rsa_TaoKhoa.IsEnabled = false;
                                    rsa_btMaHoa.IsEnabled = true;
                                }
                            }
                        }
                    }

                }
            }
        }

        private void rsa_btMaHoa_Click(object sender, RoutedEventArgs e)
        {
            if (RSA_d_dau != 1)
            { MessageBox.Show("Bạn chưa tạo khóa!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information); }

            else
            {
                if (rsa_BanRo.Text == "")
                {
                    MessageBox.Show("Bạn chưa nhập bản rõ để mã hóa!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    // thực hiện mã hóa
                    try
                    {
                        RSA_MaHoa(rsa_BanRo.Text);
                        rsa_btMaHoa.IsEnabled = false;
                        rsa_btGiaiMa.IsEnabled = true;
                        RSA_d_dau = 2;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void rsa_btGiaiMa_Click(object sender, RoutedEventArgs e)
        {

            if (RSA_d_dau != 2)
                MessageBox.Show("Bạn phải tạo khóa trước ", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                try
                {
                    RSA_GiaiMa(rsa_BanMaHoa.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            rsa_btGiaiMa.IsEnabled = false;
            RSA_d_dau = 1;
            rsa_maHoaBanRoMoi.IsEnabled = true;
        }


        private void rd_tdRSA_Checked(object sender, RoutedEventArgs e)
        {
            rsa_TaoKhoa.IsEnabled = true;
            rsa_soP.Text = rsa_soQ.Text = rsa_soPhiN.Text = rsa_soN.Text = rsa_soE.Text = rsa_soD.Text = string.Empty;
            rsa_soP.IsEnabled = rsa_soQ.IsEnabled = rsa_soPhiN.IsEnabled = rsa_soN.IsEnabled = rsa_soE.IsEnabled = rsa_soD.IsEnabled = false;

        }

        private void rd_tcRSA_Checked(object sender, RoutedEventArgs e)
        {
            rsa_TaoKhoa.IsEnabled = true;
            rsa_soP.Text = rsa_soQ.Text = rsa_soPhiN.Text = rsa_soN.Text = rsa_soE.Text = rsa_soD.Text = string.Empty;
            rsa_soP.IsEnabled = rsa_soQ.IsEnabled = rsa_soPhiN.IsEnabled = rsa_soN.IsEnabled = rsa_soE.IsEnabled = rsa_soD.IsEnabled = true;
        }


        private void rsa_maHoaBanRoMoi_Click(object sender, RoutedEventArgs e)
        {
            rsa_btMaHoa.IsEnabled = true;
            rsa_BanRo.Text = rsa_BanMaHoa.Text = rsa_banMaHoaGuiDen.Text = rsa_banGiaiMa.Text = string.Empty;
            RSA_d_dau = 1;
            rsa_maHoaBanRoMoi.IsEnabled = false;
        }

        private void rsa_btThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rsa_TaoKhoaMoi_Click(object sender, RoutedEventArgs e)
        {
            rsa_maHoaBanRoMoi.IsEnabled = false;
            RSA_d_dau = 0;
            rsa_TaoKhoa.IsEnabled = true;
            rd_tdRSA.IsEnabled = true;
            rd_tdRSA.IsChecked = true;
            rd_tcRSA.IsEnabled = true;
            rd_tcRSA.IsChecked = false;
            rsa_soP.Text = rsa_soQ.Text = rsa_soPhiN.Text = rsa_soN.Text = rsa_soE.Text = rsa_soD.Text = string.Empty;

            rsa_banGiaiMa.Text = rsa_BanMaHoa.Text = rsa_BanRo.Text = rsa_banMaHoaGuiDen.Text = string.Empty;
            rsa_btGiaiMa.IsEnabled = false; rsa_btMaHoa.IsEnabled = false;

        }

        private void rsa_soP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "." && IsNumber(e.Text) == false)
            {
                e.Handled = true;
            }
            else if (e.Text == ".")
            {
                if (((TextBox)sender).Text.IndexOf(e.Text) > -1)
                {
                    e.Handled = true;
                }
            }
        }
        private bool IsNumber(string Text_x)
        {
            int outPut;
            return int.TryParse(Text_x, out outPut);
        }
        #endregion
        #region Code chữ ký RSA

        private void F_TaoKhoa()
        {
            //Tinh n=p*q
            RSA_soN = RSA_soP * RSA_soQ;
            //Tính Phi(n)=(p-1)*(q-1)
            RSA_soPhi_n = (RSA_soP - 1) * (RSA_soQ - 1);
            //Tính e là một số ngẫu nhiên có giá trị 0< e <phi(n) và là số nguyên tố cùng nhau với Phi(n)
            do
            {
                Random RSA_rd = new Random();
                RSA_soE = RSA_rd.Next(2, RSA_soPhi_n);
            }
            while (!nguyenToCungNhau(RSA_soE, RSA_soPhi_n));
            //Tính d là nghịch đảo modular của e
            RSA_soD = 0;
            int i = 2;
            while (((1 + i * RSA_soPhi_n) % RSA_soE) != 0 || RSA_soD <= 0)
            {
                i++;
                RSA_soD = (1 + i * RSA_soPhi_n) / RSA_soE;
            }
        }

        private void F_reset_()
        {
            F_rsa_soP.Text = F_rsa_soQ.Text = F_rsa_soPhiN.Text = F_rsa_soN.Text = F_rsa_soE.Text = F_rsa_soD.Text = string.Empty;
        }
        int F_rsa_d_dau = 0;

        // hàm mã hóa
        public string F_MaHoa_RSA(string ChuoiVao1)
        {
            byte[] F_mh_temp1 = Encoding.Unicode.GetBytes(ChuoiVao1);
            string F_base64 = Convert.ToBase64String(F_mh_temp1);

            // Chuyen xau thanh ma Unicode
            int[] F_mh_temp2 = new int[F_base64.Length];
            for (int i = 0; i < F_base64.Length; i++)
            {
                F_mh_temp2[i] = (int)F_base64[i];
            }

            //Mảng a chứa các kí tự đã mã hóa
            int[] F_mh_temp3 = new int[F_mh_temp2.Length];
            for (int i = 0; i < F_mh_temp2.Length; i++)
            {
                F_mh_temp3[i] = RSA_mod(F_mh_temp2[i], RSA_soE, RSA_soN); // mã hóa
            }

            //Chuyển sang kiểu kí tự trong bảng mã Unicode
            string Fmh_str = "";
            for (int i = 0; i < F_mh_temp3.Length; i++)
            {
                Fmh_str = Fmh_str + (char)F_mh_temp3[i];
            }

            byte[] Fmh_data = Encoding.Unicode.GetBytes(Fmh_str);
            string F_ChuoiVBkemChuky = string.Empty;
            F_ChuoiVBkemChuky = Convert.ToBase64String(Fmh_data);
            return F_ChuoiVBkemChuky;
        }
        // hàm giải mã
        public string F_GiaiMa_RSA(string F_ChuoiVao2)
        {
            byte[] Fgm_temp1 = Convert.FromBase64String(F_ChuoiVao2);
            string Fgm_giaima = Encoding.Unicode.GetString(Fgm_temp1);

            int[] Fgm_temp2 = new int[Fgm_giaima.Length];
            for (int i = 0; i < Fgm_giaima.Length; i++)
            {
                Fgm_temp2[i] = (int)Fgm_giaima[i];
            }
            //Giải mã
            int[] Fgm_temp3 = new int[Fgm_temp2.Length];
            for (int i = 0; i < Fgm_temp3.Length; i++)
            {
                Fgm_temp3[i] = RSA_mod(Fgm_temp2[i], RSA_soD, RSA_soN);// giải mã
            }

            string Fgm_str2 = "";
            for (int i = 0; i < Fgm_temp3.Length; i++)
            {
                Fgm_str2 = Fgm_str2 + (char)Fgm_temp3[i];
            }
            byte[] F_gm_data2 = Convert.FromBase64String(Fgm_str2);

            string F_GM_ChuoiVBkemChuky = string.Empty;
            F_GM_ChuoiVBkemChuky = Encoding.Unicode.GetString(F_gm_data2);
            return F_GM_ChuoiVBkemChuky;
        }

        private void F_rsa_TaoKhoa_Click(object sender, RoutedEventArgs e)
        {
            btChonFileKy_RSA.IsEnabled = btChonFileKy_RSA.IsEnabled = btChonFileXacThuc_RSA.IsEnabled = true;

            F_reset_();
            RSA_soP = RSA_soQ = 0;
            do
            {
                RSA_soP = RSA_ChonSoNgauNhien();
                RSA_soQ = RSA_ChonSoNgauNhien();
            }
            while (RSA_soP == RSA_soQ || !RSA_kiemTraNguyenTo(RSA_soP) || !RSA_kiemTraNguyenTo(RSA_soQ));
            F_rsa_soP.Text = RSA_soP.ToString();
            F_rsa_soQ.Text = RSA_soQ.ToString();
            F_TaoKhoa();

            F_rsa_soPhiN.Text = RSA_soPhi_n.ToString();
            F_rsa_soE.Text = RSA_soE.ToString();
            F_rsa_soD.Text = RSA_soD.ToString();
            F_rsa_soN.Text = RSA_soN.ToString();
            F_rsa_d_dau = 1;
            F_rsa_TaoKhoa.IsEnabled = false;
        }

        private void btChonFileKy_RSA_Click(object sender, RoutedEventArgs e)
        {
            /////// chọn file để ký 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtDuongDanFileKy_RSA.Text = openFileDialog.FileName;
            btThucHienKy_RSA.IsEnabled = true;
        }

        private void btThoatEF_RSA_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btThucHienKy_RSA_Click(object sender, RoutedEventArgs e)
        {
            // thực hiên ký
            if (F_rsa_d_dau != 1)
            {
                MessageBox.Show("Bạn chưa tạo chữ ký", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (F_rsa_d_dau == 1)
                {
                    byte mk = byte.Parse("111");
                    if (!File.Exists(txtDuongDanFileKy_RSA.Text))
                    {
                        // MessageBox.Show("Phải nhập đủ 2 số ", "Thông Báo ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show("Bạn chưa chọn file thực hiện ký!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (File.Exists(txtDuongDanFileKy_RSA.Text))
                    {
                        FileStream fsFileDauVao = new FileStream(txtDuongDanFileKy_RSA.Text, FileMode.Open);
                        SHA256 mySHA256 = SHA256Managed.Create();
                        byte[] FileVBKy_temp1 = mySHA256.ComputeHash(fsFileDauVao);

                        string FileVBKy = Convert.ToBase64String(FileVBKy_temp1);
                        string VBKemChuKy = F_MaHoa_RSA(FileVBKy);
                        txtTepKyDuocGuiDi_RSA.Text = VBKemChuKy;

                        fsFileDauVao.Close();
                        fsFileDauVao.Dispose();
                        F_rsa_d_dau = 2;
                        MessageBox.Show("Thực hiện ký thành công !", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        btThucHienKy_RSA.IsEnabled = false;
                    }
                }
            }
        }

        private void btChonFileXacThuc_RSA_Click(object sender, RoutedEventArgs e)
        {
            /////// chọn file để ký xác thực
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                txtDuongDanFileXacThuc_RSA.Text = openFileDialog.FileName;
            btKiemTraXacThuc_RSA.IsEnabled = true;
        }
        private void btKiemTraXacThuc_RSA_Click(object sender, RoutedEventArgs e)
        {
            // thực hiên kiểm tra
            if (F_rsa_d_dau == 2)
            {
                if (!File.Exists(txtDuongDanFileXacThuc_RSA.Text))
                {
                    MessageBox.Show("Bạn chưa chọn Tài liệu kiểm tra chữ ký", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (File.Exists(txtDuongDanFileXacThuc_RSA.Text))
                {
                    FileStream fsFileDauVao = new FileStream(txtDuongDanFileXacThuc_RSA.Text, FileMode.Open);
                    SHA256 mySHA256 = SHA256Managed.Create();
                    byte[] FileVBKy_temp2 = mySHA256.ComputeHash(fsFileDauVao);
                    string ChuoiVBdiKem_ShA = Convert.ToBase64String(FileVBKy_temp2);

                    string VBKemChuKyGM = F_GiaiMa_RSA(txtTepKyDuocGuiDi_RSA.Text); // thực hiện giải mã chữ ký
                    //txtChuKySoGiaiMa.Text = VBKemChuKyGM;     
                    int result = 0;

                    result = string.Compare(VBKemChuKyGM, ChuoiVBdiKem_ShA, true);

                    if (result == 0)
                    {
                        MessageBox.Show("Tài liệu gửi đến không bị chỉnh sửa gì", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        btKiemTraXacThuc_RSA.IsEnabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Tài liệu gửi đến đã bị thay đổi", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        btKiemTraXacThuc_RSA.IsEnabled = false;
                    }
                    fsFileDauVao.Close();
                    fsFileDauVao.Dispose();

                }
            }
            else
            {
                MessageBox.Show("Chưa thực hiện ký và gửi file ký", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                btKiemTraXacThuc_RSA.IsEnabled = false;
            }
        }
        private void btChonVBMoiRSA_Click(object sender, RoutedEventArgs e)
        {
            txtDuongDanFileXacThuc_RSA.Text = txtDuongDanFileKy_RSA.Text = txtTepKyDuocGuiDi_RSA.Text = string.Empty;
            F_rsa_d_dau = 1;
        }
        private void btTaoChuKymoi_RSA_Click(object sender, RoutedEventArgs e)
        {
            txtDuongDanFileXacThuc_RSA.Text = txtDuongDanFileKy_RSA.Text = txtTepKyDuocGuiDi_RSA.Text = string.Empty;
            F_rsa_d_dau = 0;
            F_reset_();
            F_rsa_TaoKhoa.IsEnabled = true;
        }

        #endregion
    }
}
