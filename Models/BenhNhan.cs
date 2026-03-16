using System.Collections.Generic;

namespace QuanLyPhongKham_Final.Models
{
    public class BenhNhan
    {
        public string MaBenhNhan { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string GioiTinh { get; set; } = string.Empty;
        public int NamSinh { get; set; }
        public string DiaChi { get; set; } = string.Empty;

        public int STT { get; set; }

        // Navigation Properties
        public virtual ICollection<ChiTietKhamBenh> ChiTietKhamBenhs { get; set; } = new List<ChiTietKhamBenh>();
    }
}