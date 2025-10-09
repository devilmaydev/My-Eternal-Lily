using UnityEngine;
using Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using _MAIN.Scripts.Core.ScriptableObjects;
using Core.Utils.Enums;
using Core.Systems.Characters.Events;

namespace Core.Systems.Characters.Factory
{
    /// <summary>
    /// Factory para criação de personagens
    /// Implementa o padrão Factory para criar diferentes tipos de personagens
    /// </summary>
    public static class CharacterFactory
    {
        /// <summary>
        /// Cria um personagem baseado no tipo especificado
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="config">Configuração do personagem</param>
        /// <param name="prefab">Prefab do personagem</param>
        /// <param name="rootCharacterFolder">Pasta raiz do personagem</param>
        /// <returns>Instância do personagem criado</returns>
        public static Character CreateCharacter(string name, CharacterConfigData config, GameObject prefab = null, string rootCharacterFolder = "")
        {
            if (config == null)
            {
                Debug.LogError($"CharacterFactory: Cannot create character '{name}' - config is null!");
                return null;
            }
            
            Character character = config.characterType switch
            {
                ECharacterType.Text => new CharacterText(name, config),
                ECharacterType.Sprite or ECharacterType.SpriteSheet => new CharacterSprite(name, config, prefab, rootCharacterFolder),
                ECharacterType.Live2D => new CharacterLive2D(name, config, prefab, rootCharacterFolder),
                ECharacterType.Model3D => new CharacterModel3D(name, config, prefab, rootCharacterFolder),
                _ => null
            };
            
            if (character == null)
            {
                Debug.LogError($"CharacterFactory: Failed to create character '{name}' of type '{config.characterType}'!");
                return null;
            }
            
            // Disparar evento de criação
            CharacterEvents.InvokeCharacterCreated(character);
            
            return character;
        }
        
        /// <summary>
        /// Cria um personagem baseado no tipo especificado com validação
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="config">Configuração do personagem</param>
        /// <param name="prefab">Prefab do personagem</param>
        /// <param name="rootCharacterFolder">Pasta raiz do personagem</param>
        /// <param name="validateConfig">Se deve validar a configuração</param>
        /// <returns>Instância do personagem criado</returns>
        public static Character CreateCharacterWithValidation(string name, CharacterConfigData config, GameObject prefab = null, string rootCharacterFolder = "", bool validateConfig = true)
        {
            if (validateConfig && !ValidateCharacterConfig(name, config))
            {
                return null;
            }
            
            return CreateCharacter(name, config, prefab, rootCharacterFolder);
        }
        
        /// <summary>
        /// Valida a configuração de um personagem
        /// </summary>
        /// <param name="name">Nome do personagem</param>
        /// <param name="config">Configuração do personagem</param>
        /// <returns>True se a configuração é válida</returns>
        public static bool ValidateCharacterConfig(string name, CharacterConfigData config)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("CharacterFactory: Character name cannot be null or empty!");
                return false;
            }
            
            if (config == null)
            {
                Debug.LogError($"CharacterFactory: Character config for '{name}' cannot be null!");
                return false;
            }
            
            // Validar tipo de personagem
            switch (config.characterType)
            {
                case ECharacterType.Text:
                    // Text characters don't need additional validation
                    break;
                    
                case ECharacterType.Sprite:
                case ECharacterType.SpriteSheet:
                    // Sprite characters validation - sprites are loaded dynamically from Resources
                    // No additional validation needed as sprites are loaded at runtime
                    break;
                    
                case ECharacterType.Live2D:
                    // Live2D characters validation - models are loaded dynamically from Resources
                    // No additional validation needed as models are loaded at runtime
                    break;
                    
                case ECharacterType.Model3D:
                    // 3D Model characters validation - models are loaded dynamically from Resources
                    // No additional validation needed as models are loaded at runtime
                    break;
                    
                default:
                    Debug.LogError($"CharacterFactory: Unknown character type '{config.characterType}' for character '{name}'!");
                    return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Obtém o tipo de personagem baseado na configuração
        /// </summary>
        /// <param name="config">Configuração do personagem</param>
        /// <returns>Tipo do personagem</returns>
        public static ECharacterType GetCharacterType(CharacterConfigData config)
        {
            return config?.characterType ?? ECharacterType.Text;
        }
        
        /// <summary>
        /// Verifica se um tipo de personagem é suportado
        /// </summary>
        /// <param name="characterType">Tipo do personagem</param>
        /// <returns>True se o tipo é suportado</returns>
        public static bool IsCharacterTypeSupported(ECharacterType characterType)
        {
            return characterType switch
            {
                ECharacterType.Text => true,
                ECharacterType.Sprite => true,
                ECharacterType.SpriteSheet => true,
                ECharacterType.Live2D => true,
                ECharacterType.Model3D => true,
                _ => false
            };
        }
    }
}
