using System.Collections.ObjectModel;

namespace Nuclear.AbilitySystem
{
    public interface IAbilityExecutionResult
    {
        ReadOnlyCollection<ICombatCommand> Commands { get; }
        ICombatState ResultState { get; }
    }

    public record AbilityExecutionResult(ReadOnlyCollection<ICombatCommand> Commands, ICombatState ResultState)
        : IAbilityExecutionResult;

}