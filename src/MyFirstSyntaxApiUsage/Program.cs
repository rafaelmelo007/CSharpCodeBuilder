using MyFirstSyntaxApiUsage;

var scriptBuilder = new CSharpCodeBuilder();

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

