using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Adapter")]
[assembly: InternalsVisibleTo("Consumer")]
[assembly: InternalsVisibleTo("UnitTests")]
[assembly: ExcludeFromCodeCoverage]
