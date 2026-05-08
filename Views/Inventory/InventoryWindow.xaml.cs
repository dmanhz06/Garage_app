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
        
        // Dữ liệu ẩn để giữ lại nguyên trạng khi Sửa, chống lỗi Format và ghi đè
        public decimal GiaNhapRaw { get; set; }
        public int TonToiThieuRaw { get; set; }
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
                        GiaBan = p.Price.ToString("N0"),
                        // Lưu trữ dữ liệu gốc
                        GiaNhapRaw = p.ImportPrice,
                        TonToiThieuRaw = p.MinimumStock
                    }).ToList();
                }
                else
                {
                    _allInventory = new List<InventoryModel>();
                }

                dgvInventory.ItemsSource = _allInventory;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi");
            }
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
            var addWin = new AddPartWindow { Owner = this };
            if (addWin.ShowDialog() == true)
            {
                _partRepo.AddPart(addWin.NewPart);
                LoadData(); // Load lại DataGrid
                MessageBox.Show("Đã thêm phụ tùng mới!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is InventoryModel selectedItem)
            {
                var editWin = new EditPartWindow(selectedItem) { Owner = this };

                if (editWin.ShowDialog() == true)
                {
                    try 
                    {
                        // Dọn dẹp chuỗi tiền tệ do lấy từ TextBox giao diện
                        string cleanPrice = selectedItem.GiaBan?.Replace(",", "").Replace(".", "").Replace(" ", "").Trim() ?? "0";
                        if (!decimal.TryParse(cleanPrice, out decimal finalPrice))
                        {
                            finalPrice = 0; 
                        }

                        // Gom đầy đủ dữ liệu đưa xuống Database
                        var partUpdate = new Part {
                            PartID = selectedItem.PartID,
                            PartName = selectedItem.TenPhuTung,
                            Unit = selectedItem.ĐVT,
                            Price = finalPrice,
                            StockQuantity = selectedItem.TonKho,
                            ImportPrice = selectedItem.GiaNhapRaw,    // Giá trị được bảo toàn
                            MinimumStock = selectedItem.TonToiThieuRaw // Giá trị được bảo toàn
                        };

                        _partRepo.UpdatePart(partUpdate);
                        LoadData();
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (sender is Button btn && btn.DataContext is InventoryModel selectedItem)
            {
                var confirmWin = new DeleteConfirmWindow(selectedItem.TenPhuTung) { Owner = this };
                if (confirmWin.ShowDialog() == true)
                {
                    _partRepo.DeletePart(selectedItem.PartID);
                    LoadData();
                }
            }
        }
    }
}