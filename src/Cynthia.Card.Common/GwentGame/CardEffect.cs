using System.Threading.Tasks;

namespace Cynthia.Card
{
    public class CardEffect
    {
        public GameCard Card { get; set; }//宿主
        public IGwentServerGame Game { get; set; }//游戏本体

        public CardEffect(IGwentServerGame game, GameCard card)
        {
            Game = game;
            Card = card;
        }

        //-----------------------------------------------------------
        //公共效果
        public virtual async Task ToCemetery()//进入墓地触发
        {
            await Game.ShowCardMove(new CardLocation() { RowPosition = RowPosition.MyCemetery, CardIndex = 0 }, Card);
            await Task.Delay(400);
            if (Card.Status.IsDoomed)//如果是佚亡,放逐
            {
                await Banish();
            }
            await Game.SetCemeteryInfo(Card.PlayerIndex);
        }
        public virtual async Task Banish()//放逐
        {
            //需要补充
            await Task.CompletedTask;
        }

        //-----------------------------------------------------------
        //特殊卡的单卡使用
        public virtual async Task CardUse()//使用
        {
            await CardUseStart();
            await CardUseEffect();
            await CardUseEnd();
        }
        public virtual async Task CardUseStart()//使用前移动
        {
            await Game.ShowCardMove(new CardLocation() { RowPosition = RowPosition.MyStay, CardIndex = 0 }, Card);
            await Task.Delay(400);
        }
        public virtual async Task CardUseEffect()//使用效果
        {
            await Task.CompletedTask;
        }
        public virtual async Task CardUseEnd()//使用结束
        {
            await ToCemetery();
        }

        //-----------------------------------------------------------
        //单位卡的单卡放置
        public virtual async Task Play(CardLocation location)//放置
        {
            await CardPlayStart(location);
            var count = await CardPlayEffect();
            if (Card.Status.CardRow.IsOnPlace())
                await CardDown();
            await PlayStayCard(count);
            if (Card.Status.CardRow.IsOnPlace())
                await CardDownEffect();
        }
        public virtual async Task CardPlayStart(CardLocation location)//先是移动到目标地点
        {
            await Game.ShowCardOn(Card);
            await Game.ShowCardMove(location, Card);
        }
        public virtual async Task PlayStayCard(int count)
        {
            await Task.CompletedTask;
        }
        public virtual async Task<int> CardPlayEffect()
        {
            await Task.Delay(400);
            //await Game.DrawCard(1, 1);
            return 0;
        }
        public virtual async Task CardDown()
        {
            await Game.ShowCardDown(Card);
            await Game.SetPointInfo();
        }
        public virtual async Task CardDownEffect()
        {
            await Task.CompletedTask;
        }
        //------------------------------------------------------------
        //单位卡的单卡所受效果
        public virtual async Task BigRoundEnd()//小局结束
        {
            //if (Card.CardStatus.Location.RowPosition)
            await Task.CompletedTask;
        }
        public virtual async Task Strengthen(int num, CardLocation taget = null)//强化
        {
            Card.Status.Strength += num;
            await Task.CompletedTask;
        }
        public virtual async Task Weaken(int num, CardLocation taget = null)//削弱
        {
            Card.Status.Strength -= num;
            if (Card.Status.Strength < 0)
            {
                Card.Status.Strength = 0;
                await Banish();
            }
        }
        public virtual async Task Boost(int num, CardLocation taget = null)//增益
        {
            Card.Status.HealthStatus += num;
            await Task.CompletedTask;
        }
        public virtual async Task Damage(int num, CardLocation taget = null)//伤害
        {
            Card.Status.HealthStatus -= num;
            if (Card.Status.HealthStatus + Card.Status.Strength < 0)
            {
                Card.Status.HealthStatus = -Card.Status.Strength;
                await ToCemetery();
            }
        }
        public virtual async Task Reset(CardLocation taget = null)//重置
        {
            Card.Status.HealthStatus = 0;
            await Task.CompletedTask;
        }
        public virtual async Task Heal(CardLocation taget = null)//治愈
        {
            await Task.CompletedTask;
            if (Card.Status.HealthStatus < 0)
                Card.Status.HealthStatus = 0;
        }
    }
}