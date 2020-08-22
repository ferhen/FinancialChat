using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace FinancialChat.Helpers
{
    // from https://github.com/dotnet/AspNetCore.Docs/issues/17751#issuecomment-612584037

    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            // save the original OnMessageReceived event
            var originalOnMessageReceived = options.Events.OnMessageReceived;

            options.Events.OnMessageReceived = async context =>
            {
                // call the original OnMessageReceived event
                await originalOnMessageReceived(context);

                if (string.IsNullOrEmpty(context.Token))
                {
                    // attempt to read the access token from the query string
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                }
            };
        }
    }
}
