using System;
using AarpgTutorial.Common.Utilities;

namespace AarpgTutorial.Common.Exceptions;

/// <summary>Thrown by <see cref="Extensions.Require{T}"/> when a required Godot export is not assigned in the editor.</summary>
public class ExportNullException(Type exportType, string nodeName) 
    : Exception($"{nodeName}: {exportType.Name} export is not assigned.");
