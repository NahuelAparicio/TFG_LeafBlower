using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractUIManager : MonoBehaviour
{
    // Se pueden asignar directamente en el Inspector o buscar en la jerarquía
    //[SerializeField] private GameObject speechBubble;
    [SerializeField] private GameObject interactIcon;
    //[SerializeField] private GameObject threePoints;
    //[SerializeField] private GameObject exclamation;
    //[SerializeField] private GameObject tick;

    // Diccionario para almacenar el estado (visible/oculto) de cada icono

    private void Awake()
    {
        //// Si no se asignan en el Inspector, se buscan en la jerarquía
        //if (speechBubble == null || exclamation == null || tick == null || interactIcon == null || threePoints == null)
        //{
        //    FindIcons();
        //}

        SetIconVisibility(false);
    }
    public void SetIconVisibility(bool isVisible)
    {
        interactIcon.SetActive(isVisible);
    }
    public void HideIcon()
    {
        interactIcon.SetActive(false);
    }
    //private void FindIcons()
    //{
    //    // Se busca en la jerarquía según la nueva estructura: los iconos están en un objeto hermano
    //    Transform parent = transform.parent;
    //    if (parent == null)
    //    {
    //        Debug.LogError("El objeto " + gameObject.name + " no tiene padre.");
    //        return;
    //    }

    //    foreach (Transform sibling in parent)
    //    {
    //        if (sibling == transform)
    //            continue;

    //        speechBubble = sibling.Find("speechBubble")?.GetComponent<RawImage>();
    //        exclamation = sibling.Find("exclamation")?.GetComponent<RawImage>();
    //        tick = sibling.Find("tick")?.GetComponent<RawImage>();
    //        interactIcon = sibling.Find("interactIcon")?.GetComponent<RawImage>();
    //        threePoints = sibling.Find("threePoints")?.GetComponent<RawImage>();

    //        // Se asume que todos los iconos se encuentran en el mismo objeto hermano
    //        if (speechBubble != null || exclamation != null || tick != null || interactIcon != null || threePoints != null)
    //            break;
    //    }

    //    if (!speechBubble || !exclamation || !tick || !interactIcon || !threePoints)
    //    {
    //        Debug.LogError("Uno o más iconos no fueron encontrados en la jerarquía del padre del objeto: " + gameObject.name);
    //    }
    //}

    //public void SetIconVisibility(bool isVisible, params RawImage[] icons)
    //{
    //    foreach (var icon in icons)
    //    {
    //        icon?.gameObject.SetActive(isVisible);
    //    }
    //}

    //public RawImage GetIcon(string iconName)
    //{
    //    return iconName.ToLower() switch
    //    {
    //        "speechbubble" => speechBubble,
    //        "exclamation" => exclamation,
    //        "tick" => tick,
    //        "interacticon" => interactIcon,
    //        "threepoints" => threePoints,
    //        _ => null
    //    };
    //}

    //// Oculta todos los iconos a la vez
    //public void HideAllIcons()
    //{
    //    speechBubble?.gameObject.SetActive(false);
    //    exclamation?.gameObject.SetActive(false);
    //    tick?.gameObject.SetActive(false);
    //    interactIcon?.gameObject.SetActive(false);
    //    threePoints?.gameObject.SetActive(false);
    //}

    //// Muestra todos los iconos salvo los que se indican en el parámetro
    //public void ShowAllIconsExcept(params RawImage[] excludeIcons)
    //{
    //    RawImage[] allIcons = { speechBubble, exclamation, tick, interactIcon, threePoints };

    //    foreach (var icon in allIcons)
    //    {
    //        if (System.Array.Exists(excludeIcons, e => e == icon))
    //        {
    //            icon?.gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            icon?.gameObject.SetActive(true);
    //        }
    //    }
    //}

    //// Guarda el estado actual de visibilidad de todos los iconos
    //public void SaveVisibilityState()
    //{
    //    if (speechBubble != null)
    //        savedVisibilityStates["speechBubble"] = speechBubble.gameObject.activeSelf;
    //    if (exclamation != null)
    //        savedVisibilityStates["exclamation"] = exclamation.gameObject.activeSelf;
    //    if (tick != null)
    //        savedVisibilityStates["tick"] = tick.gameObject.activeSelf;
    //    if (interactIcon != null)
    //        savedVisibilityStates["interactIcon"] = interactIcon.gameObject.activeSelf;
    //    if (threePoints != null)
    //        savedVisibilityStates["threePoints"] = threePoints.gameObject.activeSelf;
    //}

    //// Restaura el estado de visibilidad de los iconos al último guardado
    //public void RestoreVisibilityState()
    //{
    //    if (savedVisibilityStates == null || savedVisibilityStates.Count == 0)
    //    {
    //        Debug.LogWarning("No se ha guardado ningún estado de visibilidad.");
    //        return;
    //    }

    //    if (speechBubble != null && savedVisibilityStates.ContainsKey("speechBubble"))
    //        speechBubble.gameObject.SetActive(savedVisibilityStates["speechBubble"]);
    //    if (exclamation != null && savedVisibilityStates.ContainsKey("exclamation"))
    //        exclamation.gameObject.SetActive(savedVisibilityStates["exclamation"]);
    //    if (tick != null && savedVisibilityStates.ContainsKey("tick"))
    //        tick.gameObject.SetActive(savedVisibilityStates["tick"]);
    //    if (interactIcon != null && savedVisibilityStates.ContainsKey("interactIcon"))
    //        interactIcon.gameObject.SetActive(savedVisibilityStates["interactIcon"]);
    //    if (threePoints != null && savedVisibilityStates.ContainsKey("threePoints"))
    //        threePoints.gameObject.SetActive(savedVisibilityStates["threePoints"]);
    //}
}
