using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thesis_Rillan_Trading
{
    public partial class formSupplier : Form
    {
        //- - Variables - -
        public Form refAdminHome { get; set; }


        public formSupplier()
        {
            InitializeComponent();
        }

        //- - Form Load - - 
        private void formSupplier_Load(object sender, EventArgs e)
        {
            timer.Start();
            lbl_DateTime.Text = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToLongDateString();
        }

        // - - Back Button - -
        private void btn_Back_Click(object sender, EventArgs e)
        {
            formAdminHome fHome = new formAdminHome();
            fHome.refSupplier = this;
            fHome.Show();
            this.Hide();
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            formLogin fLogin = new formLogin();
            fLogin.refSupplier = this;
            fLogin.Show();
            this.Hide();
        }

        private void btn_Settings_Click(object sender, EventArgs e)
        {
            formEmployee fEmp = new formEmployee();
            fEmp.refSupplier = this;
            fEmp.Show();
            this.Hide();
        }

        private void formSupplier_FormClosing(object sender, FormClosingEventArgs e)
        {
            formLogin fLogin = new formLogin();
            fLogin.refSupplier = this;
            fLogin.Show();
            this.Close(); // bug
        }
    }
}
