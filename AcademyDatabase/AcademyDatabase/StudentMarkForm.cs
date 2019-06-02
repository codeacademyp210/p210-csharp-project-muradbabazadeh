using AcademyDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task = AcademyDatabase.Models.Task;

namespace AcademyDatabase
{
    public partial class StudentMarkForm : Form
    {
       List<Student> students;
        Task task;
        public StudentMarkForm( List<Student> _students,Task _task)
        {
            task = _task;
            students = _students;
            InitializeComponent();
            FillStudent();
        }
        private void FillStudent()
        {
            dataGridView1.Rows.Clear();
            using(AcademyEntities db = new AcademyEntities())
            {
                List<GroupTask> groupTasks = db.GroupTasks.Where(s => s.TaskId == task.Id).ToList();
                foreach (var item in groupTasks)
                {
                  Student student=  students.Where(s => s.Id == item.StudentId).FirstOrDefault();
                    string fullname = student.Name + " " + student.Surname;
                    dataGridView1.Rows.Add(item.StudentId,fullname, item.Mark);
                }




                //foreach (var item in students)
                //{

                //    List<GroupTask> groupTasks = db.GroupTasks.Where(s => s.StudentId == item.Id).ToList();

                //    if (groupTasks.Count != 0)
                //    {
                //        string fullname = item.Name + " " + item.Surname;

                //        foreach (var i in groupTasks)
                //        {
                //            dataGridView1.Rows.Add(fullname, i.Mark);
                //        }
                //    }
                //    else
                //    {
                //        string fullname = item.Name + " " + item.Surname;
                //        dataGridView1.Rows.Add(fullname, "qiymet daxil edilmeyib.");
                //    }


                //}
            }
        }
        int selectedStudent;
       

        private void Button1_Click(object sender, EventArgs e)
        {
            using (AcademyEntities db = new AcademyEntities())
            {
                decimal mark = Convert.ToDecimal(textBox1.Text);

                GroupTask groupTask = db.GroupTasks.Where(g => g.TaskId == task.Id && g.StudentId == selectedStudent).FirstOrDefault();
                groupTask.Mark = mark;
                var entry = db.Entry(groupTask);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
            textBox1.Text = "";
            FillStudent();
        }

        private void DataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            selectedStudent = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
        }
    }
}
