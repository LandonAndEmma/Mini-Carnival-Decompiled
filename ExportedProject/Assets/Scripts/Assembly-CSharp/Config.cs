public class Config
{
	public struct CShop
	{
		public int item_num;

		public int max_size;

		public int resell_price;

		public int tax_rate;

		public int first_buy;

		public int suit_discount;
	}

	public struct CCollect
	{
		public int max_size;
	}

	public struct CFollow
	{
		public int max_size;
	}

	public struct CAd
	{
		public int cache_size;

		public int day_limit;

		public int price;
	}

	public struct CPraise
	{
		public int deal_praise;

		public int top_size;
	}

	public struct CNewest
	{
		public int cache_size;
	}

	public struct CBag
	{
		public int min_size;

		public int max_size;

		public int unlock_cost;
	}

	public struct CMail
	{
		public int max_size;

		public int keep_day;
	}

	public struct CFriend
	{
		public int max_size;
	}

	public CShop Shop = default(CShop);

	public CCollect Collect = default(CCollect);

	public CFollow Follow = default(CFollow);

	public CAd Ad = default(CAd);

	public CPraise Praise = default(CPraise);

	public CNewest Newest = default(CNewest);

	public CBag Bag = default(CBag);

	public CMail Mail = default(CMail);

	public CFriend Friend = default(CFriend);
}
