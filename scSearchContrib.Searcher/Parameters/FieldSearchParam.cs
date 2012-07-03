using System;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sitecore.Search;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Searcher.Parameters
{
    public class FieldSearchParam : SearchParam
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public bool Partial { get; set; }

        public override BooleanQuery ProcessQuery(QueryOccurance occurance, Index index)
        {
            var query = base.ProcessQuery(occurance, index) ?? new BooleanQuery();

            if (Partial)
            {
                AddPartialFieldValueClause(index, query, FieldName, FieldValue);
            }
            else
            {
                AddExactFieldValueClause(index, query, FieldName, FieldValue);
            }

            return query;
        }

        protected void AddPartialFieldValueClause(Index index, BooleanQuery query, string fieldName, string fieldValue)
        {
            if (String.IsNullOrEmpty(fieldValue)) return;

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);

            var fieldQuery = new QueryParser(fieldName.ToLowerInvariant(), index.Analyzer).Parse(fieldValue);

            query.Add(fieldQuery, BooleanClause.Occur.MUST);
        }

        protected void AddExactFieldValueClause(Index index, BooleanQuery query, string fieldName, string fieldValue)
        {
            //if (String.IsNullOrEmpty(fieldValue)) return;

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(fieldName.ToLowerInvariant(), fieldValue));

            query.Add(phraseQuery, BooleanClause.Occur.MUST);
        }
    }
}
