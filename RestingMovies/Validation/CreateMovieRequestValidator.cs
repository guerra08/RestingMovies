using FluentValidation;
using RestingMovies.Api.Contracts.Requests;

namespace RestingMovies.Api.Validation;

public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Movie name cannot be empty");
        RuleFor(x => x.Genre)
            .NotEmpty()
            .WithMessage("Movie genre cannot be empty");
        RuleFor(x => x.Director)
            .NotEmpty()
            .WithMessage("Movie director cannot be empty");
        RuleFor(x => x.ReleaseYear)
            .NotNull()
            .WithMessage("Movie release year must be present");
    }
}