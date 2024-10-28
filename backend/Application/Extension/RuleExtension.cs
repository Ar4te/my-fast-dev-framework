using System.Dynamic;
using Domain.Entity.RuleEngine;
using Domain.Enum.RuleEngine;
using Jint;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Application.Extension;

public static class RuleExtension
{
    public static object? ExecuteRule(this Rule rule, Dictionary<string, object>? inputs = null)
    {
        return ExecuteScript(rule.Script, rule.ScriptNeedParameters, rule.ScriptLanguageType, inputs);
    }

    public static object? ExecuteScript(string script, bool scriptNeedParameters, ScriptLanguageType scriptLanguageType, Dictionary<string, object>? inputs = null)
    {
        var _inputs = scriptNeedParameters ? inputs : null;
        return ExecuteScript(script, scriptLanguageType, _inputs);
    }

    public static object? ExecuteScript(string script, ScriptLanguageType scriptLanguageType, Dictionary<string, object>? inputs = null)
    {
        object? result = null;
        switch (scriptLanguageType)
        {
            case ScriptLanguageType.Javascript:
                result = ExecuteJsScript(script, inputs);
                break;
            case ScriptLanguageType.Python:
                break;
            case ScriptLanguageType.Lua:
                break;
            case ScriptLanguageType.CSharp:
                result = ExecuteCSharpScript(script, inputs);
                break;
            default:
                break;
        }
        return result;
    }

    private static object? ExecuteJsScript(string script, Dictionary<string, object>? inputs = null)
    {
        try
        {
            using var engine = new Engine();
            if (inputs != null && inputs.Count > 0)
            {
                foreach (var input in inputs)
                {
                    engine.SetValue(input.Key, input.Value);
                }
            }
            return engine.Evaluate(script).ToObject();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static object? ExecuteCSharpScript(string script, Dictionary<string, object>? inputs = null)
    {
        try
        {
            if (inputs != null && inputs.Count > 0)
            {
                dynamic globals = new ExpandoObject();
                var globalsDict = (IDictionary<string, object>)globals;
                foreach (var item in inputs)
                {
                    globalsDict[item.Key] = item.Value;
                }
                return CSharpScript.RunAsync(script, globals: globals).Result.ReturnValue;
            }


            return CSharpScript.RunAsync(script).Result.ReturnValue;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}