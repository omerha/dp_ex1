using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookApp;
using FacebookWrapper.ObjectModel;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    public partial class MainPage : Form
    {
        private AppLogic m_AppLogic;
        private UserData m_UserData;

        public MainPage()
        {
            InitializeComponent();
            m_AppLogic = AppLogic.Instance;
        }

        private void FormLogin_Load(object sender = null, EventArgs e = null)
        {
            hideAllPanels();
            LoginButton.Visible = true;
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

        private void showUIComponents()
        {
            panelOptions.Visible = true;
            panelPageOwner.Visible = true;
            LoginButton.Visible = false;
            panelUpPart.Visible = true;
            buttonLogout.Visible = true;
        }

        private void hideUIComponents()
        {
            panelOptions.Visible = false;
            panelPageOwner.Visible = false;
            panelUpPart.Visible = false;
            buttonLogout.Visible = false;        
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
            catch(Exception)
            {
                MessageBox.Show("There was error trying to login to Facebook, please try again.");
            }

            updateUserPanel(m_UserData.LocalUser);
            showMainFeed_Click();
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
            showEvents_Click();
        }

        private void buttonPosts_Click(object sender, EventArgs e)
        {
            showPostsPage_Click();
        }

        private void buttonMain_Click(object sender, EventArgs e)
        {
            showMainFeed_Click();
        }

        private void showEvents_Click()
        {
            hideAllPanels();
            panelEvents.Visible = true;
            mainLabel.Text = "Events";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.Events;
        }

        private void showMainFeed_Click()
        {
            hideAllPanels();
            panelMainPosts.Visible = true;
            mainLabel.Text = "Main Feed";
            mainListBox.DataSource = postBindingSource;
            postBindingSource.DataSource = m_UserData.LocalUser.NewsFeed;           
        }

        private void showPostsPage_Click()
        {
            hideAllPanels();
            mainLabel.Text = "Posts";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.Posts;
        }

        private void buttonFriends_Click(object sender, EventArgs e)
        {
            showFriends_Click();
        }

        private void showFriends_Click()
        {
            hideAllPanels();
            panelFriends.Visible = true;
            mainLabel.Text = "Friends";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.Friends;
        }

        private void buttonPages_Click(object sender, EventArgs e)
        {
            showPages_Click();
        }

        private void showPages_Click()
        {
            hideAllPanels();
            panelPage.Visible = true;
            mainLabel.Text = "Liked Pages";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.LikedPages;
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            showUserPage_Click();
        }

        private void showUserPage_Click()
        {
            hideAllPanels();
            panelProfile.Visible = true;
            mainLabel.Text = "User Wall";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.WallPosts;
        }

        private void buttonPhotos_Click(object sender, EventArgs e)
        {
            showPhotos_Click();
        }

        private void showPhotos_Click()
        {
            hideAllPanels();
            panelPhotos.Visible = true;
            mainLabel.Text = "Photos";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = m_UserData.LocalUser.PhotosTaggedIn;
        }

        private void buttonBestFriends_Click(object sender, EventArgs e)
        {
            List<string> bestFriendsNames = m_AppLogic.GetTopFiveBestFriends(m_UserData);
            hideAllPanels();
            panelPhotos.Visible = true;
            mainLabel.Text = "Those are your best friends!";
            mainListBox.DataSource = mainBindingSource;
            mainBindingSource.DataSource = bestFriendsNames;
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            try
            {
                m_AppLogic.PostStatus(textBoxPostStatus.Text, m_UserData.LocalUser);
            }
            catch (Exception)
            {
                MessageBox.Show("There was error trying to post status");
            }
        }

        private void buttonPostOnAllFriends_Click(object sender, EventArgs e)
        {
            foreach (User currUser in m_UserData.UserFriendsList)
            {
                try
                {
                    m_AppLogic.PostStatus(textBoxPostStatus.Text, currUser);
                }
                catch(Exception)
                {
                    MessageBox.Show("There was error trying to post status");
                    break;
                }
            }
        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            m_AppLogic.LogOutFromFacebook();
            hideUIComponents();
            FormLogin_Load();
        }
    }
}
