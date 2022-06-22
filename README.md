# Rhino Mocks To Moq
[![NuGet Version](https://img.shields.io/nuget/vpre/RhinoMocksToMoq.svg?style=flat)](https://www.nuget.org/packages/RhinoMocksToMoq/)

There are old projects that using Rhino mocks with many xxx test cases wishing to upgrade to .NET Core. This project provide an adapter for easy migrate to Moq (.NET Core) without a lots of rewriting efforts

# Note
- Don't forget to remove existing RhinoMocks reference
- The argument matchers are not implemented and should be replaced with matchers in Moq directly.
