using System.Collections.Generic;
using System.Linq;
using Infra.Models.News;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LatestNewsWeb.Infra.Validate
{
    public abstract class Validator<T>
    {
        protected Dictionary<string, List<string>> ValidateResult { get; } = new Dictionary<string, List<string>>();

        protected bool IsValid
        {
            get
            {
                var errorMessagesCount = ValidateResult.SelectMany(v => v.Value)
                                                       .Count();

                return errorMessagesCount == 0;
            }
        }

        /// <summary>
        /// 驗証
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="validateVm"></param>
        /// <returns>true：代表輸入的資料有效</returns>
        public void Validate(ModelStateDictionary modelState, T validateVm)
        {
            ToValidateResult(modelState);

            CustomValidate(validateVm);
        }

        /// <summary>
        /// 將 ModelState 內的資料轉成 ValidateResult
        /// </summary>
        private void ToValidateResult(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return;
            }

            foreach (var kv in modelState)
            {
                if (kv.Value.ValidationState == ModelValidationState.Invalid)
                {
                    AddErrors(kv.Key, kv.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                }
            }
        }

        protected abstract void CustomValidate(T dto);

        /// <summary>
        /// 如果驗証失敗，就拋出 Exception
        /// </summary>
        protected void Validate(T dto)
        {
            if (IsValid)
            {
                return;
            }

            throw new ValidateFormFailedException
                  {
                      Dto            = dto,
                      ValidateResult = ValidateResult,
                  };
        }

        protected void AddErrors(string propertyName, params string[] newErrorMessages)
        {
            if (ValidateResult.TryGetValue(propertyName, out var errorMessages))
            {
                errorMessages.AddRange(newErrorMessages);
                return;
            }

            ValidateResult.Add(propertyName, newErrorMessages.ToList());
        }
    }
}
