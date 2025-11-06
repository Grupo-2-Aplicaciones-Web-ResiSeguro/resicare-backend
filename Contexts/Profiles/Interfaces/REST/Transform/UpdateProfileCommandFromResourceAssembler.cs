using learning_center_webapi.Contexts.Profiles.Domain.Commands;
using learning_center_webapi.Contexts.Profiles.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST.Transform;

public static class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommand(int id, UpdateProfileResource resource)
    {
        return new UpdateProfileCommand
        {
            Id = id,
            Nombre = resource.Nombre,
            Correo = resource.Correo,
            Edad = resource.Edad,
            Residencia = resource.Residencia,
            Telefono = resource.Telefono,
            Genero = resource.Genero,
            NivelInstruccion = resource.NivelInstruccion,
            PhotoDni = resource.PhotoDni,
            PhotoCredential = resource.PhotoCredential,
            Bio = resource.Bio
        };
    }
}