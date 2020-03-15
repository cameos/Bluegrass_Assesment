using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Context;
using Abstraction.Abstract;
using System.Data;
using System.Data.Entity;

namespace Abstraction.Concrete
{
    public class AdminConcrete : IAdmin
    {

        private BlueContext _context;

        public List<Admin> all_admin()
        {
            List<Admin> admins = new List<Admin>();
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                        _context.Database.Connection.Open();
                    try
                    {
                        admins = (from a in _context.Admin
                                  select a).ToList<Admin>();
                        _context.SaveChanges();
                        if(admins.Count() == 0)
                        {
                            _transaction.Rollback();
                            return admins;
                        }
                        else
                        {
                            _transaction.Commit();
                        }
                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            _context.Database.Connection.Close();
            return admins;
        }

        public bool new_admin(Admin admin)
        {
            bool flag = false;
            if(string.IsNullOrWhiteSpace(admin.Salt)|| string.IsNullOrWhiteSpace(admin.Password)|| string.IsNullOrWhiteSpace(admin.Email))
                return (flag = false);
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        _context.Admin.Add(admin);
                        _context.SaveChanges();
                        _transaction.Commit();
                        flag = true;

                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return flag;
        }

        public Admin show_admin(Guid id)
        {
            Admin admin = new Admin();
            if(id== null)
            {
                return admin;
            }
            using (_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();
                        admin = (from a in _context.Admin
                                 where (a.AdminId == id)
                                 select a).FirstOrDefault<Admin>();
                        _context.SaveChanges();
                        if(admin==null)
                        {
                            _transaction.Rollback();
                            return admin;
                        }
                        else
                        {
                            _transaction.Commit();
                        }
                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            _context.Database.Connection.Close();
            return admin;
        }

        public Admin show_by_email(string email)
        {
            Admin admin = new Admin();
            if (string.IsNullOrWhiteSpace(email) || !(email.Contains("@")))
                return admin;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        admin = (from a in _context.Admin
                                 where (a.Email == email)
                                 select a).FirstOrDefault<Admin>();
                        _context.SaveChanges();
                        if(admin == null)
                        {
                            _transaction.Rollback();
                            return admin;
                        }
                        else
                        {
                            _transaction.Commit();
                        }
                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return admin;
        }

        public bool update_admin(Admin admin)
        {
            bool flag = false;
            if (admin.AdminId == null)
                return (flag = false);
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();
                        _context.Entry<Admin>(admin).State = EntityState.Modified;
                        _context.SaveChanges();
                        _transaction.Commit();
                        flag = true;
                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message, new Exception());
                    }
                }
            }
            _context.Database.Connection.Close();
            return flag;
        }
    }
}
