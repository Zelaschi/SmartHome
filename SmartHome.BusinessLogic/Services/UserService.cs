using System;
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
using SmartHome.BusinessLogic.InitialSeedData;
using System.Linq.Expressions;

namespace SmartHome.BusinessLogic.Services;
public sealed class UserService : IHomeOwnerLogic, IUsersLogic, IBusinessOwnerLogic, IAdminLogic
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IRoleLogic _roleService;

    private const int MinPasswordLength = 6;
    public UserService(IGenericRepository<User> userRepository, IRoleLogic roleLogic)
    {
        _userRepository = userRepository;
        _roleService = roleLogic;
    }

    public User CreateHomeOwner(User user)
    {
        ValidateUser(user);

        EmailIsUnique(user.Email);

        user.Role = _roleService.GetHomeOwnerRole();

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

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        var isEmailFormatValid = Regex.IsMatch(email, emailPattern);

        return isEmailFormatValid;
    }

    private void EmailIsUnique(string email)
    {
        var user = _userRepository.Find(u => u.Email.ToLower().Equals(email.ToLower()));
        if (user != null)
        {
            throw new UserException("User with that email already exists");
        }
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.FindAll();
    }

    public User CreateBusinessOwner(User user)
    {
        ValidateUser(user);

        EmailIsUnique(user.Email);

        user.Role = _roleService.GetBusinessOwnerRole();
        user.Complete = false;

        User newBusinesssOwner = _userRepository.Add(user);
        return newBusinesssOwner;
    }

    public User CreateAdmin(User user)
    {
        ValidateUser(user);

        EmailIsUnique(user.Email);

        user.Role = _roleService.GetAdminRole();

        User newAdmin = _userRepository.Add(user);

        return newAdmin;
    }

    private void EnsureAdminCannotBeDeletedIfOnlyOneExists()
    {
        IList<User> users = _userRepository.FindAll().ToList();
        var adminUsers = users.Where(user => user.Role == _roleService.GetAdminRole()).ToList();
        if (adminUsers.Count <= 1)
        {
            throw new UserException("Cannot delete the only admin user");
        }
    }

    private void ValidateAdminExistance(Guid adminId)
    {
        var admin = _userRepository.Find(user => user.Id == adminId && user.Role == _roleService.GetAdminRole());
        if (admin == null)
        {
            throw new UserException("Admin not found");
        }
    }

    public void DeleteAdmin(Guid adminId)
    {
        ValidateAdminExistance(adminId);
        EnsureAdminCannotBeDeletedIfOnlyOneExists();
        _userRepository.Delete(adminId);
    }

    public void UpdateAdminRole(User user)
    {
        user.Role = _roleService.GetAdminHomeOwnerRole();
        _userRepository.Update(user);
    }

    public void UpdateBusinessOwnerRole(User user)
    {
        user.Role = _roleService.GetBusinessOwnerHomeOwnerRole();
        _userRepository.Update(user);
    }

    public IEnumerable<User> GetUsers(int? pageNumber, int? pageSize, string? role, string? fullName)
    {
        Expression<Func<User, bool>> filter = user =>
                    (string.IsNullOrEmpty(role) || user.Role.Name == role) &&
                    (string.IsNullOrEmpty(fullName) ||
                        (user.Name.ToLower() + " " + user.Surname.ToLower()).Contains(fullName.ToLower()) ||
                        (user.Surname.ToLower() + " " + user.Name.ToLower()).Contains(fullName.ToLower()));

        if (pageNumber == null && pageSize == null)
        {
            return _userRepository.FindAllFiltered(filter);
        }

        return _userRepository.FindAllFiltered(filter, pageNumber ?? 1, pageSize ?? 10);
    }
}
