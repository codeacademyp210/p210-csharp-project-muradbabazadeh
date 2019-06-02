using AcademyDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task = AcademyDatabase.Models.Task;

namespace AcademyDatabase
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            FillTasks();
        }
        private void FillTasks()
        {
            dataGridView1.Rows.Clear();
            comboBox1.Items.Clear();
            using (AcademyEntities db = new AcademyEntities())
            {
                List<Models.Task> task = db.Tasks.ToList();

                List<Group> groups = db.Groups.ToList();
                foreach (var item in groups)
                {
                    comboBox1.Items.Add(item.Name);
                }
                
                foreach (var item in task)
                {
                    string statusTrue = item.Status.ToString();
                    if (statusTrue =="True")
                    {
                        string groupss = "";
                        groupss += item.Group.Name + ",";
                        if (groupss.EndsWith(","))
                        {
                            groupss = groupss.Remove(groupss.Length - 1, 1);
                        }
                        dataGridView1.Rows.Add(item.Id, item.Name, item.Deadline, groupss);
                    }
                }

            }
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (AcademyEntities db = new AcademyEntities())
            {
                if (textBox1.Text != "" && dateTimePicker1.Text != "")
                {
                    Group group = db.Groups.Where(c => c.Name == comboBox1.Text).FirstOrDefault();

                    Models.Task task = new Models.Task()
                    {
                        Name = textBox1.Text,
                        Deadline = dateTimePicker1.Value,
                        GroupId = group.Id,
                        Status = true
                    };
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    
                    List<Student> students = db.Students.Where(s => s.Group.Name == comboBox1.Text).ToList();
                    foreach (var item in students)
                    {
                        GroupTask groupTask = new GroupTask()
                        {
                            TaskId = task.Id,
                            StudentId = item.Id,
                        };
                        db.GroupTasks.Add(groupTask);
                        db.SaveChanges();

                    }
                }
                FillTasks();
                textBox1.Text = "";
                dateTimePicker1.Text = "";
                comboBox1.Text = "";
            }
        }

        private void DataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)

        {
            string name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
         using (AcademyEntities db = new AcademyEntities())
            {
                Task task = db.Tasks.Where(c => c.Id == id).FirstOrDefault();
                Group group = db.Groups.Where(c => c.Name == name).FirstOrDefault();
                List<Student> students = db.Students.Where(c => c.GroupId ==group.Id).ToList();
                StudentMarkForm studentMark = new StudentMarkForm(students,task);
                studentMark.ShowDialog();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {

        }
    }  }
 

