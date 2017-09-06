
if exist bin\FSharp.Core.dll del bin\FSharp.Core.dll
if exist bin\WebApplication1.dll del bin\WebApplication1.dll

fsc /lib:bin /r:Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll /r:Microsoft.Data.Edm.dll /r:Microsoft.Data.OData.dll /r:Microsoft.Data.Services.Client.dll /r:Microsoft.Data.Services.dll /r:System.Spatial.dll /r:System.ServiceModel.dll /r:System.ServiceModel.Web.dll /r:System.Xml.Linq.dll /target:library /out:bin\WebApplication1.dll WcfDataService1.svc.fs
pause
