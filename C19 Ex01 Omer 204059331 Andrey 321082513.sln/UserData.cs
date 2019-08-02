using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookApp
{
    class UserData
    {
        public List<User> m_UserFriends { get; set; }
        public List<Album> m_ListOfNoEmptyAlbums { get; set; }
        public User m_User { get; set; }
    }
}
