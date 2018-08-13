namespace IttyBittyApocalypse
{
	internal class Event
	{
		public delegate void EventDelegate(Player player);

		private readonly EventDelegate eventFunction;

		public Event(EventDelegate eventFunction)
		{
			this.eventFunction = eventFunction;
		}

		public void Execute(Player player)
		{
			eventFunction(player);
		}
	}
}
