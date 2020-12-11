using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;

namespace EDFI.Modules.Search.Lucene
{
    public class NameSynonymFilter : TokenFilter
    {
        private readonly ILookup<string, string> _synonymLookup;
        private Queue<string> phoneticKeyQueue = new Queue<string>();
        private Queue<Token> synonymTokenQueue = new Queue<Token>();
        private ITermAttribute _termAttribute;
        private IPositionIncrementAttribute _postionIncrementAttribute;
        private ITypeAttribute _typeAttribute;
        private Metaphone3 _methaphone;

        public NameSynonymFilter(TokenStream input, ILookup<string, string> synonymLookup): base(input)
        {
            _synonymLookup = synonymLookup;
            _termAttribute = AddAttribute<ITermAttribute>();
            _postionIncrementAttribute = AddAttribute<IPositionIncrementAttribute>();
            _typeAttribute = AddAttribute<ITypeAttribute>();
            _methaphone = new Metaphone3();
        }

        public override bool IncrementToken()
        {
            if (synonymTokenQueue.Count > 0)
            {
                var phoeticKey = phoneticKeyQueue.Dequeue();
                _termAttribute.SetTermBuffer(phoeticKey);
                _postionIncrementAttribute.PositionIncrement = 0; // put this at the same position as the original word
                
                return true;
            }

            // if our synonymTokens queue contains any tokens, return the next one.
            if (!input.IncrementToken())
                return false;

            // Convert term to phonetic key
            var originalTerm = _termAttribute.Term;
            var termPhoneticKeys = EncodeWord(originalTerm)
                .ToList();

            var firstKey = termPhoneticKeys.First();
            _termAttribute.SetTermBuffer(firstKey);

            // If multiple phonetic keys were created for this term, add them too
            termPhoneticKeys.Remove(firstKey);
            termPhoneticKeys.ForEach(key => phoneticKeyQueue.Enqueue(key));

            //retrieve the synonyms
            IEnumerable<string> synonyms = _synonymLookup[originalTerm];
            var synonymPhoneticKeys =
                synonyms.SelectMany(synonym => EncodeWord(synonym).Select(key => key))
                .Where(synonym => synonym != originalTerm)
                .Distinct()
                .ToList();

            // add the synonyms for token processing
            synonymPhoneticKeys.ForEach(phoneticKeyQueue.Enqueue);

            return true;
        }

        private IEnumerable<string> EncodeWord(string word)
        {
            return _methaphone.GetPhoneticKeys(word);
        }
    }
}