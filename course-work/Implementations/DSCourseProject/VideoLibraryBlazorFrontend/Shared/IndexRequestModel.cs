namespace VideoLibraryBlazorFrontend.Shared
{
    public class IndexRequestModel<TFilter> 
        where TFilter : Filter
    {
        public Pager Pager { get; set; }
        public TFilter Filter { get; set; }
    }
}
