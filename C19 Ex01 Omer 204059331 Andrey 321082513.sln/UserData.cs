using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookWrapper.ObjectModel;

namespace FacebookApp
{
    public class UserData
    {
        public Dictionary<string, int> BestFriendsDict { get; set; }

        public List<User> UserFriendsList { get; set; }

        public List<Album> NonEmptyAlbumsList { get; set; }

        public List<Status> UserStatusList { get; set; }

        public User LocalUser { get; set; }

        public UserData(User i_User)
        {
            LocalUser = i_User;
            BestFriendsDict = new Dictionary<string, int>();
            UserFriendsList = new List<User>();
            NonEmptyAlbumsList = new List<Album>();
            UserStatusList = new List<Status>();
        }

        public Dictionary<string, int> OrderDictByValueInt(Dictionary<string, int> i_Dict)
        {
            Dictionary<string, int> resDict = i_Dict.OrderByDescending(r => r.Value).Take(5).ToDictionary(pair => pair.Key, pair => pair.Value);
            return resDict;
        }
    }
}
