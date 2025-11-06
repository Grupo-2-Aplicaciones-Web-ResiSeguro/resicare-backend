using learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;

public class RegisteredObjectCommandService(IRegisteredObjectRepository repository, IUnitOfWork unitOfWork) : IRegisteredObjectCommandService
{
    public async Task<RegisteredObject> Handle(CreateRegisteredObjectCommand command)
    {
        var registeredObject = new RegisteredObject
        {
            Tipo = command.Tipo,
            Nombre = command.Nombre,
            DescripcionBreve = command.DescripcionBreve,
            Precio = command.Precio,
            NumeroSerie = command.NumeroSerie,
            Foto = command.Foto,
            FechaRegistro = command.FechaRegistro,
            UserId = command.UserId,
            CreatedDate = DateTime.UtcNow
        };

        await repository.AddAsync(registeredObject);
        await unitOfWork.CompleteAsync();
        return registeredObject;
    }

    public async Task<RegisteredObject?> Handle(UpdateRegisteredObjectCommand command)
    {
        var existing = await repository.FindByIdAsync(command.ObjectId);
        if (existing == null) return null;

        if (!string.IsNullOrWhiteSpace(command.Tipo)) existing.Tipo = command.Tipo;
        if (!string.IsNullOrWhiteSpace(command.Nombre)) existing.Nombre = command.Nombre;
        if (!string.IsNullOrWhiteSpace(command.DescripcionBreve)) existing.DescripcionBreve = command.DescripcionBreve;
        if (command.Precio.HasValue) existing.Precio = command.Precio.Value;
        if (!string.IsNullOrWhiteSpace(command.NumeroSerie)) existing.NumeroSerie = command.NumeroSerie;
        if (!string.IsNullOrWhiteSpace(command.Foto)) existing.Foto = command.Foto;
        if (command.FechaRegistro.HasValue) existing.FechaRegistro = command.FechaRegistro.Value;
        existing.UpdatedDate = DateTime.UtcNow;

        repository.Update(existing);
        await unitOfWork.CompleteAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int objectId)
    {
        var existing = await repository.FindByIdAsync(objectId);
        if (existing == null) return false;
        repository.Remove(existing);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
