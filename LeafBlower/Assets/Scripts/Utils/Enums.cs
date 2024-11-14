public class Enums
{
    public enum GameState { Menu, Playing, PauseMenu, Exit }
    public enum CharacterState { Idle, Moving, Interacting }
    public enum QuestState { Locked, Unlocked, InProgress, Completed, Canceled }
    public enum NPCMovements { InPlace, MoveAround, MoveSequence }
    public enum ModifierType { Flat, PercentualToBase }


    public enum DialogueTypingType { NoEffect, TypingMachine }

    public enum CharacterNames
    {
        None = 0,
        MalePlayer,
        FemalePlayer
    }
}
