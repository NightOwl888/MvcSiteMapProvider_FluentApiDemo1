using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Builder.Fluent;

namespace MvcSiteMapProvider_FluentApiDemo1._FluentSiteMapNodeProviders
{
    public class SemanticSiteMapNodeProviderWithoutDynamicNodeProviders
        : ISiteMapNodeProvider
    {
        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            // Try the API! Let us know if there is something we can do to make it better.

            // This is the same sample as the MvcMusicStore demo, and shows how you can
            // mark up the API semantically as a replacement for XML. This example doesn't
            // use Dynamic Node Providers as we are evaluating whether it makes sense to 
            // support them. This would be how the semantic provider would look when used
            // in conjunction with the DynamicSiteMapNodeProvider.cs sample.

            // Note that the base node of this tree doesn't necessarily have to be the home page. 
            // You can specify .WithParentKey(string) to indicate that this branch is descending from
            // any node defined in any other node source (XML, .NET Attributes, IDynamicNodeProivder, 
            // ISiteMapNodeBuilder).

            // Be sure to try adding nodes yourself to see if you can suggest ways to make this 
            // more intuitive, easier to read, or easier to write.

            return helper.RegisterNode()
                .MatchingRoute(x => x.WithController("Home").WithAction("Index"))
                .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,HomeTitle").WithDescription("This is the home page"))
                .WithSeoValues(x => x.WithSiteMaps(y => y.WithChangeFrequency(ChangeFrequency.Always).WithUpdatePriority(UpdatePriority.Normal).WithLastModifiedDate(DateTime.Parse("2002-05-30T09:00:00"))))
                .WithChildNodes(homeChildren =>
                {
                    homeChildren.RegisterNode()
                        .MatchingRoute(x => x.WithController("Store").WithAction("Index"))
                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,BrowseGenresTitle"))
                        .WithChildNodes(genreChildren =>
                        {
                            genreChildren.RegisterNode()
                                .MatchingRoute(x => x.WithController("Store").WithAction("Browse").WithValue("id", "Jazz").WithValue("someOtherParameter", "whatever2"))
                                .WithDisplayValues(x => x.WithTitle("Jazz 4"))
                                .WithSeoValues(x => x.WithCanonicalKey("ABC123"));
                            genreChildren.RegisterNode()
                                .MatchingRoute(x => x.WithController("Store").WithAction("Browse").WithValue("id", "Jazz").WithValue("someParameter", "hello"))
                                .WithDisplayValues(x => x.WithTitle("Jazz 2"))
                                .WithSeoValues(x => x.WithCanonicalUrl("/Store/Browse/Jazz").WithMetaRobotsValues(MetaRobots.NoIndex | MetaRobots.NoFollow | MetaRobots.NoOpenDirectoryProject))
                                .WithKey("ABC123");
                            genreChildren.RegisterNode()
                                .MatchingRoute(x => x.WithController("Store").WithAction("Browse").WithValue("id", "Jazz").WithValue("someParameter", "goodbye"))
                                .WithDisplayValues(x => x.WithTitle("Jazz 5"))
                                .WithSeoValues(x => x.WithMetaRobotsValues(MetaRobots.NoIndex | MetaRobots.NoFollow | MetaRobots.NoOpenDirectoryProject | MetaRobots.NoYahooDirectory));
                            genreChildren.RegisterNode()
                                .MatchingUrl("~//Store/Browse/Jazz?someParameter=goodbye3") // NOTE: It is not valid to apply route values to a URL based node // TODO: Add inheritable route values.
                                .WithDisplayValues(x => x.WithTitle("Jazz 6"))
                                .WithInheritableRouteValues(x => x.WithController("Store").WithAction("Browse").WithValues(new Dictionary<string, object> { { "id", "Jazz" }, { "someOtherParameter", "whatever" } }))
                                .WithSeoValues(x => x.WithMetaRobotsValues(MetaRobots.NoIndex | MetaRobots.NoFollow | MetaRobots.NoOpenDirectoryProject | MetaRobots.NoYahooDirectory));
                        });
                    homeChildren.RegisterNode()
                        .MatchingRoute(x => x.WithController("ShoppingCart").WithAction("Index"))
                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,ReviewCartTitle"));
                    homeChildren.RegisterNode()
                        .AsGroupingNodeTitled("$resources:SiteMapLocalizations,CheckoutTitle")
                        .WithKey("Checkout")
                        .WithInheritableRouteValues(x => x.WithController("Checkout"));
                    homeChildren.RegisterNode()
                        .AsGroupingNodeTitled("$resources:SiteMapLocalizations,AccountTitle")
                        .WithInheritableRouteValues(x => x.WithController("Account"))
                        .WithChildNodes(accountChildren =>
                        {
                            accountChildren.RegisterNode()
                                .MatchingRoute(x => x.WithAction("LogOn"))
                                .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,LogOnTitle").WithVisibilityProvider("MvcMusicStore.Code.NonAuthenticatedVisibilityProvider, Mvc Music Store"));
                            accountChildren.RegisterNode()
                                .MatchingRoute(x => x.WithAction("LogOff"))
                                .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,LogOffTitle").WithVisibilityProvider("MvcMusicStore.Code.AuthenticatedVisibilityProvider, Mvc Music Store"));
                            accountChildren.RegisterNode()
                                .MatchingRoute(x => x.WithAction("Register"))
                                .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,RegisterTitle").WithVisibilityProvider("MvcMusicStore.Code.NonAuthenticatedVisibilityProvider, Mvc Music Store"));
                            accountChildren.RegisterNode()
                                .MatchingRoute(x => x.WithAction("ChangePassword"))
                                .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,ChangePasswordTitle").WithVisibilityProvider("MvcMusicStore.Code.AuthenticatedVisibilityProvider, Mvc Music Store"));
                            accountChildren.RegisterNode()
                                .MatchingRoute(x => x.WithController("Store").WithAction("Browse").WithValue("id", "Jazz").WithValue("someOtherParameter", "whatever"))
                                .WithDisplayValues(x => x.WithTitle("Jazz 3").WithVisibilityProvider("MvcMusicStore.Code.AuthenticatedVisibilityProvider, Mvc Music Store"))
                                .WithSeoValues(x => x.WithCanonicalUrl("http://www.whatever.com/Store/Browse/Jazz"));
                        });
                    homeChildren.RegisterNode()
                        .MatchingRoute(x => x.WithArea("Admin").WithController("Home").WithAction("Index"))
                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,AdministrationTitle").WithVisibility("SiteMapPathHelper,!*").WithVisibilityProvider("MvcSiteMapProvider.FilteredSiteMapNodeVisibilityProvider, MvcSiteMapProvider"))
                        .WithChildNodes(adminChildren =>
                        {
                            adminChildren.RegisterNode()
                                .MatchingRoute(x => x.WithArea("Admin").WithController("StoreManager").WithAction("Index"))
                                .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,StoreManagerTitle"))
                                .WithChildNodes(storeManagerChildren =>
                                {
                                    storeManagerChildren.RegisterNode()
                                        .MatchingRoute(x => x.WithAction("Create"))
                                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,CreateAlbumTitle"));
                                    storeManagerChildren.RegisterNode()
                                        .MatchingRoute(x => x.WithAction("Edit"))
                                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,EditAlbumTitle"));
                                    storeManagerChildren.RegisterNode()
                                        .MatchingRoute(x => x.WithAction("Delete"))
                                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,DeleteAlbumTitle"));
                                });
                        });
                    homeChildren.RegisterNode()
                        .MatchingRoute(x => x.WithAction("SiteMap"))
                        .WithDisplayValues(x => x.WithTitle("$resources:SiteMapLocalizations,SitemapTitle"))
                        .WithUrlResolutionValues(x => x.WithUrlResolver("MvcMusicStore.Code.UpperCaseSiteMapNodeUrlResolver, Mvc Music Store"));
                    homeChildren.RegisterNode()
                        .MatchingUrl("http://www.microsoft.com/")
                        .WithDisplayValues(x => x.WithTitle("Microsoft"))
                        .WithKey("Microsoft");
                })
                .ToList();
        }

        #endregion
    }
}