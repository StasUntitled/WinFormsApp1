using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        SqlConnection SqlConnection;
        public Form1()
        {
            InitializeComponent();
        }
        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Действие при загрузке формы. Описание связи с Sql DB
        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\я\source\repos\WinFormsApp1\Database1.mdf;Integrated Security=True";
            SqlConnection = new SqlConnection(connectionString);
            await SqlConnection.OpenAsync();

            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Table]", SqlConnection);

            SqlDataReader sqlReader1 = null;
            SqlCommand command1 = new SqlCommand("SELECT * FROM [Table1]", SqlConnection);
            
            //Load Select Form
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + "        " + Convert.ToString(sqlReader["Name"]) + "     " + Convert.ToString(sqlReader["Count"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }

            //Load Order List
            try
            {
                sqlReader1 = await command1.ExecuteReaderAsync();
                while (await sqlReader1.ReadAsync())
                {
                    listBox2.Items.Add(Convert.ToString(sqlReader1["Id"]) + "    " + Convert.ToString(sqlReader1["Name"]) + "    " + Convert.ToString(sqlReader1["Count"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                if (sqlReader1 != null)
                {
                    sqlReader1.Close();
                }
            }
        }

        // Меню File => exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SqlConnection != null && SqlConnection.State != ConnectionState.Closed)
                SqlConnection.Close();
        }

        //Закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SqlConnection != null && SqlConnection.State != ConnectionState.Closed)
                SqlConnection.Close();
        }

        // Описание кнопки Add Note
        private async void button1_Click(object sender, EventArgs e)
        {
            if (label7.Visible)
                label7.Visible = false;
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Table] (Name, Count) VALUES(@Name, @Count)", SqlConnection);
                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Count", textBox2.Text);
                await command.ExecuteNonQueryAsync();

            }
            else
            {
                label7.Visible = true;
                label7.Text = "Field 'Name' and 'Count' is empty!!!";
            }
        }

        
        //строка меню Sevice => Update List
        private async void updateListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Update Select List
            listBox1.Items.Clear();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Table]", SqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["Id"]) + "        " + Convert.ToString(sqlReader["Name"]) + "     " + Convert.ToString(sqlReader["Count"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                if (sqlReader != null)
                {
                    sqlReader.Close();
                }
            }


            //Update Order List
            listBox2.Items.Clear();
            SqlDataReader sqlReader1 = null;
            SqlCommand command1 = new SqlCommand("SELECT * FROM [Table1]", SqlConnection);
            try
            {
                sqlReader1 = await command1.ExecuteReaderAsync();
                while (await sqlReader1.ReadAsync())
                {
                    listBox2.Items.Add(Convert.ToString(sqlReader1["Id"]) + "    " + Convert.ToString(sqlReader1["Name"]) + "    " + Convert.ToString(sqlReader1["Count"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                if (sqlReader1 != null)
                {
                    sqlReader1.Close();
                }
            }
        }

        //обновление записи
        private async void button2_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text))
            {
                SqlCommand command = new SqlCommand("UPDATE [Table] SET [Name] = @Name, [Count] = @Count WHERE [Id]= @Id", SqlConnection);
                command.Parameters.AddWithValue("Id", textBox3.Text);
                command.Parameters.AddWithValue("Name", textBox4.Text);
                command.Parameters.AddWithValue("Count", textBox5.Text);

                await command.ExecuteNonQueryAsync();
            }
            else if (string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrWhiteSpace(textBox3.Text))
            {
                label8.Visible = true;
                label8.Text = "Field box 'Index' is empty!!!";

            }
            else
            {
                label8.Visible = true;
                label8.Text = "Field boxes 'Name' and 'Count' is empty!!!";
            }
        }

        //Удаление записи
        private async void button3_Click(object sender, EventArgs e)
        {
            if (label9.Visible)
                label9.Visible = false;

            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text)) {
                SqlCommand command = new SqlCommand("DELETE FROM [Table] WHERE [Id]=@Id", SqlConnection);
                command.Parameters.AddWithValue("Id", textBox6.Text);
                await command.ExecuteNonQueryAsync();
                
            } else
            {
                label9.Visible = true;
                label9.Text = "Field box 'Index' is empty!!!";
            }
        }

        //Button Add to order
        private async void button4_Click(object sender, EventArgs e)
        {
            if (label7.Visible)
                label7.Visible = false;
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Table1] (Name, Count) VALUES(@Name, @Count)", SqlConnection);
                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Count", textBox2.Text);
                await command.ExecuteNonQueryAsync();

            }
            else
            {
                label7.Visible = true;
                label7.Text = "Field 'Name' and 'Count' is empty!!!";
            }
        }
         //Update order Table
        private async void button5_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text))
            {
                SqlCommand command = new SqlCommand("UPDATE [Table1] SET [Name] = @Name, [Count] = @Count WHERE [Id]= @Id", SqlConnection);
                command.Parameters.AddWithValue("Id", textBox3.Text);
                command.Parameters.AddWithValue("Name", textBox4.Text);
                command.Parameters.AddWithValue("Count", textBox5.Text);

                await command.ExecuteNonQueryAsync();
            }
            else if (string.IsNullOrEmpty(textBox3.Text) && string.IsNullOrWhiteSpace(textBox3.Text))
            {
                label8.Visible = true;
                label8.Text = "Field box 'Index' is empty!!!";

            }
            else
            {
                label8.Visible = true;
                label8.Text = "Field boxes 'Name' and 'Count' is empty!!!";
            }
        }

        //Delete From order
        private async void button6_Click(object sender, EventArgs e)
        {
            if (label9.Visible)
                label9.Visible = false;

            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                SqlCommand command = new SqlCommand("DELETE FROM [Table1] WHERE [Id]=@Id", SqlConnection);
                command.Parameters.AddWithValue("Id", textBox6.Text);
                await command.ExecuteNonQueryAsync();

            }
            else
            {
                label9.Visible = true;
                label9.Text = "Field box 'Index' is empty!!!";
            }
        }
        //Meow
    }
}