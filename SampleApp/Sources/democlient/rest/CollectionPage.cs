using System;
using System.Collections.Generic;

namespace SampleApp.Sources.democlient.rest
{
    class CollectionPage<TE> : List<TE>
    {
        public int TotalSize { get; }

        public Uri NextPage { get; }

        public Uri PrevPage { get; }

        public Uri Self { get; }

        public List<TE> Page { get; }

        public CollectionPage(
            List<TE> page,
            Uri self,
            Uri nextPage,
            Uri prevPage,
            int totalSize
            )
        {
            Page = page;
            TotalSize = totalSize;
            NextPage = nextPage;
            PrevPage = prevPage;
            Self = self;
        }
    }
}
