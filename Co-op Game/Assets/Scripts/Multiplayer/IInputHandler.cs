public interface IInputHandler 
{
    void HandleInput(string playerId, string action, float x = 0f, float y = 0f);
}
