using FacebookApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public partial class MainPage : Form
    {
        AppLogic m_AppLogic;
        UserData m_UserData;
        public MainPage()
        {
            InitializeComponent();
            m_AppLogic = AppLogic.Instance;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                m_UserData = m_AppLogic.LoginAndInit();
            }
            catch(Exception exce)
            {
               
                //handle exception
            }
        }
    }
}
