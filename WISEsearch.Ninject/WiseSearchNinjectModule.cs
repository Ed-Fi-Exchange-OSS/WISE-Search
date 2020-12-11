using EDFI.Modules.Search.Lucene;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace WISEsearch.Ninject
{

    public class WiseSearchNinjectModule : NinjectModule
    {
        private LuceneSearcherContextFactory _luceneSearcherContextFactory;

        public override void Load()
        {
            Kernel
                .Bind(x => x.FromAssemblyContaining(
                typeof(WISEsearch.Web.Api.Search.IPersonSearchController),
                typeof(ILuceneQueryParser),
                typeof(WISEsearch.Business.IPersonSearchBusiness))
                    //typeof(WISEsearch.MVC.WebApiConfig))
                .SelectAllClasses()
                .BindDefaultInterface());
            _luceneSearcherContextFactory = new LuceneSearcherContextFactory();
            Kernel.Rebind<ILuceneSearcherContextFactory>().ToConstant(_luceneSearcherContextFactory).InSingletonScope();
            Kernel.Bind<IPhoneticEncoder>().To<Metaphone3>();
        }
    }
}
