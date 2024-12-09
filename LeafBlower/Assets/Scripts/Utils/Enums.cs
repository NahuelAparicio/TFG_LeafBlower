public class Enums
{
    #region States
    public enum GameState { Menu, Playing, PauseMenu, Exit }
    public enum CharacterState { Idle, Moving, Interacting, Talking }
    public enum CharacterMoveState { None, Walking, Running, Air }
    public enum QuestState { Locked, Unlocked, InProgress, Completed, Canceled }
    #endregion

    public enum Movements { Dash, Jump, Hover, GroundMovement, AirMovement }
    public enum NPCMovements { InPlace, MoveAround, MoveSequence }
    public enum ModifierType { Flat, PercentualToBase }
    public enum BlowType { RealisticBlow, PuzzleBlow, DirectionalBlow }
    public enum DialogueTypingType { NoEffect, TypingMachine }

    public enum CharacterNames
    {
        None = 0,
        Kayan,
        Noseque
    }

    public enum ObjectWeight { Leaf, Ball = 1, Low = 3, Medium = 5, Heavy = 7, SuperHeavy = 10 }
}
