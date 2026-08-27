namespace CommerceHub.Web.Models
{

    public interface IPricableOrder
    {
        decimal GetTotal();
    }
    public class Order : IPricableOrder, IEntity
    {
        public List<decimal> ItemPrices { get; set; }
        public virtual decimal GetTotal()=> ItemPrices.Sum();
    }


    public class GiftOrder //: Order
    {
        public string Note { get; set; }
        //public override decimal GetTotal()
        //{
        //    throw new NotSupportedException("Hediye siparişlerde toplam tutar hesaplanmıyor!");
        //}
    }

    /*
     * GiftOrder, hediye olduğu için, Toplamını alma durumunda hata verdik!
     * Liskov diyor ki, alt sınıflar, üst sınıfların yerine kullanılabilmelidir. Başka bir değişle miras alan sınıf; veren sınıfın yapısına müdahale edemez.
     * 
     */
}
