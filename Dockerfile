FROM mcr.microsoft.com/dotnet/core/runtime:3.1
COPY MdParser/bin/Release/netcoreapp3.1/publish/ MdParser/
ENTRYPOINT ["dotnet", "MdParser/MdParser.dll"]