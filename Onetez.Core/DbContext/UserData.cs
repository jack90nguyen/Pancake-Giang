using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Core.Data_v1
{
    public class UserData
    {
        #region Thong Tin User

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="uPass"></param>
        /// <returns></returns>
        public static bool Login(string uName, string uPass)
        {
            UserEntity user = null;
            int status = 5;

            //Don't do anything if we don't have a valid email or password
            if (!string.IsNullOrEmpty(uName) || !string.IsNullOrEmpty(uPass))
            {

                var users = new UserCollection();
                IPredicateExpression filter = new PredicateExpression(UserFields.UserName == uName);

                if (users.GetMulti(filter))
                {
                    if (users.Count > 0)
                    {
                        user = new UserEntity();
                        user.FetchUsingPK(users[0].UserId);
                    }
                }

                if (user == null)
                {
                    status = 0;
                }
                else
                {
                    if (user.IsLocked)
                        status = 1;

                    // Is the password correct?
                    string t = FormsAuthentication.HashPasswordForStoringInConfigFile(uPass, "MD5");
                    if (FormsAuthentication.HashPasswordForStoringInConfigFile(uPass, "MD5") == user.Password)
                    {
                        if (!user.IsActive)
                            status = 3;
                    }
                    else
                    {
                        status = 2;
                    }
                }
            }
            return status == 5 ? true : false;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="uPassMd5"></param>
        /// <returns></returns>
        public static bool LoginMd5(string uName, string uPassMd5)
        {
            UserEntity user = null;

            //Don't do anything if we don't have a valid email or password
            if (!string.IsNullOrEmpty(uName) || !string.IsNullOrEmpty(uPassMd5))
            {
                var users = new UserCollection();
                IPredicateExpression filter = new PredicateExpression(UserFields.UserName == uName);
                filter.AddWithOr(UserFields.EmailAddress == uName);
                if (users.GetMulti(filter))
                {
                    if (users.Count > 0)
                    {
                        user = new UserEntity();
                        user.FetchUsingPK(users[0].UserId);
                    }
                }

                if (user == null)// not had username
                    return false;
                else
                {
                    if (user.IsLocked)
                        return false;
                    else
                    {
                        // Is the password correct?
                        if (uPassMd5 == FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "MD5"))
                        {
                            if (!user.IsActive)
                                return false;
                            else
                                return true;
                        }
                        else
                            return false;
                    }
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Tao User Moi
        /// </summary>
        /// <returns></returns>
        public static int CreateUser(string userName, string email, string password, out UserEntity user)
        {
            user = null;
            int status = 1;
            var users = new UserCollection();
            IPredicateExpression filter = new PredicateExpression(UserFields.UserName == userName);

            if (users.GetMulti(filter))
            {
                if (users.Count > 0)
                {
                    status = 0;
                }
            }

            if (status == 1)
            {
                var userEntity = new UserEntity
                {
                    UserName = userName,
                    EmailAddress = email,
                    Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"),
                    Avatar = "/Images/noavatar.png",
                    IsActive = true,
                    IsLocked = false
                };
                userEntity.Save();
                userEntity.UserGuid = FormsAuthentication.HashPasswordForStoringInConfigFile(userEntity.UserId.ToString(), "MD5");
                userEntity.Save();

                user = userEntity;
            }
            return status;
        }

        /// <summary>
        /// Danh Sach User Kha Dung
        /// </summary>
        /// <returns></returns>
        public static UserCollection UserList()
        {
            UserCollection userCollection = new UserCollection();
            IPredicateExpression filter = new PredicateExpression();
            filter.AddWithAnd(UserFields.IsLocked == false);
            filter.AddWithAnd(UserFields.IsActive == true);
            userCollection.GetMulti(filter);
            return userCollection;
        }

        /// <summary>
        /// Thong Tin User: UserId
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static UserEntity RetrieveUser(Int64 uId)
        {
            var query = from p in UserList()
                        where p.UserId == uId
                        select p;
            if (query.Count() == 1)
                return query.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// Thong Tin User: UserName/Email
        /// </summary>
        /// <param name="uName"></param>
        /// <returns></returns>
        public static UserEntity RetrieveUser(string uName)
        {
            if (uName.Contains("@"))
            {
                var query = from p in UserList()
                            where p.EmailAddress.ToLower() == uName.ToLower()
                            select p;
                if (query.Count() > 0)
                    return query.FirstOrDefault();
            }
            else
            {
                var query = from p in UserList()
                            where p.UserName.ToLower() == uName.ToLower()
                            select p;
                if (query.Count() > 0)
                    return query.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Thong Tin User: UserGuid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static UserEntity RetrieveUserGuid(string guid)
        {
            var db = new LinqMetaData();
            var query = from u in db.User
                        where !u.IsLocked && u.UserGuid == guid
                        select u;
            if (query.Count() > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Thong Tin User: PasswordResetCode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static UserEntity RetrieveUserByCode(string code)
        {
            var db = new LinqMetaData();
            var query = from u in db.User
                        where !u.IsLocked && u.PasswordResetCode == code
                        select u;
            if (query.Count() > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Lấy thông tin User, kể cả đã xóa
        /// </summary>
        /// <returns></returns>
        public static UserEntity GetAnyUser(long id)
        {
            var db = new LinqMetaData();
            var query = from u in db.User
                        where u.UserId == id
                        select u;
            if (query.Count() > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        #endregion

        #region Phan Quyen

        /// <summary>
        /// Danh Sach Quyen
        /// </summary>
        /// <returns></returns>
        public static List<RoleEntity> RetrieveRole()
        {
            var db = new LinqMetaData();
            var query = (from u in db.Role
                         select u).ToList();
            return query;
        }

        /// <summary>
        /// Danh Sach Quyen cua User
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string[] GetRolesForUser(string username)
        {
            var user = RetrieveUser(username);
            if (user == null)
                return new string[] { };
            else
            {
                var db = new LinqMetaData();
                var query = from u in db.UserRole
                            join r in db.Role on u.RoleId equals r.RoleId
                            where u.UserId == user.UserId
                            select r.RoleName;

                return query.ToArray();
            }
        }

        /// <summary>
        /// Danh Sach Quyen cua User, trả về tên quyền
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string[] GetRolesForUser(long userId)
        {
            var db = new LinqMetaData();
            var query = from u in db.UserRole
                        join r in db.Role on u.RoleId equals r.RoleId
                        where u.UserId == userId
                        select r.Description;
            return query.ToArray();
        }

        /// <summary>
        /// Danh Sach User co Quyen roleName
        /// </summary>
        /// <param name="roleName"></param>
        public static string[] RetrieveUsersInRole(string roleName)
        {
            var db = new LinqMetaData();
            var query = (from u in db.User
                         join ur in db.UserRole
                         on u.UserId equals ur.UserId
                         join r in db.Role
                         on ur.RoleId equals r.RoleId
                         where r.RoleName == roleName
                         select u.EmailAddress).ToArray();
            return query;
        }

        /// <summary>
        /// Danh Sach User co Quyen
        /// roleName: Admin|Manager|Staff
        /// </summary>
        /// <returns></returns>
        public static List<UserEntity> RetrieveListUsersInRole(string roleName)
        {

            List<string> tt = new List<string>();
            string[] listRole = roleName.Split('|');
            for (int i = 0; i < listRole.Length; i++)
            {
                if (!string.IsNullOrEmpty(listRole[i]))
                    tt.Add(listRole[i]);
            }

            if (tt.Count > 0)
            {
                var db = new LinqMetaData();
                var query = from ur in db.UserRole
                            join r in db.Role on ur.RoleId equals r.RoleId
                            join u in db.User on ur.UserId equals u.UserId
                            where tt.Contains(r.RoleName) && u.IsActive && !u.IsLocked
                            orderby u.UserName
                            select u;
                if (query.Count() > 0)
                    return query.ToList();
            }
            return null;
        }

        /// <summary>
        /// Danh Sach Quyen cua 1 User
        /// </summary>
        /// <returns></returns>
        public static List<UserRoleModel> RetrieveUserInRole(long userId)
        {
            var db = new LinqMetaData();
            var query = from u in db.UserRole
                        join r in db.Role on u.RoleId equals r.RoleId
                        where u.UserId == userId
                        orderby u.RoleId
                        select new UserRoleModel
                        {
                            UserInRoleId = u.UserInRoleId,
                            RoleId = u.RoleId,
                            RoleName = r.RoleName,
                            Description = r.Description
                        };
            return query.ToList();
        }

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        /// <returns></returns>
        public static RoleCollection RoleList()
        {
            RoleCollection roleCollection = new RoleCollection();
            roleCollection.GetMulti(null);
            return roleCollection;
        }

        /// <summary>
        /// Thong Tin Quyen theo RoleName
        /// </summary>
        /// <param name="rName"></param>
        /// <returns></returns>
        public static RoleEntity RetrieveRole(string rName)
        {
            var query = from p in RoleList()
                        where p.RoleName == rName
                        select p;
            if (query.Count() == 1)
                return query.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// Kiem Tra User co Quyen hay khong
        /// roleName: Admin|Manager|Staff|Accountant
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool RetrieveRole(long userId, string roleName)
        {
            List<string> tt = new List<string>();
            string[] listRole = roleName.Split('|');
            for (int i = 0; i < listRole.Length; i++)
            {
                if (!string.IsNullOrEmpty(listRole[i]))
                    tt.Add(listRole[i]);
            }

            if (tt.Count > 0)
            {
                var db = new LinqMetaData();
                var query = from ur in db.UserRole
                            join r in db.Role on ur.RoleId equals r.RoleId
                            where ur.UserId == userId && tt.Contains(r.RoleName)
                            select ur;
                if (query.Count() > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Kiem Tra User Co Quyen Truy Cap hay khong
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckAccessPermissions(UserEntity user)
        {
            if (user != null)
                return true;
            return false;
        }

        /// <summary>
        /// Them Quyen cho 1 User
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public static void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var i in usernames)
            {
                foreach (var k in roleNames)
                {
                    UserEntity user = RetrieveUser(i);
                    RoleEntity role = RetrieveRole(k);

                    //Kiem tra User co quyen hay chua, chua thi them quyen
                    if (!RetrieveRole(user.UserId, role.RoleName))
                    {
                        var newRole = new UserRoleEntity { UserId = user.UserId, RoleId = role.RoleId };
                        newRole.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Xoa Quyen
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public static void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            int userInRoleId = 0;
            foreach (var i in usernames)
            {
                foreach (var k in roleNames)
                {
                    if (IsUserInRole(i, k, out userInRoleId))
                    {
                        var userInRole = RetrieveUserInRole(userInRoleId);
                        if(userInRole != null)
                            userInRole.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Ngay nay co quyen nay hay khong
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool IsUserInRole(string username, string roleName, out int userInRoleId)
        {
            userInRoleId = 0;
            UserEntity user = RetrieveUser(username);
            RoleEntity role = RetrieveRole(roleName);
            if (user == null)

                return false;
            var userRoleCollection = new UserRoleCollection();
            var filter = new PredicateExpression();
            filter.AddWithAnd(UserRoleFields.UserId == user.UserId);
            filter.AddWithAnd(UserRoleFields.RoleId == role.RoleId);
            userRoleCollection.GetMulti(filter);

            if (userRoleCollection.Count > 0)
            {
                userInRoleId = userRoleCollection.First().UserInRoleId;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ngay nay co quyen nay hay khong
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool IsUserInRole(string username, string roleName)
        {
            UserEntity user = RetrieveUser(username);
            RoleEntity role = RetrieveRole(roleName);
            if (user == null)
                return false;
            var userRoleCollection = new UserRoleCollection();
            var filter = new PredicateExpression();
            filter.AddWithAnd(UserRoleFields.UserId == user.UserId);
            filter.AddWithAnd(UserRoleFields.RoleId == role.RoleId);
            userRoleCollection.GetMulti(filter);

            return userRoleCollection.Count > 0 ? true : false;
        }

        /// <summary>
        /// Quan he cua User va Role
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        public static UserRoleEntity RetrieveUserInRole(int userRoleId)
        {

            var data = new LinqMetaData();
            var query = from p in data.UserRole
                        where p.UserInRoleId == userRoleId
                        select p;
            if (query.Count() > 0) return query.FirstOrDefault();
            return null;
        }

        #endregion

        #region Danh Sach User

        /// <summary>
        /// Tim kiem danh sach thanh vien
        /// </summary>
        /// <returns></returns>
        public static List<UserInfoModel> SearchUserList(int paging, int size, string keyword, int role, out int total)
        {
            keyword = ConvertString.NoVNeseLower(keyword).Replace("-", " ");
            var db = new LinqMetaData();
            List<UserInfoModel> list = new List<UserInfoModel>();
            
            //Tat ca User
            List<UserInfoModel> listUser;
            if (role == 0)
                listUser = (from u in db.User
                            where !u.IsLocked
                            orderby u.UserName
                            select new UserInfoModel
                            {
                                UserId = u.UserId,
                                UserGuid = u.UserGuid,
                                UserName = u.UserName,
                                EmailAddress = u.EmailAddress,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Phone = u.Phone,
                                Address = u.Address,
                                Avatar = u.Avatar,
                                IsActive = u.IsActive,
                                LastOnline = u.LastOnline,
                                Birthday = u.Birthday,
                                BranchId = u.BranchId
                            }).ToList();
            else
            {
                listUser = (from u in db.User
                            join p in db.UserRole
                            on u.UserId equals p.UserId
                            where !u.IsLocked && p.RoleId == role
                            orderby u.UserName
                            select new UserInfoModel
                            {
                                UserId = u.UserId,
                                UserGuid = u.UserGuid,
                                UserName = u.UserName,
                                EmailAddress = u.EmailAddress,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Phone = u.Phone,
                                Address = u.Address,
                                Avatar = u.Avatar,
                                IsActive = u.IsActive,
                                LastOnline = u.LastOnline,
                                Birthday = u.Birthday,
                                BranchId = u.BranchId
                            }).ToList();
            }

            //Bộ lọc tìm kiếm
            foreach (var info in listUser)
            {
                //Tìm kiếm theo tên đăng nhập
                bool resultUser = false;
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (info.UserName.ToLower().Contains(keyword))
                        resultUser = true;
                }
                else
                    resultUser = true;

                //Tìm kiếm theo họ tên
                bool resultName = false;
                if (!string.IsNullOrEmpty(keyword))
                {
                    string fullName = ConvertString.NoVNeseLower(info.FirstName + " " + info.LastName).Replace("-", " ");
                    if (fullName.Contains(keyword))
                        resultName = true;
                }
                else
                    resultName = true;

                //Tìm kiếm theo email
                bool resultEmail = false;
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (info.EmailAddress.ToLower().Contains(keyword))
                        resultEmail = true;
                }
                else
                    resultEmail = true;

                //Tìm kiếm theo email
                bool resultPhone = false;
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (info.Phone.ToLower().Contains(keyword))
                        resultPhone = true;
                }
                else
                    resultPhone = true;

                //Gộp kết quả tìm kiếm
                if (resultUser || resultName || resultEmail || resultPhone)
                {
                    info.Avatar = !string.IsNullOrEmpty(info.Avatar) ? info.Avatar : "/Images/noavatar.png";
                    info.RoleArray = GetRolesForUser(info.UserId);
                    list.Add(info);
                }
            }

            total = list.Count;
            return list.Skip(size * (paging - 1)).Take(size).ToList();
        }


        /// <summary>
        /// Tim kiem danh sach thanh vien
        /// </summary>
        /// <returns></returns>
        public static List<UserModel> GetJsonUserList(List<long> listId)
        {
            var db = new LinqMetaData();

            var listUser = (from u in db.User
                            where listId.Contains(u.UserId)
                            orderby u.UserName
                            select new UserModel
                            {
                                id = u.UserId,
                                guid = u.UserGuid,
                                username = u.UserName,
                                avatar = u.Avatar,
                                link = "/user/info/" + u.UserGuid
                            }).ToList();

            return listUser;
        }

        #endregion

        #region Du Lieu Co Dinh

        /// <summary>
        /// Gioi Tinh
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticListGender()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Nam" });
            list.Add(new SelectModel { id = "2", name = "Nữ" });
            return list;
        }

        #endregion
    }
}
