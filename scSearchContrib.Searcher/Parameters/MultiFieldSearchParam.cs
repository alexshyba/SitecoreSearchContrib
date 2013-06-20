namespace scSearchContrib.Searcher.Parameters
{
    using System.Collections.Generic;

    using Lucene.Net.Search;

    public class MultiFieldSearchParam : MultiSearchParam
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

        public IEnumerable<Refinement> Refinements { get; set; }

        protected override Query BuildQuery(BooleanClause.Occur innerCondition)
        {
            return QueryBuilder.BuildMultiFieldQuery(this.Refinements, innerCondition);
        }
    }
}
