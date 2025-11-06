using learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;

public class Profile : BaseEntity
{
    public int UserId { get; set; }
    public PersonName Name { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Age Age { get; private set; }
    public Address Residence { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Gender Gender { get; private set; }
    public EducationLevel EducationLevel { get; private set; }
    public string? PhotoDni { get; set; }
    public string? PhotoCredential { get; set; }
    public string? Bio { get; set; }

    public Profile() { }

    public Profile(
        int userId,
        PersonName name,
        Email email,
        Age age,
        Address residence,
        PhoneNumber phone,
        Gender gender,
        EducationLevel educationLevel)
    {
        UserId = userId;
        Name = name;
        Email = email;
        Age = age;
        Residence = residence;
        Phone = phone;
        Gender = gender;
        EducationLevel = educationLevel;
    }

    public void UpdatePersonalInfo(PersonName name, Email email, Age age, PhoneNumber phone, Address residence)
    {
        Name = name;
        Email = email;
        Age = age;
        Phone = phone;
        Residence = residence;
    }

    public void UpdateDemographics(Gender gender, EducationLevel educationLevel)
    {
        Gender = gender;
        EducationLevel = educationLevel;
    }

    public void UpdatePhotos(string? photoDni, string? photoCredential)
    {
        PhotoDni = photoDni;
        PhotoCredential = photoCredential;
    }

    public void UpdateBio(string? bio)
    {
        Bio = bio;
    }
}