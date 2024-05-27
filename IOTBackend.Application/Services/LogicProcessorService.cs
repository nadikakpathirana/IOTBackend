using IOTBackend.Application.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace IOTBackend.Application.Services
{
    public class LogicProcessorService : ILogicProcessorService
    {
        public async Task<bool> Process(int sensorValue, string condition)
        {
            try
            {
                // Replace sensor1 with the actual sensor value
                condition = condition.Replace("x", sensorValue.ToString());
                condition = condition.Replace("AND", "&&").Replace("OR", "||").Replace("NOT", "!");

                // Evaluate the condition using Roslyn scripting
                return await EvaluateConditionAsync(condition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid logic" + ex.Message);
                return false;
            }
        }

        private async Task<bool> EvaluateConditionAsync(string condition)
        {
            try
            {
                var options = ScriptOptions.Default
                    .WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic && !string.IsNullOrEmpty(asm.Location)))
                    .WithImports("System");

                var result = await CSharpScript.EvaluateAsync<bool>(condition, options);
                return result;
            }
            catch
            {
                return false;
            }
        }
    }
}
