namespace scSearchContrib.Searcher
{
    using System.Collections.Generic;
    using System.Linq;

    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;

    using scSearchContrib.Searcher.Parameters;
    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Search;

    internal static class QueryBuilder
    {
        public static Query BuildPartialFieldValueClause(Index index, string fieldName, string fieldValue)
        {
            Assert.ArgumentNotNull(index, "Index");
            Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");

            if (string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);
            var fieldQuery = new QueryParser(fieldName.ToLowerInvariant(), index.Analyzer).Parse(fieldValue);
            return fieldQuery;
        }

        public static Query BuildExactFieldValueClause(Index index, string fieldName, string fieldValue)
        {
            Assert.ArgumentNotNull(index, "Index");

            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            fieldValue = IdHelper.ProcessGUIDs(fieldValue);

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(fieldName.ToLowerInvariant(), fieldValue.ToLowerInvariant()));

            return phraseQuery;
        }

        public static Query BuildTemplateQuery(string templateIds)
        {
            return BuildIdFilter(BuiltinFields.Template, templateIds);
        }

        public static Query BuildLanguageQuery(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return null;
            }

            var phraseQuery = new PhraseQuery();
            phraseQuery.Add(new Term(BuiltinFields.Language, language.ToLowerInvariant()));
            return phraseQuery;
        }

        public static Query BuildFullTextQuery(string searchText, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");
            if (string.IsNullOrEmpty(searchText))
            {
                return null;
            }

            return new QueryParser(BuiltinFields.Content, index.Analyzer).Parse(searchText);
        }

        public static Query BuildLocationFilter(string locationIds)
        {
            return BuildIdFilter(BuiltinFields.Path, locationIds);
        }

        public static Query BuildRelationFilter(string ids)
        {
            return BuildIdFilter(BuiltinFields.Links, ids);
        }

        public static Query BuildFieldQuery(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                return null;
            }

            // if we are searching by _id field, do not lowercase
            fieldValue = IdHelper.ProcessGUIDs(fieldValue);
            return new TermQuery(new Term(fieldName.ToLowerInvariant(), fieldValue));
        }

        public static Query BuildNumericRangeSearchParam(IEnumerable<NumericRangeSearchParam.NumericRangeField> ranges, BooleanClause.Occur condition)
        {
            Assert.ArgumentNotNull(ranges, "Ranges");

            if (!ranges.Any())
            {
                return null;
            }

            if (ranges.Count() == 1)
            {
                return BuildNumericRangeQuery(ranges.First());
            }

            var innerQuery = new BooleanQuery();
            foreach (var range in ranges)
            {
                innerQuery.Add(BuildNumericRangeQuery(range), condition);
            }

            return innerQuery;
        }

        public static Query BuildDateRangeSearchParam(IEnumerable<DateRangeSearchParam.DateRange> ranges, BooleanClause.Occur condition)
        {
            Assert.ArgumentNotNull(ranges, "Ranges");

            if (!ranges.Any())
            {
                return null;
            }

            if (ranges.Count() == 1)
            {
                return BuildDateRangeQuery(ranges.First());
            }

            var innerQuery = new BooleanQuery();
            foreach (var range in ranges)
            {
                innerQuery.Add(BuildDateRangeQuery(range), condition);
            }

            return innerQuery;
        }

        public static Query BuildIdFilter(string fieldName, string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return null;
            }

            var values = IdHelper.ParseId(ids);

            if (values == null || !values.Any())
            {
                return null;
            }

            if (values.Count() == 1)
            {
                return BuildFieldQuery(fieldName, values[0]);
            }

            var query = new BooleanQuery();
            foreach (var value in values.Where(ID.IsID))
            {
                query.Add(BuildFieldQuery(fieldName, value), BooleanClause.Occur.SHOULD);
            }

            return query;
        }

        private static Query BuildDateRangeQuery(DateRangeSearchParam.DateRange dateRange)
        {
            var startDateTime = dateRange.StartDate;
            var endDateTime = dateRange.EndDate;

            // converting to lucene format
            var startDate = DateTools.DateToString(startDateTime, DateTools.Resolution.DAY);
            var endDate = DateTools.DateToString(endDateTime, DateTools.Resolution.DAY);

            return new RangeQuery(new Term(dateRange.FieldName, startDate), new Term(dateRange.FieldName, endDate), true);
        }

        private static Query BuildNumericRangeQuery(NumericRangeSearchParam.NumericRangeField range)
        {
            var startTerm = new Term(range.FieldName, NumberTools.LongToString(range.Start));
            var endTerm = new Term(range.FieldName, NumberTools.LongToString(range.End));
            return new RangeQuery(startTerm, endTerm, true);
        }

        public static Query BuildMultiFieldQuery(IEnumerable<MultiFieldSearchParam.Refinement> refinements, BooleanClause.Occur condition)
        {
            Assert.ArgumentNotNull(refinements, "Refinements");

            if (!refinements.Any())
            {
                return null;
            }

            if (refinements.Count() == 1)
            {
                return BuildFieldQuery(refinements.First().Name, refinements.First().Value);
            }

            var innerQuery = new BooleanQuery();
            foreach (var refinement in refinements)
            {
                innerQuery.Add(BuildFieldQuery(refinement.Name, refinement.Value), condition);
            }

            return innerQuery;
        }
    }
}
