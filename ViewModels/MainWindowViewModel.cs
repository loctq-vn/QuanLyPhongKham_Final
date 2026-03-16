using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuanLyPhongKham_Final.Models;
using QuanLyPhongKham_Final.Repositories;

namespace QuanLyPhongKham_Final.ViewModels
{
    /// <summary>
    /// ViewModel cho MainWindow - Sử dụng MVVM Toolkit
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly RepoBenhNhan repoBenhNhan = new();
        private readonly RepoLoaiPhongKham repoLoaiPhongKham = new();
        private readonly RepoKhamBenh repoKhamBenh = new();
        private readonly RepoChiTietKhamBenh repoChiTietKhamBenh = new();

        // ========== OBSERVABLE PROPERTIES ==========

        [ObservableProperty]
        private ObservableCollection<LoaiPhongKham> danhSachLoaiPhong = new();

        [ObservableProperty]
        private ObservableCollection<BenhNhan> danhSachAllBenhNhan = new();

        [ObservableProperty]
        private DateTime? ngayKham;

        [ObservableProperty]
        private LoaiPhongKham? loaiPhongDuocChon;

        [ObservableProperty]
        private BenhNhan? benhNhanDuocChon;

        [ObservableProperty]
        private ObservableCollection<BenhNhan> danhSachBenhNhan = new();

        // ========== RELAY COMMANDS ==========

        /// <summary>
        /// Command để thêm bệnh nhân được chọn từ ComboBox vào danh sách
        /// </summary>
        [RelayCommand]
        private void ThemBenhNhan()
        {
            if (!NgayKham.HasValue || LoaiPhongDuocChon == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn ngày khám và loại phòng!", "Cảnh báo");
                return;
            }

            if (BenhNhanDuocChon == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn bệnh nhân!", "Cảnh báo");
                return;
            }

            // Kiểm tra bệnh nhân đã tồn tại trong danh sách chưa
            if (DanhSachBenhNhan.Any(b => b.MaBenhNhan == BenhNhanDuocChon.MaBenhNhan))
            {
                MessageBox.Show("⚠️ Bệnh nhân này đã có trong danh sách!", "Cảnh báo");
                return;
            }

            // Tạo bản sao của bệnh nhân được chọn và thêm vào danh sách
            var benhNhanMoi = new BenhNhan
            {
                MaBenhNhan = BenhNhanDuocChon.MaBenhNhan,
                HoTen = BenhNhanDuocChon.HoTen,
                GioiTinh = BenhNhanDuocChon.GioiTinh,
                NamSinh = BenhNhanDuocChon.NamSinh,
                DiaChi = BenhNhanDuocChon.DiaChi,
                STT = DanhSachBenhNhan.Count + 1
            };

            DanhSachBenhNhan.Add(benhNhanMoi);
            BenhNhanDuocChon = null; // Reset ComboBox
        }

        /// <summary>
        /// Command để lưu phiếu khám vào database
        /// Tạo danh sách khám bệnh mới và chi tiết bệnh nhân
        /// </summary>
        [RelayCommand]
        private void LuuPhieuKham()
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ValidateDuLieuDauVao())
                return;

            try
            {
                // 1. Tạo phiếu khám bệnh mới
                var khamBenhMoi = TaoPhieuKhamBenh();
                if (khamBenhMoi == null || string.IsNullOrEmpty(khamBenhMoi))
                {
                    MessageBox.Show("❌ Lỗi tạo phiếu khám bệnh!", "Lỗi");
                    return;
                }

                // 2. Tạo danh sách chi tiết khám bệnh
                var danhSachChiTiet = TaoDanhSachChiTietKhamBenh(khamBenhMoi);
                if (!danhSachChiTiet)
                {
                    MessageBox.Show("⚠️ Một số bệnh nhân không được lưu thành công!", "Cảnh báo");
                    return;
                }

                // 3. Xóa dữ liệu cũ nếu lưu thành công
                XoaDuLieuSauKhiLuu();
                MessageBox.Show("✅ Lưu phiếu khám và chi tiết bệnh nhân thành công!", "Thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi");
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu đầu vào trước khi lưu
        /// </summary>
        private bool ValidateDuLieuDauVao()
        {
            if (!NgayKham.HasValue)
            {
                MessageBox.Show("⚠️ Vui lòng chọn ngày khám!", "Cảnh báo");
                return false;
            }

            if (LoaiPhongDuocChon == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn loại phòng khám!", "Cảnh báo");
                return false;
            }

            if (DanhSachBenhNhan.Count == 0)
            {
                MessageBox.Show("⚠️ Danh sách bệnh nhân không được trống!", "Cảnh báo");
                return false;
            }

            // Kiểm tra từng bệnh nhân có đủ thông tin không
            foreach (var bn in DanhSachBenhNhan)
            {
                if (string.IsNullOrWhiteSpace(bn.MaBenhNhan))
                {
                    MessageBox.Show($"⚠️ Dòng {bn.STT}: Vui lòng chọn mã bệnh nhân!", "Cảnh báo");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(bn.HoTen))
                {
                    MessageBox.Show($"⚠️ Dòng {bn.STT}: Vui lòng nhập họ tên bệnh nhân!", "Cảnh báo");
                    return false;
                }

                if (bn.NamSinh <= 0 || bn.NamSinh > DateTime.Now.Year)
                {
                    MessageBox.Show($"⚠️ Dòng {bn.STT}: Năm sinh không hợp lệ!", "Cảnh báo");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tạo phiếu khám bệnh mới trong database
        /// </summary>
        private string TaoPhieuKhamBenh()
        {
            var ngayKhamValue = NgayKham.Value;
            var maLoaiPhong = LoaiPhongDuocChon?.MaLoaiPhongKham ?? "";

            // Tạo phiếu khám bệnh mới
            string maKhamBenh = repoKhamBenh.Them(ngayKhamValue, maLoaiPhong);
            
            return maKhamBenh;
        }

        /// <summary>
        /// Tạo danh sách chi tiết khám bệnh cho tất cả bệnh nhân
        /// </summary>
        private bool TaoDanhSachChiTietKhamBenh(string maKhamBenh)
        {
            bool ketQuaTatCa = true;

            foreach (var bn in DanhSachBenhNhan)
            {
                try
                {
                    // Tạo chi tiết khám bệnh cho từng bệnh nhân
                    bool ketQua = repoChiTietKhamBenh.Them(maKhamBenh, bn.MaBenhNhan);
                    
                    if (!ketQua)
                    {
                        ketQuaTatCa = false;
                        MessageBox.Show($"⚠️ Lỗi thêm chi tiết khám cho bệnh nhân: {bn.HoTen}", "Cảnh báo");
                    }
                }
                catch (Exception ex)
                {
                    ketQuaTatCa = false;
                    MessageBox.Show($"❌ Lỗi xử lý bệnh nhân {bn.HoTen}: {ex.Message}", "Lỗi");
                    break;
                }
            }

            return ketQuaTatCa;
        }

        /// <summary>
        /// Xóa dữ liệu cũ sau khi lưu thành công
        /// </summary>
        private void XoaDuLieuSauKhiLuu()
        {
            DanhSachBenhNhan.Clear();
            BenhNhanDuocChon = null;
            NgayKham = DateTime.Now;
            LoaiPhongDuocChon = null;
        }

        // ========== CONSTRUCTOR & INITIALIZATION ==========

        public MainWindowViewModel()
        {
            InitializeData();
        }

        /// <summary>
        /// Khởi tạo dữ liệu từ database
        /// </summary>
        private void InitializeData()
        {
            try
            {
                // Lấy danh sách loại phòng khám
                var loaiPhongList = repoLoaiPhongKham.LayDanhSachAll();
                DanhSachLoaiPhong = new ObservableCollection<LoaiPhongKham>(loaiPhongList);

                // Lấy danh sách bệnh nhân cho AutoComplete
                var benhNhanList = repoBenhNhan.LayDanhSachAll();
                DanhSachAllBenhNhan = new ObservableCollection<BenhNhan>(benhNhanList);

                // Đặt ngày mặc định là hôm nay
                NgayKham = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khởi tạo dữ liệu: {ex.Message}", "Lỗi");
            }
        }

        // ========== PARTIAL METHODS (for MVVM Toolkit) ==========

        /// <summary>
        /// Được gọi tự động khi NgayKham thay đổi
        /// </summary>
        partial void OnNgayKhamChanged(DateTime? oldValue, DateTime? newValue)
        {
            // Logic khi ngày khám thay đổi (nếu cần)
        }

        /// <summary>
        /// Được gọi tự động khi LoaiPhongDuocChon thay đổi
        /// </summary>
        partial void OnLoaiPhongDuocChonChanged(LoaiPhongKham? oldValue, LoaiPhongKham? newValue)
        {
            // Logic khi loại phòng thay đổi (nếu cần)
        }
    }
}