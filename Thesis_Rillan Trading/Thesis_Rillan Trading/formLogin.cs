﻿using System;
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
    public partial class formLogin : Form
    {

        //- - - Database - - -
        public MySqlConnection conn; //connection string

        //Variables
        public Form refAdminHome { get; set; }
        public Form refEmployee { get; set; }

        public int empID;

        public formLogin()
        {
            InitializeComponent();
            conn = new MySqlConnection("Server=localhost; Database=rillan_trading; Uid=root; Pwd=root;");
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                MySqlCommand comm = new MySqlCommand("SELECT * FROM employee", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                conn.Close();
            }
            catch (Exception ee)
            {
                conn.Close();
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                // User did not enter either his username/password
                if (tbox_Username.Text == "" || tbox_Password.Text == "")
                {
                    MessageBox.Show("Please enter your username/password.", "Error!", MessageBoxButtons.OK);
                }
                else
                {
                    // Connection to Database
                    conn.Open(); // Connection opened

                    // Get data from DB
                    MySqlCommand comm = new MySqlCommand("SELECT * FROM employee WHERE emp_username = '" + tbox_Username.Text + "' AND emp_password = '" + tbox_Password.Text + "'", conn);
                    MySqlDataAdapter adp = new MySqlDataAdapter(comm);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    conn.Close();


                    if (dt.Rows.Count == 1) // DB returned 1 row  
                    {
                        string username, password;
                        int stat;

                        // gets data from DB >> pass the values to its corresponding variables
                        username = dt.Rows[0]["emp_username"].ToString();
                        password = dt.Rows[0]["emp_password"].ToString();
                        stat = int.Parse(dt.Rows[0]["emp_status"].ToString());
                        empID = int.Parse(dt.Rows[0]["emp_id"].ToString());

                        if (tbox_Username.Text != username || tbox_Password.Text != password)
                        {
                            MessageBox.Show("The user credentials are incorrect.", "", MessageBoxButtons.OK);
                        }

                        if (tbox_Username.Text == username && tbox_Password.Text == password)
                        {
                            if (stat == 1) // 1 means active account
                            {
                                // Opens Menu
                                formAdminHome fAdminHome = new formAdminHome();
                                fAdminHome.refLogin = this;
                                fAdminHome.ref_empID = empID;
                                fAdminHome.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("This account is inactive. Please contact your admin! ", "", MessageBoxButtons.OK);
                            }

                        }
                        
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                conn.Close();
            }
            
        }


    }
}
