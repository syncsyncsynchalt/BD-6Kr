namespace Server_Controllers
{
	public class Api_Result<T>
	{
		public Api_Result_State state;

		public TurnState t_state;

		public T data;

		public Api_Result()
		{
			state = Api_Result_State.Success;
			t_state = TurnState.CONTINOUS;
		}
	}
}
