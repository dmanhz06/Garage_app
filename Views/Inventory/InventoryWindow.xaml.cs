using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using test2.Models;
using test2.Repositories;
using test2.Views.Model_Box; 

namespace test2.Views.Inventory
{
    public class InventoryModel
    {
        public int STT { get; set; }
        public int PartID { get; set; } 
        public string MaPhuTung { get; set; }
        public string TenPhuTung { get; set; }
        public string ĐVT { get; set; } 
        public int TonKho { get; set; }
        public string GiaNhap { get; set; }
        public string GiaBan { get; set; }
    }

    public partial class InventoryWindow : Window
    {
        private PartRepository _partRepo = new PartRepository();
        private List<InventoryModel> _allInventory;

        public InventoryWindow()
        {
            InitializeComponent();
            LoadData(); 

            if (txtSearch != null)
            {
                txtSearch.TextChanged += TxtSearch_TextChanged;
            }
        }

        private void LoadData()
        {
            try
            {
                var rawParts = _partRepo.GetAllParts();

                if (rawParts != null && rawParts.Count > 0)
                {
                    _allInventory = rawParts.Select((p, index) => new InventoryModel
                    {
                        STT = index + 1,
                        PartID = p.PartID,
                        MaPhuTung = "PT" + p.PartID.ToString("D3"),
                        TenPhuTung = p.PartName,
                        ĐVT = p.Unit,
                        TonKho = p.StockQuantity,
                        GiaNhap = p.ImportPrice.ToString("N0"),
                        GiaBan = p.Price.ToString("N0")
                    }).ToList();
                }
                else
                {
                    _allInventory = GetDummyData();
                }

                dgvInventory.ItemsSource = _allInventory;
            }
            catch (Exception)
            {
                _allInventory = GetDummyData();
                dgvInventory.ItemsSource = _allInventory;
            }
        }

        private List<InventoryModel> GetDummyData()
        {
            return new List<InventoryModel>
            {
                new InventoryModel { STT = 1, PartID = 1, MaPhuTung = "PT001", TenPhuTung = "Lọc nhớt Toyota Vios", ĐVT = "Cái", TonKho = 45, GiaNhap = "120,000", GiaBan = "180,000" },
                new InventoryModel { STT = 2, PartID = 2, MaPhuTung = "PT002", TenPhuTung = "Bugi Denso Iridium", ĐVT = "Cái", TonKho = 120, GiaNhap = "150,000", GiaBan = "220,000" },
                new InventoryModel { STT = 3, PartID = 3, MaPhuTung = "PT003", TenPhuTung = "Dầu nhớt Castrol Magnatec 4L", ĐVT = "Can", TonKho = 15, GiaNhap = "580,000", GiaBan = "750,000" },
                new InventoryModel { STT = 4, PartID = 4, MaPhuTung = "PT004", TenPhuTung = "Má phanh trước Honda City", ĐVT = "Bộ", TonKho = 8, GiaNhap = "450,000", GiaBan = "620,000" },
                new InventoryModel { STT = 5, PartID = 5, MaPhuTung = "PT005", TenPhuTung = "Lọc gió động cơ Mazda 3", ĐVT = "Cái", TonKho = 25, GiaNhap = "180,000", GiaBan = "280,000" },
            //     new InventoryModel { STT = 6, PartID = 6, MaPhuTung = "PT006", TenPhuTung = "Bình ắc quy GS 12V-45Ah", ĐVT = "Bình", TonKho = 10, GiaNhap = "1,100,000", GiaBan = "1,450,000" },
            //     new InventoryModel { STT = 7, PartID = 7, MaPhuTung = "PT007", TenPhuTung = "Dây curoa tổng Gates", ĐVT = "Sợi", TonKho = 12, GiaNhap = "320,000", GiaBan = "480,000" },
            //     new InventoryModel { STT = 8, PartID = 8, MaPhuTung = "PT008", TenPhuTung = "Gạt mưa Bosch AeroFit", ĐVT = "Cặp", TonKho = 30, GiaNhap = "250,000", GiaBan = "380,000" },
            //     new InventoryModel { STT = 9, PartID = 9, MaPhuTung = "PT009", TenPhuTung = "Nước làm mát xanh Prestone", ĐVT = "Can", TonKho = 20, GiaNhap = "140,000", GiaBan = "210,000" },
            //     new InventoryModel { STT = 10, PartID = 10, MaPhuTung = "PT010", TenPhuTung = "Đèn Halogen Philips H7", ĐVT = "Cái", TonKho = 55, GiaNhap = "90,000", GiaBan = "160,000" }
            };
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_allInventory == null) return;
            string searchText = txtSearch.Text.ToLower();
            var filteredList = _allInventory.Where(p =>
                p.TenPhuTung.ToLower().Contains(searchText) ||
                p.MaPhuTung.ToLower().Contains(searchText)
            ).ToList();

            dgvInventory.ItemsSource = filteredList;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWin = new AddPartWindow();
            addWin.Owner = this;
            if (addWin.ShowDialog() == true)
            {
                _partRepo.AddPart(addWin.NewPart);
                LoadData();
                MessageBox.Show("Đã thêm phụ tùng mới!");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy dữ liệu an toàn từ DataContext của nút bấm
            if (sender is Button btn && btn.DataContext is InventoryModel selectedItem)
            {
                // 2. Mở cửa sổ sửa
                var editWin = new EditPartWindow(selectedItem);
                editWin.Owner = this;

                if (editWin.ShowDialog() == true)
                {
                    try 
                    {
                        // 3. Xử lý chuỗi giá tiền an toàn (loại bỏ mọi ký tự không phải số trừ dấu phẩy/chấm)
                        string cleanPrice = selectedItem.GiaBan?.Replace(",", "").Replace(".", "").Trim() ?? "0";
                        
                        if (!decimal.TryParse(cleanPrice, out decimal finalPrice))
                        {
                            finalPrice = 0; // Nếu không đọc được số thì để là 0
                        }

                        // 4. Ánh xạ sang model Database
                        var partUpdate = new Part {
                            PartID = selectedItem.PartID, // Nhớ check xem InventoryModel có PartID chưa nha ní
                            PartName = selectedItem.TenPhuTung,
                            Unit = selectedItem.ĐVT,
                            Price = finalPrice,
                            StockQuantity = selectedItem.TonKho
                        };

                        // 5. Cập nhật và Load lại
                        _partRepo.UpdatePart(partUpdate);
                        LoadData();
                        MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi");
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as InventoryModel;
            if (selectedItem != null)
            {
                var confirmWin = new DeleteConfirmWindow(selectedItem.TenPhuTung);
                confirmWin.Owner = this;
                if (confirmWin.ShowDialog() == true)
                {
                    _partRepo.DeletePart(selectedItem.PartID);
                    LoadData();
                }
            }
        }
    }
}