namespace QuanLyPhongKham_Final.Models
{
    public class ChiTietKhamBenh
    {
        public string MaKhamBenh { get; set; }
        public string MaBenhNhan { get; set; }

        // Navigation Properties
        public virtual KhamBenh KhamBenh { get; set; }
        public virtual BenhNhan BenhNhan { get; set; }
    }
}