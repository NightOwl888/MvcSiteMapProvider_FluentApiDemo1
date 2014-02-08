FLUENT API DEMO

This is a non-functioning demo intended to show the API interface and how it enforces the business rules
by not allowing invalid combinations of values to be set. The intent is to get feedback on the API to 
help drive the direction into an interface that is both easy to write and easy to read and also to ensure 
we haven't left out any important combinations of values that make sense to use together.

The API enforces the following:

1.	A node can have 4 types:
	a.	Matching Route
	b.	Matching Url
	c.	Dynamic Node Provider Definition
	d.	Grouping node
2.	Route nodes must have a title and must have route values.
3.	Url nodes must have a title and must have a url.
4.	A dynamic node provider must provide the name of the provider and can have configured values that will be inherited by the dynamic nodes.
5.	A grouping node must have a title.
6.	A node can have a canonical key or a canonical URL, but not both.
7.	A descendant node can inherit route values from the parent node.
8.	A descendant node can inherit custom attributes from the parent node.
9.	A descendant node cannot specify a parent key (because it is done semantically already).
10.	From the base node (root of the current tree) you can specify to create an IEnumerable<ISiteMapNodeToParentRelation> by specifying ToList().
11.	From the base node you can specify to return just the base node by specifying Single().
12.	Node trees can be chained together using ToList().Union().
13.	ToList() and Single() do not appear until all of the required values have been entered.

Note that when using MatchingRoute(), the required controller and action values are not currently enforced. We are still contemplating how best
to deal with node inheritance (suggestions welcome).

To see the API in (non)action, see the classes in the \_FluentSiteMapNodeProviders\ directory. Keep in mind this doesn't run, it is just
a first look at how to design the interface.
