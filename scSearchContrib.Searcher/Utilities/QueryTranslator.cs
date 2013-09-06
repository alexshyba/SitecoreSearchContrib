using System;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sitecore.Diagnostics;
using Sitecore.Search;

namespace scSearchContrib.Searcher.Utilities
{
    public class QueryTranslator
    {
        private Analyzer _analyzer;

        protected QueryTranslator() { }

        public QueryTranslator(ILuceneIndex index)
        {
            Assert.ArgumentNotNull(index, "index");
            Initialize(index, true);
        }

        protected void Initialize(ILuceneIndex index, bool close)
        {
            Assert.ArgumentNotNull(index, "index");
            _analyzer = index.Analyzer;
            Assert.IsNotNull(_analyzer, "Failed to request analyzer from the index");
        }

        public virtual Query Translate(QueryBase query)
        {
            var fieldQuery = query as FieldQuery;
            if (fieldQuery != null)
            {
                return ConvertFieldQuery(fieldQuery);
            }

            var combinedQuery = query as CombinedQuery;
            if (combinedQuery != null)
            {
                return ConvertCombinedQuery(combinedQuery);
            }

            var fullTextQuery = query as FullTextQuery;
            if (fullTextQuery == null)
            {
                throw new Exception("Unknown query type");
            }

            Assert.IsNotNull(fullTextQuery.Query, "Full text query is empty");
            Assert.IsNotNullOrEmpty(fullTextQuery.Query.Trim(), "Full text query is empty");

            return InternalParse(fullTextQuery.Query);
        }

        protected virtual Query ConvertFieldQuery(FieldQuery query)
        {
            try
            {
                return InternalParse(query.FieldValue, Escape(query.FieldName));
            }
            catch
            {
                return InternalParse(Escape(query.FieldValue), Escape(query.FieldName));
            }
        }

        public static string Escape(string query)
        {
            return QueryParser.Escape(query);
        }

        public virtual BooleanQuery ConvertCombinedQuery(CombinedQuery query)
        {
            var booleanQuery = new BooleanQuery();
            foreach (var clause in query.Clauses)
            {
                var translatedQuery = Translate(clause.Query);
                if (translatedQuery != null)
                {
                    booleanQuery.Add(translatedQuery, GetOccur(clause.Occurance));
                }
            }
            return booleanQuery;
        }

        public virtual BooleanClause.Occur GetOccur(QueryOccurance condition)
        {
            if (condition == QueryOccurance.Must)
            {
                return BooleanClause.Occur.MUST;
            }

            if (condition == QueryOccurance.MustNot)
            {
                return BooleanClause.Occur.MUST_NOT;
            }

            if (condition == QueryOccurance.Should)
            {
                return BooleanClause.Occur.SHOULD;
            }

            throw new Exception("Unknown condition");
        }

        protected Query InternalParse(string query)
        {
            Assert.ArgumentNotNullOrEmpty(query, "query");
            return InternalParse(query, BuiltinFields.Content);
        }

        protected virtual Query InternalParse(string query, string defaultField)
        {
            Assert.ArgumentNotNullOrEmpty(query, "query");
            Assert.ArgumentNotNullOrEmpty(defaultField, "defaultField");
            try
            {
                return Assert.ResultNotNull(new QueryParser(defaultField, _analyzer).Parse(query), "Query parser returned null reference");
            }
            catch (ParseException)
            {
                return Assert.ResultNotNull(new QueryParser(defaultField, _analyzer).Parse(Escape(query)), "Query parser returned null reference");
            }
        }
    }
}
