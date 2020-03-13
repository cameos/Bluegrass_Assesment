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
    public class AddressConcrete :IAddress
    {
        private BlueContext _context;

        public List<Address> all()
        {
            List<Address> addresses = new List<Address>();
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        addresses = (from a in _context.Address
                                     select a).ToList<Address>();
                        if (addresses.Count() == 0)
                            return addresses;
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
            return addresses;
        }

        public bool insert(Address ad)
        {
            bool flag = false;
            if (string.IsNullOrWhiteSpace(ad.AddressNumber))
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Address.Add(ad);
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

        public bool remove(Address ad)
        {
            bool flag = false;
            if (ad.AddressId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var address = (from a in _context.Address
                                       where (a.AddressId == ad.AddressId)
                                       select a).FirstOrDefault<Address>();
                        if (address == null)
                            return (flag = false);
                        else
                        {
                            _context.Address.Remove(address);
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
                        var address = (from a in _context.Address
                                       where (a.AddressId == id)
                                       select a).FirstOrDefault<Address>();
                        if (address == null)
                            return (flag = false);
                        else
                        {
                            _context.Address.Remove(address);
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

        public Address show(Address ad)
        {
            Address address = new Address();
            if (ad.AddressId == null)
                return address;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        address = (from a in _context.Address
                                   where (a.AddressId == ad.AddressId)
                                   select a).FirstOrDefault<Address>();
                        if (address == null)
                            return address;
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
            return address;
        }

        public Address show_by_id(Guid id)
        {
            Address address = new Address();
            if (id == null)
                return address;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        address = (from a in _context.Address
                                   where (a.AddressId == id)
                                   select a).FirstOrDefault<Address>();
                        if (address == null)
                            return address;
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
            return address;
        }

        public bool update(Address ad)
        {
            bool flag = false;
            if (ad.AddressId == null)
                return (flag = false);
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        _context.Entry<Address>(ad).State = EntityState.Modified;
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
