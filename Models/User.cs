using System;
using System.Collections.Generic;
using System.Text;

namespace test2.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Lưu ý: Sau này nên mã hóa mật khẩu
        public string Role { get; set; } // Ví dụ: "Admin", "Staff"
    }
}
