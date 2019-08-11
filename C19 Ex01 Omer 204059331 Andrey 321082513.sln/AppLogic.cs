using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C19_Ex01_Omer_204059331_Andrey_321082513.sln;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

namespace FacebookApp
{
    public class AppLogic
    {
        private static readonly object m_AppLogicLock = new object();
        private static AppLogic m_AppLogic = null;
        private readonly string r_TokenFileName = "DB.txt";
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

        public void PostPhoto(Photo i_Photo, User i_User)
        {
            i_User.PostLink(i_Photo.Link);
        }

        public UserData LoginAndInit()
        {
            UserData resUserData = null;
            if (m_StoreToken.LoadLogin(r_TokenFileName))
            {
                LoginResult = FacebookService.Connect(m_StoreToken.m_Token);
            }
            else
            {
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
                    m_StoreToken.SaveLogin(LoginResult.AccessToken, r_TokenFileName);
                }
            }

            resUserData = new UserData(LoginResult.LoggedInUser);
            FetchUserData(resUserData);
            return resUserData;
        }

        public Status PostStatus(string io_text, User i_User)
        {
            Status postedStatus = i_User.PostStatus(io_text);
            return postedStatus;
        }

        private void getAllUserStatus(UserData i_UserData)
        {
            foreach (Status currStatus in i_UserData.LocalUser.Statuses)
            {
                i_UserData.UserStatusList.Add(currStatus);
            }
        }

        public User FindFriendByName(string i_friendNameToFind, UserData i_UserData)
        {
            User friend = null;
            foreach (User user in i_UserData.UserFriendsList)
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

        public void FetchUserData(UserData i_UserData)
        {
            getAllTheNoEmptyAlbums(i_UserData);
            getAllUserFriends(i_UserData);
            getAllUserStatus(i_UserData);
            getAllTaggedFriendsFromCheckins(i_UserData);
            getAllTaggedFriendsFromPhotos(i_UserData);
        }

        private void getAllTheNoEmptyAlbums(UserData i_UserData)
        {
            foreach (Album album in i_UserData.LocalUser.Albums)
            {
                if (album.Count != 0)
                {
                    i_UserData.NonEmptyAlbumsList.Add(album);
                }
            }
        }

        private string getUserFullName(User i_User)
        {
            string name;
            try
            {
                name = i_User.FirstName + " " + i_User.LastName;
            }
            catch (Exception)
            {
                name = i_User.Name;
            }

            return name;
        }

        private void collectUsersFromTag(PhotoTag i_Tag, UserData i_UserData)
        {
            string currUserNameAsKey = string.Empty;
            currUserNameAsKey = getUserFullName(i_Tag.User);
            if(checkIfFriendIsNotLocalUser(i_UserData.LocalUser, i_Tag.User))
            {
                return;
            }

            if (i_UserData.BestFriendsDict.ContainsKey(currUserNameAsKey))
            {
                i_UserData.BestFriendsDict[currUserNameAsKey] += 1;
            }
            else
            {
                i_UserData.BestFriendsDict.Add(currUserNameAsKey, 1);
            }
        }

        private bool checkIfFriendIsNotLocalUser(User i_LocalUser, User i_FriendUser)
        {
            bool isLocalUser = false;
            string currLoggedInUserFullName = getUserFullName(i_LocalUser);
            string friendFullName = getUserFullName(i_FriendUser);
            isLocalUser = friendFullName == currLoggedInUserFullName;
            return isLocalUser;
        }

        private void getAllTaggedFriendsFromPhotos(UserData i_UserData)
        {
            foreach (Album currAlbum in i_UserData.NonEmptyAlbumsList)
            {
                foreach (Photo currPhoto in currAlbum.Photos)
                {
                    if (currPhoto.Tags != null)
                    {
                        foreach (PhotoTag currPhotoTag in currPhoto.Tags)
                        {
                            collectUsersFromTag(currPhotoTag, i_UserData);
                        }
                    }
                }
            }
        }

        private void getAllTaggedFriendsFromCheckins(UserData i_UserData)
        {
            string currUserNameAsKey = string.Empty;
            foreach (Checkin currCheckin in i_UserData.LocalUser.Checkins)
            {
                foreach (User currUser in currCheckin.TaggedUsers)
                {
                    currUserNameAsKey = getUserFullName(currUser);
                    if (checkIfFriendIsNotLocalUser(i_UserData.LocalUser, currUser)) 
                    {
                        continue;
                    }

                    if (i_UserData.BestFriendsDict.ContainsKey(currUserNameAsKey))
                    {
                        i_UserData.BestFriendsDict[currUserNameAsKey] += 1;
                    }
                    else
                    {
                        i_UserData.BestFriendsDict.Add(currUserNameAsKey, 1);
                    }
                }
            }
        }

        private void getAllUserFriends(UserData i_UserData)
        {
            foreach (User user in i_UserData.LocalUser.Friends)
            {
                i_UserData.UserFriendsList.Add(user);
            }
        }

        public List<string> GetTopFiveBestFriends(UserData i_UserData)
        {
            return new List<string>(i_UserData.OrderDictByValueInt(i_UserData.BestFriendsDict).Keys);
        }

        public void LogOutFromFacebook()
        {
            m_StoreToken.SaveLogin(string.Empty, r_TokenFileName);
        }
    }
}