using System.Reflection;
using System.Runtime.CompilerServices;
using CommandLine.Text;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.

[assembly: AssemblyTitle(ThisAssembly.Title)]
[assembly: AssemblyProduct("SSL Expiration Checker")]
[assembly: AssemblyDescription("Checks the expiration date of a SSL certificate from a given URL")]
[assembly: AssemblyCopyright(ThisAssembly.Copyright)]
// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.
[assembly: AssemblyVersion(ThisAssembly.Version)]
[assembly: AssemblyInformationalVersionAttribute(ThisAssembly.InformationalVersion)]
[assembly: AssemblyCulture("")]


[assembly: AssemblyUsage(
  "Usage: ssl-checker URL1 [URL2...URL10] --help", "You can query up to 10 URLs.")]

