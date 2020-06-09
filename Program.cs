using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphServiceClient_ConfidentialClient
{
    class Program
    {
        static ClientCredentialProvider authProvider
        {
            get
            {
                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create( AuthSettings.client_id )
                    .WithAuthority( AuthSettings.authority )
                    .WithClientSecret( AuthSettings.client_secret )
                    .Build();

                // if you do not specify the scopes here, then graph will default to https://graph.microsoft.com/.default for this flow -- which is wrong for a gov tenant
                ClientCredentialProvider authProvider = new ClientCredentialProvider( confidentialClientApplication, AuthSettings.scope );
                return authProvider;
            }
        }


        static void Main( string[] args )
        {
            Get_Users().Wait();
            Console.WriteLine( $"Operation is complete. Press any key to close..." );
            Console.ReadKey();
        }


        static async Task Get_Users()
        {
            GraphServiceClient graphClient = new GraphServiceClient( authProvider );

            // if you do not change the base url, graph will default to https://graph.microsoft.com and your scope aud will not match for a gov tenant
            graphClient.BaseUrl = AuthSettings.graph_baseURL;

            // this code allows for paging as graph will ( by default ) return only 100 results per page
            List<User> user_pages = new List<User>();
            try
            {
                IGraphServiceUsersCollectionPage user_page = await graphClient.Users.Request().GetAsync();
                if (user_page != null )
                {
                    user_pages.AddRange( user_page );
                    while ( user_page.NextPageRequest != null )
                    {
                        user_page = await user_page.NextPageRequest.GetAsync();
                        user_pages.AddRange( user_page );
                    }
                }
            } catch ( Exception ex )
            {
                Console.WriteLine( $"Exception: {ex.Message}" );
            }

            foreach( User u in user_pages )
            {
                Console.WriteLine( $"User: {u.UserPrincipalName}" );
            }

            Console.WriteLine( $"\nTotal of {user_pages.Count} user returned." );

            
        }
    }
}
