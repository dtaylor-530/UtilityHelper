using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper_Framework
{

    public static class MapperHelper
    {

        public static IMapper FromConfigExpression<T, R>(Func<IMapperConfigurationExpression, IMappingExpression<T, R>> f)
        {
            return (new MapperConfiguration((cfg) => f(cfg)).CreateMapper());

        }
        public static IMapper FromConfigExpressionLowerToUpper<T, R>(Func<IMapperConfigurationExpression, IMappingExpression<T, R>> f)
        {
            return (new MapperConfiguration((cfg) =>
            {
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
                f(cfg);
            }).CreateMapper());

        }
    }

}
