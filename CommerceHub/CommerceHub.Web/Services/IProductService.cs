using CommerceHub.Web.Features.DataTransferObjects;
using CommerceHub.Web.Models;

namespace CommerceHub.Web.Services
{
    public interface IProductService
    {
        /*
         * Bu arayüzdeki her fonksiyon, bir Product nesnesi ile çalışan uygulama özelliği (feature).
         * Yani gelecekte, yeni bir özellik eklemek isterseniz, buraya yeni bir fonksiyon yazmalısınız.
         * 
         * Eğer bu fonksiyonların ne kadar fazla olacağını bilmiyorsanız, yönetmesi zorlaşacak demektir. 
         * 
         * O halde her özellik -> fonksiyon yerine her özellik -> class olsun.
         * 
         * Örnek: Ürün ekleme özelliği
         * 1. Eklenecek ürünü taşıyan dto (request)
         * 2. Bu nesneyi yakalayan ve kullanan (db işlemini yapan) handler.
         * 3. Handle edildikten sonra ne dönecek?
         */
        Task<decimal> GetFinalPrice(int id);
        Task<IEnumerable<GetAllProductResponse>> GetProducts();
        Task<GetAllProductResponse> GetProduct(int id);
        void SendMailToSupplier();
        Task Create(Product product);
        Task Update(Product product);
        Task Delete(int id);

        Task<IEnumerable<GetAllProductResponse>> Search(string keyword);






    }
}