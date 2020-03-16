using System;
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
    public class ProvinceConcrete : IProvince
    {

        private BlueContext _context;

        public List<Province> all()
        {
            List<Province> provinces = new List<Province>();
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        provinces = (from p in _context.Province
                                     select p).ToList<Province>();
                        if (provinces.Count() == 0)
                            return provinces;
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
            return provinces;
        }

        public bool insert(Province pr)
        {
            bool flag = false;
            if (string.IsNullOrWhiteSpace(pr.ProvinceName))
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Province.Add(pr);
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

        public List<Province> provinces_by_country(Guid id)
        {
            List<Province> provinces = new List<Province>();
            if (id == null)
                return provinces;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction((IsolationLevel.Serializable)))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();
                        provinces = (from p in _context.Province
                                     where (p.CountryId == id)
                                     select p).ToList<Province>();
                        _context.SaveChanges();
                        if(provinces.Count() == 0)
                        {
                            _transaction.Rollback();
                            return provinces;
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
            return provinces;
        }

        public bool remove(Province pr)
        {
            bool flag = false;
            if (pr.ProvinceId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var province = (from p in _context.Province
                                        where (p.ProvinceId == pr.ProvinceId)
                                        select p).FirstOrDefault<Province>();
                        if (province == null)
                            return (flag = false);
                        else
                        {
                            _context.Province.Remove(province);
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
                        var province = (from c in _context.Province
                                        where (c.ProvinceId == id)
                                        select c).FirstOrDefault<Province>();
                        if (province == null)
                            return (flag = false);
                        else
                        {
                            _context.Province.Remove(province);
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

        public Province search_by_name(string name)
        {
            Province province = new Province();
            if (string.IsNullOrWhiteSpace(name))
                return province;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();
                        province = (from p in _context.Province
                                    where
                                    (p.ProvinceName == name)
                                    select p).FirstOrDefault<Province>();
                        _context.SaveChanges();
                        if (province == null)
                        {
                            _transaction.Rollback();
                            return province;
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
            return province;
        }

        public Province show(Province pr)
        {
            Province province = new Province();
            if (pr.ProvinceId == null)
                return province;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        province = (from p in _context.Province
                                    where (p.ProvinceId == pr.ProvinceId)
                                    select p).FirstOrDefault<Province>();
                        if (province == null)
                            return province;
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
            return province;
        }

        public Province show_by_id(Guid id)
        {
            Province province = new Province();
            if (id == null)
                return province;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        province = (from p in _context.Province
                                    where (p.ProvinceId == id)
                                    select p).FirstOrDefault<Province>();
                        if (province == null)
                            return province;
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
            return province;
        }

        public bool update(Province pr)
        {
            bool flag = false;
            if (pr.ProvinceId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Entry<Province>(pr).State = EntityState.Modified;
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
