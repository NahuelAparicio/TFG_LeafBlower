public class Enums
{
    #region States
    public enum GameState { Menu, Playing, PauseMenu, Exit }
    public enum CharacterState { Idle, Moving, Interacting, Talking }
    public enum CharacterMoveState { None, Walking, Running, Air }
    public enum QuestState { RequirementNotMet, CanStart, InProgress, CanFinish, Finished }
    #endregion

    public enum NPCMovements { InPlace, MoveAround, MoveSequence }
    public enum ModifierType { Flat, PercentualToBase }
    public enum BlowType { RealisticBlow, DirectionalBlow }
    public enum DialogueTypingType { NoEffect, TypingMachine }
    public enum VolumeType { Master, Music, Sfx, Ambience }
    public enum CharacterNames
    {
        Gnomo = 0,
        Broki,
        Penny
    }

    public enum MenuState { MainMenu, SettingsMenu, PauseMenu}

    public enum ObjectType { Colectionable, Leaf, Object }

    public enum CollectionableType { None, Coin, Jordan }

}
