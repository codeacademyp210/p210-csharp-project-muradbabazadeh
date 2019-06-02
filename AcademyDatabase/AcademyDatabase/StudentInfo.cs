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
using AcademyDatabase;

namespace AcademyDatabase
{
    public partial class StudentInfo : Form
    {
        Student Student;
        public StudentInfo(Student student)
        {
            Student = student;
            InitializeComponent();
            StudentAdd();


        }
        public void StudentAdd()
        {
            using (AcademyEntities db = new AcademyEntities())
            {
                Form1 form1 = new Form1();

                List<Models.GroupTask> tasks = db.GroupTasks.Where(c => c.StudentId == Student.Id).ToList();
                txtStudentMarks.Text = Student.Name.ToString() + " " + Student.Surname.ToString() + "'s all tasks marks.";

                foreach (var i in tasks)
                {
                   
                    StudentGrid.Rows.Add(i.Task.Name,i.Mark);
                }



            }

        }

        



    }
}
