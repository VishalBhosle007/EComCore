using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EComCore.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (context.ModelState.IsValid == false)
        //    {
        //        context.Result = new BadRequestResult();
        //    }
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Collect all error messages into a single string
                var errors = context.ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var combinedMessage = string.Join(" | ", errors);

                context.Result = new BadRequestObjectResult(new
                {
                    error = combinedMessage
                });
            }
        }
    }
}
