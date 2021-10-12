using MediatR;

namespace MartenTrial.Model.Quest.Start
{
    public sealed record StartQuest(string Name) : IRequest;
}