using System.Text.RegularExpressions;

namespace Utilidades
{
    public static class Constants
    {
        public const string UriForDefaultWebApi = "api/v1/";

        public const string NameProject = "API INTEGRATIONS MTWO";
        public const string NameProjectOrchestrator = "API INTEGRATIONS MTWO - ORCHESTRATOR";
        public const string NameProjectSap = "API INTEGRATIONS MTWO - SAP";

        public const string DescriptionProject = "API for consuming services and integration configuration and as a basis for integration with other sources";

        public const string AzureBlobVariable = "AzureStorage";

        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public const string EmailRegex = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";

        public const string MessageFail = "We present a technical failure, it is not possible to process the request.";

        public const string MessageUnauthorized = "You are not authorized to perform this action.";

        public const string ContentType = "application/json";

        public const string MessageErrorSession = "Don't start session";

        public const string MessageError = "We are present a techinical issue. Try again";






        public static bool ExpresionRegular(string texto, string patron)
        {
            return Regex.IsMatch(texto, patron);
        }
    }

    public static class Authorization
    {
        public const string
        AUTHORIZATION = "Authorization",
        BEARER = "Bearer",
        EMAILS = "emails",
        ClientContext = "Client-Context",
        CLIENT = "CodeClient";
    }
}

