namespace scSearchContrib.Searcher.Parameters
{
    using System.Collections.Generic;
    using System.Linq;

    using Lucene.Net.Search;

    using Sitecore.Diagnostics;

    using scSearchContrib.Searcher.Utilities;

    using Sitecore.Search;

    public class MultiFieldSearchParam : SearchParam
    {
        public struct Refinement
        {
            public string Name { get; private set; }

            public string Value { get; private set; }

            public Refinement(string name, string value) : this() { Name = name; Value = value; }
        }

        public MultiFieldSearchParam()
        {
            Refinements = new List<Refinement>();
        }

        public QueryOccurance InnerCondition { get; set; }

        public IEnumerable<Refinement> Refinements { get; set; }

        public override Query ProcessQuery(QueryOccurance condition, Index index)
        {
            Assert.ArgumentNotNull(index, "Index");

            var translator = new QueryTranslator(index);
            Assert.IsNotNull(translator, "Query Translator");

            var innerCondition = translator.GetOccur(InnerCondition);
            var outerCondition = translator.GetOccur(condition);

            var baseQuery = base.ProcessQuery(condition, index) as BooleanQuery ?? new BooleanQuery();

            var multiFieldQuery = QueryBuilder.BuildMultiFieldQuery(this.Refinements.ToList(), innerCondition);
            if (multiFieldQuery != null)
            {
                baseQuery.Add(multiFieldQuery, outerCondition);
            }

            return baseQuery;
        }
    }
}
