using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcademyDatabase.Models;

namespace AcademyDatabase
{
    public partial class TeacherCrud : Form
    {
       public TeacherCrud()
        {
            InitializeComponent();
            FillTeacher();
            using (AcademyEntities db = new AcademyEntities())
            {
                List<Models.Education> educations = db.Educations.ToList();
                foreach (var item in educations)
                {
                    comSpeciality.Items.Add(item.Name);

                }
            }
        }
        int selectedTeacher;
        Teacher teacherUpdated = new Teacher();
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //string Name = txtName.Text;
            //dataGridView1.SelectedRows[0].Cells[0].Value = Name;
            //dataGridView1.SelectedRows[0].Cells[1].Value = txtSurname.Text;
            //dataGridView1.SelectedRows[0].Cells[2].Value = txtEmail.Text;
            //dataGridView1.SelectedRows[0].Cells[3].Value = txtPhone.Text;
            //dataGridView1.SelectedRows[0].Cells[4].Value = comSpeciality.Text;
        }
        private void FillTeacher()
        {
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Rows.Clear();
            using (AcademyEntities db = new AcademyEntities())
            {
                List<Models.Teacher> teachers = db.Teachers.ToList();
                foreach (var item in teachers)
                {
                    List<Models.Group> groups = db.Groups.Where(c => c.TeacherId.ToString().Contains(item.Id.ToString())).ToList();
                    string teachergroups = "";
                    foreach (var tt in groups)
                    {
                        teachergroups += tt.Name + ",";
                    }
                    string status = item.Status.ToString();
                    if (status == "True")
                    {
                        if (teachergroups.EndsWith(","))
                        {
                            teachergroups = teachergroups.Remove(teachergroups.Length - 1, 1);
                        }
                        dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Speciality, teachergroups);
                    }
                }
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Surname = txtSurname.Text;
            string Email = txtEmail.Text;
            string Phone = txtPhone.Text;
            bool statusdefault = true;
            string letters = @"^[a-zA-Z]+$";
            string emailreg = @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            string phonereg = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2}\s)?(?<areaCode>0?[1-9]\d{0,4})(?<number>\s[1-9][\d]{5,12})(?<extension>\sx\d{0,4})?$";

            Match result = Regex.Match(Name, letters);
            Match surnameMath = Regex.Match(Surname, letters);
            Match emailMatch = Regex.Match(Email, emailreg);
            Match phoneMatch = Regex.Match(Phone, phonereg);

            if (!result.Success)
            {
                MessageBox.Show("Adi duzgun daxil edin.");
            }
            if (!surnameMath.Success)
            {
                MessageBox.Show("Soyadi duzgun daxil edin.");
            }
            if (!emailMatch.Success)
            {
                MessageBox.Show("Emaili duzgun daxil edin.");
            }
            if (!phoneMatch.Success)
            {
                MessageBox.Show("Nomreni duzgun daxil edin.");
            }
            else
            {
                using (AcademyEntities db = new AcademyEntities())
                {
                    Models.Teacher teacher = new Teacher()
                    {
                        Name = Name,
                        Surname = Surname,
                        Email = Email,
                        Phone = Phone,
                        Speciality = comSpeciality.Text,
                        Status = statusdefault
                    };
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                }
            }
            ResetTxt();
            dataGridView1.Rows.Clear();
            FillTeacher();
        }    
        private void ResetTxt()
        {
            txtName.Text = "";
            txtSurname.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            comSpeciality.Text = "";
        }
        private void DataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            comSpeciality.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            selectedTeacher= (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            using (AcademyEntities db = new AcademyEntities())
            {
                teacherUpdated = db.Teachers.Where(q => q.Id == selectedTeacher).FirstOrDefault();
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Surname = txtSurname.Text;
            string Email = txtEmail.Text;
            string Phone = txtPhone.Text;
            string letters = @"^[a-zA-Z]+$";
            string emailreg = @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            string phonereg = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2}\s)?(?<areaCode>0?[1-9]\d{0,4})(?<number>\s[1-9][\d]{5,12})(?<extension>\sx\d{0,4})?$";
            Match result = Regex.Match(Name, letters);
            Match surnameMath = Regex.Match(Surname, letters);
            Match emailMatch = Regex.Match(Email, emailreg);
            Match phoneMatch = Regex.Match(Phone, phonereg);
            if (!result.Success)
            {
                MessageBox.Show("Adi duzgun daxil edin.");
            }
            if (!surnameMath.Success)
            {
                MessageBox.Show("Soyadi duzgun daxil edin.");
            }
            if (!emailMatch.Success)
            {
                MessageBox.Show("Emaili duzgun daxil edin.");
            }
            if (!phoneMatch.Success)
            {
                MessageBox.Show("Nomreni duzgun daxil edin.");
            }
            else
            {
                using (AcademyEntities db = new AcademyEntities())
                {
                    teacherUpdated.Name = txtName.Text;
                    teacherUpdated.Surname = txtSurname.Text;
                    teacherUpdated.Email = txtEmail.Text;
                    teacherUpdated.Phone = txtPhone.Text;
                    teacherUpdated.Speciality = comSpeciality.Text;
                    var entry = db.Entry(teacherUpdated);
                    entry.State = EntityState.Modified;
                    db.SaveChanges();
                }
                ResetTxt();
                FillTeacher();
                MessageBox.Show("Emeliyyat ugurla yerine yetirildi.");
            }
        }

        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            using (AcademyEntities db = new AcademyEntities())
            {
                teacherUpdated.Status = false;
                var entry = db.Entry(teacherUpdated);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
            ResetTxt();
            FillTeacher();
            MessageBox.Show("Emeliyyat ugurla yerine yetirildi."+ teacherUpdated.Name+" "+ teacherUpdated.Surname+ " statusu deaktiv edildi.");
        }
    }
}
