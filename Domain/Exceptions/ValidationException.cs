using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace EmployeeManagement.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<ValidationFailure> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> errors)
            : base("One or more validation failures have occurred.")
        {
            Errors = errors;
        }
    }
} 