
using learning_center_webapi.Contexts.Profiles.Domain.Commands;
using learning_center_webapi.Contexts.Profiles.Domain.Exceptions;
using learning_center_webapi.Contexts.Profiles.Domain.Infraestructure;
using learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Profiles.Application.CommandServices;

public class ProfileCommandService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProfileCommandService(IProfileRepository profileRepository, IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Profile> Handle(CreateProfileCommand command)
    {
        var existing = await _profileRepository.FindByUserIdAsync(command.UserId);
        if (existing != null)
            throw new ProfileAlreadyExistsException(command.UserId);

        var name = new PersonName(command.Nombre);
        var email = new Email(command.Correo);
        var age = command.Edad.HasValue ? new Age(command.Edad.Value) : Age.Default();
        var residence = !string.IsNullOrEmpty(command.Residencia) ? new Address(command.Residencia) : Address.Empty();
        var phone = !string.IsNullOrEmpty(command.Telefono) ? new PhoneNumber(command.Telefono) : PhoneNumber.Empty();
        var gender = new Gender(command.Genero);
        var educationLevel = new EducationLevel(command.NivelInstruccion);

        var profile = new Profile(
            command.UserId,
            name,
            email,
            age,
            residence,
            phone,
            gender,
            educationLevel
        );

        profile.UpdatePhotos(command.PhotoDni, command.PhotoCredential);
        profile.UpdateBio(command.Bio);

        await _profileRepository.AddAsync(profile);
        await _unitOfWork.CompleteAsync();

        return profile;
    }

    public async Task<Profile> Handle(UpdateProfileCommand command)
    {
        var profile = await _profileRepository.FindByIdAsync(command.Id);
        
        if (profile == null)
            throw new ProfileNotFoundException(command.Id);

        if (!string.IsNullOrEmpty(command.Nombre) || !string.IsNullOrEmpty(command.Correo))
        {
            var name = !string.IsNullOrEmpty(command.Nombre) ? new PersonName(command.Nombre) : profile.Name;
            var email = !string.IsNullOrEmpty(command.Correo) ? new Email(command.Correo) : profile.Email;
            var age = command.Edad.HasValue ? new Age(command.Edad.Value) : profile.Age;
            var phone = command.Telefono != null ? new PhoneNumber(command.Telefono) : profile.Phone;
            var residence = command.Residencia != null ? new Address(command.Residencia) : profile.Residence;
            
            profile.UpdatePersonalInfo(name, email, age, phone, residence);
        }

        if (!string.IsNullOrEmpty(command.Genero) || !string.IsNullOrEmpty(command.NivelInstruccion))
        {
            var gender = !string.IsNullOrEmpty(command.Genero) ? new Gender(command.Genero) : profile.Gender;
            var educationLevel = !string.IsNullOrEmpty(command.NivelInstruccion) ? new EducationLevel(command.NivelInstruccion) : profile.EducationLevel;
            
            profile.UpdateDemographics(gender, educationLevel);
        }

        if (command.PhotoDni != null || command.PhotoCredential != null)
        {
            profile.UpdatePhotos(
                command.PhotoDni ?? profile.PhotoDni,
                command.PhotoCredential ?? profile.PhotoCredential
            );
        }

        if (command.Bio != null)
        {
            profile.UpdateBio(command.Bio);
        }

        _profileRepository.Update(profile);
        await _unitOfWork.CompleteAsync();

        return profile;
    }

    public async Task<bool> Handle(DeleteProfileCommand command)
    {
        var profile = await _profileRepository.FindByIdAsync(command.Id);
        
        if (profile == null)
            return false;

        _profileRepository.Remove(profile);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}