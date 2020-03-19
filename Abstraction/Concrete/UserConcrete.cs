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
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();


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
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        //find addresses and delete
                        var addressId = (from a in _context.UserAddress
                                         where (a.UserId == id)
                                         select a).ToList<UserAddress>();
                        List<Address> addresses = new List<Address>();
                        foreach(var ads in addressId)
                        {
                            var adss = (from ad in _context.Address
                                        where (ad.AddressId == ads.AddressId)
                                        select ad).FirstOrDefault<Address>();
                            addresses.Add(adss);
                        }
                        _context.Address.RemoveRange(addresses);
                        _context.SaveChanges();

                        //remove user
                        var user = (from u in _context.User
                                    where (u.UserId == id)
                                    select u).FirstOrDefault<User>();
                        if (user == null)
                        {
                            _transaction.Rollback();
                            return (flag = false);
                        }
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

        public List<User> search_first(string name)
        {
            List<User> users = new List<User>();
            if (string.IsNullOrWhiteSpace(name))
                return users;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();


                        users = (from u in _context.User
                                 where (u.FirstName.Contains(name))
                                 select u).ToList<User>();
                        _context.SaveChanges();
                        
                        if(users.Count() == 0)
                        {
                            _transaction.Rollback();
                            return users;
                        }

                        _transaction.Commit();

                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return users;
        }

        public List<User> search_surname(string surname)
        {
            List<User> users = new List<User>();
            if (string.IsNullOrWhiteSpace(surname))
                return users;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        users = (from u in _context.User
                                 where (u.LastName.Contains(surname))
                                 select u).ToList<User>();
                        _context.SaveChanges();
                        if(users.Count() == 0)
                        {
                            _transaction.Rollback();
                            return users;
                        }
                        _transaction.Commit();
                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return users;
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

                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        user = (from u in _context.User
                                   where (u.UserId == id)
                                   select u).FirstOrDefault<User>();
                        _context.SaveChanges();
                        if (user == null)
                        {
                            _transaction.Rollback();
                            return user;
                        }
                        else
                        {
                           
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

                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

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
