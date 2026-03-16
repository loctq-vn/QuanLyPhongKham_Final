using System.Collections.Generic;

namespace QuanLyPhongKham_Final.Models
{
    public class LoaiPhongKham
    {
        public string MaLoaiPhongKham { get; set; } = string.Empty;
        public string TenLoaiPhongKham { get; set; } = string.Empty;
        public int SoLuongToiDa { get; set; }

        // Navigation Properties
        public virtual ICollection<KhamBenh> KhamBenhs { get; set; } = new List<KhamBenh>();
    }
}