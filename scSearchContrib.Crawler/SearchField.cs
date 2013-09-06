namespace scSearchContrib.Crawler
{
    public class SearchField
    {
        public SearchField()
        {
        }

        public SearchField(string storageType, string indexType, string vectorType, string boost, string exclude)
        {
            SetStorageType(storageType);
            SetIndexType(indexType);
            SetVectorType(vectorType);
            SetBoost(boost);
            Exclude = Sitecore.MainUtil.StringToBool(exclude, false);
        }

        #region Public Properties

        public Lucene.Net.Documents.Field.Store StorageType { get; set; }

        public Lucene.Net.Documents.Field.Index IndexType { get; set; }

        public Lucene.Net.Documents.Field.TermVector VectorType { get; set; }

        public float Boost { get; set; }

        public bool Exclude { get; set; }

        #endregion

        #region Setters

        public void SetStorageType(string storageType)
        {
            switch (storageType)
            {
                case "YES":
                    {
                        StorageType = Lucene.Net.Documents.Field.Store.YES;
                        break;
                    }
                case "NO":
                    {
                        StorageType = Lucene.Net.Documents.Field.Store.NO;
                        break;
                    }
                case "COMPRESS":
                    {
                        StorageType = Lucene.Net.Documents.Field.Store.COMPRESS;
                        break;
                    }
                default:
                    break;
            }
        }

        public void SetIndexType(string indexType)
        {
            switch (indexType)
            {
                case "NO":
                    {
                        IndexType = Lucene.Net.Documents.Field.Index.NO;
                        break;
                    }
                case "NO_NORMS":
                    {
                        IndexType = Lucene.Net.Documents.Field.Index.NO_NORMS;
                        break;
                    }
                case "TOKENIZED":
                    {
                        IndexType = Lucene.Net.Documents.Field.Index.TOKENIZED;
                        break;
                    }
                case "UN_TOKENIZED":
                    {
                        IndexType = Lucene.Net.Documents.Field.Index.UN_TOKENIZED;
                        break;
                    }
                default:
                    break;
            }
        }

        public void SetVectorType(string vectorType)
        {
            switch (vectorType)
            {
                case "NO":
                    {
                        VectorType = Lucene.Net.Documents.Field.TermVector.NO;
                        break;
                    }
                case "WITH_OFFSETS":
                    {
                        VectorType = Lucene.Net.Documents.Field.TermVector.WITH_OFFSETS;
                        break;
                    }
                case "WITH_POSITIONS":
                    {
                        VectorType = Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS;
                        break;
                    }
                case "WITH_POSITIONS_OFFSETS":
                    {
                        VectorType = Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS;
                        break;
                    }
                case "YES":
                    {
                        VectorType = Lucene.Net.Documents.Field.TermVector.YES;
                        break;
                    }
                default:
                    break;
            }
        }

        public void SetBoost(string boost)
        {
            float boostReturn;

            if (float.TryParse(boost, out boostReturn))
            {
                Boost = boostReturn;
                return;
            }

            Boost = 1;
        }

        #endregion
    }
}
