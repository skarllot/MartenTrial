using System;
using MediatR;

namespace MartenTrial.Model.Quest.JoinMembers
{
    public sealed record JoinMembersToQuest(Guid Quest, int Day, string Location, params string[] Members) : IRequest;
}