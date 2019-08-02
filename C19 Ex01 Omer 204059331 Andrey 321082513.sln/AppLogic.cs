using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookApp
{
    class AppLogic
    {
        private static readonly object m_AppLogicLock = new object();
        private static AppLogic m_AppLogic = null;

        private AppLogic()
        {
        }

        public static AppLogic Instance
        {
            get
            {
                if (m_AppLogic == null)
                {
                    lock (m_AppLogicLock)
                    {
                        if (m_AppLogic == null)
                        {
                            m_AppLogic = new AppLogic();
                        }
                    }
                }

                return m_AppLogic;
            }
        }

        public LoginResult LoginResult { get; set; }

        

        public void PostPhoto(Photo i_Photo, UserData i_UserData)
        {
            i_UserData.m_User.PostLink(i_Photo.Link);
        }

        public UserData LoginAndInit()
        {
            UserData resUserData = new UserData();
            LoginResult = FacebookService.Login(
                "1450160541956417",
                "public_profile",
                "email",
                "publish_to_groups",
                "user_birthday",
                "user_age_range",
                "user_gender",
                "user_link",
                "user_tagged_places",
                "user_videos",
                "publish_to_groups",
                "groups_access_member_info",
                "user_friends",
                "user_events",
                "user_likes",
                "user_location",
                "user_photos",
                "user_posts",
                "user_hometown");

            if (!string.IsNullOrEmpty(LoginResult.AccessToken))
            {
                resUserData.m_User = LoginResult.LoggedInUser;
            }
            return resUserData;
        }

        public Status PostStatus(string io_text, UserData i_UserData)
        {
            Status postedStatus = i_UserData.m_User.PostStatus(io_text);
            return postedStatus;
        }


        public User FindFriendByName(string i_friendNameToFind, UserData i_UserData)
        {
            User friend = null;
            foreach (User user in i_UserData.m_UserFriends)
            {
                    string name = user.Name.ToUpper();

                    if (name.Equals(i_friendNameToFind.ToUpper()) == true)
                    {
                        friend = user;
                        break;
                    }
            }

            return friend;
        }

        private void getAllTheNoEmptyAlbums(UserData i_UserData)
        {
            foreach (Album album in i_UserData.m_User.Albums)
            {
                if (album.Count != 0)
                {
                    i_UserData.m_ListOfNoEmptyAlbums.Add(album);
                }
            }
        }

        private void findAllUserFriends(UserData i_UserData)
        {
            foreach (User user in i_UserData.m_User.Friends)
            {
                i_UserData.m_UserFriends.Add(user);
            }
        }

    }
}

