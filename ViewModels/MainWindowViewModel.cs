using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyPhongKham_Final.Models;
using QuanLyPhongKham_Final.Repositories; // Thêm thư viện gọi file HamLayDanhSach

namespace QuanLyPhongKham_Final.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // --- SỬA LỖI NON-NULLABLE: Khởi tạo sẵn giá trị rỗng thay vì để trống ---
        private ObservableCollection<BenhNhan> danhSachBenhNhan = new ObservableCollection<BenhNhan>();
        private ObservableCollection<ChiTietKhamBenh> danhSachKhamBenh = new ObservableCollection<ChiTietKhamBenh>();

        // -----------------------------------------------------------------
        // --- PHẦN THÊM VÀO: Các biến để liên kết với giao diện (XAML) ---
        // -----------------------------------------------------------------
        private HamLayDanhSach repository = new HamLayDanhSach(); // Gọi class kết nối DB

        // --- SỬA LỖI NON-NULLABLE: Khởi tạo sẵn giá trị rỗng ---
        private ObservableCollection<LoaiPhongKham> danhSachLoaiPhong = new ObservableCollection<LoaiPhongKham>();
        public ObservableCollection<LoaiPhongKham> DanhSachLoaiPhong
        {
            get => danhSachLoaiPhong;
            set { danhSachLoaiPhong = value; OnPropertyChanged(); }
        }

        private DateTime? ngayKham;
        public DateTime? NgayKham
        {
            get => ngayKham;
            set
            {
                ngayKham = value;
                OnPropertyChanged();
                LocDanhSachBenhNhan(); // Cứ đổi ngày là tự động lọc lại
            }
        }

        // --- SỬA LỖI NON-NULLABLE: Thêm dấu ? vì lúc đầu chưa có phòng nào được chọn ---
        private LoaiPhongKham? loaiPhongDuocChon;
        public LoaiPhongKham? LoaiPhongDuocChon
        {
            get => loaiPhongDuocChon;
            set
            {
                loaiPhongDuocChon = value;
                OnPropertyChanged();
                LocDanhSachBenhNhan(); // Cứ chọn loại phòng là tự động lọc lại
            }
        }
        // -----------------------------------------------------------------


        public ObservableCollection<BenhNhan> DanhSachBenhNhan
        {
            get => danhSachBenhNhan;
            set { danhSachBenhNhan = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ChiTietKhamBenh> DanhSachKhamBenh
        {
            get => danhSachKhamBenh;
            set { danhSachKhamBenh = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            // Tải danh sách bệnh nhân từ database
            DanhSachBenhNhan = new ObservableCollection<BenhNhan>(GetBenhNhanFromDatabase());
            DanhSachKhamBenh = new ObservableCollection<ChiTietKhamBenh>();

            // --- PHẦN THÊM VÀO: Lấy sẵn dữ liệu cho ComboBox khi mới chạy App ---
            DanhSachLoaiPhong = new ObservableCollection<LoaiPhongKham>(repository.LayDanhSachLoaiPhong());
            NgayKham = DateTime.Now; // Đặt ngày mặc định là hôm nay
            // -----------------------------------------------------------------
        }

        private List<BenhNhan> GetBenhNhanFromDatabase()
        {
            // Gọi database để lấy danh sách bệnh nhân
            // Ví dụ: return dbContext.BenhNhans.ToList();
            return new List<BenhNhan>(); // Giữ nguyên của bạn (khởi tạo danh sách rỗng ban đầu)
        }

        // -----------------------------------------------------------------
        // --- PHẦN THÊM VÀO: Hàm thực hiện việc lọc danh sách từ DB ---
        // -----------------------------------------------------------------
        private void LocDanhSachBenhNhan()
        {
            if (NgayKham.HasValue && LoaiPhongDuocChon != null)
            {
                var ketQua = repository.LayBenhNhanTheoDieuKien(NgayKham.Value, LoaiPhongDuocChon.MaLoaiPhongKham);

                DanhSachBenhNhan.Clear();
                int soThuTu = 1; // Khởi tạo biến đếm
                foreach (var bn in ketQua)
                {
                    bn.STT = soThuTu++; // Gán STT và tăng dần (Nhớ thêm property STT vào class BenhNhan nhé)
                    DanhSachBenhNhan.Add(bn);
                }
            }
        }
        // -----------------------------------------------------------------

        // Thêm dấu ? để không bị báo lỗi Nullable ở event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}