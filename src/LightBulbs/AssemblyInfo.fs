namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("LightBulbs")>]
[<assembly: AssemblyProductAttribute("LightBulbs")>]
[<assembly: AssemblyDescriptionAttribute("Evaluating light bulb purchases")>]
[<assembly: AssemblyVersionAttribute("0.0.1")>]
[<assembly: AssemblyFileVersionAttribute("0.0.1")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.0.1"
