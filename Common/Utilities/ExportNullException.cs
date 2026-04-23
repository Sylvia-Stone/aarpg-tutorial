using System;

namespace AarpgTutorial.Common.Utilities;

public class ExportNullException(Type exportType, string nodeName) 
    : Exception($"{nodeName}: {exportType.Name} export is not assigned.");
