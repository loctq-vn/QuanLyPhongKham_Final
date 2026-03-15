using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QuanLyPhongKham_Final.Models;

namespace QuanLyPhongKham_Final.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BenhNhan> danhSachBenhNhan;
        private ObservableCollection<ChiTietKhamBenh> danhSachKhamBenh;

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
        }

        private List<BenhNhan> GetBenhNhanFromDatabase()
        {
            // Gọi database để lấy danh sách bệnh nhân
            // Ví dụ: return dbContext.BenhNhans.ToList();
            return new List<BenhNhan>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}