using System.Collections.Generic;

namespace QuanLyPhongKham_Final.Models
{
    public class BenhNhan
    {
        public string MaBenhNhan { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public int NamSinh { get; set; }
        public string DiaChi { get; set; }

        // Navigation Properties
        public virtual ICollection<ChiTietKhamBenh> ChiTietKhamBenhs { get; set; } = new List<ChiTietKhamBenh>();
    }
}