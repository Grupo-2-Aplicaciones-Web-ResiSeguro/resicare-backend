using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;

public class RegisteredObject : BaseEntity
{
    /// <summary>
    /// Object type label selected by the user.
    /// </summary>
    public required string Tipo { get; set; }

    /// <summary>
    /// Object name.
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Brief description of the object.
    /// </summary>
    public string DescripcionBreve { get; set; } = string.Empty;

    /// <summary>
    /// Declared price of the object.
    /// </summary>
    public decimal Precio { get; set; }

    /// <summary>
    /// Serial number or identifier.
    /// </summary>
    public string NumeroSerie { get; set; } = string.Empty;

    /// <summary>
    /// Base64 representation of the object photo.
    /// </summary>
    public string Foto { get; set; } = string.Empty;

    /// <summary>
    /// Date the object was registered by the user.
    /// </summary>
    public DateTime FechaRegistro { get; set; }

    /// <summary>
    /// Identifier of the user that owns the object.
    /// </summary>
    public int UserId { get; set; }
}
