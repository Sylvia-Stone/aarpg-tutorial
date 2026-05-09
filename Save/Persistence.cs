using Godot;

namespace AarpgTutorial.Save;

/// <summary>
/// Attach to any node that needs to remember a binary state across scene loads.
/// Generates a unique ID from the scene path and node hierarchy automatically.
/// </summary>
public partial class Persistence : Node
{
    #region Signals

    [Signal]
    public delegate void MarkRestoredEventHandler();

    #endregion

    #region Properties

    public bool IsMarked { get; private set; }

    #endregion

    #region Lifecycle Methods

    public override void _Ready()
    {
        CallDeferred(MethodName.RestoreMark);
    }

    #endregion

    #region Public Methods

    /// <summary>Adds this node's generated ID to the save manager's persistence list.</summary>
    public void Mark()
    {
        SaveManager.Instance.AddPersistenceId(GenerateId());
    }

    #endregion

    #region Private Methods

    /// <summary>Builds a unique ID from the current scene path, parent node name, and this node's name.</summary>
    /// <returns>A path-style string that uniquely identifies this node in the scene.</returns>
    private string GenerateId()
    {
        var filePath = GetTree().CurrentScene.SceneFilePath;
        var parentName = GetParent().Name;

        return $"{filePath}/{parentName}/{Name}";
    }

    /// <summary>Checks the save manager for this node's ID and emits <see cref="MarkRestored"/>.</summary>
    private void RestoreMark()
    {
        IsMarked = SaveManager.Instance.CheckPersistenceId(GenerateId());
        EmitSignalMarkRestored();
    }

    #endregion
}