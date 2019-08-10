using FacebookApp;
using FacebookWrapper.ObjectModel;
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
            //loginFacebook();
            hideAllPanels();
            Width = 380;
            Height = 160;
        }
        private void updateUserPanel(User i_ToShow)
        {
            userImage.Image = i_ToShow.ImageLarge;
            userName.Text = i_ToShow.Name;
            userBirthday.Text = i_ToShow.Birthday;
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            loginFacebook();
        }

        void showUIComponents()
        {
            panelOptions.Visible = true;
            panelPageOwner.Visible = true;
            LoginButton.Visible = false;
            panelUpPart.Visible = true;
        }
        private void loginFacebook()
        {
            try
            {
                m_UserData = m_AppLogic.LoginAndInit();
                showUIComponents();
                Width = 1000;
                Height = 700;
            }
            catch(Exception exce)
            {
               
                //handle exception
            }

            updateUserPanel(m_UserData.LocalUser);
            showMainFeed();
            Text = "Welcome To Facebook!";
        }

        private void hideAllPanels()
        {
            panelMainPosts.Visible = false;
            panelProfile.Visible = false;
            panelPhotos.Visible = false;
            panelEvents.Visible = false;
            panelPage.Visible = false;
            panelFriends.Visible = false;
        }

        private void buttonEvents_Click(object sender, EventArgs e)
        {
            showEvents();
        }

        private void buttonPosts_Click(object sender, EventArgs e)
        {
            showPostsPage();
        }

        private void buttonMain_Click(object sender, EventArgs e)
        {
            showMainFeed();
        }

        private void showEvents()
        {
            hideAllPanels();
            panelEvents.Visible = true;
            mainLabel.Text = "Events";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.Events;
        }
        private void showMainFeed()
        {
            hideAllPanels();
            panelMainPosts.Visible = true;
            mainLabel.Text = "Main Feed";
            mainListBox.DataSource = postBindingSource;
            postBindingSource.DataSource = m_UserData.LocalUser.NewsFeed;
            
        }

        private void showPostsPage()
        {
            hideAllPanels();
            mainLabel.Text = "Posts";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.Posts;

        }

        private void buttonFriends_Click(object sender, EventArgs e)
        {
            showFriends();
        }

        private void showFriends()
        {
            hideAllPanels();
            panelFriends.Visible = true;
            mainLabel.Text = "Friends";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.Friends;

        }

        private void buttonPages_Click(object sender, EventArgs e)
        {
            showPages();
        }

        private void showPages()
        {
            hideAllPanels();
            panelPage.Visible = true;
            mainLabel.Text = "Liked Pages";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.LikedPages;

        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            showUserPage();
        }

        private void showUserPage()
        {
            hideAllPanels();
            panelProfile.Visible = true;
            mainLabel.Text = "User Wall";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.WallPosts;

        }

        private void buttonPhotos_Click(object sender, EventArgs e)
        {
            showPhotos();
        }

        private void showPhotos()
        {
            hideAllPanels();
            panelPhotos.Visible = true;
            mainLabel.Text = "Photos";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.PhotosTaggedIn;
            

        }
        
        
    }
}
