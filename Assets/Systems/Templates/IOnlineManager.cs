
using Photon.Realtime;

public interface IOnlineManager
{
        public ClientState OnlineState { get; }
        public bool IsOwnerClient { get; }
        public void Login();
}