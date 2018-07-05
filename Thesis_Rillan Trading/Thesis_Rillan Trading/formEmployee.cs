using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Thesis_Rillan_Trading
{
    public partial class formEmployee : Form
    {
        // Variable 
        public MySqlConnection conn;
        public MySqlCommand command;
        public MySqlDataAdapter adapter;
        public DataTable dataTable;
        int emp_id;


        // Variable for Reference Forms
        public Form refAdminHome { get; set; }
        public Form refSupplier { get; set; }
        public int ref_Emp_empID;


        public formEmployee()
        {
            InitializeComponent();
            conn = new MySqlConnection("Server=localhost; Database=rillan_trading; Uid=root; Pwd=root;");
        }

        // - - - Form Load - - -
        private void formEmployee_Load(object sender, EventArgs e)
        {
            // - - Date and Time label - -
            timer.Start();
            lbl_DateTime.Text = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToLongDateString();

            // - - Data Grid View - - 
            EmpTableLoad();
        }

        // Logout Button
        private void btn_Logout_Click_1(object sender, EventArgs e)
        {
            formLogin fLogin = new formLogin();
            fLogin.refEmployee = this;
            fLogin.Show();
            this.Hide();
        }

        // Back Button
        private void btn_Back_Click(object sender, EventArgs e)
        {
            formAdminHome fAdminHome = new formAdminHome();
            fAdminHome.refEmployee = this;
            fAdminHome.ref_empID = ref_Emp_empID;
            fAdminHome.Show();
            this.Hide();
        }
        
        // Save Button - saving input to database
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (btn_Save.Text == "Update")
            {
                editEmp();
            }
            else
            {
                addEmployee();
            }
        }

        // Info Button - - Create/Find
        private void btn_Info_Click(object sender, EventArgs e)
        {
          
        }

        private void pnl_Header_Paint(object sender, PaintEventArgs e)
        {

        }

        //If user click the cell's content, informations will apear to the fields
        private void dataGV_Emp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("Do you want to edit this employee's details?", "Edit employee", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                emp_id = int.Parse(dataGV_Emp.SelectedRows[0].Cells[0].Value.ToString());
                tbox_firstName.Text = dataGV_Emp.SelectedRows[0].Cells[1].Value.ToString();
                tbox_middleName.Text = dataGV_Emp.SelectedRows[0].Cells[2].Value.ToString();
                tbox_lastName.Text = dataGV_Emp.SelectedRows[0].Cells[3].Value.ToString();
                tbox_mobileNum.Text = dataGV_Emp.SelectedRows[0].Cells[4].Value.ToString();
                tbox_address.Text = dataGV_Emp.SelectedRows[0].Cells[5].Value.ToString();
                dtp_Birthdate.Text = dataGV_Emp.SelectedRows[0].Cells[6].Value.ToString();
                comboBox_role.Text = dataGV_Emp.SelectedRows[0].Cells[8].Value.ToString();
                cmbBox_Branch.Text = dataGV_Emp.SelectedRows[0].Cells[9].Value.ToString();
                tbx_userName.Text = dataGV_Emp.SelectedRows[0].Cells[11].Value.ToString();
                tbx_password.Text = dataGV_Emp.SelectedRows[0].Cells[12].Value.ToString();
                
                if (dataGV_Emp.SelectedRows[0].Cells[7].Value.ToString() == "Male")
                {
                    rdbtn_sexMale.Checked = true;
                }
                else
                {
                    rdbtn_sexFemale.Checked = true;
                }

                if (dataGV_Emp.SelectedRows[0].Cells[10].Value.ToString() == "Active")
                {
                    radioB_active.Checked = true;
                }
                else
                {
                    radioB_deac.Checked = true;
                }

                /*tbox_firstName.BackColor = Color.LightBlue;
                tbox_lastName.BackColor = Color.LightBlue;
                tbox_middleName.BackColor = Color.LightBlue;
                tbox_mobileNum.BackColor = Color.LightBlue;*/
                btn_delete.Text = "Cancel";
                btn_Save.Text = "Update";
                btn_delete.Visible = true;

            }
        }

        private void lbl_lastName_Click(object sender, EventArgs e)
        {

        }

        private void tbox_lastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void formEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            formLogin fLogin = new formLogin();
            fLogin.refEmployee = this;
            fLogin.Show();
            this.Close(); //bug
        }



        private void EmpTableLoad()
        {
            try
            {

                conn.Open();
                command = new MySqlCommand("(SELECT emp_id, emp_firstName, emp_lastName, emp_middleName, " +
                    "emp_contactNum, emp_address, emp_birthdate, IF(emp_sex = 0, 'Male', 'Female') as Sex, " +
                    "IF(emp_role = 0, 'Sales Person', 'Cashier') as Role, emp_branch, IF(emp_status = 1, 'Active', 'Inactive') as Status, emp_username, emp_password FROM employee)", conn);

                adapter = new MySqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGV_Emp.DataSource = dataTable;
                conn.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }

            dataGV_Emp.Columns["emp_id"].Visible = false;
            dataGV_Emp.Columns["emp_firstName"].HeaderText = "First Name";
            dataGV_Emp.Columns["emp_middleName"].HeaderText = "Middle Name";
            dataGV_Emp.Columns["emp_lastName"].HeaderText = "Last Name";
            dataGV_Emp.Columns["emp_contactNum"].HeaderText = "Contact Number";
            dataGV_Emp.Columns["emp_address"].HeaderText = "Address";
            dataGV_Emp.Columns["emp_birthdate"].HeaderText = "Birthdate";
            dataGV_Emp.Columns["emp_branch"].HeaderText = "Branch";
            dataGV_Emp.Columns["emp_username"].HeaderText = "Username";
            dataGV_Emp.Columns["emp_password"].HeaderText = "Password";


        }

        private void addEmployee()
        {
            //Validation
            if (string.IsNullOrWhiteSpace(tbox_firstName.Text.ToString()))
            {
                MessageBox.Show("Please fill in for employee's first name");
            }
            else if (string.IsNullOrWhiteSpace(tbox_middleName.Text.ToString()))
            {
                MessageBox.Show("Please fill in for employee's middle name");
            }
            else if (string.IsNullOrWhiteSpace(tbox_lastName.Text.ToString()))
            {
                MessageBox.Show("Please fill in for employee's last name");
            }
            else if (string.IsNullOrWhiteSpace(tbox_address.Text.ToString()))
            {
                MessageBox.Show("Please fill in for employee's address");
            }
            else if (string.IsNullOrWhiteSpace(tbox_mobileNum.Text.ToString()))
            {
                MessageBox.Show("Please fill in for employee's contact number");
            }
            else if (rdbtn_sexMale.Checked == false && rdbtn_sexFemale.Checked == false)
            {
                MessageBox.Show("Please select employee's sex");
            }
            else if (dtp_Birthdate.Value.Date == DateTime.Today)
            {
                MessageBox.Show("Please do not use current date");
            }
            else if (comboBox_role.SelectedItem == null)
            {
                MessageBox.Show("Please select employee's role");
            }
            else if (cmbBox_Branch.SelectedItem == null)
            {
                MessageBox.Show("Please select employee's branch");
            }
            else if (cmbBox_Branch.SelectedItem == null)
            {
                MessageBox.Show("Please select employee's branch");
            }
            else if (radioB_active.Checked == false && radioB_deac.Checked == false)
            {
                MessageBox.Show("Please select employee's status");
            }
            else if (string.IsNullOrWhiteSpace(tbx_userName.Text.ToString()))
            {
                MessageBox.Show("Please select employee's username");
            }
            else if (string.IsNullOrWhiteSpace(tbx_password.Text.ToString()))
            {
                MessageBox.Show("Please select employee's password");
            }
            else
            {
                try
                {
                    conn.Open();

                    //Inserting  values to MySql Emp table
                    MySqlCommand DatabaseCommand = conn.CreateCommand();
                    DatabaseCommand.CommandText = "INSERT INTO employee (emp_firstName, emp_middleName, emp_lastName, emp_contactNum, emp_address, emp_birthdate, emp_sex, emp_role, emp_branch, emp_status, emp_username, emp_password) VALUES " +
                                                    "( '" + tbox_firstName.Text + "', '" + tbox_middleName.Text + "', '" + tbox_lastName.Text + "', '" + tbox_mobileNum.Text + "', " +
                                                    "'" + tbox_address.Text + "', '" + dtp_Birthdate.Value.Date.ToString("yyyy-MM-dd") + "', '" + EmpSex() + "', '" + EmpRole() + "'," +
                                                    "'" + cmbBox_Branch.Text + "', '" + EmpStatus() + "', '" + tbx_userName.Text + "', '" + tbx_password.Text + "'  )";
                    //Add employee confirmation
                    if (MessageBox.Show("Are you sure you want to add this employee profile?", "Add employee", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DatabaseCommand.ExecuteNonQuery();
                        conn.Close();
                        conn.Dispose();

                        MessageBox.Show("Successfully added an employee");
                        EmpTableLoad();
                        fieldsReset();
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString());
                }


            }
        }

        //Update emp func
        private void editEmp()
        {
            if (btn_Save.Text == "Update")
            {
                try
                {
                    String q = "UPDATE employee SET emp_firstName = '" + tbox_firstName.Text + "', emp_middleName = '" + tbox_middleName.Text + "', emp_lastName = '" + tbox_lastName.Text + "', " +
                        " emp_contactNum = '" + tbox_mobileNum.Text + "', emp_address = '" + tbox_address.Text + "', emp_birthdate = '" + this.dtp_Birthdate.Value.Date.ToString("yyyy-MM-dd") + "', " +
                        " emp_sex = '" + EmpSex() + "', emp_role = '" + EmpRole() + "', emp_branch = '" + cmbBox_Branch.Text + "', emp_status = '" + EmpStatus() + "', emp_username = '" + tbx_userName.Text + "', " +
                        " emp_password = '" + tbx_password.Text + "' WHERE emp_id = '" + emp_id + "' ";

                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(q, conn);
                    int count = adapter.SelectCommand.ExecuteNonQuery();

                    if (count >= 1)
                    {
                        MessageBox.Show("Employee successfully Updated!");
                    }
                    conn.Close();
                    EmpTableLoad();
                    fieldsReset();
                    btn_Save.Text = "Save";
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString());
                }
            }
            btn_Save.Text = "Save";
            btn_delete.Visible = false;
        }

        private void fieldsReset() //clears the textbox fields
        {
            tbox_firstName.Clear();
            tbox_middleName.Clear();
            tbox_lastName.Clear();
            tbox_mobileNum.Clear();
            tbox_address.Clear();
            dtp_Birthdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            comboBox_role.Text = " ";
            rdbtn_sexMale.Checked = false;
            rdbtn_sexFemale.Checked = false;
            radioB_active.Checked = false;
            radioB_deac.Checked = false;
            tbx_password.Clear();
            tbx_userName.Clear();
            cmbBox_Branch.Text = " ";

            if (btn_Save.Text == "Update")
            {
                tbox_firstName.BackColor = Color.White;
                tbox_lastName.BackColor = Color.White;
                tbox_middleName.BackColor = Color.White;
                tbox_mobileNum.BackColor = Color.White;
            }
        }



        //Converting string to int for employee sex
        public int EmpSex()
        {
            if (rdbtn_sexMale.Checked)
            {
                return 0;
            }

            else
            {
                return 1;
            }
        }

        //Converting string to int for employee role
        public int EmpRole()
        {
            if (comboBox_role.Text == "Cashier")
            {
                return 0;
            }

            else
            {
                return 1;
            }
        }

        //Converting string to int for employee status
        public int EmpStatus()
        {
            if (radioB_active.Checked)
            {
                return 1;
            }

            else
            {
                return 0;
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if(btn_delete.Text == "Cancel")
            {
                fieldsReset();
                btn_delete.Visible = false;
            }
        }

        private void picBox_Search_Click(object sender, EventArgs e) 
        {
            if (cmbBox_filter.Text != "Filter By")
            {
                if (cmbBox_filter.SelectedItem.ToString() == "Last Name")
                {
                    if (FieldIsString(tbox_Search.Text))
                    {
                        try
                        {
                            MySqlDataAdapter sda = new MySqlDataAdapter("SELECT * FROM employee WHERE emp_lastName LIKE '" + tbox_Search.Text + "%'", conn);
                            DataTable data = new DataTable();
                            sda.Fill(data);
                            dataGV_Emp.DataSource = data;
                        }
                        catch (Exception x)
                        {
                            MessageBox.Show(x.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Do not use numbers.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Select filter for search");
            }
        }

        private Boolean FieldIsString(String i)
        {
            if (tbox_Search.Text is String)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            EmpTableLoad();
        }

        private void btn_refresh_Click_1(object sender, EventArgs e)
        {
            EmpTableLoad();
        }
    }
}
