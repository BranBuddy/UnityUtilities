/*
    Interface which requires all interactables to have these
*/
public interface IInteractable
{
    int id { get; set; }
    bool interactable { get; set; }
    string interactionName { get; set;}
}
