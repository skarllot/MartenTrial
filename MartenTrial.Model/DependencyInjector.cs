using System.ComponentModel;
using MartenTrial.Common;
using MartenTrial.Common.DependencyInjection;
using MartenTrial.Model.Quest.JoinMembers;
using MartenTrial.Model.Quest.Start;
using Microsoft.Extensions.DependencyInjection;

namespace MartenTrial.Model
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DependencyInjector
    {
        public static IServiceCollection AddQuestHandlers(this IServiceCollection services)
        {
            return services
                .TryAddSingletonAbstractions<StartingQuest>()
                .TryAddSingletonAbstractions<JoiningMembersToQuest>();
        }
    }
}