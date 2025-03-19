namespace HyperCasual.Runner
{
    /// <summary>
    /// Defines unique identifiers for all audio clips in the game.
    /// Used by the AudioManager to reference and play specific sounds.
    /// </summary>
    public enum SoundID
    {
        /// <summary>No sound selected</summary>
        None = 0,
        
        // Gameplay Sound Effects
        /// <summary>Sound played when collecting coins</summary>
        CoinSound,
        /// <summary>Sound played when collecting keys</summary>
        KeySound,
        
        // UI Sound Effects
        /// <summary>Sound played when interacting with buttons</summary>
        ButtonSound,
        
        // Background Music
        /// <summary>Music played in menu screens</summary>
        MenuMusic,
        /// <summary>Music played during gameplay</summary>
        GameplayMusic,
        /// <summary>Music played on victory screen</summary>
        VictoryMusic,
        /// <summary>Music played on game over screen</summary>
        GameOverMusic
    }
}