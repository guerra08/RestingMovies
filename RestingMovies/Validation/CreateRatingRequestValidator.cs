using FluentValidation;
using RestingMovies.Api.Contracts.Requests;

namespace RestingMovies.Api.Validation;

public class CreateRatingRequestValidator : AbstractValidator<CreateRatingRequest>
{
    public CreateRatingRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Rating text cannot be empty");
        RuleFor(x => x.Score)
            .NotNull()
            .WithMessage("Rating score must be present");
        RuleFor(x => x.MovieId)
            .NotNull()
            .WithMessage("Rating must contain movie id");
    }
}