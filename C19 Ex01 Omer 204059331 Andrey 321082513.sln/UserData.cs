using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookApp
{
    public class UserData
    {
        public Dictionary<string, int> m_BestFriendsDict= new Dictionary<string, int>();
        public List<User> m_UserFriends { get; set; }
        public List<Album> m_ListOfNoEmptyAlbums { get; set; }
        public User m_User { get; set; }
        public UserData(User i_User)
        {
            m_User = i_User;
        }
        public Dictionary<string,int> OrderDictByValueInt(Dictionary<string, int> i_Dict)
        {
            Dictionary<string, int> resDict = i_Dict.OrderByDescending(r => r.Value).Take(5).ToDictionary(pair => pair.Key, pair => pair.Value);
            return resDict;
        }
    }
}
