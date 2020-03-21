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

        public UserCities contact_home()
        {
            UserCities userCities = new UserCities();
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        var users = (from u in _context.User
                                     select u).ToList<User>();
                        _context.SaveChanges();


                        var countries = (from c in _context.Country
                                         select (c)).ToList<Country>();
                        _context.SaveChanges();


                        if (users.Count() != 0)
                        {
                            userCities.Users = users;
                        }
                        if (countries.Count() != 0)
                        {
                            userCities.Countries = countries;
                        }

                    }
                    catch (Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return userCities;
        }

        public FullUserInformation full_info(Guid id)
        {
            FullUserInformation fullUserInformation = new FullUserInformation();
            if (id == null || id == Guid.Empty)
                return fullUserInformation;
            using(_context = new BlueContext())
            {
                using(var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        var user = (from u in _context.User
                                    where (u.UserId == id)
                                    select u).SingleOrDefault<User>();
                        _context.SaveChanges();

                        var userAddress = (from us in _context.UserAddress
                                           where (us.UserId == user.UserId)
                                           select us).SingleOrDefault<UserAddress>();
                        _context.SaveChanges();

                        var address = (from a in _context.Address
                                       where (a.AddressId == userAddress.AddressId)
                                       select a).SingleOrDefault<Address>();
                        _context.SaveChanges();

                        var country = (from c in _context.Country
                                       where (c.CountryId == address.CountryId)
                                       select c).SingleOrDefault<Country>();
                        _context.SaveChanges();

                        var province = (from p in _context.Province
                                        where (p.ProvinceId == address.ProvinceId)
                                        select p).SingleOrDefault<Province>();
                        _context.SaveChanges();

                        var city = (from ci in _context.City
                                    where (ci.CityId == address.CityId)
                                    select ci).SingleOrDefault<City>();
                        _context.SaveChanges();

                        fullUserInformation.User = user;
                        fullUserInformation.Address = address;
                        fullUserInformation.Country = country;
                        fullUserInformation.Province = province;
                        fullUserInformation.City = city;

                        _transaction.Commit();


                    }
                    catch(Exception e)
                    {
                        _transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
            return fullUserInformation;
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

                        if (user_id == null)
                        {
                            _transaction.Rollback();
                            return (flag = false);
                        }

                        //add address for the user
                        _context.Address.Add(address);
                        _context.SaveChanges();
                        var addr_id = address.AddressId;

                        if (addr_id == null)
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
                        foreach (var ads in addressId)
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

        public List<User> search_first(PredictiveFilter filter)
        {
            List<User> users = new List<User>();
            if (string.IsNullOrWhiteSpace(filter.First))
                return users;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();


                        List<Address> addresses = new List<Address>();
                        if (filter.CountryId != Guid.Empty && filter.ProvinceId != Guid.Empty && filter.CityId != Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.CountryId == filter.CountryId && a.ProvinceId == filter.ProvinceId && a.CityId == filter.CityId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }


                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }


                        }
                        else if (filter.CountryId != Guid.Empty && filter.ProvinceId != Guid.Empty && filter.CityId == Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.CountryId == filter.CountryId && a.ProvinceId == filter.ProvinceId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }

                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }



                        }
                        else if (filter.CountryId != Guid.Empty && filter.ProvinceId == Guid.Empty && filter.CityId == Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.CountryId == filter.CountryId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }

                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }


                        }
                        else if (filter.CountryId == Guid.Empty && filter.ProvinceId != Guid.Empty && filter.CityId != Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.ProvinceId == filter.ProvinceId && a.CityId == filter.CityId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }

                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }



                        }
                        else if (filter.CountryId != Guid.Empty && filter.ProvinceId == Guid.Empty && filter.CityId != Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.CountryId == filter.CountryId && a.CityId == filter.CityId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }


                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }


                        }
                        else if (filter.CountryId == Guid.Empty && filter.ProvinceId == Guid.Empty && filter.CityId != Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.CityId == filter.CityId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }
                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }
                        }
                        else if (filter.CountryId == Guid.Empty && filter.ProvinceId != Guid.Empty && filter.CityId == Guid.Empty)
                        {
                            addresses.Clear();
                            var ad1 = (from a in _context.Address
                                       where (a.CityId == filter.CityId)
                                       select a).ToList<Address>();
                            if (ad1.Count() != 0)
                            {
                                addresses.AddRange(ad1);
                            }
                            List<UserAddress> userAddresses = new List<UserAddress>();
                            if (addresses.Count() > 0)
                            {
                                userAddresses.Clear();
                                foreach (var a in addresses)
                                {
                                    var ads = (from ad in _context.UserAddress
                                               where (ad.AddressId == a.AddressId)
                                               select ad).SingleOrDefault<UserAddress>();
                                    userAddresses.Add(ads);
                                }
                            }

                            if (userAddresses.Count() > 0)
                            {
                                users.Clear();
                                foreach (var ds in userAddresses)
                                {
                                    var user = (from u in _context.User
                                                where (u.UserId == ds.UserId && u.FirstName.Contains(filter.First))
                                                select u).SingleOrDefault<User>();
                                    users.Add(user);
                                }
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            users = (from u in _context.User
                                     where (u.FirstName.Contains(filter.First))
                                     select u).ToList<User>();
                        }



                        if (users.Count() == 0)
                        {
                            _transaction.Rollback();
                            return users;
                        }

                        _transaction.Commit();

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

        public List<User> search_surname(string surname)
        {
            List<User> users = new List<User>();
            if (string.IsNullOrWhiteSpace(surname))
                return users;
            using (_context = new BlueContext())
            {
                using (var _transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        if (_context.Database.Connection.State == ConnectionState.Closed || _context.Database.Connection.State == ConnectionState.Broken)
                            _context.Database.Connection.Open();

                        users = (from u in _context.User
                                 where (u.LastName.Contains(surname))
                                 select u).ToList<User>();
                        _context.SaveChanges();
                        if (users.Count() == 0)
                        {
                            _transaction.Rollback();
                            return users;
                        }
                        _transaction.Commit();
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
