# CSharpCodeBuilder
c# helper to compile c# source code in runtime using Microsoft.CodeAnalysis.

### Sample code
```c#
var sourceCode = @"using System; 
public class ConsoleLogger : ILogger
{ 
    public void Info(string msg) 
    { 
        Console.WriteLine($""[INFO] {msg}"", msg); 
    } 
    public void Warn(string msg) 
    { 
        Console.WriteLine($""[WARN] {msg}"", msg); 
    } 
    public DateTime Now() 
    { 
        return DateTime.Now; 
    } 
}";

// 1. Compile source code and generate its assembly
var scriptBuilder = new CSharpCodeBuilder();
var assembly = scriptBuilder.CompileCode(sourceCode, typeof(Console), typeof(ILogger), typeof(DateTime));

// 2. Instantiate ConsoleLogger class
var consoleLogger = scriptBuilder.CreateInstance<ILogger>(assembly);
if (consoleLogger is null)
{
    throw new Exception("Unable to create instance of ConsoleLogger.");
}

// 3. Execute a method implemented in ConsoleLogger
var now = consoleLogger.Now(); 
consoleLogger.Info($"Print INFO at {now}!");
consoleLogger.Warn($"Print WARN at {now}!");

// ****************************** //

public interface ILogger
{
    void Info(string msg);
    void Warn(string msg);
    DateTime Now();
}


```

### Output in console will be:
```shell
[INFO] Print INFO at 2/12/2022 11:02:42 AM!
[WARN] Print WARN at 2/12/2022 11:02:42 AM!
```

