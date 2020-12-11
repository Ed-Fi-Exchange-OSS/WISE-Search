using System;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Version = Lucene.Net.Util.Version;

namespace EDFI.Modules.Search.Lucene
{
    public interface ILuceneSynonymAnalyzer
    {
        Lazy<ILookup<string, string>> SynonymLookup { get; set; }
    }

    public class LuceneSynonymAnalyzer : Analyzer, ILuceneSynonymAnalyzer
    {
        public Lazy<ILookup<string, string>> SynonymLookup { get; set; }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            //create the tokenizer
            TokenStream result = new StandardTokenizer(Version.LUCENE_30, reader);

            //add in filters
            // first normalize the StandardTokenizer
            result = new StandardFilter(result);

            // makes sure everything is lower case
            result = new LowerCaseFilter(result);

            // use the default list of Stop Words, provided by the StopAnalyzer class.
            result = new StopFilter(true, result, StopAnalyzer.ENGLISH_STOP_WORDS_SET);

            // injects the synonyms. 
            result = new NameSynonymFilter(result, SynonymLookup.Value);

            //return the built token stream.
            return result;
        }
    }
}