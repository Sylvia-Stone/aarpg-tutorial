namespace AarpgTutorial.Save.Models;

/// <summary>Serializable representation of a single inventory slot, storing only the item's resource path and quantity.</summary>
public record InventorySlotDto(string? ItemPath, int Quantity);