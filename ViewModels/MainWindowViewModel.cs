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
        private BenhNhan? benhNhanDuocChon; [ObservableProperty]
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

            // --- PHẦN THÊM VÀO: KIỂM TRA QUY ĐỊNH 1 - SỐ LƯỢNG TỐI ĐA TRONG NGÀY ---
            // 1. Lấy số lượng đã có trong Database
            int soLuongTrongDB = repoKhamBenh.LaySoLuongBenhNhanTrongNgay(NgayKham.Value, LoaiPhongDuocChon.MaLoaiPhongKham);
            // 2. Lấy số lượng đang nằm chờ lưu trên giao diện
            int soLuongDangCho = DanhSachBenhNhan.Count;

            // Nếu Tổng số (Đã lưu + Đang chờ thêm) >= Số lượng tối đa của loại phòng đó (40 hoặc 20)
            if (soLuongTrongDB + soLuongDangCho >= LoaiPhongDuocChon.SoLuongToiDa)
            {
                MessageBox.Show($"⚠️ KHÔNG THỂ THÊM BỆNH NHÂN!\n\n" +
                                $"{LoaiPhongDuocChon.TenLoaiPhongKham} chỉ nhận tối đa {LoaiPhongDuocChon.SoLuongToiDa} bệnh nhân/ngày.\n" +
                                $"• Đã lưu hệ thống: {soLuongTrongDB} người.\n" +
                                $"• Đang chờ trong bảng: {soLuongDangCho} người.",
                                "Quy định phòng khám", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // ----------------------------------------------------------------------

            // Kiểm tra bệnh nhân đã tồn tại trong danh sách chưa
            if (DanhSachBenhNhan.Any(b => b.MaBenhNhan == BenhNhanDuocChon.MaBenhNhan))
            {
                MessageBox.Show("⚠️ Bệnh nhân này đã có trong danh sách chờ lưu!", "Cảnh báo");
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

            foreach (var bn in DanhSachBenhNhan)
            {
                if (string.IsNullOrWhiteSpace(bn.MaBenhNhan) || string.IsNullOrWhiteSpace(bn.HoTen) || bn.NamSinh <= 0)
                {
                    MessageBox.Show($"⚠️ Dòng {bn.STT}: Thiếu thông tin hoặc không hợp lệ!", "Cảnh báo");
                    return false;
                }
            }
            return true;
        }

        private string TaoPhieuKhamBenh()
        {
            var ngayKhamValue = NgayKham ?? DateTime.Now;
            var maLoaiPhong = LoaiPhongDuocChon?.MaLoaiPhongKham ?? "";

            string maKhamBenh = repoKhamBenh.Them(ngayKhamValue, maLoaiPhong);
            return maKhamBenh;
        }

        private bool TaoDanhSachChiTietKhamBenh(string maKhamBenh)
        {
            bool ketQuaTatCa = true;
            foreach (var bn in DanhSachBenhNhan)
            {
                try
                {
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

        private void InitializeData()
        {
            try
            {
                var loaiPhongList = repoLoaiPhongKham.LayDanhSachAll();
                DanhSachLoaiPhong = new ObservableCollection<LoaiPhongKham>(loaiPhongList);

                var benhNhanList = repoBenhNhan.LayDanhSachAll();
                DanhSachAllBenhNhan = new ObservableCollection<BenhNhan>(benhNhanList);

                NgayKham = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khởi tạo dữ liệu: {ex.Message}", "Lỗi");
            }
        }

        // ========== PARTIAL METHODS (Sự kiện tự động của MVVM Toolkit) ==========

        /// <summary>
        /// Được gọi tự động khi NgayKham thay đổi
        /// </summary>
        partial void OnNgayKhamChanged(DateTime? oldValue, DateTime? newValue)
        {
            // BẢO MẬT DỮ LIỆU: Đổi sang ngày khác thì phải xóa sạch danh sách hiện tại
            // Tránh việc lập 5 người ngày hôm nay xong đổi lịch thành ngày mai bấm lưu
            if (DanhSachBenhNhan.Count > 0)
            {
                DanhSachBenhNhan.Clear();
            }
        }

        /// <summary>
        /// Được gọi tự động khi LoaiPhongDuocChon thay đổi
        /// </summary>
        partial void OnLoaiPhongDuocChonChanged(LoaiPhongKham? oldValue, LoaiPhongKham? newValue)
        {
            // BẢO MẬT DỮ LIỆU: Đổi loại phòng thì phải xóa sạch danh sách hiện tại
            if (DanhSachBenhNhan.Count > 0)
            {
                DanhSachBenhNhan.Clear();
            }
        }
    }
}