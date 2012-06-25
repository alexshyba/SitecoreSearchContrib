using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using Sitecore.Search;
using scSearchContrib.Searcher.Utilities;

namespace scSearchContrib.Searcher.Parameters
{
   public class MultiFieldSearchParam : SearchParam
   {
      public struct Refinement
      {
         public string Name { get; private set; }
         public string Value { get; private set; }

         public Refinement(string name, string value): this() { Name = name; Value = value; }
      }

      public MultiFieldSearchParam()
      {
         Refinements = new List<Refinement>();
      }

      public QueryOccurance InnerCondition { get; set; }

      public IEnumerable<Refinement> Refinements { get; set; }

      public override BooleanQuery ProcessQuery(QueryOccurance occurance, Index index)
      {
         var outerQuery = new BooleanQuery();

         var refinementQuery = ApplyRefinements(Refinements, InnerCondition);

         var translator = new QueryTranslator(index);
         var refBooleanQuery = translator.ConvertCombinedQuery(refinementQuery);
         var outerOccurance = translator.GetOccur(occurance);

         if (refBooleanQuery != null && refBooleanQuery.Clauses().Count > 0)
            outerQuery.Add(refBooleanQuery, outerOccurance);

         var baseQuery = base.ProcessQuery(occurance, index);
         if (baseQuery != null)
            outerQuery.Add(baseQuery, outerOccurance);

         return outerQuery;
      }

      protected CombinedQuery ApplyRefinements(IEnumerable<Refinement> refinements, QueryOccurance occurance)
      {
         if (refinements.Count() <= 0) return new CombinedQuery();

         var innerQuery = new CombinedQuery();

         foreach (var refinement in refinements)
         {
            AddFieldValueClause(innerQuery, refinement.Name, refinement.Value, occurance);
         }

         return innerQuery;
      }
   }
}
