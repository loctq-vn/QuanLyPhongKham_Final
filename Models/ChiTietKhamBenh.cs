namespace QuanLyPhongKham_Final.Models
{
    public class ChiTietKhamBenh
    {
        public string MaKhamBenh { get; set; } = string.Empty;
        public string MaBenhNhan { get; set; } = string.Empty;

        // Navigation Properties
        public virtual KhamBenh? KhamBenh { get; set; }
        public virtual BenhNhan? BenhNhan { get; set; }
    }
}