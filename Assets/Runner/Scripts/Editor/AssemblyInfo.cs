using System.Runtime.CompilerServices;

/// <summary>
/// Assembly visibility configuration for Unity editor integration.
/// This file controls which assemblies can access internal members of this assembly.
/// </summary>

// Allow the Unity.Hypercasual.Tutorials.Editor assembly to access internal members
// This is necessary for editor tooling and tutorial integration
[assembly: InternalsVisibleTo("Unity.Hypercasual.Tutorials.Editor")]
