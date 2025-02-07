public class Enums
{
    #region States
    public enum GameState { Menu, Playing, PauseMenu, Exit }
    public enum CharacterState { Idle, Moving, Interacting, Talking }
    public enum CharacterMoveState { None, Walking, Running, Air }
    public enum QuestState { Locked, Unlocked, InProgress, Completed, Finished }
    #endregion

    public enum Movements { Dash, Jump, Hover, GroundMovement, AirMovement }
    public enum NPCMovements { InPlace, MoveAround, MoveSequence }
    public enum ModifierType { Flat, PercentualToBase }
    public enum BlowType { RealisticBlow, DirectionalBlow }
    public enum DialogueTypingType { NoEffect, TypingMachine }

    public enum CharacterNames
    {
        None = 0,
        Kayan,
        Noseque
    }

    public enum ObjectWeight {Leaf = 0, Low , Medium, Heavy, SuperHeavy, MegaHeavy }
}
