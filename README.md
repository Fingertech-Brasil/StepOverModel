# StepOver Model
Um modelo do uso das funções dos SDKs da StepOver (SigDeviceAPI, SigSignAPI) para assinatura de documentos PDF.

## Compilação
1. Deve ser adicionado o NuGet Source da StepOver para que as dependências possam sejam baixadas.
```bash
dotnet nuget add source https://nuget.stepover.com/nuget/stepover/ --name StepOver
```
2. Restaurar as dependências
```bash
dotnet restore
```

# Documentação
A documentação dos SDKs da StepOver para o C# pode ser encontrado em [StepOver Documentation](https://www.stepoverinfo.net/dotnetDocu/api/Sig.DeviceAPI.Driver.html).