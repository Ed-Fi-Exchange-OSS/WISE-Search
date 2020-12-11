# WISE-Search

An MVC application and WebApi services to maintain and perform customizable
Lucene searches.  The WISEsearch application is not opinionated on the form of
the data that needs to be indexed and is ignorant as to the source of the data.
This allows you to decide on the information that is important for indexing and
WISEsearch also allows the creating of any number of indexes for different
entities.

What is **not** included at the moment

* Name Variations - WISEid has a large list of common variations for names (ex:
  Jeff => Jeffrey, Jeffery, Geoffrey) that plays a big part in matching. This
  was derrived from a list of names purchased from Peacock Data, but we
  currently do not have a license to share the synonyms
* Phonetic indexing - Although, the search has the option to translate words
  into the Metaphone3 phonetic encoding, this is not current implemented on the
  indexing side
* Weighting and disabling of coordination score for booleanQueries

## Requirements

* .Net 4.5.1
* MVC 5.2.2
* Access to create a folder to store indexes outside of the MVC application

## Setup

* Either Create IIS application or run WISEsearch.MVC.csproj with IISExpress
* All other dependencies contained in local nuget package directory
* The search index will be empty by default and requires you to create the indexes by using the Put API call in WISEsearch.Web.Api.Client, before performing any searches (see integration tests for examples)

## Points of Interest

### WISEsearch.Web.Api

For any state wishing to perform their own custom searches, we are thinking that
they would need to create a project similar to this that would provide their Web
API endpoints for ED-FI to hook into.

* **PersonSearchController** - This is an example of how a state could send Web
  Api requests that are closer aligned to their domain model
* **SearchIndexController** - Similar in function to the PersonSearchController,
  but a generic format that could be reused by any index entity type

### WISEsearch.Web.Api.Client & WISEsearch.Web.Api.Domain

We have wrapped these library objects into a nuget package that simplify the
WebApi calls in WISEid and whatever application needs to use our search service

### WISEsearch.Web.Api.Client.Tests

Provide some examples on how to use each of the Web Api endpoints

### EDFI.Modules.Search

Encapsulates the Lucene index/search logic. We envision ED-FI using this to
create a nuget package for anyone wishing to override the default ED-FI search
logic.

### SearchConfig.xml

This is the configuration for all the queries that WISEsearch can run.  It
allows you to build boolean queries for your search requests will execute in
Lucene.  Queries can be weighted to affect score and field queries currently
allow for exact, phonetic (based of
[Metaphone3](http://en.wikipedia.org/wiki/Metaphone) algorithm), and fuzzy
matches (within a tolerance of between 0 and 1, based off the [Levenshtein
distance algorithm](http://en.wikipedia.org/wiki/Levenshtein_distance))

The search queries also allow you to replace any attribute value with tokens
using the format "${tokenName}", so that the code calling the search can easily
configure the weights applied per invocation.  If you specify a token in the
config, it will be required when you search.

The TestPersonSearch search is the current search we are using for WISEid with all the configuration parameters filled in (so you don't have to pass any parameters in your queries calls).
We have constructed this query as:

* Any WISEid or Local row key matches should always be returned
* Otherwise, at least two of First Name, Last Name, Birth Date must match (as
  specified by the Minimum matches field)
* Optionally, if gender, middle name, or suffix match the score will be boosted,
  but they are not sufficient  
    on their own to be considered a match

Each search must have a name that is used when calling search API and a target
index which corresponds to same name that was used when calling the Put API
action used to update/create indexes (this will also correspond to the folder
that is used to store the Lucene index files).

## Legal Information

Copyright (c) 2015, Wisconsin Department of Public Instruction

Source code provided under the terms of the [Apache License, version
2.0](LICENSE)