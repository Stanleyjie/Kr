using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorityCenter
{
    public static class Config
    {
        /// <summary>
        /// 注册的账号
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "m1",
                    Password = "password",

                    Claims = new []
                    {
                        new Claim("name", "mname1"),
                        new Claim("website", "https://mname1.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "m2",
                    Password = "password",

                    Claims = new []
                    {
                        new Claim("name", "mname2"),
                        new Claim("website", "https://mname2.com")
                    }
                }
            };
        }

        /// <summary>
        /// 获取标识资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        /// <summary>
        /// 作用域（可以控制不同的方向）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "API1"),
                new ApiResource("api2", "API2")
            };
        }

        /// <summary>
        /// 客户端接入的模式
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // 没有交互用户，使用clientid/secret进行身份验证 （方式1）
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // 身份验证秘钥
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // 客户端有权访问的作用域 要在ApiResource 里面有才可以使用
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "clientanduser",

                    // 资源所有者密码授予客户端，这个需要有用户账号密码（方式2）
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                     // 身份验证秘钥
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // 客户端有权访问的作用域
                    AllowedScopes = { "api1" }
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    // OpenID Connect隐式流客户端（MVC）单点登录使用（方式3）
                    AllowedGrantTypes = GrantTypes.Implicit,//隐式方式
                    RequireConsent=false,//如果不需要显示否同意授权 页面 这里就设置为false
                    AccessTokenLifetime = 3600 * 2, //6小时

                    RedirectUris = { "http://localhost:5002/signin-oidc" },//登录成功后返回的客户端地址
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },//注销登录后返回的客户端地址

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }
    }
}
