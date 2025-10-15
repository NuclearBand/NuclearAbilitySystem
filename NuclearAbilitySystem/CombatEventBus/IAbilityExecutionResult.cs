using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface IAbilityExecutionResult
    {
        ReadOnlyCollection<ICombatCommand> Commands { get; }
        ICombatStateMutable ResultState { get; }
    }

    public record AbilityExecutionResult(ReadOnlyCollection<ICombatCommand> Commands, ICombatStateMutable ResultState)
        : IAbilityExecutionResult;

}