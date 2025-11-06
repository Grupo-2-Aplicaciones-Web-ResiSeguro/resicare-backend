
using learning_center_webapi.Contexts.IAM.Domain.Commands;
using learning_center_webapi.Contexts.IAM.Domain.Exceptions;
using learning_center_webapi.Contexts.IAM.Domain.Infraestructure;
using learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.IAM.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.IAM.Application.CommandServices;

public class AuthCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthCommandService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> Handle(SignUpCommand command)
    {
        var existingUser = await _userRepository.FindByEmailAsync(command.Email);
        if (existingUser != null)
            throw new UserAlreadyExistsException(command.Email);

        var email = new Email(command.Email);
        var passwordHash = PasswordHash.FromPlainPassword(command.Password);
        var role = new UserRole(command.Rol ?? "cliente");

        var user = new User(command.Name, email, passwordHash, role);

        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<User> Handle(SignInCommand command)
    {
        var user = await _userRepository.FindByEmailAsync(command.Email);
        
        if (user == null)
            throw new InvalidCredentialsException();

        if (!user.VerifyPassword(command.Password))
            throw new InvalidCredentialsException();

        return user;
    }
}