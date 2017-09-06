
if exist bin\WebApplication1.dll del bin\WebApplication1.dll

csc /lib:bin /r:Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll /r:Microsoft.Data.Edm.dll /r:Microsoft.Data.OData.dll /r:Microsoft.Data.Services.Client.dll /r:Microsoft.Data.Services.dll /r:System.Spatial.dll /target:library /out:bin\WebApplication1.dll WcfDataService1.svc.cs
pause
