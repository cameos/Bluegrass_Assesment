﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Abstract;
using DataAccess.Entities;
using DataAccess.Context;
using System.Data;
using System.Data.Entity;

namespace Abstraction.Concrete
{
    public class UserConcrete : IUser
    {

        private BlueContext _context;

        public List<User> all()
        {
            List<User> users = new List<User>();
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        users = (from u in _context.User
                                 select u).ToList<User>();
                        if (users.Count() == 0)
                            return users;
                        else
                        {
                            _context.SaveChanges();
                            _transaction.Commit();

                        }
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return users;
        }

        public bool insert(User user, Address address)
        {
            bool flag = false;
            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(address.AddressNumber))
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        //add user first
                        _context.User.Add(user);
                        _context.SaveChanges();
                        var user_id = user.UserId;

                        if(user_id == null)
                        {
                            _transaction.Rollback();
                            return (flag = false);
                        }

                        //add address for the user
                        _context.Address.Add(address);
                        _context.SaveChanges();
                        var addr_id = address.AddressId;

                        if(addr_id == null)
                        {
                            _transaction.Rollback();
                            return (flag = false);
                        }

                        //add user_adress
                        UserAddress userAddress = new UserAddress
                        {
                            AddressId = addr_id,
                            UserId = user_id,
                        };
                        _context.UserAddress.Add(userAddress);
                        _context.SaveChanges();

                        _transaction.Commit();
                        flag = true;
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return flag;
        }

        public bool remove(User user)
        {
            bool flag = false;
            if (user.UserId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var ur = (from u in _context.User
                                       where (u.UserId == user.UserId)
                                       select u).FirstOrDefault<User>();
                        if (ur == null)
                            return (flag = false);
                        else
                        {
                            _context.User.Remove(ur);
                            _context.SaveChanges();
                            _transaction.Commit();
                            flag = true;
                        }
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message, new Exception());
                    }
                }
            }
            return flag;
        }

        public bool remove_by_id(Guid id)
        {
            bool flag = false;
            if (id == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var user = (from u in _context.User
                                       where (u.UserId == id)
                                       select u).FirstOrDefault<User>();
                        if (user == null)
                            return (flag = false);
                        else
                        {
                            _context.User.Remove(user);
                            _context.SaveChanges();
                            _transaction.Commit();
                            flag = true;
                        }
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message, new Exception());
                    }
                }
            }
            return flag;
        }

        public User show(User user)
        {
            User show = new User();
            if (user.UserId == null)
                return show;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        show = (from u in _context.User
                                   where (u.UserId == user.UserId)
                                   select u).FirstOrDefault<User>();
                        if (show == null)
                            return user;
                        else
                        {
                            _context.SaveChanges();
                            _transaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return show;
        }

        public User show_by_id(Guid id)
        {
            User user = new User();
            if (id == null)
                return user;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        user = (from u in _context.User
                                   where (u.UserId == id)
                                   select u).FirstOrDefault<User>();
                        if (user == null)
                            return user;
                        else
                        {
                            _context.SaveChanges();
                            _transaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return user;
        }

        public bool update(User user)
        {
            bool flag = false;
            if (user.UserId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Entry<User>(user).State = EntityState.Modified;
                        _context.SaveChanges();
                        _transaction.Commit();
                        flag = true;
                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return flag;
        }
    }
}