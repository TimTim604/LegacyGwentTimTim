using System.Linq;
using System.Threading.Tasks;
using Alsein.Utilities;

namespace Cynthia.Card
{
	[CardEffectId("64003")]//海玫家族佛兰明妮卡
	public class HeymaeyFlaminica : CardEffect
	{//移除所在排的灾厄，并将2个友军单位移至该排。
		public HeymaeyFlaminica(IGwentServerGame game, GameCard card) : base(game, card){}
		public override async Task<int> CardPlayEffect(bool isSpying)
		{
			return 0;
		}
	}
}