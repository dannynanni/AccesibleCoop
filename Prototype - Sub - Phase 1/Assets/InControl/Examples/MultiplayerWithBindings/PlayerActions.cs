namespace MultiplayerWithBindingsExample
{
	using InControl;


	public class PlayerActions : PlayerActionSet
	{
        public PlayerAction Green;
        public PlayerAction Red;
        public PlayerAction Blue;
        public PlayerAction Yellow;
        public PlayerAction Left;
		public PlayerAction Right;
		public PlayerAction Up;
		public PlayerAction Down;
        public PlayerAction AButton;
        public PlayerAction BButton;
        public PlayerAction XButton;
        public PlayerAction YButton;
        public PlayerAction Start;
        public PlayerTwoAxisAction Rotate;

        public PlayerActions()
		{
			Left = CreatePlayerAction( "Left" );
			Right = CreatePlayerAction( "Right" );
			Up = CreatePlayerAction( "Up" );
			Down = CreatePlayerAction( "Down" );

			Rotate = CreateTwoAxisPlayerAction( Left, Right, Down, Up );

            AButton = CreatePlayerAction( "AButton" );
            BButton = CreatePlayerAction( "BButton" );
            XButton = CreatePlayerAction( "XButton" );
            YButton = CreatePlayerAction( "YButton" );

            Green = CreatePlayerAction( "Green" );
            Red = CreatePlayerAction( "Red" );
            Blue = CreatePlayerAction( "Blue" );
            Yellow = CreatePlayerAction( "Yellow" );

            Start = CreatePlayerAction( "Start" );

        }


		//public static PlayerActions CreateWithKeyboardBindings()
		//{
		//	var actions = new PlayerActions();

		//	actions.Green.AddDefaultBinding( Key.A );
		//	actions.Red.AddDefaultBinding( Key.S );
		//	actions.Blue.AddDefaultBinding( Key.D );
		//	actions.Yellow.AddDefaultBinding( Key.F );

		//	actions.Up.AddDefaultBinding( Key.UpArrow );
		//	actions.Down.AddDefaultBinding( Key.DownArrow );
		//	actions.Left.AddDefaultBinding( Key.LeftArrow );
		//	actions.Right.AddDefaultBinding( Key.RightArrow );

		//	return actions;
		//}


		public static PlayerActions CreateWithJoystickBindings()
		{
			var actions = new PlayerActions();

			actions.AButton.AddDefaultBinding( InputControlType.Action1 );
			actions.BButton.AddDefaultBinding( InputControlType.Action2 );
			actions.XButton.AddDefaultBinding( InputControlType.Action3 );
			actions.YButton.AddDefaultBinding( InputControlType.Action4 );

            actions.Green.AddDefaultBinding(InputControlType.Action1);
            actions.Red.AddDefaultBinding(InputControlType.Action2);
            actions.Blue.AddDefaultBinding(InputControlType.Action3);
            actions.Yellow.AddDefaultBinding(InputControlType.Action4);

            actions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
			actions.Down.AddDefaultBinding( InputControlType.LeftStickDown );
			actions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
			actions.Right.AddDefaultBinding( InputControlType.LeftStickRight );

			actions.Up.AddDefaultBinding( InputControlType.DPadUp );
			actions.Down.AddDefaultBinding( InputControlType.DPadDown );
			actions.Left.AddDefaultBinding( InputControlType.DPadLeft );
			actions.Right.AddDefaultBinding( InputControlType.DPadRight );

            actions.Start.AddDefaultBinding( InputControlType.Command );

			return actions;
		}
	}
}

