using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniGamesSystem
{
	public class PlayerInfo
	{
		public string nick;
		public int Coins;
		public List<string> ListaCzapek = new List<string>();

		public PlayerInfo(string nick)
		{
			this.nick = nick;
			Coins = 0;
		}
	}
}
