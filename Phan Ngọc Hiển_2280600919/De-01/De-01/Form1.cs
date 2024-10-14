using De_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De_01
{
    public partial class frmSinhVien : Form
    {
        private bool isEditing = false;
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadLop();
            SetButtonState(true);
            btTim.Click += btTim_Click;
        }
        private void SetButtonState(bool isInitial)
        {
            btThem.Enabled = isInitial;
            btXoa.Enabled = isInitial;
            btSua.Enabled = isInitial;
            btLuu.Enabled = !isInitial;
            btKhong.Enabled = !isInitial;
            btThoat.Enabled = true; 
        }
        private void LoadData()
        {
            using (var context = new SinhVienModels()) 
            {
               
                var sinhVienList = context.SinhViens
                    .Select(sv => new
                    {
                        sv.MaSV,
                        sv.HoTenSV,
                        sv.NgaySinh,
                        sv.MaLop,
                        TenLop = sv.Lop.TenLop 
                    })
                    .ToList();

                
                dgvSinhVien.DataSource = sinhVienList;
            }
        }

        private void LoadLop()
        {
            using (var context = new SinhVienModels())
            {
                
                var lopHocList = context.Lops
                    .Select(l => new
                    {
                        l.MaLop,
                        l.TenLop
                    })
                    .ToList();

                
                cboLop.DataSource = lopHocList;
                cboLop.DisplayMember = "TenLop"; 
                cboLop.ValueMember = "MaLop";   
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
               
                DataGridViewRow selectedRow = dgvSinhVien.Rows[e.RowIndex];

                
                txtMaSV.Text = selectedRow.Cells["MaSV"].Value.ToString();
                txtHotenSV.Text = selectedRow.Cells["HoTenSV"].Value.ToString();

               
                if (selectedRow.Cells["NgaySinh"].Value != DBNull.Value)
                {
                    dtNgaysinh.Value = Convert.ToDateTime(selectedRow.Cells["NgaySinh"].Value);
                }
                else
                {
                    dtNgaysinh.Value = DateTime.Now; 
                }

               
                cboLop.SelectedValue = selectedRow.Cells["MaLop"].Value.ToString();
            }
        }

        private void ClearInputFields()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtNgaysinh.Value = DateTime.Now;
            cboLop.SelectedIndex = -1; 
        }
        private bool isAdding = false;
        private void btThem_Click(object sender, EventArgs e)
        {
            isAdding = true; 
            ClearInputFields(); 
            SetButtonState(false); 
            txtMaSV.Enabled = true; 
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            if (isAdding) 
            {
                
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHotenSV.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (var context = new SinhVienModels())
                    {
                        
                        var existingStudent = context.SinhViens.Find(txtMaSV.Text);
                        if (existingStudent != null)
                        {
                            MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                       
                        var newStudent = new SinhVien
                        {
                            MaSV = txtMaSV.Text,
                            HoTenSV = txtHotenSV.Text,
                            NgaySinh = dtNgaysinh.Value,
                            MaLop = cboLop.SelectedValue.ToString()
                        };

                       
                        context.SinhViens.Add(newStudent);
                        context.SaveChanges();
                    }

                   
                    MessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadData(); 
                    ClearInputFields(); 
                    SetButtonState(true);
                    isAdding = false; 
                    txtMaSV.Enabled = false; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (isDeleting) 
            {
                using (var context = new SinhVienModels())
                {
                    var studentToDelete = context.SinhViens.Find(txtMaSV.Text);

                    if (studentToDelete != null)
                    {
                        context.SinhViens.Remove(studentToDelete);
                        context.SaveChanges(); 

                        MessageBox.Show("Xóa sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                     
                        LoadData();
                        ClearInputFields();
                    }
                }

                isDeleting = false; 
                SetButtonState(true); 
            }
            if (isEditingg) 
            {
                using (var context = new SinhVienModels())
                {
                    var studentToEdit = context.SinhViens.Find(txtMaSV.Text);

                    if (studentToEdit != null)
                    {
                        studentToEdit.HoTenSV = txtHotenSV.Text;
                        studentToEdit.NgaySinh = dtNgaysinh.Value;
                        studentToEdit.MaLop = cboLop.SelectedValue.ToString();

                        context.SaveChanges();

                        MessageBox.Show("Cập nhật sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        
                        LoadData();
                        ClearInputFields();
                    }
                }

                isEditingg = false; 
                SetButtonState(true); 
            }
        }

        private void btKhong_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn không lưu thao tác này saoo?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearInputFields();
                SetButtonState(true);
                isAdding = false;
                txtMaSV.Enabled = false;
            }
            if (isEditingg) 
            {
                if (result == DialogResult.Yes)
                {
                  
                    isEditing = false;
                    ClearInputFields(); 
                    LoadData(); 
                    SetButtonState(true); 
                }
                else
                {
                    MessageBox.Show("Tiếp tục chỉnh sửa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private bool isDeleting = false;

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmDelete = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmDelete == DialogResult.Yes)
            {
                isDeleting = true; 
                SetButtonState(false); 
                MessageBox.Show("Nhấn Lưu để xác nhận việc xóa, hoặc Không Lưu để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool isEditingg = false;

        private void btSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            isEditingg = true;
            SetButtonState(false); 
            MessageBox.Show("Chỉnh sửa thông tin và nhấn Lưu để xác nhận, hoặc Không Lưu để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btTim_Click(object sender, EventArgs e)
        {
            string searchName = txtTim.Text.Trim(); 

            using (var context = new SinhVienModels())
            {
              
                var sinhVienList = context.SinhViens
                    .Where(sv => sv.HoTenSV.Contains(searchName)) 
                    .Select(sv => new
                    {
                        sv.MaSV,
                        sv.HoTenSV,
                        sv.NgaySinh,
                        sv.MaLop,
                        TenLop = sv.Lop.TenLop 
                    })
                    .ToList();

                dgvSinhVien.DataSource = sinhVienList;

               
                if (sinhVienList.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sinh viên nào với tên '" + searchName + "'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
    

