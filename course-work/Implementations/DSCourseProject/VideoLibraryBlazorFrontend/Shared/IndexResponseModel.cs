namespace VideoLibraryBlazorFrontend.Shared
{
    public class IndexResponseModel<TItem, TFilter> 
        where TFilter : Filter
    {
        public List<TItem> Items { get; set; }
        public Pager Pager { get; set; }
        public TFilter Filter { get; set; }
    }
}
