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
    public class CityConcrete : ICity
    {

        private BlueContext _context;

        public List<City> all()
        {
            List<City> cities = new List<City>();
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        cities = (from c in _context.City
                                     select c).ToList<City>();
                        if (cities.Count() == 0)
                            return cities;
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
            return cities;
        }

        public bool insert(City city)
        {
            bool flag = false;
            if (string.IsNullOrWhiteSpace(city.CityName) || city.CountryId == null || city.ProvinceId ==null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();


                        _context.City.Add(city);
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

        public bool remove(City city)
        {
            bool flag = false;
            if (city.CityId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var cit = (from c in _context.City
                                       where (c.CityId == city.CityId)
                                       select c).FirstOrDefault<City>();
                        if (city== null)
                            return (flag = false);
                        else
                        {
                            _context.City.Remove(cit);
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
                        var city = (from c in _context.City
                                       where (c.CityId == id)
                                       select c).FirstOrDefault<City>();
                        if (city == null)
                            return (flag = false);
                        else
                        {
                            _context.City.Remove(city);
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

        public City show(City city)
        {
            City cit = new City();
            if (city.CityId == null)
                return city;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        cit = (from c in _context.City
                                   where (c.CityId == city.CityId)
                                   select c).FirstOrDefault<City>();
                        if (city == null)
                            return city;
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
            return cit;
        }

        public City show_by_id(Guid id)
        {
            City city = new City();
            if (id == null)
                return city;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        city = (from c in _context.City
                                   where (c.CityId == id)
                                   select c).FirstOrDefault<City>();
                        if (city == null)
                            return city;
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
            return city;
        }

        public bool update(City city)
        {
            bool flag = false;
            if (city.CityId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Entry<City>(city).State = EntityState.Modified;
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
