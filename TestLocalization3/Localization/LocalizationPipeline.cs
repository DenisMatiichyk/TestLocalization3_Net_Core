using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace TestLocalization3.Localization
{
    public class LocalizationPipeline
    {
        public void Configure(IApplicationBuilder app,
            IOptions<RequestLocalizationOptions> options)
        {
            app.UseRequestLocalization(options.Value);
        }
    }
}
