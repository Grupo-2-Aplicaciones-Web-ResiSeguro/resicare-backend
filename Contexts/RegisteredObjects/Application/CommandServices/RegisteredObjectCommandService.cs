using learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;

public class RegisteredObjectCommandService(IRegisteredObjectRepository repository, IUnitOfWork unitOfWork) : IRegisteredObjectCommandService
{
    // Mapeo que acepta tanto inglés como español y normaliza a español para la DB
    private static readonly Dictionary<string, string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Inglés -> español
        { "Electronic", "electronico" },
        { "Suitcase", "maleta" },
        { "Books", "libros" },
        { "Supplies", "utiles" },
        { "Others", "otros" },
        // Español -> español (para que también acepte directamente español)
        { "electronico", "electronico" },
        { "maleta", "maleta" },
        { "libros", "libros" },
        { "utiles", "utiles" },
        { "otros", "otros" }
    };

    private const decimal MinPrice = 0.01m;
    private const decimal MaxPrice = 1000000m;
    private const int MinNameLength = 3;
    private const int MaxNameLength = 100;

    /// <summary>
    /// Handles the creation of a new registered object
    /// </summary>
    /// <param name="command">command containing object creation data</param>
    /// <returns>the created registered object entity</returns>
    /// <exception cref="InvalidObjectNameException">when object name is invalid</exception>
    /// <exception cref="InvalidObjectTypeException">when object type is invalid</exception>
    /// <exception cref="InvalidObjectPriceException">when price is outside valid range</exception>
    /// <exception cref="DuplicateSerialNumberException">when serial number already exists</exception>
    public async Task<RegisteredObject> Handle(CreateRegisteredObjectCommand command)
    {
        // Validate name
        ValidateName(command.Nombre);

        var normalizedType = ValidateAndNormalizeType(command.Tipo);

        ValidatePrice(command.Precio);

        if (!string.IsNullOrWhiteSpace(command.NumeroSerie))
        {
            await ValidateNoDuplicateSerialNumber(command.NumeroSerie, command.UserId);
        }

        var registeredObject = new RegisteredObject
        {
            Tipo = normalizedType,
            Nombre = command.Nombre.Trim(),
            DescripcionBreve = command.DescripcionBreve?.Trim() ?? string.Empty,
            Precio = command.Precio,
            NumeroSerie = command.NumeroSerie?.Trim() ?? string.Empty,
            Foto = command.Foto ?? string.Empty,
            FechaRegistro = command.FechaRegistro,
            UserId = command.UserId,
            CreatedDate = DateTime.UtcNow
        };

        await repository.AddAsync(registeredObject);
        await unitOfWork.CompleteAsync();
        return registeredObject;
    }

    /// <summary>
    /// Handles the update of an existing registered object.
    /// </summary>
    /// <param name="command">Command containing object update data.</param>
    /// <returns>The updated registered object entity.</returns>
    /// <exception cref="RegisteredObjectNotFoundException">when object is not found</exception>
    /// <exception cref="InvalidObjectNameException">when object name is invalid</exception>
    /// <exception cref="InvalidObjectTypeException">when object type is invalid</exception>
    /// <exception cref="InvalidObjectPriceException">when price is outside valid range</exception>
    /// <exception cref="DuplicateSerialNumberException">when serial number already exists</exception>
    public async Task<RegisteredObject?> Handle(UpdateRegisteredObjectCommand command)
    {
        var existing = await repository.FindByIdAsync(command.ObjectId);
        if (existing == null)
            throw new RegisteredObjectNotFoundException(command.ObjectId);

        if (!string.IsNullOrWhiteSpace(command.Nombre))
        {
            ValidateName(command.Nombre);
            existing.Nombre = command.Nombre.Trim();
        }

        if (!string.IsNullOrWhiteSpace(command.Tipo))
        {
            var normalizedType = ValidateAndNormalizeType(command.Tipo);
            existing.Tipo = normalizedType;
        }

        if (command.Precio.HasValue)
        {
            ValidatePrice(command.Precio.Value);
            existing.Precio = command.Precio.Value;
        }

        if (!string.IsNullOrWhiteSpace(command.NumeroSerie) && command.NumeroSerie != existing.NumeroSerie)
        {
            await ValidateNoDuplicateSerialNumber(command.NumeroSerie, existing.UserId, command.ObjectId);
            existing.NumeroSerie = command.NumeroSerie.Trim();
        }

        if (!string.IsNullOrWhiteSpace(command.DescripcionBreve))
            existing.DescripcionBreve = command.DescripcionBreve.Trim();

        if (!string.IsNullOrWhiteSpace(command.Foto))
            existing.Foto = command.Foto;

        if (command.FechaRegistro.HasValue)
            existing.FechaRegistro = command.FechaRegistro.Value;

        existing.UpdatedDate = DateTime.UtcNow;

        repository.Update(existing);
        await unitOfWork.CompleteAsync();
        return existing;
    }

        public async Task<bool> DeleteAsync(int objectId)
    {
        var existing = await repository.FindByIdAsync(objectId);
        if (existing == null)
            throw new RegisteredObjectNotFoundException(objectId);

        repository.Remove(existing);
        await unitOfWork.CompleteAsync();
        return true;
    }

     private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidObjectNameException(name ?? string.Empty, MinNameLength, MaxNameLength);

        var trimmedName = name.Trim();
        if (trimmedName.Length < MinNameLength || trimmedName.Length > MaxNameLength)
            throw new InvalidObjectNameException(name, MinNameLength, MaxNameLength);
    }

    /// <summary>
    /// Validates that the object type is allowed and normalizes it to database format.
    /// </summary>
    /// <param name="type">The object type to validate (from frontend).</param>
    /// <returns>The normalized type name for database storage.</returns>
    /// <exception cref="InvalidObjectTypeException">Thrown when type is invalid.</exception>
    private static string ValidateAndNormalizeType(string type)
    {
        if (AllowedTypes.TryGetValue(type, out var normalizedType))
        {
            return normalizedType;
        }

        throw new InvalidObjectTypeException(type, AllowedTypes.Keys.ToArray());
    }

     private static void ValidatePrice(decimal price)
    {
        if (price < MinPrice || price > MaxPrice)
            throw new InvalidObjectPriceException(price, MinPrice, MaxPrice);
    }

    /// <summary>
    /// Validates that the serial number doesn't already exist for the user.
    /// </summary>
    /// <param name="serialNumber">The serial number to check.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="excludeObjectId">Optional object ID to exclude (for updates).</param>
    /// <exception cref="DuplicateSerialNumberException">Thrown when serial number already exists.</exception>
    private async Task ValidateNoDuplicateSerialNumber(string serialNumber, int userId, int? excludeObjectId = null)
    {
        var existing = await repository.FindBySerialNumberAndUserAsync(serialNumber, userId);

        if (existing != null && existing.Id != excludeObjectId)
        {
            throw new DuplicateSerialNumberException(serialNumber, userId, existing.Id);
        }
    }
}