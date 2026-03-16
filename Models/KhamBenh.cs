using System;
using System.Collections.Generic;

namespace QuanLyPhongKham_Final.Models
{
    public class KhamBenh
    {
        public string MaKhamBenh { get; set; } = string.Empty;
        public DateTime NgayKham { get; set; }
        public string MaLoaiPhongKham { get; set; } = string.Empty;

        // Navigation Properties
        public virtual LoaiPhongKham? LoaiPhongKham { get; set; }
        public virtual ICollection<ChiTietKhamBenh> ChiTietKhamBenhs { get; set; } = new List<ChiTietKhamBenh>();
    }
}