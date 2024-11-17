using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using _MAIN.Scripts.Extensions;
using UnityEngine;

namespace _MAIN.Scripts.Core.Commands.Database.Extensions
{
    public class CmdDatabaseExtensionCharacters : CmdDatabaseExtension
    {
        private static string[] ParamEnable => new[] { "-e", "-enable" };
        private static string[] ParamImmediate => new[] { "-i", "-immediate" };
        private static string[] ParamSpeed => new[] { "-spd", "-speed" };
        private static string[] ParamSmooth => new[] { "-sm", "-smooth" };
        private static string ParamXPos => "-x";
        private static string ParamYPos => "-y";
        
        public new static void Extend(CommandsDatabase database)
        {
            database.AddCommand("createcharacter", new Action<string[]>(CreateCharacter));
            database.AddCommand("movecharacter", new Func<string[], IEnumerator>(MoveCharacter));
            database.AddCommand("show", new Func<string[], IEnumerator>(ShowAll));
            database.AddCommand("hide", new Func<string[], IEnumerator>(HideAll));
            database.AddCommand("sort", new Action<string[]>(Sort));
            database.AddCommand("highlight", new Func<string[], IEnumerator>(HighlightAll));
            database.AddCommand("unhighlight", new Func<string[], IEnumerator>(UnhighlightAll));

            //Add commands to characters
            var baseCommands = CommandManager.Instance.CreateSubDatabase(CommandManager.DatabaseCharactersBase);
            baseCommands.AddCommand("move", new Func<string[], IEnumerator>(MoveCharacter));
            baseCommands.AddCommand("show", new Func<string[], IEnumerator>(Show));
            baseCommands.AddCommand("hide", new Func<string[], IEnumerator>(Hide));
            baseCommands.AddCommand("setpriority", new Action<string[]>(SetPriority));
            baseCommands.AddCommand("setposition", new Action<string[]>(SetPosition));
            baseCommands.AddCommand("setcolor", new Func<string[], IEnumerator>(SetColor));
            baseCommands.AddCommand("highlight", new Func<string[], IEnumerator>(Highlight));
            baseCommands.AddCommand("unhighlight", new Func<string[], IEnumerator>(Unhighlight));

            //Add character specific databases
            var spriteCommands = CommandManager.Instance.CreateSubDatabase(CommandManager.DatabaseCharactersSprite);
            spriteCommands.AddCommand("setSprite", new Func<string[], IEnumerator>(SetSprite));
        }
        
        #region Global Commands
        
        private static void CreateCharacter(string[] data)
        {
            var characterName = data[0];
            if (!data[1].Contains("-"))
                characterName = characterName + " " + data[1];
            
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamEnable, out bool enable, defaultValue: false);
            parameters.TryGetValue(ParamImmediate, out bool immediate, defaultValue: false);

            var character = CharacterManager.Instance.CreateCharacter(characterName);

            if (!enable)
                return;

            if (immediate)
                character.IsVisible = true;
            else
                character.Show();
                
        }

        private static void Sort(string[] data) => CharacterManager.Instance.SortCharacters(data);

        private static IEnumerator MoveCharacter(string[] data)
        {
            string characterName = data[0];
            Character character = CharacterManager.Instance.GetCharacter(characterName);

            if (character == null)
                yield break;

            var parameters = ConvertDataToParameters(data);
            
            //try to get the x-axis position
            parameters.TryGetValue(ParamXPos, out float x);

            //try to get the y-axis position
            parameters.TryGetValue(ParamYPos, out float y);

            //try to get the speed
            parameters.TryGetValue(ParamSpeed, out float speed, defaultValue: 1);

            //try to get the smoothing
            parameters.TryGetValue(ParamSmooth, out bool smooth, defaultValue: false);

            //try to get imediate setting of position
            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);
            
            var position = new Vector2(x, y);

            if (immediate)
                character.SetPosition(position);
            else
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character?.SetPosition(position); });
                yield return character.MoveToPosition(position, speed, smooth);
            }
        }

        private static IEnumerator ShowAll(string[] data)
        {
            var characters = new List<Character>();

            foreach (var characterName in data) 
            {
                var character = CharacterManager.Instance.GetCharacter(characterName, createIfDoesNotExist: false);
                if (character != null)
                    characters.Add(character);
            }

            if (characters.Count == 0)
                yield break;

            //Convert the data array to a parameter container
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);
            parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 1f);

            //Call the logic on all the characters
            foreach (var character in characters)
            {
                if (immediate)
                    character.IsVisible = true;
                else
                    character.Show(speed);
            }

            if (!immediate)
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach(var character in characters)
                        character.IsVisible = true;
                });

                while (characters.Any(c => c.IsRevealing))
                    yield return null;
            }
        }

        private static IEnumerator HideAll(string[] data)
        {
            var characters = new List<Character>();

            foreach (var s in data)
            {
                var character = CharacterManager.Instance.GetCharacter(s, createIfDoesNotExist: false);
                if (character != null)
                    characters.Add(character);
            }

            if (characters.Count == 0)
                yield break;

            //Convert the data array to a parameter container
            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(ParamImmediate, out var immediate, defaultValue: false);
            parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 1f);

            //Call the logic on all the characters
            foreach (Character character in characters)
            {
                if (immediate)
                    character.IsVisible = false;
                else
                    character.Hide(speed);
            }

            if (!immediate)
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach (Character character in characters)
                        character.IsVisible = false;
                });

                while (characters.Any(c => c.IsHiding))
                    yield return null;
            }
        }

        public static IEnumerator HighlightAll(string[] data)
        {
            var characters = new List<Character>();
            var unspecifiedCharacters = new List<Character>();

            //Add any characters specified to be highlighted.
            for (int i = 0; i < data.Length; i++)
            {
                var character = CharacterManager.Instance.GetCharacter(data[i], createIfDoesNotExist: false);
                if (character != null)
                    characters.Add(character);
            }

            if (characters.Count == 0)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new[] { "-i", "-immediate" }, out var immediate, defaultValue: false);
            parameters.TryGetValue(new[] { "-o", "-only" }, out var handleUnspecifiedCharacters, defaultValue: true);

            //Make all characters perform the logic
            foreach (var character in characters)
                character.Highlight(immediate: immediate);

            //If we are forcing any unspecified characters to use the opposite highlighted status
            if (handleUnspecifiedCharacters)
            {
                foreach (var character in CharacterManager.Instance.AllCharacters)
                {
                    if (characters.Contains(character))
                        continue;

                    unspecifiedCharacters.Add(character);
                    character.UnHighlight(immediate: immediate);
                }
            }

            //Wait for all characters to finish highlighting
            if (!immediate)
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach (var character in characters)
                        character.Highlight(immediate: true);

                    if (!handleUnspecifiedCharacters) return;

                    foreach (var character in unspecifiedCharacters)
                        character.UnHighlight(immediate: true);
                });

                while (characters.Any(c => c.IsHighlighting) || (handleUnspecifiedCharacters && unspecifiedCharacters.Any(uc => uc.IsUnHighlighting)))
                    yield return null;
            }
        }

        public static IEnumerator UnhighlightAll(string[] data)
        {
            var characters = new List<Character>();
            var unspecifiedCharacters = new List<Character>();

            //Add any characters specified to be highlighted.
            foreach (var value in data)
            {
                var character = CharacterManager.Instance.GetCharacter(value, createIfDoesNotExist: false);
                if (character != null)
                    characters.Add(character);
            }

            if (characters.Count == 0)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new string[] { "-i", "-immediate" }, out var immediate, defaultValue: false);
            parameters.TryGetValue(new string[] { "-o", "-only" }, out var handleUnspecifiedCharacters, defaultValue: true);

            //Make all characters perform the logic
            foreach (var character in characters)
                character.UnHighlight(immediate: immediate);

            //If we are forcing any unspecified characters to use the opposite highlighted status
            if (handleUnspecifiedCharacters)
            {
                foreach (var character in CharacterManager.Instance.AllCharacters)
                {
                    if (characters.Contains(character))
                        continue;

                    unspecifiedCharacters.Add(character);
                    character.Highlight(immediate: immediate);
                }
            }

            //Wait for all characters to finish highlighting
            if (!immediate)
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() =>
                {
                    foreach (var character in characters)
                        character.UnHighlight(immediate: true);

                    if (!handleUnspecifiedCharacters) return;

                    foreach (var character in unspecifiedCharacters)
                        character.Highlight(immediate: true);
                });

                while (characters.Any(c => c.IsUnHighlighting) || (handleUnspecifiedCharacters && unspecifiedCharacters.Any(uc => uc.IsHighlighting)))
                    yield return null;
            }
        }
        #endregion

        #region Base Character Commands
        private static IEnumerator Show(string[] data)
        {
            var character = CharacterManager.Instance.GetCharacter(data[0]);

            if (character == null)
                yield break;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(new[] { "-i", "-immediate" }, out var immediate, defaultValue: false);

            if (immediate)
                character.IsVisible = true;
            else
            {
                //A long-running process should have a stop action to cancel out the coroutine and run logic that should complete this command if interrupted
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character.IsVisible = true; });

                yield return character.Show();
            }
        }

        private static IEnumerator Hide(string[] data)
        {
            var character = CharacterManager.Instance.GetCharacter(data[0]);

            if (character == null)
                yield break;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(new[] { "-i", "-immediate" }, out var immediate, defaultValue: false);

            if (immediate)
                character.IsVisible = false;
            else
            {
                //A long-running process should have a stop action to cancel out the coroutine and run logic that should complete this command if interrupted
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character.IsVisible = false; });

                yield return character.Hide();
            }
        }

        public static void SetPosition(string[] data)
        {
            var character = CharacterManager.Instance.GetCharacter(data[0], createIfDoesNotExist: false);

            if (character == null || data.Length < 2)
                return;

            var parameters = ConvertDataToParameters(data, 1);

            parameters.TryGetValue(ParamXPos, out float x, defaultValue: 0);
            parameters.TryGetValue(ParamYPos, out float y, defaultValue: 0);

            character.SetPosition(new Vector2(x, y));
        }

        public static void SetPriority(string[] data)
        {
            var character = CharacterManager.Instance.GetCharacter(data[0], createIfDoesNotExist: false);

            if (character == null || data.Length < 2)
                return;

            if (!int.TryParse(data[1], out var priority))
                priority = 0;

            character.SetPriority(priority);
        }

        public static IEnumerator SetColor(string[] data)
        {
            var character = CharacterManager.Instance.GetCharacter(data[0], createIfDoesNotExist: false);
            bool immediate;

            if (character == null || data.Length < 2)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            //Try to get the color name
            parameters.TryGetValue(new[] { "-c", "-color" }, out string colorName);
            //Try to get the speed of the transition
            bool specifiedSpeed = parameters.TryGetValue(new[] { "-spd", "-speed" }, out var speed, defaultValue: 1f);
            //Try to get the instant value
            if (!specifiedSpeed)
                parameters.TryGetValue(new[] { "-i", "-immediate" }, out immediate, defaultValue: true);
            else
                immediate = false;

            //Get the color value from the name
            var color = Color.white;
            color = color.GetColorFromName(colorName);

            if (immediate)
                character.SetColor(color);
            else
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character?.SetColor(color); });
                character.TransitionColor(color, speed);
            }
        }

        public static IEnumerator Highlight(string[] data)
        {
            //format: SetSprite(character sprite)
            var character = CharacterManager.Instance.GetCharacter(data[0], createIfDoesNotExist: false);

            if (character == null)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new[] { "-i", "-immediate" }, out var immediate, defaultValue: false);

            if (immediate)
                character.Highlight(immediate: true);
            else
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character.Highlight(immediate: true); });
                yield return character.Highlight();
            }
        }

        public static IEnumerator Unhighlight(string[] data)
        {
            //format: SetSprite(character sprite)
            var character = CharacterManager.Instance.GetCharacter(data[0], createIfDoesNotExist: false);

            if (character == null)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            parameters.TryGetValue(new[] { "-i", "-immediate" }, out var immediate, defaultValue: false);

            if (immediate)
                character.UnHighlight(immediate: true);
            else
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character?.UnHighlight(immediate: true); });
                yield return character.UnHighlight();
            }

        }
        #endregion

        #region SPRITE CHARACTER COMMANDS
        public static IEnumerator SetSprite(string[] data)
        {
            //format: SetSprite(character sprite)
            var character = CharacterManager.Instance.GetCharacter(data[0], createIfDoesNotExist: false) as CharacterSprite;
            var immediate = false;

            if (character == null || data.Length < 2)
                yield break;

            //Grab the extra parameters
            var parameters = ConvertDataToParameters(data, startingIndex: 1);

            //Try to get the sprite name
            parameters.TryGetValue(new[] { "-s", "-sprite" }, out string spriteName);
            //Try to get the layer
            parameters.TryGetValue(new[] { "-l", "-layer" }, out var layer, defaultValue: 0);

            //Try to get the transition speed
            bool specifiedSpeed = parameters.TryGetValue(ParamSpeed, out var speed, defaultValue: 0.1f);

            //Try to get whether this is an immediate transition or not
            if (!specifiedSpeed)
                parameters.TryGetValue(ParamImmediate, out immediate, defaultValue: true);

            //Run the logic
            Sprite sprite = character.GetSprite(spriteName);

            if (sprite == null)
                yield break;

            if (immediate)
            {
                character.SetSprite(sprite, layer);
            }
            else
            {
                CommandManager.Instance.AddTerminationActionToCurrentProcess(() => { character?.SetSprite(sprite, layer); });
                yield return character.TransitionSprite(sprite, layer, speed);
            }

        }
        #endregion
    }
}