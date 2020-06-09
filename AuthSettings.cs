using System;

namespace GraphServiceClient_ConfidentialClient
{
    class AuthSettings
    {
        public static String instance = "https://login.microsoftonline.us"; // gov tenants would be .us where commercial would be .com
        public static String graph_endpoint = "https://graph.microsoft.us"; // gov tenants would be .us where commercial would be .com
        public static String graph_version = "v1.0"; // values are v1.0 or beta
        public static String tenant_id = ""; // can be the tenant guid or name
        public static String client_id = ""; // the app id of the app registration in the tenant
        public static String client_secret = ""; // a client secret must be configured on the app registration for the client credentials flow
        
        public static String authority = $"{instance}/{tenant_id}";
        // for the client credentials flow, must always use the .default and any application permissions 
        // consented to will appear in the token. You cannot do dynamic scopes
        public static String scope = $"{graph_endpoint}/.default"; 
        public static String graph_baseURL = $"{graph_endpoint}/{graph_version}";
    }
}
