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
    public class CountryConcrete : ICountry
    {


        private BlueContext _context;

        public List<Country> all()
        {
            List<Country> countries = new List<Country>();
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {

                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        countries = (from c in _context.Country
                                     select c).ToList<Country>();
                        _context.SaveChanges();
                        if (countries.Count() == 0)
                        {
                            _context.Database.Connection.Close();
                            return countries;
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
           
            return countries;
        }

        public bool insert(Country cou)
        {
            bool flag = false;
            if (string.IsNullOrWhiteSpace(cou.CountryName))
                return (flag = false);
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();


                        _context.Country.Add(cou);
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

        public bool remove(Country cou)
        {
            bool flag = false;
            if (cou.CountryId == null)
                return (flag = false);
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var country = (from c in _context.Country
                                       where (c.CountryId == cou.CountryId)
                                       select c).FirstOrDefault<Country>();
                        if (country == null)
                            return (flag = false);
                        else
                        {
                            _context.Country.Remove(country);
                            _context.SaveChanges();
                            _transaction.Commit();
                            flag = true;
                        }
                    }
                    catch(Exception e)
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
                        var country = (from c in _context.Country
                                       where (c.CountryId == id)
                                       select c).FirstOrDefault<Country>();
                        if (country == null)
                            return (flag = false);
                        else
                        {
                            _context.Country.Remove(country);
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

        public Country search_by_name(string name)
        {
            Country country = new Country();
            if (string.IsNullOrWhiteSpace(name))
                return country;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();
                        country = (from c in _context.Country
                                   where
                                   (c.CountryName == name)
                                   select c).FirstOrDefault<Country>();
                        _context.SaveChanges();
                        if(country == null)
                        {
                            _transaction.Rollback();
                            return country;
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
            return country;
        }

        public Country show(Country cou)
        {
            Country country = new Country();
            if (cou.CountryId == null)
                return country;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        country = (from c in _context.Country
                                   where (c.CountryId == cou.CountryId)
                                   select c).FirstOrDefault<Country>();
                        if (country == null)
                            return country;
                        else
                        {
                            _context.SaveChanges();
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
            return country;
        }

        public Country show_by_id(Guid id)
        {
            Country country = new Country();
            if (id == null)
                return country;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        country = (from c in _context.Country
                                   where (c.CountryId == id)
                                   select c).FirstOrDefault<Country>();
                        if (country == null)
                            return country;
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
            return country;
        }

        public bool update(Country cou)
        {
            bool flag = false;
            if (cou.CountryId == null)
                return (flag = false);
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Entry<Country>(cou).State = EntityState.Modified;
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
    }
}
