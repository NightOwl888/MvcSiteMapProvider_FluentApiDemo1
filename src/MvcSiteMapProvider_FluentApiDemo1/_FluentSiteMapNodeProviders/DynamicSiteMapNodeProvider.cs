using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Builder.Fluent;

namespace MvcSiteMapProvider_FluentApiDemo1._FluentSiteMapNodeProviders
{
    public class DynamicSiteMapNodeProvider
        : ISiteMapNodeProvider
    {
        #region ISiteMapNodeProvider Members

        public IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper)
        {
            // Try the API! Let us know if there is something we can do to make it better.

            // This is a replacement for all of the DynamicNodeProvider implementations within 
            // the MvcMusicStore demo, to show how this could be done. Note that in a real-world scenario
            // you would need to remove all of the DynamicNodeProvider declarations in the SemanticSiteMapNodeProvider
            // because they would be completely unnecessary.

            // Be sure to try adding nodes yourself to see if you can suggest ways to make this 
            // more intuitive, easier to read, or easier to write.

            using (var storeDB = new MusicStoreEntities())
            {
                // Create a node for each genre
                foreach (var genre in storeDB.Genres)
                {
                    // This is the original dynamic node definition

                    //DynamicNode dynamicNode = new DynamicNode("Genre_" + genre.Name, genre.Name);
                    //dynamicNode.RouteValues.Add("genre", genre.Name);

                    //yield return dynamicNode;


                    // This is the fluent API version.

                    // Note that because there is no inheritance when we do it this way, we are specifying controller and action here, too.

                    yield return helper.RegisterNode()
                        .MatchingRoute(x => x.WithController("Store").WithAction("Browse").AlwaysMatchingKey("browse").WithValue("genre", genre.Name))
                        .WithDisplayValues(x => x.WithTitle(genre.Name))
                        .WithKey("Genre_" + genre.Name)
                        .Single();
                }

                // Create a node for each album
                foreach (var album in storeDB.Albums.Include("Genre"))
                {
                    // This is the original dynamic node definition

                    //DynamicNode dynamicNode = new DynamicNode();
                    //dynamicNode.Title = album.Title;
                    //dynamicNode.ParentKey = "Genre_" + album.Genre.Name;
                    //dynamicNode.RouteValues.Add("id", album.AlbumId);

                    //if (album.Title.Contains("Hit") || album.Title.Contains("Best"))
                    //{
                    //    dynamicNode.Attributes.Add("bling", "<span style=\"color: Red;\">hot!</span>");
                    //}

                    //yield return dynamicNode;


                    // This is the fluent API version.

                    // Note that because there is no inheritance when we do it this way, we are specifying controller and action here, too.

                    var customAttributes = new Dictionary<string, object>();
                    if (album.Title.Contains("Hit") || album.Title.Contains("Best"))
                    {
                        customAttributes.Add("bling", "<span style=\"color: Red;\">hot!</span>");
                    }

                    yield return helper.RegisterNode()
                        .MatchingRoute(x => x.WithController("Store").WithAction("Details").WithValue("id", album.AlbumId))
                        .WithDisplayValues(x => x.WithTitle(album.Title))
                        .WithParentKey("Genre_" + album.Genre.Name)
                        .WithCustomAttributes(customAttributes)
                        .Single();
                }
            }
        }

        #endregion
    }
}