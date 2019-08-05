using C19_Ex01_Omer_204059331_Andrey_321082513.sln;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookApp
{
    public class AppLogic
    {
        private static readonly object m_AppLogicLock = new object();
        private static AppLogic m_AppLogic = null;
        private StoreToken m_StoreToken = new StoreToken();
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
                m_StoreToken.SaveLogin(LoginResult.AccessToken, "test1");
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
        
        public void GetAllTaggedFriendsFromPhotos(UserData i_UserData)
        {
            string currUserNameAsKey = "";
            foreach (Album currAlbum in i_UserData.m_ListOfNoEmptyAlbums)
            {
                foreach (Photo currPhoto in currAlbum.Photos)
                {
                    foreach (PhotoTag currPhotoTag in currPhoto.Tags)
                    {
                        currUserNameAsKey = currPhotoTag.User.FirstName + " " + currPhotoTag.User.LastName;
                        if(i_UserData.m_BestFriendsDict.ContainsKey(currUserNameAsKey))
                        {
                            i_UserData.m_BestFriendsDict[currUserNameAsKey] += 1;
                        }
                        else
                        {
                            i_UserData.m_BestFriendsDict.Add(currUserNameAsKey, 1);
                        }
                    }

                }
            }
        }

        public void GetAllTaggedFriendsFromCheckins(UserData i_UserData)
        {
            string currUserNameAsKey = "";
            foreach (Checkin currCheckin in i_UserData.m_User.Checkins)
            {
                foreach (User currUser in currCheckin.TaggedUsers)
                {
                    currUserNameAsKey = currUser.FirstName + " " + currUser.LastName;
                    if (i_UserData.m_BestFriendsDict.ContainsKey(currUserNameAsKey))
                    {
                        i_UserData.m_BestFriendsDict[currUserNameAsKey] += 1;
                    }
                    else
                    {
                        i_UserData.m_BestFriendsDict.Add(currUserNameAsKey, 1);
                    }
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

