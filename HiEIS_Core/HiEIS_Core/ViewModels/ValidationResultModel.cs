using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiEIS_Core.ViewModels
{
    public class ValidationResultModel : List<ValidationModel>
    {
        private IdentityResult createUserResult;

        public ValidationResultModel(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.Add(new ValidationModel { error = error.ToString() });
            }
        }
        public ValidationResultModel(ModelStateDictionary modelState)
        {
            if (modelState.IsValid) return;

            foreach (var state in modelState)
            {
                if (modelState[state.Key].Errors.Any())
                {
                    foreach (var error in modelState[state.Key].Errors)
                    {
                        Add(new ValidationModel
                        {
                            name = state.Key,
                            error = error.ErrorMessage
                        });
                    }
                }
            }
        }

    }

    public class ValidationModel
    {
        public string name { get; set; }
        public string error { get; set; }
    }
}
