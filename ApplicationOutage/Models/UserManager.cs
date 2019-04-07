using ApplicationOutage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationOutage.Models
{
    public class UserManager
    {
        public bool RegisterUser(UsersViewModel User)
        {
            try
            {
                using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
                {
                    UsersInfo mappedUser = new UsersInfo() { FirstName = User.FirstName, LastName = User.LastName, Password = User.EncryptPassword, UserEmail = User.UserEmail, IsActive = false };
                    entities.UsersInfoes.Add(mappedUser);
                    entities.SaveChanges();
                    return true;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool IsUserActivated(LoginModel loginUser)
        {
            try
            {
                using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
                {
                    UsersInfo user = entities.UsersInfoes.FirstOrDefault(x => x.UserEmail == loginUser.UserEmail && x.Password == loginUser.EncryptPassword && x.IsActive);
                    if(user!=null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal UsersViewModel GetUser(int? id)
        {
            try
            {
                using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
                {
                    var user = entities.UsersInfoes.FirstOrDefault(x => x.Id == id);
                    if (user != null)
                    {
                        return new UsersViewModel() { Id = user.Id, FirstName = user.FirstName, IsActive = user.IsActive, LastName = user.LastName, UserEmail = user.UserEmail };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal bool EditUser(UsersViewModel updatedUser)
        {
            try
            {
                using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
                {
                    var user = entities.UsersInfoes.FirstOrDefault(x => x.Id == updatedUser.Id);
                    if (user != null)
                    {
                        user.IsActive = updatedUser.IsActive;
                        user.LastName = updatedUser.LastName;
                        user.FirstName = updatedUser.FirstName;
                        user.UserEmail = updatedUser.UserEmail;
                        entities.Entry(user).CurrentValues.SetValues(user);
                        entities.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<UsersViewModel> GetRegisteredUsers()
        {
            try
            {
                using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
                {
                   return entities.UsersInfoes.Where(x=>!x.IsAdmin).Select(x => new UsersViewModel { Id = x.Id, IsActive = x.IsActive,
                                                FirstName = x.FirstName,
                                                LastName = x.LastName,
                                                UserEmail = x.UserEmail })
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ResetPassword(ForgotPasswordModel resetUser)
        {
            try
            {
                using (ApplicationOutageEntities entities = new ApplicationOutageEntities())
                {
                    var user = entities.UsersInfoes.FirstOrDefault(x => x.UserEmail.ToLower().Trim() == resetUser.UserEmail.ToLower().Trim());
                    if (user != null)
                    {
                        user.Password = resetUser.EncryptPassword;
                        entities.Entry(user).CurrentValues.SetValues(user);
                        entities.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}