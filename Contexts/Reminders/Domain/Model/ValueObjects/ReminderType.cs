using System;
using System.Collections.Generic;
using System.Linq;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;

public record ReminderType
{
    public string Value { get; }

    private static readonly HashSet<string> ValidTypes = new()
    {
        "medicación", "chequeo", "pago", "prevención", "seguridad"
    };

    public ReminderType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Reminder type cannot be empty");

        value = value.Trim().ToLowerInvariant();

        if (!ValidTypes.Contains(value))
            throw new ArgumentException($"Invalid reminder type: {value}. Valid types: {string.Join(", ", ValidTypes)}");

        Value = value;
    }

    public bool IsMedication() => Value == "medicación";
    public bool IsCheckup() => Value == "chequeo";
    public bool IsPayment() => Value == "pago";
    public bool IsPrevention() => Value == "prevención";
    public bool IsSecurity() => Value == "seguridad";

    public override string ToString() => Value;

    public static implicit operator string(ReminderType type) => type.Value;
}