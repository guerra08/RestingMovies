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

        RuleFor(x => x.Score)
            .LessThanOrEqualTo(5)
            .WithMessage("Score must be smaller or equal to 5");
        RuleFor(x => x.Score)
            .GreaterThan(0)
            .WithMessage("Score must be greater than 0");
    }
}