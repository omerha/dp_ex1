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
        private readonly string tokenFileName = "DB.txt"; //TODO: change variable name to Guy's stupid convention
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
            i_UserData.LocalUser.PostLink(i_Photo.Link);
        }

        public UserData LoginAndInit()
        {
            UserData resUserData = null;
            if (m_StoreToken.LoadLogin(tokenFileName))
            {
                //Not sure why its not working, can you check?
                LoginResult = FacebookService.Connect(m_StoreToken.m_Token);
            }
            else
            {
                LoginResult = FacebookService.Login(
                    "1450160541956417",
                    // "753926335063958",
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
                    m_StoreToken.SaveLogin(LoginResult.AccessToken, tokenFileName);
                }
            }
            resUserData = new UserData(LoginResult.LoggedInUser);
            FetchUserData(resUserData);
            return resUserData;
        }


        public Status PostStatus(string io_text, UserData i_UserData)
        {
            Status postedStatus = i_UserData.LocalUser.PostStatus(io_text);
            return postedStatus;
        }


        private void GetAllUserStatus(UserData i_UserData)
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
            GetAllTheNoEmptyAlbums(i_UserData);
            GetAllUserFriends(i_UserData);
            GetAllUserStatus(i_UserData);
            GetAllTaggedFriendsFromCheckins(i_UserData);
            GetAllTaggedFriendsFromPhotos(i_UserData);
        }


        private void GetAllTheNoEmptyAlbums(UserData i_UserData)
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
            string currUserNameAsKey = "";
            currUserNameAsKey = getUserFullName(i_Tag.User);

            if (i_UserData.BestFriendsDict.ContainsKey(currUserNameAsKey))
            {
                i_UserData.BestFriendsDict[currUserNameAsKey] += 1;
            }
            else
            {
                i_UserData.BestFriendsDict.Add(currUserNameAsKey, 1);
            }
        }
        
        public void GetAllTaggedFriendsFromPhotos(UserData i_UserData)
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


        private void GetAllTaggedFriendsFromCheckins(UserData i_UserData)
        {
            string currUserNameAsKey = "";
            foreach (Checkin currCheckin in i_UserData.LocalUser.Checkins)
            {
                foreach (User currUser in currCheckin.TaggedUsers)
                {
                    currUserNameAsKey = currUser.FirstName + " " + currUser.LastName;
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


        private void GetAllUserFriends(UserData i_UserData)
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
    }
}

