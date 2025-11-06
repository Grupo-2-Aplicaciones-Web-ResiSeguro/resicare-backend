using learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;

public interface IRegisteredObjectCommandService
{
    Task<RegisteredObject> Handle(CreateRegisteredObjectCommand command);
    Task<RegisteredObject?> Handle(UpdateRegisteredObjectCommand command);
    Task<bool> DeleteAsync(int objectId);
}
