using MediatR;

namespace Application.MatchContactUserCase.Dto;

public class MatchContactInput : IRequest<MatchContactOutput>
{
    public IList<string[]> ContactsList { get; set; }
}