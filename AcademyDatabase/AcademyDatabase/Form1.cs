using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcademyDatabase.Models;

namespace AcademyDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FillStudents();
            FillTeacher();
        }
        private void StudentBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            using(AcademyEntities db = new AcademyEntities())
            {
                string searchStudent = txtStudent.Text;
                List<Models.Student> students = db.Students.Where(c =>
                    c.Name.ToLower().Contains(searchStudent) ||
                    c.Surname.ToLower().Contains(searchStudent) ||
                    c.Email.ToLower().Contains(searchStudent) ||
                    c.Phone.ToLower().Contains(searchStudent)
                ).ToList();
                foreach (var item in students)
                {
                    string status = item.Status.ToString();
                    if (status == "True")
                    {
                        List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == item.Id).ToList();
                        int educationId = (int)item.Group.EducationId;
                        Education[] education = db.Educations.Where(c => c.Id == educationId).ToArray();

                        decimal averageMark = 0;
                        if (tasks.Count != 0)
                        {
                            foreach (var i in tasks)
                            {
                                averageMark += i.Mark;
                            }
                            averageMark = averageMark / tasks.Count;
                        }
                        dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Group.Name, item.Fee, averageMark);
                        if (item.Fee < (int)education[0].Price)
                        {

                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            }
        }

        private void FillStudents()
        {
            dataGridView1.Rows.Clear();
            using (AcademyEntities db = new AcademyEntities())
            {
                List<Models.Student> students = db.Students.ToList();

                foreach (var item in students)
                {
                    string status = item.Status.ToString();
                    if (status == "True")
                    {
                        List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == item.Id).ToList();
                        int educationId = (int)item.Group.EducationId;
                        Education[] education = db.Educations.Where(c => c.Id == educationId).ToArray();

                        decimal averageMark = 0;
                        if (tasks.Count != 0)
                        {
                            foreach (var i in tasks)
                            {
                                averageMark += i.Mark;
                            }
                            averageMark = averageMark / tasks.Count;
                        }
                        dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Group.Name, item.Fee, averageMark);
                        if (item.Fee < (int)education[0].Price)
                        {

                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            }
              

            
        }
        private void FillTeacher()
        {
            dataGridView2.Rows.Clear();
            using (AcademyEntities db = new AcademyEntities())
            {
                List<Models.Teacher> teachers = db.Teachers.ToList();

                foreach (var item in teachers)
                {
                    List<Group> groups = db.Groups.Where(c => c.TeacherId.ToString().Contains(item.Id.ToString())).ToList();
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
                        dataGridView2.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Speciality, teachergroups);
                    }
                }


            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void TeacherBtn_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            using (AcademyEntities db = new AcademyEntities())
            {
                string searchTeacher = txtTeacher.Text;
                List<Models.Teacher> teacher = db.Teachers.Where(c =>
                    c.Name.ToLower().Contains(searchTeacher) ||
                    c.Surname.ToLower().Contains(searchTeacher) ||
                    c.Email.ToLower().Contains(searchTeacher) ||
                    c.Phone.ToLower().Contains(searchTeacher)
                ).ToList();
              

                foreach (var item in teacher)
                {
                    List<Group> groups = db.Groups.Where(c => c.TeacherId.ToString().Contains(item.Id.ToString())).ToList();
                    string teachergroups = "";
                    foreach(var tt in groups)
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
        
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;

            int value = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;

            using (AcademyEntities academy = new AcademyEntities())
            {
               Student student = academy.Students.Where(x => x.Id == value).FirstOrDefault();

                StudentInfo studentInfo = new StudentInfo(student);

                studentInfo.ShowDialog();

            }
        }

        private void TeachersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TeacherCrud teacherCrud = new TeacherCrud();
            teacherCrud.ShowDialog();
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            using (AcademyEntities db = new AcademyEntities())
            {
                List<Models.Student> students = db.Students.ToList();
                if (comStudentMark.Text == "")
                {

                    foreach (var item in students)
                    {
                        string status = item.Status.ToString();
                        if (status == "True")
                        {
                            List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == item.Id).ToList();
                            int educationId = (int)item.Group.EducationId;
                            Education[] education = db.Educations.Where(c => c.Id == educationId).ToArray();

                            decimal averageMark = 0;
                            if (tasks.Count != 0)
                            {
                                foreach (var i in tasks)
                                {
                                    averageMark += i.Mark;
                                }
                                averageMark = averageMark / tasks.Count;
                            }
                            dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Group.Name, item.Fee, averageMark);
                            if (item.Fee < (int)education[0].Price)
                            {

                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                        }
                    }
                }
                if (comStudentMark.Text == "50 baldan asagi telebeler.")
                {

                    foreach (var item in students)
                    {
                        string status = item.Status.ToString();
                        if (status == "True")
                        {
                            List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == item.Id).ToList();
                            int educationId = (int)item.Group.EducationId;
                            Education[] education = db.Educations.Where(c => c.Id == educationId).ToArray();

                            decimal averageMark = 0;
                            if (tasks.Count != 0)
                            {
                                foreach (var i in tasks)
                                {
                                    averageMark += i.Mark;
                                }
                                averageMark = averageMark / tasks.Count;
                               
                            }
                            if (averageMark < 50)
                            {
                                dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Group.Name, item.Fee, averageMark);
                                if (item.Fee < (int)education[0].Price)
                                {

                                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                            }

                        }
                    }

                }
                if (comStudentMark.Text == "50 baldan yuxari telebeler.")
                {
                    foreach (var item in students)
                    {
                        string status = item.Status.ToString();
                        if (status == "True")
                        {
                            List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == item.Id).ToList();
                            int educationId = (int)item.Group.EducationId;
                            Education[] education = db.Educations.Where(c => c.Id == educationId).ToArray();

                            decimal averageMark = 0;
                            if (tasks.Count != 0)
                            {
                                foreach (var i in tasks)
                                {
                                    averageMark += i.Mark;
                                }
                                averageMark = averageMark / tasks.Count;
                                if (averageMark >= 50)
                                {
                                    dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Group.Name, item.Fee, averageMark);
                                    if (item.Fee < (int)education[0].Price)
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }

                        }
                    }
                }
                if (comStudentMark.Text == "90 baldan yuxari telebeler.")
                {
                    foreach (var item in students)
                    {
                        string status = item.Status.ToString();
                        if (status == "True")
                        {
                            List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == item.Id).ToList();
                            int educationId = (int)item.Group.EducationId;
                            Education[] education = db.Educations.Where(c => c.Id == educationId).ToArray();

                            decimal averageMark = 0;
                            if (tasks.Count != 0)
                            {
                                foreach (var i in tasks)
                                {
                                    averageMark += i.Mark;
                                }
                                averageMark = averageMark / tasks.Count;
                                if (averageMark >= 90)
                                {
                                    dataGridView1.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone, item.Group.Name, item.Fee, averageMark);
                                    if (item.Fee < (int)education[0].Price)
                                    {

                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }

                        }
                    }
                }


            }
        }

        private void TasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
