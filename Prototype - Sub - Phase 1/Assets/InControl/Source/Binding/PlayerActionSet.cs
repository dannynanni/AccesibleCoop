namespace InControl
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using UnityEngine;


	/// <summary>
	/// An action set represents a set of actions, usually for a single player. This class must be subclassed to be used.
	/// An action set can contain both explicit, bindable single value actions (for example, "Jump", "Left" and "Right") and implicit,
	/// aggregate actions which combine together other actions into one or two axes, for example "Move", which might consist
	/// of "Left", "Right", "Up" and "Down" filtered into a single two-axis control with its own applied circular deadzone,
	/// queryable vector value, etc.
	/// </summary>
	public abstract class PlayerActionSet
	{
		/// <summary>
		/// Optionally specifies a device which this action set should query from, if applicable.
		/// When set to <c>null</c> (default) this action set will try to find an active device when required.
		/// </summary>
		public InputDevice Device { get; set; }

		/// <summary>
		/// A list of devices which this action set should include when searching for an active device.
		/// When empty, all attached devices will be considered.
		/// </summary>
		public List<InputDevice> IncludeDevices { get; private set; }

		/// <summary>
		/// A list of devices which this action set should exclude when searching for an active device.
		/// </summary>
		public List<InputDevice> ExcludeDevices { get; private set; }

		/// <summary>
		/// Gets the actions in this action set as a readonly collection.
		/// </summary>
		public ReadOnlyCollection<PlayerAction> Actions { get; private set; }

		/// <summary>
		/// The last update tick on which any action in this set changed value.
		/// </summary>
		public ulong UpdateTick { get; protected set; }

		/// <summary>
		/// The binding source type that last provided input to this action set.
		/// </summary>
		public BindingSourceType LastInputType = BindingSourceType.None;

		/// <summary>
		/// Occurs when the binding source type that last provided input to this action set changes.
		/// </summary>
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		/// <summary>
		/// Whether this action set should produce input. Default: <c>true</c>
		/// </summary>
		public bool Enabled { get; set; }


		/// <summary>
		/// This property can be used to store whatever arbitrary game data you want on this action set.
		/// </summary>
		public object UserData { get; set; }


		List<PlayerAction> actions = new List<PlayerAction>();
		List<PlayerOneAxisAction> oneAxisActions = new List<PlayerOneAxisAction>();
		List<PlayerTwoAxisAction> twoAxisActions = new List<PlayerTwoAxisAction>();
		Dictionary<string, PlayerAction> actionsByName = new Dictionary<string, PlayerAction>();
		BindingListenOptions listenOptions = new BindingListenOptions();
		internal PlayerAction listenWithAction;


		protected PlayerActionSet()
		{
			Enabled = true;
			Device = null;
			IncludeDevices = new List<InputDevice>();
			ExcludeDevices = new List<InputDevice>();
			Actions = new ReadOnlyCollection<PlayerAction>( actions );
			InputManager.AttachPlayerActionSet( this );
		}


		/// <summary>
		/// Properly dispose of this action set. You should make sure to call this when the action set
		/// will no longer be used or it will result in unnecessary internal processing every frame.
		/// </summary>
		public void Destroy()
		{
			OnLastInputTypeChanged = null;
			InputManager.DetachPlayerActionSet( this );
		}


		/// <summary>
		/// Create an action on this set. This should be performed in the constructor of your PlayerActionSet subclass.
		/// </summary>
		/// <param name="name">A unique identifier for this action within the context of this set.</param>
		/// <exception cref="InControlException">Thrown when trying to create an action with a non-unique name for this set.</exception>
		protected PlayerAction CreatePlayerAction( string name )
		{
			return new PlayerAction( name, this );
		}


		internal void AddPlayerAction( PlayerAction action )
		{
			action.Device = FindActiveDevice();

			if (actionsByName.ContainsKey( action.Name ))
			{
				throw new InControlException( "Action '" + action.Name + "' already exists in this set." );
			}

			actions.Add( action );
			actionsByName.Add( action.Name, action );
		}


		/// <summary>
		/// Create an aggregate, single-axis action on this set. This should be performed in the constructor of your PlayerActionSet subclass.
		/// </summary>
		/// <example>
		/// <code>
		/// Throttle = CreateOneAxisPlayerAction( Brake, Accelerate );
		/// </code>
		/// </example>
		/// <param name="negativeAction">The action to query for the negative component of the axis.</param>
		/// <param name="positiveAction">The action to query for the positive component of the axis.</param>
		protected PlayerOneAxisAction CreateOneAxisPlayerAction( PlayerAction negativeAction, PlayerAction positiveAction )
		{
			var action = new PlayerOneAxisAction( negativeAction, positiveAction );
			oneAxisActions.Add( action );
			return action;
		}


		/// <summary>
		/// Create an aggregate, double-axis action on this set. This should be performed in the constructor of your PlayerActionSet subclass.
		/// </summary>
		/// <example>
		/// Note that, due to Unity's positive up-vector, the parameter order of <c>negativeYAction</c> and <c>positiveYAction</c> might seem counter-intuitive.
		/// <code>
		/// Move = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
		/// </code>
		/// </example>
		/// <param name="negativeXAction">The action to query for the negative component of the X axis.</param>
		/// <param name="positiveXAction">The action to query for the positive component of the X axis.</param>
		/// <param name="negativeYAction">The action to query for the negative component of the Y axis.</param>
		/// <param name="positiveYAction">The action to query for the positive component of the Y axis.</param>
		protected PlayerTwoAxisAction CreateTwoAxisPlayerAction( PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction )
		{
			var action = new PlayerTwoAxisAction( negativeXAction, positiveXAction, negativeYAction, positiveYAction );
			twoAxisActions.Add( action );
			return action;
		}


		/// <summary>
		/// Gets the action with the specified action name. If the action does not exist, <c>null</c> is returned.
		/// </summary>
		/// <param name="actionName">The name of the action to get.</param>
		public PlayerAction this[string actionName]
		{
			get
			{
				PlayerAction action;
				if (actionsByName.TryGetValue( actionName, out action ))
				{
					return action;
				}
				throw new KeyNotFoundException( "Action '" + actionName + "' does not exist in this action set." );
			}
		}


		internal void Update( ulong updateTick, float deltaTime )
		{
			var device = Device ?? FindActiveDevice();

			var lastInputType = LastInputType;

			var actionsCount = actions.Count;
			for (var i = 0; i < actionsCount; i++)
			{
				var action = actions[i];

				action.Update( updateTick, deltaTime, device );

				if (action.UpdateTick > UpdateTick)
				{
					UpdateTick = action.UpdateTick;
					lastInputType = action.LastInputType;
				}
			}

			var oneAxisActionsCount = oneAxisActions.Count;
			for (var i = 0; i < oneAxisActionsCount; i++)
			{
				oneAxisActions[i].Update( updateTick, deltaTime );
			}

			var twoAxisActionsCount = twoAxisActions.Count;
			for (var i = 0; i < twoAxisActionsCount; i++)
			{
				twoAxisActions[i].Update( updateTick, deltaTime );
			}

			if (LastInputType != lastInputType)
			{
				LastInputType = lastInputType;
				if (OnLastInputTypeChanged != null)
				{
					OnLastInputTypeChanged.Invoke( lastInputType );
				}
			}
		}


		/// <summary>
		/// Reset the bindings on all actions in this set.
		/// </summary>
		public void Reset()
		{
			var actionCount = actions.Count;
			for (var i = 0; i < actionCount; i++)
			{
				actions[i].ResetBindings();
			}
		}


		InputDevice FindActiveDevice()
		{
			var hasIncludeDevices = IncludeDevices.Count > 0;
			var hasExcludeDevices = ExcludeDevices.Count > 0;

			if (hasIncludeDevices || hasExcludeDevices)
			{
				var foundDevice = InputDevice.Null;
				var deviceCount = InputManager.Devices.Count;
				for (var i = 0; i < deviceCount; i++)
				{
					var device = InputManager.Devices[i];
					if (device != foundDevice && device.LastChangedAfter( foundDevice ))
					{
						if (hasExcludeDevices && ExcludeDevices.Contains( device ))
						{
							continue;
						}

						if (!hasIncludeDevices || IncludeDevices.Contains( device ))
						{
							foundDevice = device;
						}
					}
				}
				return foundDevice;
			}

			return InputManager.ActiveDevice;
		}


		public void ClearInputState()
		{
			var actionsCount = actions.Count;
			for (var i = 0; i < actionsCount; i++)
			{
				actions[i].ClearInputState();
			}

			var oneAxisActionsCount = oneAxisActions.Count;
			for (var i = 0; i < oneAxisActionsCount; i++)
			{
				oneAxisActions[i].ClearInputState();
			}

			var twoAxisActionsCount = twoAxisActions.Count;
			for (var i = 0; i < twoAxisActionsCount; i++)
			{
				twoAxisActions[i].ClearInputState();
			}
		}


		internal bool HasBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return false;
			}

			var actionsCount = actions.Count;
			for (var i = 0; i < actionsCount; i++)
			{
				if (actions[i].HasBinding( binding ))
				{
					return true;
				}
			}

			return false;
		}


		internal void RemoveBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return;
			}

			var actionsCount = actions.Count;
			for (var i = 0; i < actionsCount; i++)
			{
				actions[i].FindAndRemoveBinding( binding );
			}
		}


		/// <summary>
		/// Configures how in an action in this set listens for new bindings when the action does not
		/// explicitly define its own listen options.
		/// </summary>
		public BindingListenOptions ListenOptions
		{
			get
			{
				return listenOptions;
			}

			set
			{
				listenOptions = value ?? new BindingListenOptions();
			}
		}


		/// <summary>
		/// Returns the state of this action set and all bindings encoded into a string
		/// that you can save somewhere.
		/// Pass this string to Load() to restore the state of this action set.
		/// </summary>
		public string Save()
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new BinaryWriter( stream, System.Text.Encoding.UTF8 ))
				{
					// Write header.
					writer.Write( (byte) 'B' );
					writer.Write( (byte) 'I' );
					writer.Write( (byte) 'N' );
					writer.Write( (byte) 'D' );

					// Write version.
					writer.Write( (UInt16) 1 );

					// Write actions.
					var actionCount = actions.Count;
					writer.Write( actionCount );
					for (var i = 0; i < actionCount; i++)
					{
						actions[i].Save( writer );
					}
				}

				return Convert.ToBase64String( stream.ToArray() );
			}
		}


		/// <summary>
		/// Load a state returned by calling Dump() at a prior time.
		/// </summary>
		/// <param name="data">The data string.</param>
		public void Load( string data )
		{
			if (data == null)
			{
				return;
			}

			try
			{
				using (var stream = new MemoryStream( Convert.FromBase64String( data ) ))
				{
					using (var reader = new BinaryReader( stream ))
					{
						if (reader.ReadUInt32() != 0x444E4942)
						{
							throw new Exception( "Unknown data format." );
						}

						if (reader.ReadUInt16() != 1)
						{
							throw new Exception( "Unknown data version." );
						}

						var actionCount = reader.ReadInt32();
						for (var i = 0; i < actionCount; i++)
						{
							PlayerAction action;
							if (actionsByName.TryGetValue( reader.ReadString(), out action ))
							{
								action.Load( reader );
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.LogError( "Provided state could not be loaded:\n" + e.Message );
				Reset();
			}
		}
	}
}
