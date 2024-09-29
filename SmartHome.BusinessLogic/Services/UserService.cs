﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.BusinessLogic.Domain;
using SmartHome.BusinessLogic.Interfaces;
using SmartHome.BusinessLogic.GenericRepositoryInterface;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.CustomExceptions;

namespace SmartHome.BusinessLogic.Services;
public sealed class UserService : IHomeOwnerLogic, IUsersLogic, IBusinessOwnerLogic
{
    private readonly IGenericRepository<User> _userRepository;

    private const int MinPasswordLength = 6;
    public UserService(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public HomeMember CreateHomeMember(HomeMember homeMember)
    {
        throw new NotImplementedException();
    }

    public User CreateHomeOwner(User user)
    {
        ValidateUser(user);

        if (!EmailIsUnique(user.Email))
        {
            throw new UserException("User with that email already exists");
        }

        User newHomeOwner = _userRepository.Add(user);
        return newHomeOwner;
    }

    public void ValidateUser(User user)
    {
        if (!IsValidEmail(user.Email))
        {
            throw new UserException("Invalid email, must contain @ and .");
        }

        if (!IsValidPassword(user.Password))
        {
            throw new UserException("Invalid password, must contain special character and be longer than 6 characters");
        }

        if (string.IsNullOrEmpty(user.Name))
        {
            throw new UserException("Invalid name, cannot be empty");
        }

        if (string.IsNullOrEmpty(user.Surname))
        {
            throw new UserException("Invalid surname, cannot be empty");
        }
    }

    private static bool IsValidPassword(string password)
    {
        var specialCharacters = @"!@#$%^&*()";

        return password.Length >= MinPasswordLength &&
               Regex.IsMatch(password, "[A-Z]") &&
               Regex.IsMatch(password, "[a-z]") &&
               Regex.IsMatch(password, "[0-9]") &&
               password.IndexOfAny(specialCharacters.ToCharArray()) >= 0;
    }

    public bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        var isEmailFormatValid = Regex.IsMatch(email, emailPattern);

        return isEmailFormatValid;
    }

    public bool EmailIsUnique(string email)
    {
        var user = _userRepository.Find(u => u.Email.ToLower().Equals(email.ToLower()));
        return user == null;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.FindAll();
    }

    public IEnumerable<Home> GetAllHomes()
    {
        throw new NotImplementedException();
    }

    public User CreateBusinessOwner(User user)
    {
        ValidateUser(user);

        if (!EmailIsUnique(user.Email))
        {
            throw new UserException("User with that email already exists");
        }

        User newBusinesssOwner = _userRepository.Add(user);
        return newBusinesssOwner;
    }
}
