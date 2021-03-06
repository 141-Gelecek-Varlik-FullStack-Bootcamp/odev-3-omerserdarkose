using FluentValidation;
using HelenSposa.Core.CrossCuttingConcerns.Validation.FluentValidation;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HelenSposa.Core.Aspects.Postsharp
{
    [Serializable]
    public class FluentValidationAspect:OnMethodBoundaryAspect
    {
        //reflection ile validator tipi ogreniliyor
        Type _validatorType;
        public FluentValidationAspect(Type validatorType)
        {
            _validatorType = validatorType;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            //ogrenilen validation tipinde runtime da bir instance olusturuluyor
            var validator = (IValidator)Activator.CreateInstance(_validatorType);

            //validate edilecek entity nin tipini ogrenmek icin buraya kendisi gonderilen validation tipinin base sinifinin aldigi generic tipi ogreniyoruz. tek bir generic ile calistigindan bu yontem ise yariyor
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];

            var entities = args.Arguments.Where(t => t.GetType() == entityType);

            foreach (var entity in entities)
            {
                ValidatorTool.FluentValidate(validator, entity);
            }
        }
    }
}
